﻿using System;
using System.Collections.Generic;
using StEn.FinCalcR.Common.Attributes;
using StEn.FinCalcR.Common.Extensions;

namespace StEn.FinCalcR.Common.Converter
{
    public static class EnumConverter
    {
        public static TEnum ParseToEnum<TEnum, TAttribute>(string id)
            where TEnum : Enum
            where TAttribute : Attribute, IAttributeParsingExtender
        {
            foreach (var enumMember in (TEnum[])Enum.GetValues(typeof(TEnum)))
            {
                var attribute = enumMember.GetAttribute<TAttribute>();
                if (attribute != null && attribute.ParsingId == id)
                {
                    return enumMember;
                }
            }

            throw new KeyNotFoundException();
        }

        public static T GetAttributeOfType<T>(this Enum enumVal)
            where T : Attribute
        {
            var type = enumVal.GetType();
            var memInfo = type.GetMember(enumVal.ToString());
            var attributes = memInfo[0].GetCustomAttributes(typeof(T), false);
            return (attributes.Length > 0) ? (T)attributes[0] : null;
        }
    }
}
