using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MySql.Data.MySqlClient;
using System.Data.OracleClient;

using  DataAngine.DBUtility;

namespace DataAngine.DAL
{
    /// <summary>
    /// hitalert:
	/// </summary>

    public partial class  hitalert
    {
        private readonly DataAngine.DAL.hitrecord hitrecorddal = new DataAngine.DAL.hitrecord();

        private readonly DataAngine.DAL.hitrecord_detail hitrecord_detaildal = new DataAngine.DAL.hitrecord_detail();

        #region  BasicMethod
        /// <summary>
        /// 添加一条数据
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public bool Add(DataAngine.Model.hitalert model)
        {        
            if (!hitrecorddal.Add(model.hit))
            {
                return false;
            }
            int hit_record_id = hitrecorddal.GetMaxId()-1;

            bool addState = true;

            for (int n = 0; n < model.details.Length; n++)
            {
                model.details[n].hit_record_id=hit_record_id;
                model.details[n].rank = n;
                addState = hitrecord_detaildal.Add(model.details[n]);
                
               if(false == addState)
               {
                   break;
               }
            }

            return addState;


/*
            int rows = 0;

            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into TH_FACE_HITRECORD_DETAIL(");
            strSql.Append("hit_record_id,user_id,rank,score)");
            strSql.Append(" values (");
            strSql.Append(":hit_record_id,:user_id,:rank,:score)");

            OracleParameter[] parameters = {
					new OracleParameter(":hit_record_id", OracleType.Int32),
					new OracleParameter(":user_id", OracleType.Int32),
					new OracleParameter(":rank", OracleType.Int32),
					new OracleParameter(":score", OracleType.Float)};

            for (int n = 0; n < model.details.Length; n++)
            {
                parameters[0].Value = hit_record_id;
                parameters[1].Value = model.details[n].user_id;
                parameters[2].Value = n;
                parameters[3].Value = model.details[n].score;

                rows += DbHelperOracle.ExecuteNonQuery(CommandType.Text, strSql.ToString(), parameters);
            }

            if (rows > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
 */
           
        }


        public bool Update(DataAngine.Model.hitalert model)
        {
            DateTime dateNow = DateTime.Now;
            DateTime dateStart = new DateTime(dateNow.Year, dateNow.Month, dateNow.Day, 6, 0, 0);
            DateTime dateEnd = new DateTime(dateNow.Year, dateNow.Month, dateNow.Day, 24, 0, 0);

            bool addState = true;

            for (int n = 0; n < model.details.Length; n++)
            {
               List<DataAngine.Model.hitrecord_detail> hitDetails = hitrecord_detaildal.GetModelByHitUserId(model.details[n].user_id);
               
               foreach(DataAngine.Model.hitrecord_detail hitDetail in hitDetails)
               {
                  DataAngine.Model.hitrecord hitRecord = hitrecorddal.GetModel(hitDetail.hit_record_id);

                  DateTime occureTime = hitRecord.occur_time;

                  if (occureTime.CompareTo(dateStart) >= 0 && occureTime.CompareTo(dateEnd) <=0)
                  {
                      hitDetail.rank = model.details[n].rank;
                      hitDetail.score = model.details[n].score;

                      hitrecord_detaildal.Update(hitDetail);

                      addState = true;

                  }

               }

                if (false == addState)
                {
                    break;
                }
            }


            if(addState)
            {


            }


            return addState;

        }


        /// <summary>
        /// 获得数据列表
        /// </summary>
        public DataSet GetList(string strWhere)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select * ");
            strSql.Append(" FROM hitalert ");
            if (strWhere.Trim() != "")
            {
                strSql.Append(" where " + strWhere);
            }
            return DbHelperMySQL.Query(strSql.ToString());
        }

