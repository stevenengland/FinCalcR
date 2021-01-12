namespace StEn.FinCalcR.Calculations.Calculator
{
    public interface IMemoryFieldValue<T>
    {
        T DefaultValue { get; }

        // If I ever switch to decimal I should try to introduce a nullable generic here which is fine in C#9
        T Value { get; set; }
    }
}
