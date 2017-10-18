using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DataAngineTest
{
    class Program
    {
        static void userAddTest()
        {
            DataAngine.BLL.user usrbll = new DataAngine.BLL.user();
            DataAngine.Model.user usr = new DataAngine.Model.user();
            usr.card_id = "32145123451";
            usr.face_image_path = "E:/11i.jpg";
            usr.feature_data = new byte[2048];
            usr.gender = "nan";
            usr.people_id = "1";
            usr.type = "2";
            usr.quality_score = 1.2f;
            usrbll.Add(usr);
        }

        //static void libraryAddTest()
        //{
        //    DataAngine.BLL.table tablebll = new DataAngine.BLL.table();
        //    DataAngine.Model.table table = new DataAngine.Model.table();
        //    table.name = "test";

        //    tablebll.Add(table);
        //}
        static void hitrecordAddTest()
        {
            DataAngine.BLL.hitrecord habll = new DataAngine.BLL.hitrecord();
            DataAngine.Model.hitrecord hit = new DataAngine.Model.hitrecord();
            hit.threshold = 0.6f;
            hit.face_query_image_path = "D:/1.jpg";
            hit.occur_time = DateTime.Now;
            habll.Add(hit);

        }
        static void hitalertAddTest()
        {

            DataAngine.BLL.hitalert habll = new DataAngine.BLL.hitalert();
            DataAngine.Model.hitrecord_detail hd1 = new DataAngine.Model.hitrecord_detail();
            DataAngine.Model.hitrecord_detail hd2 = new DataAngine.Model.hitrecord_detail();
            DataAngine.Model.hitalert ha = new DataAngine.Model.hitalert();
            DataAngine.Model.hitrecord hit = new DataAngine.Model.hitrecord();
            hit.threshold = 0.6f;
            hit.face_query_image_path = "D:/1.jpg";
            hit.occur_time = DateTime.Now;
            hd1.rank = 1;
            hd1.score = 0.867f;
            hd2.user_id = 1;
            hd2.rank = 2;
            hd2.score = 0.8f;
            hd2.user_id = 1;
            ha.details = new DataAngine.Model.hitrecord_detail[2];
            ha.details[0] = hd1;
            ha.details[1] = hd2;
            ha.hit = hit;
            habll.Add(ha);

        }
        static void Main(string[] args)
        {
            userAddTest();
            //hitalertAddTest();
        }
    }
}
