using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Security;
using System.Web;

namespace AddMemberShipUser
{
    class Program
    {
        static void Main(string[] args)
        {


            MembershipProvider provider = Membership.Providers["ERP.FBA.MembershipProvider"];                        
            MembershipCreateStatus status = provider.CreateUser("", "", "", "", "", true, null, null);
        }
    }
}
