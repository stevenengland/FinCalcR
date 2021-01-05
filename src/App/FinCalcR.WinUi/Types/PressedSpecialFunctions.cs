using System;

namespace StEn.FinCalcR.WinUi.Types
{
    [Flags]
    public enum PressedSpecialFunctions
    {
        None = 0,
        Years = 1,
        Interest = 2,
        Start = 4,
        Rate = 8,
        End = 16,
    }
}
