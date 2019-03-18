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

        public static List<string> ToListWithoutEmptyEntitles(this string text)
        {
            return text.Split(new char[0], StringSplitOptions.RemoveEmptyEntries).ToList();
        }
    }
}
