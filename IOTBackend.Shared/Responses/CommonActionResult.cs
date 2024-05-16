using IOTBackend.Shared.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IOTBackend.Shared.Responses
{
    public class CommonActionResult<TEntity>
    {
        public ActionStatus Status { get; set; }
        public TEntity? Entity { get; set; }
        public CommonErrorResultDto? ErrorResult { get; set; }
    }

    public class CommonErrorResultDto
    {
        public Exception? exception { get; set; }
        public string customErrorCode { get; set; } = string.Empty;
    }
}
