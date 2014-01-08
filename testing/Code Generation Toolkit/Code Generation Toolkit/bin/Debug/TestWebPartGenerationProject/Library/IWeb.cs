
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TestWebPartGenerationProject
{
    public interface IWeb
    {
        object Context { get; }

        IUser CurrentUser { get; }
    }
}
