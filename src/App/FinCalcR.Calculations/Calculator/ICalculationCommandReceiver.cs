using StEn.FinCalcR.Calculations.Calculator.Commands;
using StEn.FinCalcR.Calculations.Calculator.Display;

namespace StEn.FinCalcR.Calculations.Calculator
{
    public interface ICalculationCommandReceiver : IFinancialCalculationManager, IStandardCalculationManager
    {
        IOutputText OutputText { get; }

        IInputText InputText { get; }

        // ToDo: Can I get rid of it? Suits no interface description, does it?
        MathOperator ActiveMathOperator { get; set; }

        // TODO: MAKE PRIVATE OR REMOVE
        CommandWord LastCommand { get; set; }
    }
}
