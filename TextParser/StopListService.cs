using System.Collections.Generic;

namespace TextParser
{
    public class StopListService
    {
        public List<string> Words { get; set; } = new List<string>();

        public StopListService(List<string> words)
        {
            Words = words;
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
