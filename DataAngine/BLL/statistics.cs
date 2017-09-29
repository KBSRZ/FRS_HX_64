using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DataAngine.Model;

namespace DataAngine.BLL
{
    public partial class statistics
    {
        private readonly DataAngine.DAL.statistics st = new DataAngine.DAL.statistics();
        public bool Update(DataAngine.Model.statistics model)
        {
            return st.Update(model);
        }

    }
}
