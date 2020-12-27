namespace StEn.FinCalcR.Calculations.Calculator
{
    public interface IMemoryFieldDescriptor
    {
        string Id { get; }

        string Category { get; }

        void Reset();
    }
}
