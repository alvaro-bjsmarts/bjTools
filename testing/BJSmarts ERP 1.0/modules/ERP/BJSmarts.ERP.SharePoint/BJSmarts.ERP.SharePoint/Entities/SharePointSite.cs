using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.SharePoint;

namespace BJSmarts.ERP.SharePoint.Entities
{
    public class SharePointSite : ISite, IDisposable
    {
        public SharePointSite()
			: this(SPContext.Current.Site)
		{
			// Stub.
		}

        public SharePointSite(object context)
		{
			var typeOf = context.GetType();
			if (typeOf == typeof(Guid))
			{
				this.Context = new SPSite((Guid)context);
			}
			else if (typeOf == typeof(string))
			{
				this.Context = new SPSite((string)context);
			}
			else
			{
				this.Context = context;
			}
		}

        public object Context { get; private set; }

        public string Url
        {
            get
            {
                return ((SPSite)this.Context).Url;
            }
        }

        public string Id
        {
            get
            {
                return ((SPSite)this.Context).ID.ToString();
            }
        }

        public IWeb OpenWeb()
        {
            return new SharePointWeb(((SPSite)this.Context).OpenWeb());
        }

        public IWeb OpenWeb(Guid webId)
        {
            return new SharePointWeb(((SPSite)this.Context).OpenWeb(webId));
        }

        void IDisposable.Dispose()
        {
            if (this.Context != null)
            {
                ((SPSite)this.Context).Dispose();
            }
        }
    }
}
