
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.SharePoint;

namespace TestWebPartGenerationProject
{
   public class SharePointLanguageFactory : ILanguageFactory
    {
        public SharePointLanguage GetLanguage(int LCID)
        {            
            switch (LCID)
            {
                case 1033 :
                    return new SharePointEnglishLanguage();
                case 3082 :
                    return new SharePointSpanishLanguage();
                default :
                    return null;
            }
        }    
    }
}
