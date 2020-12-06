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

            this.MemoryFields.Add(new SimpleMemoryField(MemoryFieldNames.PreOperatorNumber, 0, MemoryFieldNames.Categories.Standard));
            this.MemoryFields.Add(new SimpleMemoryField(MemoryFieldNames.PostOperatorNumber, 0, MemoryFieldNames.Categories.Standard));
            this.MemoryFields.Add(new SimpleMemoryField(MemoryFieldNames.YearsNumber, 0, MemoryFieldNames.Categories.Standard));
            this.MemoryFields.Add(new SimpleMemoryField(MemoryFieldNames.InterestNumber, 0, MemoryFieldNames.Categories.Special));
            this.MemoryFields.Add(new SimpleMemoryField(MemoryFieldNames.StartNumber, 0, MemoryFieldNames.Categories.Standard));
            this.MemoryFields.Add(new SimpleMemoryField(MemoryFieldNames.RateNumber, 0, MemoryFieldNames.Categories.Standard));
            this.MemoryFields.Add(new SimpleMemoryField(MemoryFieldNames.EndNumber, 0, MemoryFieldNames.Categories.Standard));
            this.MemoryFields.Add(new SimpleMemoryField(MemoryFieldNames.RatesPerAnnumNumber, 12, MemoryFieldNames.Categories.Standard));
            this.MemoryFields.Add(new SimpleMemoryField(MemoryFieldNames.NominalInterestRateNumber, 0, MemoryFieldNames.Categories.Standard));
            this.MemoryFields.Add(new SimpleMemoryField(MemoryFieldNames.RepaymentRateNumber, 0, MemoryFieldNames.Categories.Standard));
        }

        public List<IMemoryField> MemoryFields { get; }

        public IOutputText OutputText { get; }

        public IInputText InputText { get; }

        public MathOperator ActiveMathOperator { get; set; }

        public bool IsCalcCommandLock { get; set; }

        public void ResetMemoryFields(IList<string> categories = null)
        {
            if (categories == null || !categories.Any())
            {
                foreach (var memoryField in this.MemoryFields)
                {
                    memoryField.Reset();
                }
            }
            else
            {
                foreach (var category in categories)
                {
                    foreach (var memoryField in this.MemoryFields)
                    {
                        if (memoryField.Category == category)
                        {
                            memoryField.Reset();
                        }
                    }
                }
            }
        }
    }
}
