using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominate.Data.Model
{
    public class Exceptions
    {
        public int ExceptionsId { get; set; }
        public string? Message { get; set; }
        public string? StackTrace { get; set; }
        public string? ScreenName { get; set; }
        public DateTimeOffset Timestamp { get; set; }
    }
}
