
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TestWebPartGenerationProject
{
    public interface ILanguageFactory
    {
        SharePointLanguage GetLanguage(int LCID);
    }
}
