using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LanguageRecognitionAPI;

namespace LanguageRecognitionServiceBll
{
    public interface IRecognitionProvider
    {
        string RecognizeLanguage(string sorceWord);
    }
}
