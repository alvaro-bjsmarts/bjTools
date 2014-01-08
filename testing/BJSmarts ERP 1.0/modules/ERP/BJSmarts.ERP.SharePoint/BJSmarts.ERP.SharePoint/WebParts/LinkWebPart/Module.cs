using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;

namespace BJSmarts.ERP.SharePoint.WebParts.LinkWebPart
{
    [Serializable]
    public class Module
    {        

        #region Class Member Declarations

        private String _Title;
        private String _AlternativeTitle;
        private String _SubTitle;
        private String _AlternativeSubTitle;
        private String _SubModule;
        private Boolean _ShowModule = false;

        #endregion

        public Module()
        {

        }

        public Module(String Title, String AlternativeTitle, String SubTitle, String AlternativeSubTitle, String SubModule)
        {
            this.Title = Title;
            this.AlternativeTitle = AlternativeTitle;
            this.SubTitle = SubTitle;
            this.AlternativeSubTitle = AlternativeSubTitle;
            this.SubModule = SubModule;
        }

        #region Class Property Declarations

        public String Title
        {
            get
            {
                return _Title;
            }
            set
            {
                _Title = value;

            }
        }

        public String AlternativeTitle
        {
            get
            {
                return _AlternativeTitle;
            }
            set
            {
                _AlternativeTitle = value;

            }
        }

        public String SubTitle
        {
            get
            {
                return _SubTitle;
            }
            set
            {
                _SubTitle = value;

            }
        }

        public String AlternativeSubTitle
        {
            get
            {
                return _AlternativeSubTitle;
            }
            set
            {
                _AlternativeSubTitle = value;

            }
        }

        public String SubModule
        {
            get
            {
                return _SubModule;
            }
            set
            {
                _SubModule = value;

            }
        }

        public Boolean ShowModule
        {
            get
            {
                return _ShowModule;
            }
            set
            {
                _ShowModule = value;

            }
        }

        public bool IsEmployeeDirectoryModule
        {
            get
            {
                if (SubModule.Equals("Employee Directory"))
                {
                    return true;

                }
                else
                {
                    return false;
                }
            }
        }

        public bool IsEmployeeTimeSheetModule
        {
            get
            {
                if (SubModule.Equals("TimeSheets"))
                {
                    return true;

                }
                else
                {
                    return false;
                }
            }
        }

        public bool IsMarketingModule
        {
            get
            {
                if (SubModule.Equals("Marketing"))
                {
                    return true;

                }
                else
                {
                    return false;
                }
            }
        }

        public bool IsServiceModule
        {
            get
            {
                if (SubModule.Equals("Services"))
                {
                    return true;

                }
                else
                {
                    return false;
                }
            }
        }

        public bool IsSalesModule
        {
            get
            {
                if (SubModule.Equals("Sales"))
                {
                    return true;

                }
                else
                {
                    return false;
                }
            }
        }


        #endregion
    }

    [Serializable]
    public class ModuleCollection : CollectionBase
    {
        public Module this[int index]
        {
            get
            {
                return (Module)this.List[index];
            }
            set
            {
                this.List[index] = value;
            }
        }
        public void Add(Module obj)
        {
            this.List.Add(obj);
        }
    }
}
