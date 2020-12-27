using System;
using System.Globalization;
using System.Reflection;

namespace StEn.FinCalcR.Common.Extensions
{
	public static class EnumExtensions
	{
		public static TAttribute GetAttribute<TAttribute>(this Enum value)
			where TAttribute : Attribute
		{
			var type = value.GetType();
			var name = Enum.GetName(type, value);
			return type.GetField(name) // I prefer to get attributes this way
				.GetCustomAttribute<TAttribute>();
		}

		/// <summary>
		/// Determines whether a given flag is the only flag of all possible flags that is not set.
		/// </summary>
		/// <typeparam name="T">The Flag/Enum type.</typeparam>
		/// <param name="enumeration">The Enum in which the flags shall be checked.</param>
		/// <param name="queriedFlag">The flag that should not be set.</param>
		/// <returns>True if the flag is the only one that is not set.</returns>
		public static bool IsOnlyFlagNotSet<T>(this T enumeration, T queriedFlag)
			where T : Enum
		{
			var possibleFlags = (T[])Enum.GetValues(typeof(T));
			foreach (var flag in possibleFlags)
			{
				if (queriedFlag.Equals(flag))
				{
					// If the queried flag is set is is not unset like the title of the function implies.
					if (enumeration.HasFlag(flag))
					{
						return false;
					}
				}
				else if (!enumeration.HasFlag(flag))
				{
					return false;
				}
			}

			return true;
		}

		/// <summary>
		/// Determines whether a given flag is the only flag of all possible flags that is set.
		/// </summary>
		/// <typeparam name="T">The Flag/Enum type.</typeparam>
		/// <param name="enumeration">The Enum in which the flags shall be checked.</param>
		/// <param name="queriedFlag">The flag that should be set.</param>
		/// <returns>True if the flag is the only one that is set.</returns>
		public static bool IsOnlyFlagSet<T>(this T enumeration, T queriedFlag)
			where T : Enum
		{
			var possibleFlags = (T[])Enum.GetValues(typeof(T));
			foreach (var flag in possibleFlags)
			{
				if (queriedFlag.Equals(flag))
				{
					// If the queried flag is unset it is not set like the title of the function implies.
					if (!enumeration.HasFlag(flag))
					{
						return false;
					}
				}
				else if (enumeration.HasFlag(flag))
				{
					return false;
				}
			}

			return true;
		}

		/// <summary>
		/// Determines whether every flag of all possible flags are set.
		/// </summary>
		/// <typeparam name="T">The Flag/Enum type.</typeparam>
		/// <param name="enumeration">The Enum in which the flags shall be checked.</param>
		/// <returns>True if every flag is set.</returns>
		public static bool IsEveryFlagSet<T>(this T enumeration)
			where T : Enum
		{
			var possibleFlags = (T[])Enum.GetValues(typeof(T));
			foreach (var flag in possibleFlags)
			{
				if (!enumeration.HasFlag(flag))
				{
					return false;
				}
			}

			return true;
		}

		/// <summary>
		/// Determines whether not a single flag of all possible flags is set.
		/// </summary>
		/// <typeparam name="T">The Flag/Enum type.</typeparam>
		/// <param name="enumeration">The Enum in which the flags shall be checked.</param>
		/// <returns>True if no flag is set.</returns>
		public static bool IsNoFlagSet<T>(this T enumeration)
			where T : Enum
		{
			var possibleFlags = (T[])Enum.GetValues(typeof(T));
			foreach (var flag in possibleFlags)
			{
				if (enumeration.HasFlag(flag))
				{
					return false;
				}
			}

			return true;
		}

		public static T SetAllFlags<T>(this T enumeration, bool value)
			where T : struct, IComparable, IFormattable, IConvertible
		{
			var possibleFlags = (T[])Enum.GetValues(typeof(T));
			foreach (var flag in possibleFlags)
			{
				enumeration = enumeration.SetFlag(flag, value);
			}

			return enumeration;
		}

		public static T SetFlag<T>(this T flags, T flag, bool value)
			where T : struct, IComparable, IFormattable, IConvertible
		{
			var flagsInt = flags.ToInt32(NumberFormatInfo.CurrentInfo);
			var flagInt = flag.ToInt32(NumberFormatInfo.CurrentInfo);
			if (value)
			{
				flagsInt |= flagInt;
			}
			else
			{
				flagsInt &= ~flagInt;
			}

			return (T)(object)flagsInt;
		}
	}
}
