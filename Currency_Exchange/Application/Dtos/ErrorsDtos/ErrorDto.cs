using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Dtos.ErrorsDtos
{
    public class ResultDto
    {
        public bool IsSucceeded { get; set; }=false;
        public string Message { get; set; }=string.Empty;
    }
}
