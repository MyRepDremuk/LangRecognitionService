using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LanguageRecognitionAPI;
using LanguageRecognitionServiceDAO;

namespace LanguageRecognitionServiceBll
{
    public class RecognitionProvider : IRecognitionProvider
    {
        private ILanguageRecognizer _recognizer;
        private IMemorizationDAO _memorizationDAO;

        public RecognitionProvider()
        {
            _recognizer = new LanguageRecognizer();
            _memorizationDAO = new MemorizationSQLiteDAO();
        }


        public string RecognizeLanguage(string sorceWord)
        {
            RecognitionResult result = null;

            if (_memorizationDAO.IsWordsRecognitionResultExist(sorceWord))
            {
                result = _memorizationDAO.LoadMemorizedRecognitionResult(sorceWord);
            }
            else
            {
                result = _recognizer.Recognize(sorceWord);
                _memorizationDAO.MemorizeRecognitionResult(result);
            }


            return result.ProbabilityString();
        }
    }
}
