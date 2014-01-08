
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.SharePoint;

namespace TestWebPartGenerationProject
{
  public interface IList
    {
        string Title { get; }

        IEnumerable<IField> Fields { get; }

        bool HasLanguageField { get; }

        bool HasDeleteField { get; }

        bool CheckInternalField(string sortfield);

        SPListItemCollection GetItems();
    }
}