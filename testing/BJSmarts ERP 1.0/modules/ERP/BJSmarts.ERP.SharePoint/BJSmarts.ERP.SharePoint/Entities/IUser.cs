﻿
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BJSmarts.ERP.SharePoint.Entities
{
    public interface IUser
    {
        object Context { get; }

        string Email { get; }

        int ID { get; }

        bool IsSiteAdmin { get; }

        string Name { get; }

        string Notes { get; }

        IEnumerable<IGroup> Groups { get; }

        string LoginName { get; }
    }
}