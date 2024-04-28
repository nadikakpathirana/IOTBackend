using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IOTBackend.Shared.Responses
{
    public class CommonErrorResultDto
    {
        public Exception exception { get; set; }
        public string customErrorCode { get; set; }
    }
}
