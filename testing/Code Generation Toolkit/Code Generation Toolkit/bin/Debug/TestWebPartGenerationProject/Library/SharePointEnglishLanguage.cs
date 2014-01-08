
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.SharePoint;

namespace TestWebPartGenerationProject
{
     public class SharePointEnglishLanguage : SharePointLanguage
    {
        public override int LCID
        {
            get
            {
                return 0;
            }
        }
    }
}
