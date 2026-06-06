using System.ComponentModel.DataAnnotations;
using Core.Input;
namespace Core.Decoder;

public interface IDecoder
{
    ///<exception cref="UnsupportedInstructionException"/>
    void Decode
    (
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
        ref bool isWaitingKeyRelease
    );
}
