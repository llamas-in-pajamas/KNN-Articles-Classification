using System;
using System.Collections.Generic;
using System.Linq;

namespace TextParser
{
    public class StopListService
    {
        public List<string> Words { get; set; } = new List<string>();
        public List<string> Unknown { get; set; } = new List<string>();

        public StopListService(string text)
        {
            Words = text.Split(new char[0], StringSplitOptions.RemoveEmptyEntries).ToList();
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
