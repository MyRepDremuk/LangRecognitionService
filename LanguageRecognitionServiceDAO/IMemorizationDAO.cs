using LanguageRecognitionAPI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LanguageRecognitionServiceDAO
{
    public interface IMemorizationDAO
    {
        bool IsWordsRecognitionResultExist(string currentWord);

        void MemorizeRecognitionResult(RecognitionResult currentResult);
        
        RecognitionResult LoadMemorizedRecognitionResult(string currentString);
    }
}
