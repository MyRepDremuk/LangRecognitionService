using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LanguageRecognitionAPI
{
    public class RecognitionResult
    {
        public string Word { get; set; }
        public Dictionary<string, double> LangProbability { get; set; }

        public RecognitionResult()
        {
            Word = "";
            LangProbability = new Dictionary<string, double>();
        }
        

        public RecognitionResult(string currWord)
        {
            Word = currWord;
            LangProbability = new Dictionary<string, double>();
        }


        public RecognitionResult(string currWord, Dictionary<string, double> currLangProbability)
        {
            Word = currWord;
            LangProbability = currLangProbability;
        }


        public override string ToString()
        {
            StringBuilder strBld = new StringBuilder(Word+" \n");
            foreach(var currLang in LangProbability.Keys)
            {
                strBld.Append(currLang + " language probability is " + LangProbability[currLang] + "%\n");
            }
            return strBld.ToString();
        }


        public string ProbabilityString()
        {
            StringBuilder strBld = new StringBuilder();
            foreach (var currLang in LangProbability.Keys)
            {
                strBld.Append(currLang + " : " + LangProbability[currLang] + "%; \n");
            }
            return strBld.ToString();
        }
    }
}
