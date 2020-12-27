namespace StEn.FinCalcR.Calculations.Calculator
{
    public class SimpleMemoryField<T> : IMemoryField<T>
    {
        public SimpleMemoryField(string id, T defaultValue, string category)
        {
            this.Id = id;
            this.DefaultValue = defaultValue;
            this.Value = defaultValue;
            this.Category = category;
        }

        public string Id { get; }

        public string Category { get; }

        public T DefaultValue { get; }

        public T Value { get; set; }

        public void Reset()
        {
            this.Value = this.DefaultValue;
        }
    }
}
