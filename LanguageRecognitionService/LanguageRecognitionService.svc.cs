using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LanguageRecognitionServiceBll;
using System.Configuration;
using System.Reflection;

namespace LanguageRecognitionService
{
    
    public class LanguageRecognitionService : ILanguageRecognitionService
    {

        private IRecognitionProvider _provider;

        public LanguageRecognitionService()
        {
            _provider = new RecognitionProvider();
        }


        public string Recognize(string sourceString)
        {
            return _provider.RecognizeLanguage(sourceString);
        }
    }
}
