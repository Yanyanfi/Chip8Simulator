using Core.Input;
using Core.Logger;
using Core.Memory;
using System.Data;
using System.Security.Cryptography;
using System.Text;

namespace Core.Decoder;

public sealed class Decoder(IFontAddressProvider provider, ILogger logger) : IDecoder
{
    private readonly IFontAddressProvider _addrProvider = provider;
    private readonly ILogger _logger = logger;
    /// <exception cref="UnsupportedInstructionException">遇到未实现或不受支持的操作码组合时抛出。</exception>
    public void Decode(
        (byte OPCode, byte, byte, byte) instruction,
        byte[] memory,
        ushort[] stack,
        byte[] dataRegisters,
        bool[,] display,
        ref ushort iRegister,
        ref ushort pc,
        ref byte sp,
        ref byte delayTimer,
        ref byte soundTimer,
        Keypad keypad,
        ref bool isWaitingKeyPress,
        ref bool isWaitingKeyRelease)
    {
        _logger.Log("-----------------------------------------------------------------");
        _logger.Log($"intruction code: {instruction}");
        _logger.Log($"pc:{pc} sp:{sp} delayTimer:{delayTimer} soundTimer:{soundTimer} key:{keypad.GetKeyCode()} v0:{dataRegisters[0]} v1:{dataRegisters[1]} v2:{dataRegisters[2]} i:{iRegister}");
        switch (instruction)
        {
            case (0, 0, 14, 0):
                ClearDisplay(display);
                _logger.Log("clear display");
                break;
            case (0, 0, 14, 14):
                Return(ref sp, stack, ref pc);
                break;
            case (0, _, _, _):
                break;
            case (1, var n1, var n2, var n3):
                Jump(CombineNibble(n1, n2, n3), ref pc);
                break;
            case (2, var n1, var n2, var n3):
                Call(CombineNibble(n1, n2, n3), ref sp, stack, ref pc);
                break;
            case (3, var x, var k1, var k2):
                if (dataRegisters[x] == CombineNibble(k1, k2))
                    pc += 2;
                break;
            case (4, var x, var k1, var k2):
                if (dataRegisters[x] != CombineNibble(k1, k2))
                    pc += 2;
                break;
            case (5, var x, var y, _):
                if (dataRegisters[x] == dataRegisters[y])
                    pc += 2;
                break;
            case (6, var x, var k1, var k2):
                dataRegisters[x] = CombineNibble(k1, k2);
                break;
            case (7, var x, var k1, var k2):
                dataRegisters[x] += CombineNibble(k1, k2);
                break;
            case (8, var x, var y, 0):
                dataRegisters[x] = dataRegisters[y];
                break;
            case (8, var x, var y, 1):
                dataRegisters[x] |= dataRegisters[y];
                break;
            case (8, var x, var y, 2):
                dataRegisters[x] &= dataRegisters[y];
                break;
            case (8, var x, var y, 3):
                dataRegisters[x] ^= dataRegisters[y];
                break;
            case (8, var x, var y, 4):
                var sum = dataRegisters[x] + dataRegisters[y];
                dataRegisters[x] = (byte)sum;
                dataRegisters[15] = (byte)(sum > 255 ? 1 : 0);
                break;
            case (8, var x, var y, 5):
                var vx = dataRegisters[x];
                var vy = dataRegisters[y];
                dataRegisters[x] = (byte)(vx - vy);
                dataRegisters[15] = (byte)(vx >= vy ? 1 : 0);
                break;
            case (8, var x, var y, 6):
                vy = dataRegisters[y];
                dataRegisters[x] = (byte)(vy >> 1);
                dataRegisters[15] = (byte)(vy % 2 == 0 ? 0 : 1);
                break;
            case (8, var x, var y, 7):
                vx = dataRegisters[x];
                vy = dataRegisters[y];
                dataRegisters[x] = (byte)(vy - vx);
                dataRegisters[15] = (byte)(vy >= vx ? 1 : 0);
                break;
            case (8, var x, var y, 14):
                vx = dataRegisters[x];
                vy = dataRegisters[y];
                dataRegisters[x] = (byte)(vy << 1);
                dataRegisters[15] = (byte)(vx >= 128 ? 1 : 0);
                break;
            case (9, var x, var y, _):
                if (dataRegisters[x] != dataRegisters[y])
                    pc += 2;
                break;
            case (10, var n1, var n2, var n3):
                iRegister = CombineNibble(n1, n2, n3);
                break;
            case (11, var n1, var n2, var n3):
                Jump((ushort)(CombineNibble(n1, n2, n3) + dataRegisters[0]), ref pc);
                break;
            case (12, var x, var k1, var k2):
                dataRegisters[x] = (byte)(GetRandom() & CombineNibble(k1, k2));
                break;
            case (13, var x, var y, var n):
                DrawSprite(memory, iRegister, x, y, n, display, dataRegisters);
                break;
            case (14, var x, 9, 14):
                if (keypad.IsPressed(dataRegisters[x]))
                    pc += 2;
                break;
            case (14, var x, 10, 1):
                if (!keypad.IsPressed(dataRegisters[x]))
                    pc += 2;
                break;
            case (15, var x, 0, 7):
                dataRegisters[x] = delayTimer;
                break;
            case (15, var x, 0, 10):
                WaitForKey(x, dataRegisters, keypad, ref isWaitingKeyPress, ref isWaitingKeyRelease, ref pc);
                break;
            case (15, var x, 1, 5):
                delayTimer = dataRegisters[x];
                break;
            case (15, var x, 1, 8):
                soundTimer = dataRegisters[x];
                break;
            case (15, var x, 1, 14):
                iRegister += dataRegisters[x];
                break;
            case (15, var x, 2, 9):
                iRegister = _addrProvider.GetAddress(dataRegisters[x]);
                break;
            case (15, var x, 3, 3):
                var num = dataRegisters[x];
                var h = num / 100;
                var t = (num - h * 100) / 10;
                var o = (num - h * 100 - t * 10);
                (memory[iRegister], memory[iRegister + 1], memory[iRegister + 2]) = ((byte)h, (byte)t, (byte)o);
                break;
            case (15, var x, 5, 5):
                x %= 16;
                for (byte i = 0; i <= x; i++)
                    memory[iRegister + i] = dataRegisters[i];
                break;
            case (15, var x, 6, 5):
                x %= 16;
                for (byte i = 0; i <= x; i++)
                    dataRegisters[i] = memory[iRegister + i];
                break;
            default:
                throw new UnsupportedInstructionException();
        }
        pc += 2;
        _logger.Save();
    }

