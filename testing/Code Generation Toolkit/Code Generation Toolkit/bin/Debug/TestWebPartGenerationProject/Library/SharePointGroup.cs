
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.SharePoint;

namespace TestWebPartGenerationProject
{
     public class SharePointGroup : IGroup
    {
        public SharePointGroup(object context)
		{
			this.Context = context;
		}

		public object Context { get; private set; }

		public int ID
		{
			get
			{
                return ((SharePointGroup)this.Context).ID;
			}
		}

		public string LoginName
		{
			get
			{
                return ((SharePointGroup)this.Context).LoginName;
			}
		}

		public string Name
		{
			get
			{
                return ((SharePointGroup)this.Context).Name;
			}
		}

		public IEnumerable<IUser> Users
		{
			get
			{
                return (IEnumerable<IUser>)((SharePointGroup)this.Context).Users.Cast<SPUser>().Select(x => new SharePointUser(x));
			}
		}

        public void AddUser(IUser user)
        {
            ((SPGroup)this.Context).AddUser((SPUser)user.Context);
        }

        public void AddUser(string loginName, string email, string name, string notes)
        {
            ((SPGroup)this.Context).Users.Add(loginName, email, name, notes);
        }
    }
}
