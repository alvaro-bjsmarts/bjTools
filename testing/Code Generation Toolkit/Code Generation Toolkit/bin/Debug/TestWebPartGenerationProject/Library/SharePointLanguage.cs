
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.SharePoint;

namespace TestWebPartGenerationProject
{
     public abstract class SharePointLanguage : ILanguage
    {
        public abstract int LCID { get; }
    }
}
