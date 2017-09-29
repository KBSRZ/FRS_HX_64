using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MySql.Data.MySqlClient;
using DataAngine.DBUtility;//Please add references


namespace DataAngine.DAL
{
    public partial class statistics
    {
        public bool Update(DataAngine.Model.statistics model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update statistics set ");
            strSql.Append("startTime=@startTime,");
            strSql.Append("catchFaceImgCount=@catchFaceImgCount,");
            strSql.Append("matchFaceCount=@matchFaceCount,");
            strSql.Append("endTime=@endTime");
            strSql.Append(" where id=@id");
            MySqlParameter[] parameters = {
                    new MySqlParameter("@startTime", MySqlDbType.DateTime),
                    new MySqlParameter("@catchFaceImgCount", MySqlDbType.Int32, 255),
                    new MySqlParameter("@matchFaceCount", MySqlDbType.Int32, 255),				
					new MySqlParameter("@endTime", MySqlDbType.DateTime),
                    new MySqlParameter("@id", MySqlDbType.Int32,11)};

            parameters[0].Value = model.StartTime;
            parameters[1].Value = model.CatchFaceImgCount;
            parameters[2].Value = model.MatchFaceCount;
            parameters[3].Value = model.EndTime;
            parameters[4].Value = model.Id;

            int rows = DbHelperMySQL.ExecuteSql(strSql.ToString(), parameters);
            if (rows > 0)
            {
                return true;
            }
            else
            {
                return false;
            }

        }
    }
}
