using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LanguageRecognitionAPI
{
    public class Language
    {
        public readonly Dictionary<char, double> charFrequencyCollection;
        public readonly Dictionary<string, double> bigramsFrequencyCollection;
        public readonly Dictionary<string, double> trigramsFrequencyCollection;
        public readonly string languageNames;
        public readonly int languageNumberOfChars;
        public readonly string languageUnigramFilePathes;
        public readonly string languageBigramFilePathes;
        public readonly string languageTrigramFilePathes;

        public Language(string Name, string currUnigarmFilePath, string currBigarmFilePath, string currTrigarmFilePath, int NumberOfChars)
        {
            languageNames = Name;
            languageUnigramFilePathes = currUnigarmFilePath;
            languageBigramFilePathes = currBigarmFilePath;
            languageTrigramFilePathes = currTrigarmFilePath;
            languageNumberOfChars = NumberOfChars;

            charFrequencyCollection = new Dictionary<char,double>(languageNumberOfChars);
            bigramsFrequencyCollection = new Dictionary<string, double>();
            trigramsFrequencyCollection = new Dictionary<string,double>();
        }


        public Language(string Name, string currUnigarmFilePath, string currBigarmFilePath, string currTrigarmFilePath, int NumberOfChars, Dictionary<char, double> frequencyCollection)
        {
            languageNames = Name;
            languageUnigramFilePathes = currUnigarmFilePath;
            languageBigramFilePathes = currBigarmFilePath;
            languageTrigramFilePathes = currTrigarmFilePath;
            languageNumberOfChars = NumberOfChars;

            charFrequencyCollection = frequencyCollection;
            bigramsFrequencyCollection = new Dictionary<string, double>();
            trigramsFrequencyCollection = new Dictionary<string,double>();
        }


        public Language(string Name, string currUnigarmFilePath, string currBigarmFilePath, string currTrigarmFilePath, int NumberOfChars, Dictionary<char, double> frequencyCollection, Dictionary<string, double> bigramsCollection)
        {
            languageNames = Name;
            languageUnigramFilePathes = currUnigarmFilePath;
            languageBigramFilePathes = currBigarmFilePath;
            languageTrigramFilePathes = currTrigarmFilePath;
            languageNumberOfChars = NumberOfChars;

            charFrequencyCollection = frequencyCollection;
            bigramsFrequencyCollection = bigramsCollection;
            trigramsFrequencyCollection = new Dictionary<string, double>();
        }


        public Language(string Name, string currUnigarmFilePath, string currBigarmFilePath, string currTrigarmFilePath, int NumberOfChars, Dictionary<char, double> frequencyCollection, Dictionary<string, double> bigramsCollection, Dictionary<string, double> trigramsCollection)
        {
            languageNames = Name;
            languageUnigramFilePathes = currUnigarmFilePath;
            languageBigramFilePathes = currBigarmFilePath;
            languageTrigramFilePathes = currTrigarmFilePath;
            languageNumberOfChars = NumberOfChars;

            charFrequencyCollection = frequencyCollection;
            bigramsFrequencyCollection = bigramsCollection;
            trigramsFrequencyCollection = trigramsCollection;
        }
    }
}
