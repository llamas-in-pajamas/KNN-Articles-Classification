using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExtensionMethods
{
    public static class StringExtensionMethods
    {
        public static string RemovePunctuation(this string text)
        {
            return new string(text.Where(c => !char.IsPunctuation(c)).ToArray());
        }


        public static string RemoveDigits(this string text)
        {
            return new string(text.Where(c => !char.IsDigit(c)).ToArray());
        }

        public static string RemoveSymbols(this string text)
        {
            return new string(text.Where(c => !char.IsSymbol(c)).ToArray());
        }

        public static List<string> ToListWithoutEmptyEntries(this string text)
        {
            return text.Split(new char[0], StringSplitOptions.RemoveEmptyEntries).ToList();
        }
    }
}
