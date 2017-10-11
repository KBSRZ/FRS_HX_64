using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAngine;

using Microsoft.VisualStudio.TestTools.UnitTesting;
namespace DataAngine.BLL.Tests
{
    [TestClass()]
    public class userTests
    {
        [TestMethod()]
        public void AddTest()
        {
            BLL.user usrbll = new BLL.user();
            Model.user usr = new Model.user();
            usr.feature_data = new byte[512];
            usr.face_image_path = "1.jpg";
            usrbll.Add(usr);
          
        }
    }
}
