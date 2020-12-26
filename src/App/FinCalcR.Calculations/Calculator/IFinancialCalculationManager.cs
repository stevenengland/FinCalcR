namespace StEn.FinCalcR.Calculations.Calculator
{
    public interface IFinancialCalculationManager
    {
        bool UsesAnticipativeInterestYield { get; }

        void CalculateYears();

        void SetYears();

        void SetRatesPerAnnum();

        void CalculateEffectiveInterest();

        void SetEffectiveInterestRate();

        void GetNominalInterestRate();

        void SetNominalInterestRate();

        void CalculateStart();

        void SetStart();

        void SetAdvance(bool useAdvance);

        void CalculateRate();

        void SetRate();

        void GetRepaymentRate();

        void SetRepaymentRate();

        void SetEnd();

        void CalculateEnd();

        void CalculatePercent();
    }
}
