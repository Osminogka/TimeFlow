using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TimeFlow.DAL.SideModels
{
    public class ResponseList<T>
    {
        public bool Success { get; set; } = false;
        public string Message { get; set; } = "Error occured";

        public List<T> Enum { get; set; } = new List<T>();
    }
}