    private static void ClearDisplay(bool[,] display) => Array.Clear(display);
    private void Return(ref byte sp, ushort[] stack, ref ushort pc)
    {
        pc = stack[sp];
        sp--;
        _logger.Log(@$"return to {pc + 2}");
    }
    private void Jump(ushort addr, ref ushort pc)
    {
        pc = (ushort)(addr - 2);
        _logger.Log($"jump to {addr}");
    }

    private void Call(ushort addr, ref byte sp, ushort[] stack, ref ushort pc)
    {
        sp++;
        stack[sp] = pc;
        pc = (ushort)(addr - 2);
        _logger.Log($"call {addr}");
    }
    private static void WaitForKey(byte x, byte[] dataRegisters,Keypad keypad,ref bool isWaitingPress, ref bool isWaitingRelease,ref ushort pc)
    {
        switch (isWaitingPress, isWaitingRelease)
        {
            case (false, false):
                if (keypad.Key == Key.None)
                    isWaitingPress = true;
                pc -= 2;
                break;
            case (true, false):
                if (keypad.Key != Key.None)
                {
                    isWaitingPress = false;
                    isWaitingRelease = true;
                }
                pc -= 2;
                break;
            case (false, true):
                if(keypad.Key == Key.None)
                {
                    dataRegisters[x] = keypad.GetPreviousKeyCode();
                    isWaitingRelease = false;
                    break;
                }
                pc -= 2;
                break;
        }
    }
    private void DrawSprite(byte[] memory, ushort iRegister, byte x, byte y, byte size, bool[,] display, byte[] dataRegisters)
    {
        dataRegisters[15] = 0;
        var sprite = memory
            .Skip(iRegister)
            .Take(size)
            .Select(e => Convert.ToString(e, 2).PadLeft(8, '0'))
            .Select(e => e.Select(c => c != '0').ToArray())
            .ToArray();
        var startRow = dataRegisters[y];
        var startCol = dataRegisters[x];
        for (int i = 0; i < size; i++)
        {
            var row = (i + startRow) % 32;
            for (int j = 0; j < 8; j++)
            {
                var col = (j + startCol) % 64;
                var screenPixel = display[row, col];
                var spritePixel = sprite[i][j];
                if (screenPixel && spritePixel)
                    dataRegisters[15] = 1;
                display[row, col] ^= spritePixel;
            }
        }
        var sb = new StringBuilder("draw sprite\n");
        sb.AppendLine($"memoryLocation:{iRegister}  size:{size}  location:{x},{y}");
        foreach (var line in sprite)
        {
            foreach (var pixel in line)
            {
                sb.Append(pixel ? '#' : ' ');
            }
            sb.AppendLine();
        }
        _logger.Log(sb.ToString());

    }
    private static byte GetRandom() => (byte)RandomNumberGenerator.GetInt32(256);
    private static byte CombineNibble(byte high, byte low) => (byte)((high << 4) + low);
    private static ushort CombineNibble(byte high, byte middle, byte low) => (ushort)((high << 8) + (middle << 4) + low);
}