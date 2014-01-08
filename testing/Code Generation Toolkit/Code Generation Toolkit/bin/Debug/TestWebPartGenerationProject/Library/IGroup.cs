
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TestWebPartGenerationProject
{
   public interface IGroup
    {
        string Name { get; }

        IEnumerable<IUser> Users { get; }

        void AddUser(IUser user);

        void AddUser(string loginName, string email, string name, string notes);
    }
}
