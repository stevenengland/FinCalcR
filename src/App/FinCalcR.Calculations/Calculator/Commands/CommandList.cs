using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace StEn.FinCalcR.Calculations.Calculator.Commands
{
    public class CommandList : IEnumerable<ICalculatorCommand>
    {
        private readonly IList<ICalculatorCommand> list;

        public CommandList(IList<ICalculatorCommand> list)
        {
            this.list = list ?? throw new ArgumentNullException(nameof(list));
        }

        public T GetCommandByType<T>()
        	where T : class, ICalculatorCommand
        {
            return this.list.OfType<T>().SingleOrDefault();
        }

        public IEnumerator<ICalculatorCommand> GetEnumerator()
        {
            return this.list.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IEnumerable)this.list).GetEnumerator();
        }

        public ICalculatorCommand GetCommandByFullTypeName(string fullTypeName)
        {
            return this.list.SingleOrDefault(x => x.GetType().FullName == fullTypeName);
        }
    }
}
