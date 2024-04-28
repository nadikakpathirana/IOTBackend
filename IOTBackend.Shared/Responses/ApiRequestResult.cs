using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IOTBackend.Shared.Responses
{
    public class ApiRequestResult<T>
    {
        public bool Status { get; set; } = false;
        public string Message { get; set; } = string.Empty;
        public T? Data { get; set; }
        public ErrorDetails? ErrorResult { get; set; }
    }

    public class ErrorDetails
    {
        public List<string> ErrorMessages { get; set; } = new List<string>();
        public List<string> Headers { get; set; } = new List<string>();
    }
}
