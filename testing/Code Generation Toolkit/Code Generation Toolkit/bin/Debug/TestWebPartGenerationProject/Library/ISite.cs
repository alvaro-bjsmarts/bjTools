
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TestWebPartGenerationProject
{
    public interface ISite
    {
        string Url { get; }

        string Id { get; }

        IWeb OpenWeb();

        IWeb OpenWeb(Guid webId);
    }
}
