﻿using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Threading;

namespace StEn.FinCalcR.Calculations.Calculator.Display
{
    public class SingleNumberInput : IInputText
    {
        private readonly int maxArithmeticPrecision;

        private bool isDecimalSeparatorActive = false;
        private string wholeNumberPart = "0";
        private string fractionalNumberPart = string.Empty;
        private string currentInputFormula;

        public SingleNumberInput(string thousandSeparator, string decimalSeparator, int maxArithmeticPrecision)
        {
            this.maxArithmeticPrecision = maxArithmeticPrecision;

            this.BuildInputTextFromInternalState();
        }

        // TODO: Remove this Property
        public bool IsDecimalSeparatorActive
        {
            get { return this.isDecimalSeparatorActive; }
            set { this.isDecimalSeparatorActive = value; }
        }

        // TODO: Remove this Property
        public string WholeNumberPart
        {
            get { return this.wholeNumberPart; }
            set { this.wholeNumberPart = value; }
        }

        // TODO: Remove this Property
        public string FractionalNumberPart
        {
            get { return this.fractionalNumberPart; }
            set { this.fractionalNumberPart = value; }
        }

        /// <inheritdoc />
        /// The formula will only consist of a typed number in this implementation.
        public string GetCurrentInputFormula()
        {
            this.BuildInputTextFromInternalState();
            return this.currentInputFormula;
        }

        /// <inheritdoc />
        /// The evaluated result will be the same as the <see cref="CurrentInputFormula"/> in this implementation.
        public string GetEvaluatedResult()
        {
            return this.GetCurrentInputFormula();
        }

        public void ResetInternalState(bool updateCurrentInputText = false)
        {
            this.fractionalNumberPart = string.Empty;
            this.wholeNumberPart = "0";
            this.isDecimalSeparatorActive = false;

            if (updateCurrentInputText)
            {
                this.BuildInputTextFromInternalState();
            }
        }

        public void Set(double number)
        {
            this.BuildInternalStateFromNumber(number);
            this.BuildInputTextFromInternalState();
        }

        public void DecimalSeparator()
        {
            this.isDecimalSeparatorActive = true;
        }

        public void AlgebSign()
        {
            if (this.wholeNumberPart.StartsWith("-"))
            {
                this.wholeNumberPart = this.wholeNumberPart.Substring(1);
            }
            else
            {
                this.wholeNumberPart = "-" + this.wholeNumberPart;
            }
        }

        private void BuildInputTextFromInternalState()
        {
            this.currentInputFormula = this.wholeNumberPart
                                       + Thread.CurrentThread.CurrentUICulture.NumberFormat.NumberDecimalSeparator
                                       + this.fractionalNumberPart;
        }

        private void BuildInternalStateFromNumber(double number)
        {
            this.ResetInternalState();

            var roundedResult = Math.Round(number, this.maxArithmeticPrecision);
            var s = roundedResult.ToString(CultureInfo.InvariantCulture);
            var parts = s.Split('.');
            this.wholeNumberPart = parts[0];
            this.fractionalNumberPart = parts.Length < 2 ? string.Empty : parts[1];
        }
    }
}
