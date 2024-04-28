using IOTBackend.Shared.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IOTBackend.Shared.Responses
{
    public class CommonActionResult
    {
        public ActionStatus Status { get; set; }
        public string Message { get; set; }
        public CommonErrorResultDto ErrorResult { get; set; }
        public List<string> ErrorMessages { get; set; }
    }
}
