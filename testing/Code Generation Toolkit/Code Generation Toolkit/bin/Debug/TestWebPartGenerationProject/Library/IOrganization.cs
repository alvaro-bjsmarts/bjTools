
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TestWebPartGenerationProject
{
     public interface IOrganization
    {
        string ID { get; }

        string Name { get; }

        string Industry { get; }

        string IndustryId { get; }
    }
}
