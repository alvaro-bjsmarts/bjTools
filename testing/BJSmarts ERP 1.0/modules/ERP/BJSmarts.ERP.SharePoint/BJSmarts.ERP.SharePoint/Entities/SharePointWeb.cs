using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.SharePoint;

namespace BJSmarts.ERP.SharePoint.Entities
{
    public class SharePointWeb : IWeb
    {
        public SharePointWeb()
			: this(SPContext.Current.Web)
		{
			// Stub.
		}


        public SharePointWeb(object web)
		{
			this.Context = web;
		}
		
		public object Context { get; private set; }

        public IUser CurrentUser
		{
			get
			{
				return new SharePointUser(((SPWeb)this.Context).CurrentUser);
			}
		}
    }
}
