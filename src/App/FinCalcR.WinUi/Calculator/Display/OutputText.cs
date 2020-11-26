using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StEn.FinCalcR.WinUi.Calculator.Display
{
    public class OutputText : IOutputText
    {
        public OutputText(IOutputTextProcessor processor)
        {
            this.Processor = processor;
        }

        public string TextValue { get; }

        public double NumberValue { get; }

        public IOutputTextProcessor Processor { get; }
    }
}
