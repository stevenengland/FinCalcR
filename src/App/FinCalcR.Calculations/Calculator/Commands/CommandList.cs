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
            where T : class, ICalculatorCommand => this.list.OfType<T>().SingleOrDefault();

        public IEnumerator<ICalculatorCommand> GetEnumerator() => this.list.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => ((IEnumerable)this.list).GetEnumerator();

        public ICalculatorCommand GetCommandByFullTypeName(string fullTypeName) => this.list.SingleOrDefault(x => x.GetType().FullName == fullTypeName);
    }
}
