using StEn.FinCalcR.Calculations.Calculator.Attributes;

namespace StEn.FinCalcR.Calculations.Calculator
{
    public enum MathOperator
    {
        None = 0,
        [Token("+")]
        Add = 1,
        [Token("-")]
        Subtract,
        [Token("/")]
        Divide,
        [Token("*")]
        Multiply,
    }
}
