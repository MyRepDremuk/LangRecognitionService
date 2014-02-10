using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LanguageRecognitionAPI
{
    public interface ILanguagesFilesParser
    {
        List<Language> ParseLangugesFromConfig(string storageType, ref int maxBase);
    }
}
