using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.SharePoint;

namespace BJSmarts.ERP.SharePoint.Entities
{
    public class SharePointUser : IUser
    {
        public SharePointUser()
			: this(SPContext.Current.Web.CurrentUser)
		{
			// Stub.
		}

        public SharePointUser(object user)
		{
			this.Context = user;
		}

		public object Context { get; private set; }

		public string Email
		{
			get
			{
				return ((SPUser)this.Context).Email;
			}
		}

		public int ID
		{
			get
			{
				return ((SPUser)this.Context).ID;
			}
		}

		public bool IsSiteAdmin
		{
			get
			{
				return ((SPUser)this.Context).IsSiteAdmin;
			}
		}

		public string Name
		{
			get
			{
				return ((SPUser)this.Context).Name;
			}
		}

		public string Notes
		{
			get
			{
				return ((SPUser)this.Context).Notes;
			}
		}

		public IEnumerable<IGroup> Groups
		{
			get
			{
				if (this.Context == null)
				{
					return new List<IGroup>();
				}

				return ((SPUser)this.Context).Groups.Cast<SPGroup>().Select<SPGroup, IGroup>(x => new SharePointGroup(x));
			}
		}

		public string LoginName
		{
			get
			{
				return ((SPUser)this.Context).LoginName;
			}
		}

        String AccountName = String.Empty;

        public bool IsEmployee
        {
            get
            {
                if (this.Groups.FirstOrDefault(x => x.Name == "ERP Portal Employees") != null)
                {
                    return true;

                }
                else
                {
                    return false;
                }                
            }
        }

        public bool IsSupervisor
        {
            get
            {
                if (this.Groups.FirstOrDefault(x => x.Name == "ERP Portal Supervisors") != null)
                {
                    return true;

                }
                else
                {
                    return false;
                }
            }
        }

        public bool IsSalesPerson
        {
            get
            {
                if (this.Groups.FirstOrDefault(x => x.Name == "ERP Portal Sales") != null)
                {
                    return true;

                }
                else
                {
                    return false;
                }
            }
        }

        public bool IsServicePerson
        {
            get
            {
                if (this.Groups.FirstOrDefault(x => x.Name == "ERP Portal Services") != null)
                {
                    return true;

                }
                else
                {
                    return false;
                }
            }
        }

        public bool IsMarketingPerson
        {
            get
            {
                if (this.Groups.FirstOrDefault(x => x.Name == "ERP Portal Marketing") != null)
                {
                    return true;

                }
                else
                {
                    return false;
                }
            }
        }
    }
}
