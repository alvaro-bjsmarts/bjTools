
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.SharePoint;

namespace TestWebPartGenerationProject
{
     public class SharePointList : IList
    {

        public SharePointList(string listname, int LCID, string sortField)
            : this(SPContext.Current.Web.Lists[listname], LCID, sortField)
        {

        }

        public SharePointList(object context, int LCID, string sortField)
		{
			this.Context = context;
            this.LCID = LCID;
            this.sortField = sortField;            
		}

		public object Context { get; private set; }
        public int LCID { get; set; }
        public string sortField { get; set; }

        public string Title
        {
            get
            {
                return ((SPList)this.Context).Title;
            }
        }

        public IEnumerable<IField> Fields
        {
            get
            {
                if (this.Context == null)
                {
                    return new List<IField>();
                }    
            
                return ((SPList)this.Context).Fields.Cast<SPField>().Select<SPField, IField>(x => new SharePointField(x));
            }
        }

        public bool HasOrganizationField
        {
            get
            {
                if (this.Fields.FirstOrDefault(x => x.Title == "Organization") != null)
                {
                    return true;

                }
                else
                {
                    return false;
                }
            }
        }

        public bool HasLanguageField 
        { 
            get 
            {
                if (this.Fields.FirstOrDefault(x => x.Title == "Language") != null)
                {
                    return true;

                }
                else
                {
                    return false;
                }      
            }
        }

        public bool HasDeleteField 
        { 
            get 
            {
                if (this.Fields.FirstOrDefault(x => x.Title == "Deleted") != null)
                {
                    return true;

                }
                else
                {
                    return false;
                } 
            }
        }

        public bool CheckInternalField(string sortfield)
        {
            if (this.Fields.FirstOrDefault(x => x.Title == sortfield) != null)
            {
                return true;

            }
            else
            {
                return false;
            } 
        }

        public SPListItemCollection GetItems()
        {
            SPListItemCollection items = null;            

            SPQuery query = new SPQuery();


            if (this.HasOrganizationField)
            {
                query.Query = "<Where><And><Eq><FieldRef Name='OrganizationId'/><Value Type='Number'>" + SharePointContext.Current.Organization.ID + "</Value></Eq><Eq><FieldRef Name='Deleted'/><Value Type='Number'>0</Value></Eq></And></Where>";
                items = ((SPList)this.Context).GetItems(query);                
            }
            else
            {

                if (this.HasLanguageField)
                {
                    if (this.HasDeleteField)
                    {
                        if (this.CheckInternalField(this.sortField))
                        {
                            query.Query = "<Where><And><Eq><FieldRef Name='Language'/><Value Type='Number'>" + this.LCID + "</Value></Eq><Eq><FieldRef Name='Deleted'/><Value Type='Number'>0</Value></Eq></And></Where>" + "<OrderBy><FieldRef Ascending='TRUE' Name='" + this.sortField + "'/></OrderBy>";
                            items = ((SPList)this.Context).GetItems(query);
                        }
                        else
                        {
                            query.Query = "<Where><And><Eq><FieldRef Name='Language'/><Value Type='Number'>" + this.LCID + "</Value></Eq><Eq><FieldRef Name='Deleted'/><Value Type='Number'>0</Value></Eq></And></Where>";
                            items = ((SPList)this.Context).GetItems(query);
                        }
                    }
                    else
                    {
                        if (this.CheckInternalField(this.sortField))
                        {
                            query.Query = "<Where><Eq><FieldRef Name='Language'/><Value Type='Number'>" + this.LCID + "</Value></Eq></Where>" + "<OrderBy><FieldRef Ascending='TRUE' Name='" + sortField + "'/></OrderBy>";
                            items = ((SPList)this.Context).GetItems(query);
                        }
                        else
                        {
                            query.Query = query.Query = "<Where><Eq><FieldRef Name='Language'/><Value Type='Number'>" + this.LCID + "</Value></Eq></Where>";
                            items = ((SPList)this.Context).GetItems(query);
                        }
                    }
                }
                else
                {
                    if (this.HasDeleteField)
                    {
                        if (this.CheckInternalField(this.sortField))
                        {
                            query.Query = query.Query = "<Where><Eq><FieldRef Name='Deleted'/><Value Type='Number'>0</Value></Eq></Where>" + "<OrderBy><FieldRef Ascending='TRUE' Name='" + sortField + "'/></OrderBy>";
                            items = ((SPList)this.Context).GetItems(query);
                        }
                        else
                        {
                            query.Query = query.Query = "<Where><Eq><FieldRef Name='Deleted'/><Value Type='Number'>0</Value></Eq></Where>";
                            items = ((SPList)this.Context).GetItems(query);
                        }
                    }
                    else
                    {
                        if (this.CheckInternalField(this.sortField))
                        {
                            query.Query = query.Query = "" + "<OrderBy><FieldRef Ascending='TRUE' Name='" + this.sortField + "'/></OrderBy>";
                            items = ((SPList)this.Context).GetItems(query);
                        }
                        else
                        {
                            items = ((SPList)this.Context).GetItems();
                        }
                    }
                }

            }

            return items;
        }        
    }
}
