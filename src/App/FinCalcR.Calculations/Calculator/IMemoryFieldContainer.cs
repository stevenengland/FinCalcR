using System.Collections.Generic;

namespace StEn.FinCalcR.Calculations.Calculator
{
    public interface IMemoryFieldContainer
    {
        void Add<T>(IMemoryField<T> memoryField);

        IMemoryField<T> Get<T>(string key);

        IMemoryFieldDescriptor Get(string key);

        void Reset(IList<string> categories = null);
    }
}
