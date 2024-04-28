using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IOTBackend.Shared.Enums
{
    public enum ActionStatus
    {
        Error,
        Success,
        Failed,
        NotFound,
        DuplicateEmail,
        Unauthorized,
        Exceeded,
    }
}
