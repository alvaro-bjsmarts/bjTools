
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.SharePoint;

namespace TestWebPartGenerationProject
{
    public class SharePointField : IField
    {
        public SharePointField(object field)
		{
			this.Context = field;
		}

		public object Context { get; private set; }

        public string Title
		{
			get
			{
				return ((SPField)this.Context).Title;
			}
		}
    }
}
