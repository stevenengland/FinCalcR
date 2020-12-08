using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using StEn.FinCalcR.Calculations.Calculator.Display;
using StEn.FinCalcR.Calculations.Commands;

namespace StEn.FinCalcR.Calculations.Calculator
{
    public class Calculator : ICalculationCommandReceiver
    {
        public Calculator(IOutputText outputText, IInputText inputText)
        {
            this.OutputText = outputText;
            this.InputText = inputText;

            this.MemoryFields.Add(new SimpleMemoryField<double>(MemoryFieldNames.PreOperatorNumber, 0, MemoryFieldNames.Categories.Standard));
            this.MemoryFields.Add(new SimpleMemoryField<double>(MemoryFieldNames.PostOperatorNumber, 0, MemoryFieldNames.Categories.Standard));
            this.MemoryFields.Add(new SimpleMemoryField<double>(MemoryFieldNames.YearsNumber, 0, MemoryFieldNames.Categories.Special));
            this.MemoryFields.Add(new SimpleMemoryField<double>(MemoryFieldNames.InterestNumber, 0, MemoryFieldNames.Categories.Special));
            this.MemoryFields.Add(new SimpleMemoryField<double>(MemoryFieldNames.StartNumber, 0, MemoryFieldNames.Categories.Special));
            this.MemoryFields.Add(new SimpleMemoryField<double>(MemoryFieldNames.RateNumber, 0, MemoryFieldNames.Categories.Special));
            this.MemoryFields.Add(new SimpleMemoryField<double>(MemoryFieldNames.EndNumber, 0, MemoryFieldNames.Categories.Special));
            this.MemoryFields.Add(new SimpleMemoryField<int>(MemoryFieldNames.RatesPerAnnumNumber, 12, MemoryFieldNames.Categories.Special));
            this.MemoryFields.Add(new SimpleMemoryField<double>(MemoryFieldNames.NominalInterestRateNumber, 0, MemoryFieldNames.Categories.Special));
            this.MemoryFields.Add(new SimpleMemoryField<double>(MemoryFieldNames.RepaymentRateNumber, 0, MemoryFieldNames.Categories.Special));
            this.MemoryFields.Add(new SimpleMemoryField<bool>(MemoryFieldNames.IsAdvance, false, MemoryFieldNames.Categories.Special));
        }

        public IMemoryFieldContainer MemoryFields { get; } = new MemoryFieldContainer();

        public IOutputText OutputText { get; }

        public IInputText InputText { get; }

        public MathOperator ActiveMathOperator { get; set; }

        public bool IsCalcCommandLock { get; set; }
    }
}
