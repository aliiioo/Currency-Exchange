using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Dtos.ErrorsDtos
{
    public class ResultDto
    {
        public bool Result { get; set; }=false;
        public string ErrorMessage { get; set; }=string.Empty;
    }
}