        /// <summary>
        /// 获得数据列表
        /// </summary>
        public DataSet GetList(string strWhere, string library)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select * ");
            strSql.Append(" FROM hitalert ");
            if (strWhere.Trim() != "")
            {
                strSql.Append(" where " + strWhere);
            }
            return DbHelperMySQL.Query(strSql.ToString(), library);
        }

        /// <summary>
        /// 分页获得数据列表
        /// </summary>
        public DataSet GetList(string strWhere, int startIndex, int pageSize)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select * ");
            strSql.Append(" FROM hitalert ");
            if (strWhere.Trim() != "")
            {
                strSql.Append(" where " + strWhere);
            }
            strSql.Append(" limit " + startIndex + ", " + pageSize);

            return DbHelperMySQL.Query(strSql.ToString());
        }

        /// <summary>
        /// 分页获得数据列表
        /// </summary>
        public DataSet GetList(string strWhere, int startIndex, int pageSize,string library)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select * ");
            strSql.Append(" FROM hitalert ");
            if (strWhere.Trim() != "")
            {
                strSql.Append(" where " + strWhere);
            }
            strSql.Append(" limit " + startIndex + ", " + pageSize);

            return DbHelperMySQL.Query(strSql.ToString(), library);
        }

        /// <summary>
        /// 获取记录总数
        /// </summary>
        public int GetRecordCount(string strWhere)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(1) FROM hitalert ");
            if (strWhere.Trim() != "")
            {
                strSql.Append(" where " + strWhere);
            }
            object obj = DbHelperSQL.GetSingle(strSql.ToString());
            if (obj == null)
            {
                return 0;
            }
            else
            {
                return Convert.ToInt32(obj);
            }
        }
        /// <summary>
        /// 分页获取数据列表
        /// </summary>
        public DataSet GetListByPage(string strWhere, string orderby, int startIndex, int endIndex)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("SELECT * FROM ( ");
            strSql.Append(" SELECT ROW_NUMBER() OVER (");
            if (!string.IsNullOrEmpty(orderby.Trim()))
            {
                strSql.Append("order by T." + orderby);
            }
            else
            {
                strSql.Append("order by T. desc");
            }
            strSql.Append(")AS Row, T.*  from hitalert T ");
            if (!string.IsNullOrEmpty(strWhere.Trim()))
            {
                strSql.Append(" WHERE " + strWhere);
            }
            strSql.Append(" ) TT");
            strSql.AppendFormat(" WHERE TT.Row between {0} and {1}", startIndex, endIndex);
            return DbHelperMySQL.Query(strSql.ToString());
        }
        #endregion  BasicMethod
        #region  ExtensionMethod
        /// <summary>
        /// 通过时间得到对象实体
        /// </summary>
        /// 
        //public List<DataAngine.Model.hitalert> GetModelByTime(DateTime startTime, DateTime endTime)
        //{
        //    List<DataAngine.Model.hitalert> modelList = new List<Model.hitalert>();
        //    StringBuilder strSql = new StringBuilder();
        //    strSql.Append("select id,face_query_image_path,threshold,occur_time,detail_id,rank,score,user_id,user_name,user_gender,user_face_image_path,user_type,user_create_time,user_modified_time,user_quality_score ");
        //    strSql.Append(" FROM hitalert ");
        //    strSql.Append(" where occur_time between @start_ime and @end_time");
        //    MySqlParameter[] parameters = {
        //            new MySqlParameter("@start_ime", MySqlDbType.DateTime),
        //            new MySqlParameter("@end_time", MySqlDbType.DateTime)
        //    };
        //    parameters[0].Value = startTime;
        //    parameters[1].Value = endTime;
        //    DataAngine.Model.hitrecord model = new DataAngine.Model.hitrecord();
        //    DataSet ds = DbHelperMySQL.Query(strSql.ToString(), parameters);

        //    DataTable dt = ds.Tables[0];
        //    List<DataTable> newTables = new List<DataTable>();
        //    HashSet<int> groupIds = new HashSet<int>();
        //    foreach (DataRow row in dt.Rows)
        //    {
        //        int groupId = int.Parse(row["id"].ToString());
        //        if (!groupIds.Contains(groupId))
        //        {
        //            groupIds.Add(groupId);
        //            DataTable newTable = dt.Clone();
        //            newTable.TableName = groupId.ToString();
        //            newTable.ImportRow(row);
        //            newTables.Add(newTable);
        //        }
        //        else
        //        {
        //            DataTable newTable = newTables.Find(x => x.TableName == groupId.ToString());
        //            newTable.ImportRow(row);
        //        }
        //    }

        //    foreach (var table in newTables)
        //    {
        //        modelList.Add(DataTableToModel(table));
        //    }
        //    return modelList;

        //}
        public DataSet GetModelByTime(DateTime startTime, DateTime endTime)
        {
            
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select id,face_query_image_path,threshold,occur_time,detail_id,rank,score,user_id,user_name,user_gender,user_face_image_path,user_type,user_create_time,user_modified_time,user_quality_score ");
            strSql.Append(" FROM hitalert ");
            strSql.Append(" where occur_time between @start_ime and @end_time");
            MySqlParameter[] parameters = {
					new MySqlParameter("@start_ime", MySqlDbType.DateTime),
                    new MySqlParameter("@end_time", MySqlDbType.DateTime)
			};
            parameters[0].Value = startTime;
            parameters[1].Value = endTime;
            DataAngine.Model.hitrecord model = new DataAngine.Model.hitrecord();
            DataSet ds = DbHelperMySQL.Query(strSql.ToString(), parameters);

            return ds;

        }


        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public Model.hitalert DataTableToModel(DataTable table)
        {
            Model.hitalert model = new Model.hitalert();
            model.details = new Model.hitrecord_detail[table.Rows.Count];
            model.hit = new Model.hitrecord();
            if (table != null)
            {
                if (table.Rows[0]["id"] != null && table.Rows[0]["id"].ToString() != "")
                {
                    model.hit.id = int.Parse(table.Rows[0]["id"].ToString());
                }
                if (table.Rows[0]["face_query_image_path"] != null)
                {
                    model.hit.face_query_image_path = table.Rows[0]["face_query_image_path"].ToString();
                }
                if (table.Rows[0]["threshold"] != null && table.Rows[0]["threshold"].ToString() != "")
                {
                    model.hit.threshold = float.Parse(table.Rows[0]["threshold"].ToString());
                }
                if (table.Rows[0]["occur_time"] != null && table.Rows[0]["occur_time"].ToString() != "")
                {
                    model.hit.occur_time = DateTime.Parse(table.Rows[0]["occur_time"].ToString());
                }
                for (int n = 0; n < table.Rows.Count; n++)
                {
                    Model.hitrecord_detail detail = new Model.hitrecord_detail();
                    if (table.Rows[n]["detail_id"] != null && table.Rows[n]["detail_id"].ToString() != "")
                    {
                        detail.id = int.Parse(table.Rows[n]["detail_id"].ToString());
                    }
                    if (table.Rows[n]["rank"] != null && table.Rows[n]["rank"].ToString() != "")
                    {
                        detail.rank = int.Parse(table.Rows[n]["rank"].ToString());
                    }
                    if (table.Rows[n]["score"] != null && table.Rows[n]["score"].ToString() != "")
                    {
                        detail.score = float.Parse(table.Rows[n]["score"].ToString());
                    }
                    if (table.Rows[n]["user_id"] != null && table.Rows[n]["user_id"].ToString() != "")
                    {
                        detail.user_id = int.Parse(table.Rows[n]["user_id"].ToString());
                    }
                    model.details[n] = detail;
                }
            }

            return model;
        }
       
        #endregion  ExtensionMethod

    }
}
