using System.Net;

namespace Task8.Models
{
    public class APIResponse
    {
        public HttpStatusCode StatusCode { get; set; }

        public bool IsSuccess { get; set; }

        public List<string>? ErrorMessages { get; set; }

        public object? Result { get; set; }
    }
}
