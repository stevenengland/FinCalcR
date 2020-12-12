using System;
using System.Collections.Generic;
using System.Text;
using StEn.FinCalcR.Common.Attributes;

namespace StEn.FinCalcR.Calculations.Calculator.Attributes
{
    [AttributeUsage(AttributeTargets.Field)]
    public class TokenAttribute : Attribute, IAttributeParsingExtender
    {
        public TokenAttribute(string token)
        {
            this.Token = token;
            this.ParsingId = token;
        }

        public string Token { get; }

        public string ParsingId { get; }
    }
}
