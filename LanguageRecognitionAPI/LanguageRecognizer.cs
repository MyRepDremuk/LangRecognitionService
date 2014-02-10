using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LanguageRecognitionAPI
{
    public class LanguageRecognizer : ILanguageRecognizer
    {
        
        //dynamics params

        public const int LENTHOFBIGRAMMS = 2;
        public const int LENTHOFTRIGRAMMS = 3;

        private const double UNIGRAMSRESULTVEIGHT = 0.2;
        private const double BIGRAMSRESULTVEIGHT = 0.3;
        private const double TRIGRAMSRESULTVEIGHT = 0.5;

        private List<Language> _languagesCollection;
        private int _maxBase;
        private string _currStorage;


        public LanguageRecognizer()
        {
            ILanguagesFilesParser fileParser = new LanguagesFilesParser();
            _languagesCollection = fileParser.ParseLangugesFromConfig(LanguagesFilesParser.XMLFILE, ref _maxBase);
            _currStorage = LanguagesFilesParser.XMLFILE;
        }

        public RecognitionResult Recognize(string sourceString)
        {
            RecognitionResult result = null;
            
            switch (_currStorage)
            {
                case LanguagesFilesParser.TEXTFILE:
                    //resultString = RibbonRecognitionAlgorithm(sourceString.ToLower());
                    break;
                case LanguagesFilesParser.XMLFILE:
                    result = FrequincyRecognitionAlgorithm(sourceString.ToLower());
                    break;
            }
            return result;
        }


        private string RibbonRecognitionAlgorithm (string sourceString)
        {
            List<double> sumPoints = new List<double>(_languagesCollection.Count);
            for (int i = 0; i < sumPoints.Capacity; i++)
            {
                sumPoints.Add(0);
            }

            foreach (char currChar in sourceString)
            {
                for (int i = 0; i < sumPoints.Capacity; i++)
                {
                    if (_languagesCollection[i].charFrequencyCollection.Keys.Contains(currChar))
                    {
                        sumPoints[i] += (_maxBase - _languagesCollection[i].charFrequencyCollection[currChar]) / _maxBase;
                    }
                }
            }
            return _languagesCollection[sumPoints.IndexOf(sumPoints.Max())].languageNames;
        }


        private RecognitionResult FrequincyRecognitionAlgorithm(string sourceString)
        {
            List<double> sumPointsUnigrams = UnigramsSumCount(sourceString);

            List<double> sumPointsBigrams = BigramsSumCount(sourceString);

            List<double> sumPointsTrigrams = TrigramsSumCount(sourceString);

            RecognitionResult result = CountAllVerifies(sumPointsUnigrams, sumPointsBigrams, sumPointsTrigrams);
            result.Word = sourceString;
            return result;
        }


        private List<double> UnigramsSumCount(string sourceString)
        {
            List<double> sumPointsUnigrams = new List<double>(_languagesCollection.Count);

            for (int i = 0; i < sumPointsUnigrams.Capacity; i++)
            {
                sumPointsUnigrams.Add(0);
            }

            foreach (char currChar in sourceString)
            {
                for (int i = 0; i < sumPointsUnigrams.Capacity; i++)
                {
                    if (_languagesCollection[i].charFrequencyCollection.Keys.Contains(currChar))
                    {
                        sumPointsUnigrams[i] += _languagesCollection[i].charFrequencyCollection[currChar];
                    }
                }
            }

            return sumPointsUnigrams;
        }


        private List<double> BigramsSumCount(string sourceString)
        {
            List<double> sumPointsBigrams = new List<double>(_languagesCollection.Count);
            for (int i = 0; i < sumPointsBigrams.Capacity; i++)
            {
                sumPointsBigrams.Add(0);
            }

            for (int i = 0; i < sourceString.Length - 1; i++)
            {
                string currSubstring = sourceString.Substring(i, LENTHOFBIGRAMMS);
                for (int j = 0; j < sumPointsBigrams.Capacity; j++)
                {

                    if (_languagesCollection[j].bigramsFrequencyCollection.Keys.Contains(currSubstring))
                    {
                        sumPointsBigrams[j] += _languagesCollection[j].bigramsFrequencyCollection[currSubstring];
                    }
                }
            }

            return sumPointsBigrams;
        }


        private List<double> TrigramsSumCount(string sourceString)
        {
            List<double> sumPointsTrigrams = new List<double>(_languagesCollection.Count);
            for (int i = 0; i < sumPointsTrigrams.Capacity; i++)
            {
                sumPointsTrigrams.Add(0);
            }

            for (int i = 0; i < sourceString.Length - 2; i++)
            {
                string currSubstring = sourceString.Substring(i, LENTHOFTRIGRAMMS);
                for (int j = 0; j < sumPointsTrigrams.Capacity; j++)
                {

                    if (_languagesCollection[j].trigramsFrequencyCollection.Keys.Contains(currSubstring))
                    {
                        sumPointsTrigrams[j] += _languagesCollection[j].trigramsFrequencyCollection[currSubstring];
                    }
                }
            }
            return sumPointsTrigrams;
        }


        private RecognitionResult CountAllVerifies(List<double> sumPointsUnigrams, List<double> sumPointsBigrams, List<double>  sumPointsTrigrams)
        {
            RecognitionResult result = new RecognitionResult();

            double totalUnigramsSumFrequency = 0;
            double totalBigramsSumFrequency = 0;
            double totalTrigramsSumFrequency = 0;

            foreach (double sumPoint in sumPointsUnigrams)
            {
                totalUnigramsSumFrequency += sumPoint;
            }

            foreach (double sumPoint in sumPointsBigrams)
            {
                totalBigramsSumFrequency += sumPoint;
            }

            foreach (double sumPoint in sumPointsTrigrams)
            {
                totalTrigramsSumFrequency += sumPoint;
            }

            StringBuilder resultString = new StringBuilder();

            for (int i = 0; i < sumPointsUnigrams.Count; i++)
            {
                double resUnigrams = (totalUnigramsSumFrequency > 0)? Math.Round(100 * sumPointsUnigrams[i] / totalUnigramsSumFrequency, 2) : 0;
                double resBigrams = (totalBigramsSumFrequency > 0) ? Math.Round(100 * sumPointsBigrams[i] / totalBigramsSumFrequency, 2) : 0;
                double resTrigrams = (totalTrigramsSumFrequency > 0) ? Math.Round(100 * sumPointsTrigrams[i] / totalTrigramsSumFrequency, 2) : 0;

                result.LangProbability.Add(_languagesCollection[i].languageNames, ResultFrequencyCount( resUnigrams, resBigrams, resTrigrams));
            }

            return result;
        }


        private double ResultFrequencyCount(double resUnigrams, double resBigrams, double resTrigrams)
        {
            return UNIGRAMSRESULTVEIGHT * resUnigrams
                    + BIGRAMSRESULTVEIGHT * resBigrams + TRIGRAMSRESULTVEIGHT * resTrigrams;
        }
    
    //EOC
    }
}
