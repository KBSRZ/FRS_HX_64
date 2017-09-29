using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DataAngine.Model
{

    public partial class hitalert
    {
        public hitrecord hit { get; set; }
        public hitrecord_detail[] details { get; set; }
    }
}
