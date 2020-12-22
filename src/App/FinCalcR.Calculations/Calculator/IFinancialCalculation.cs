namespace StEn.FinCalcR.Calculations.Calculator
{
    public interface IFinancialCalculation
    {
        bool UsesAnticipativeInterestYield { get; }

        void CalculateYears();

        void SetYears();

        void SetRatesPerAnnum();

        void SetEnd();

        void CalculateEnd();

        void CalculatePercent();

        void CalculateRate();

        void SetRate();

        void GetRepaymentRate();

        void SetRepaymentRate();

        void CalculateStart();

        void SetStart();

        void SetAdvance(bool useAdvance);

        void CalculateEffectiveInterest();
    }
}
