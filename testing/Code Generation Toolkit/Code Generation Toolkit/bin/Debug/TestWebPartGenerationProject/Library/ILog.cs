
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TestWebPartGenerationProject
{
   public interface ILog
    {
        void Error(string message, string StackTrace);
    }
}
