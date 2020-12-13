﻿using System;
using System.Collections.Generic;
using System.Text;

namespace StEn.FinCalcR.Calculations.Messages
{
    public class ErrorMessages
    {
        protected ErrorMessages()
        {
        }

        public static ErrorMessages Instance { get; } = new ErrorMessages();

        public virtual string CalculationOfPercentIsNotPossible(int iterations)
        {
            return $"Calculation of p was not possible. Try to increase {nameof(iterations)}.";
        }
    }
}
