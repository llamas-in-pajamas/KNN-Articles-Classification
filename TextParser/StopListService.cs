using System;
using System.Collections.Generic;
using System.Linq;
using ExtensionMethods;

namespace TextParser
{
    public class StopListService
    {
        public List<string> Words { get; set; } = new List<string>();

        public StopListService(string text)
        {
            Words = text.RemovePunctuation().ToListWithoutEmptyEntitles();
        }

        public List<string> Call(List<string> stopList)
        {
            List<string> temp = new List<string>(Words);
            foreach (string stopWord in stopList)
            {
                if (temp.Contains(stopWord))
                {
                    temp.RemoveAll(n => n == stopWord);
                }
            }
            temp.TrimExcess();
            return temp;
        }
    }
}
