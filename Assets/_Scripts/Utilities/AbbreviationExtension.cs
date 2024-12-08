using System;

namespace Utilities
{
    /// <summary>
    /// Abbreviate a number to a more readable format.
    /// 1000 -> 1K
    /// 10000 -> 10K
    /// 1000000 -> 1M
    /// 1000000000 -> 1B
    /// 1000000000000 -> 1T
    /// </summary>
    public static class AbbreviationExtension
    {
        public static string Abbreviate(this int number)
        {
            return Abbreviate((double)number);
        }

        public static string Abbreviate(this long number)
        {
            return Abbreviate((double)number);
        }

        private static string Abbreviate(double number)
        {
            if (number >= 1_000_000_000_000)
            {
                return (number / 1_000_000_000_000).ToString("0.0") + "T";
            }
            else if (number >= 1_000_000_000)
            {
                return (number / 1_000_000_000).ToString("0.0") + "B";
            }
            else if (number >= 1_000_000)
            {
                return (number / 1_000_000).ToString("0.0") + "M";
            }
            else if (number >= 1_000)
            {
                return (number / 1_000).ToString("0.0") + "K";
            }
            else
            {
                return number.ToString();
            }
        }
    }
}