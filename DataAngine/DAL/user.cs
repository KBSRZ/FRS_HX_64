using System;
using System.Data;
using System.Text;
using System.Windows.Forms;
using MySql.Data.MySqlClient;
using System.Data.OracleClient;

using DataAngine.DBUtility;//Please add references

namespace DataAngine.DAL
{
    /// <summary>
    /// 数据访问类:user
    /// </summary>
    public partial class user
    {
        public user()
        { }
        #region  BasicMethod

        /// <summary>
        /// 得到最大ID
        /// </summary>
        public int GetMaxId()
        {
            return DbHelperMySQL.GetMaxID("id", "user");
        }

        /// <summary>
        /// 是否存在该记录
        /// </summary>
        public bool Exists(int id)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(1) from user");
            strSql.Append(" where id=@id");
            MySqlParameter[] parameters = {
					new MySqlParameter("@id", MySqlDbType.Int32)
			};
            parameters[0].Value = id;

            return DbHelperMySQL.Exists(strSql.ToString(), parameters);
        }


        /// <summary>
        /// 增加一条数据
        /// </summary>
        public bool Add(DataAngine.Model.user model)
        {
            StringBuilder strSql = new StringBuilder();


            strSql.Append("insert into user(");
            strSql.Append("people_id,name,gender,card_id,image_id,face_image_path,feature_data,type,create_time,modified_time,quality_score)");
            strSql.Append(" values (");
            strSql.Append("@people_id,@name,@gender,@card_id,@image_id,@face_image_path,@feature_data,@type,@create_time,@modified_time,@quality_score)");
            MySqlParameter[] parameters = {
                    new MySqlParameter("@people_id", MySqlDbType.VarChar,50),
					new MySqlParameter("@name", MySqlDbType.VarChar,50),
					new MySqlParameter("@gender", MySqlDbType.VarChar,5),
                    new MySqlParameter("@card_id", MySqlDbType.VarChar, 50),
                    new MySqlParameter("@image_id", MySqlDbType.VarChar, 60),
					new MySqlParameter("@face_image_path", MySqlDbType.VarChar,200),
					new MySqlParameter("@feature_data", MySqlDbType.LongBlob),
					new MySqlParameter("@type", MySqlDbType.VarChar,1),
					new MySqlParameter("@create_time", MySqlDbType.DateTime),
					new MySqlParameter("@modified_time", MySqlDbType.DateTime),
					new MySqlParameter("@quality_score", MySqlDbType.Float)};

            parameters[0].Value = model.people_id;
            parameters[1].Value = model.name;
            parameters[2].Value = model.gender;
            parameters[3].Value = model.card_id;
            parameters[4].Value = model.image_id;
            parameters[5].Value = model.face_image_path;
            parameters[6].Value = model.feature_data;
            parameters[7].Value = model.type;
            parameters[8].Value = model.create_time;
            parameters[9].Value = model.modified_time;
            parameters[10].Value = model.quality_score;

            int rows = DbHelperMySQL.ExecuteSql(strSql.ToString(), parameters);


            /*
                        strSql.Append("insert into TH_FACE_USER(");
                        strSql.Append("name,gender,id_card,face_image_path,feature_data,type,create_time,modified_time,quality_score)");
                        strSql.Append(" values (");
                        strSql.Append(":name,:gender,:id_card,:face_image_path,:feature_data,:type,:create_time,:modified_time,:quality_score)");
                        OracleParameter[] parameters = {
                                new OracleParameter(":name", OracleType.VarChar,50),
                                new OracleParameter(":gender", OracleType.VarChar,5),
                                new OracleParameter(":id_card", OracleType.VarChar, 50),
                                new OracleParameter(":face_image_path", OracleType.VarChar,200),
                                new OracleParameter(":feature_data", OracleType.Blob),
                                new OracleParameter(":type", OracleType.VarChar,5),
                                new OracleParameter(":create_time", OracleType.DateTime),
                                new OracleParameter(":modified_time", OracleType.DateTime),
                                new OracleParameter(":quality_score", OracleType.Float)};

                        parameters[0].Value = model.name;
                        parameters[1].Value = model.gender;
                        parameters[2].Value = model.idCard;
                        parameters[3].Value = model.face_image_path ;
                        parameters[4].Value = model.feature_data;
                        parameters[5].Value = model.type;
                        parameters[6].Value = model.create_time;
                        parameters[7].Value = model.modified_time;
                        parameters[8].Value = model.quality_score;

                        int rows = DbHelperOracle.ExecuteNonQuery(CommandType.Text,strSql.ToString(), parameters);
             */

            if (rows > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool Add(DataAngine.Model.user model, string library)
        {
            StringBuilder strSql = new StringBuilder();


            strSql.Append("insert into user(");
            strSql.Append("people_id,name,gender,card_id,image_id,face_image_path,feature_data,type,create_time,modified_time,quality_score)");
            strSql.Append(" values (");
            strSql.Append("@people_id,@name,@gender,@card_id,@image_id,@face_image_path,@feature_data,@type,@create_time,@modified_time,@quality_score)");
            MySqlParameter[] parameters = {
                    new MySqlParameter("@people_id", MySqlDbType.VarChar,50),
					new MySqlParameter("@name", MySqlDbType.VarChar,50),
					new MySqlParameter("@gender", MySqlDbType.VarChar,5),
                    new MySqlParameter("@card_id", MySqlDbType.VarChar, 50),
                    new MySqlParameter("@image_id", MySqlDbType.VarChar, 60),
					new MySqlParameter("@face_image_path", MySqlDbType.VarChar,200),
					new MySqlParameter("@feature_data", MySqlDbType.LongBlob),
					new MySqlParameter("@type", MySqlDbType.VarChar,1),
					new MySqlParameter("@create_time", MySqlDbType.DateTime),
					new MySqlParameter("@modified_time", MySqlDbType.DateTime),
					new MySqlParameter("@quality_score", MySqlDbType.Float)};

            parameters[0].Value = model.people_id;
            parameters[1].Value = model.name;
            parameters[2].Value = model.gender;
            parameters[3].Value = model.card_id;
            parameters[4].Value = model.image_id;
            parameters[5].Value = model.face_image_path;
            parameters[6].Value = model.feature_data;
            parameters[7].Value = model.type;
            parameters[8].Value = model.create_time;
            parameters[9].Value = model.modified_time;
            parameters[10].Value = model.quality_score;

            int rows = DbHelperMySQL.ExecuteSql(strSql.ToString(), library, parameters);
           
            if (rows > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        /// <summary>
        /// 更新一条数据
        /// </summary>
        public bool Update(DataAngine.Model.user model)
        {

            StringBuilder strSql = new StringBuilder();

            strSql.Append("update user set ");
            strSql.Append("people_id=@people_id,");
            strSql.Append("name=@name,");
            strSql.Append("gender=@gender,");
            strSql.Append("card_id=@card_id,");
            strSql.Append("image_id=@image_id,");
            strSql.Append("face_image_path=@face_image_path,");
            strSql.Append("feature_data=@feature_data,");
            strSql.Append("type=@type,");
            strSql.Append("create_time=@create_time,");
            strSql.Append("modified_time=@modified_time,");
            strSql.Append("quality_score=@quality_score");
            strSql.Append(" where id=@id");
            MySqlParameter[] parameters = {
					new MySqlParameter("@people_id", MySqlDbType.VarChar,50),
					new MySqlParameter("@name", MySqlDbType.VarChar,50),
					new MySqlParameter("@gender", MySqlDbType.VarChar,5),
                    new MySqlParameter("@card_id", MySqlDbType.VarChar, 50),
                    new MySqlParameter("@image_id", MySqlDbType.VarChar, 60),
					new MySqlParameter("@face_image_path", MySqlDbType.VarChar,200),
					new MySqlParameter("@feature_data", MySqlDbType.LongBlob),
					new MySqlParameter("@type", MySqlDbType.VarChar,1),
					new MySqlParameter("@create_time", MySqlDbType.DateTime),
					new MySqlParameter("@modified_time", MySqlDbType.DateTime),
					new MySqlParameter("@quality_score", MySqlDbType.Float)};
            parameters[0].Value = model.people_id;
            parameters[1].Value = model.name;
            parameters[2].Value = model.gender;
            parameters[3].Value = model.card_id;
            parameters[4].Value = model.image_id;
            parameters[5].Value = model.face_image_path;
            parameters[6].Value = model.feature_data;
            parameters[7].Value = model.type;
            parameters[8].Value = model.create_time;
            parameters[9].Value = model.modified_time;
            parameters[10].Value = model.quality_score;

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

        /// <summary>
        /// 删除一条数据
        /// </summary>
        public bool Delete(int id)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete from user ");
            strSql.Append(" where id=@id");
            MySqlParameter[] parameters = {
					new MySqlParameter("@id", MySqlDbType.Int32)
			};
            parameters[0].Value = id;

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
        /// <summary>
        /// 批量删除数据
        /// </summary>
        public bool Delete(string strWhere)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete from user ");
            if (strWhere.Trim() != "")
            {
                strSql.Append(" where " + strWhere);
            }
            int rows = DbHelperMySQL.ExecuteSql(strSql.ToString());
            if (rows > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public bool DeleteList(string idlist)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete from user ");
            strSql.Append(" where id in (" + idlist + ")  ");
            int rows = DbHelperMySQL.ExecuteSql(strSql.ToString());
            if (rows > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }


        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public DataAngine.Model.user GetModel(int id)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("select id,people_id,name,gender,card_id,image_id,face_image_path,feature_data,type,create_time,modified_time,quality_score from user ");
            strSql.Append(" where id=@id");
            MySqlParameter[] parameters = {
					new MySqlParameter("@id", MySqlDbType.Int32)
			};
            parameters[0].Value = id;

            DataAngine.Model.user model = new DataAngine.Model.user();
            DataSet ds = DbHelperMySQL.Query(strSql.ToString(), parameters);
            if (ds.Tables[0].Rows.Count > 0)
            {
                return DataRowToModel(ds.Tables[0].Rows[0]);
            }
            else
            {
                return null;
            }
        }


        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public DataAngine.Model.user DataRowToModel(DataRow row)
        {
            DataAngine.Model.user model = new DataAngine.Model.user();
            if (row != null)
            {
                if (row["id"] != null && row["id"].ToString() != "")
                {
                    model.id = int.Parse(row["id"].ToString());
                }
                if (row["people_id"] != null)
                {
                    model.people_id = row["people_id"].ToString();
                }
                if (row["name"] != null)
                {
                    model.name = row["name"].ToString();
                }
                if (row["gender"] != null)
                {
                    model.gender = row["gender"].ToString();
                }
                if (row["card_id"] != null)
                {
                    model.card_id = row["card_id"].ToString();
                }
                if (row["image_id"] != null)
                {
                    model.image_id = row["image_id"].ToString();
                }
                if (row["face_image_path"] != null)
                {
                    model.face_image_path = row["face_image_path"].ToString();
                }
                //model.feature_data=row["feature_data"].ToString();
                if (row["type"] != null)
                {
                    model.type = row["type"].ToString();
                }
                if (row["create_time"] != null && row["create_time"].ToString() != "")
                {
                    model.create_time = DateTime.Parse(row["create_time"].ToString());
                }
                if (row["modified_time"] != null && row["modified_time"].ToString() != "")
                {
                    model.modified_time = DateTime.Parse(row["modified_time"].ToString());
                }
                if (row["quality_score"] != null && row["quality_score"].ToString() != "")
                {
                    model.quality_score = float.Parse(row["quality_score"].ToString());
                }
                if (row["feature_data"] != null && row["feature_data"] != System.DBNull.Value)
                {

                    model.feature_data = (byte[])row["feature_data"];

                }
            }
            return model;
        }
        /// <summary>
        /// 按名字删除
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public bool DeleteByName(string name)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete from user ");
            strSql.Append(" where name=@name");
            MySqlParameter[] parameters = {
					new MySqlParameter("@name", MySqlDbType.Int32)
			};
            parameters[0].Value = name;

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
        /// <summary>
        /// 获得数据列表
        /// </summary>
        public DataSet GetList(string strWhere)
        {
            StringBuilder strSql = new StringBuilder();

            strSql.Append("select id,people_id,name,gender,card_id,image_id,face_image_path,feature_data,type,create_time,modified_time,quality_score ");
            strSql.Append(" FROM user ");

            if (strWhere.Trim() != "")
            {
                strSql.Append(" where " + strWhere);
            }
            return DbHelperMySQL.Query(strSql.ToString());

            /*
                        strSql.Append("select id,name,gender,id_card,face_image_path,feature_data,type,create_time,modified_time,quality_score ");
                        strSql.Append(" FROM TH_FACE_USER ");

                        if (strWhere.Trim() != "")
                        {
                            strSql.Append(" where " + strWhere);
                        }

                        return DbHelperOracle.ExecuteDataSet(CommandType.Text,strSql.ToString());
             */
        }

        /// <summary>
        /// 获得数据列表
        /// </summary>
        public DataSet GetList(string strWhere, string libraryname)
        {
            Console.WriteLine("gitlisthere" + libraryname);
            StringBuilder strSql = new StringBuilder();

            strSql.Append("select id,people_id,name,gender,card_id,image_id,face_image_path,feature_data,type,create_time,modified_time,quality_score ");
            strSql.Append(" FROM user");

            if (strWhere.Trim() != "")
            {
                strSql.Append(" where " + strWhere);
            }
            return DbHelperMySQL.Query(strSql.ToString(), libraryname);
        }

        /// <summary>
        /// 获得数据列表
        /// </summary>
        public DataSet GetPicPathList(string strWhere)
        {
            StringBuilder strSql = new StringBuilder();

            strSql.Append("select id,people_id,name,gender,card_id,image_id,face_image_path ");
            strSql.Append(" FROM user ");

            //if (strWhere.Trim() != "")
            //{
            //    strSql.Append(" where " + strWhere);
            //}
            return DbHelperMySQL.Query(strSql.ToString());          
        }

        /// <summary>
        /// 获得数据列表
        /// </summary>
        public DataSet GetPicPathList(string strWhere, string library)
        {
            StringBuilder strSql = new StringBuilder();

            strSql.Append("select id,people_id,name,gender,card_id,image_id,face_image_path ");
            strSql.Append(" FROM user ");

            //if (strWhere.Trim() != "")
            //{
            //    strSql.Append(" where " + strWhere);
            //}
            return DbHelperMySQL.Query(strSql.ToString(), library);
        }

        /// <summary>
        /// 分页查询
        /// </summary>
        public DataSet GetPicPathList(string strWhere, int startIndex, int pageSize)
        {
            StringBuilder strSql = new StringBuilder();

            strSql.Append("select id,people_id,name,gender,card_id,image_id,face_image_path ");
            strSql.Append(" FROM user ");
            strSql.Append(" limit " + startIndex + ", " + pageSize);

            //if (strWhere.Trim() != "")
            //{
            //    strSql.Append(" where " + strWhere);
            //}
            return DbHelperMySQL.Query(strSql.ToString());
        }

        /// <summary>
        /// 分页查询
        /// </summary>
        public DataSet GetPicPathList(string strWhere, int startIndex, int pageSize, string library)
        {
            StringBuilder strSql = new StringBuilder();

            strSql.Append("select id,people_id,name,gender,card_id,image_id,face_image_path ");
            strSql.Append(" FROM user ");
            strSql.Append(" limit " + startIndex + ", " + pageSize);

            //if (strWhere.Trim() != "")
            //{
            //    strSql.Append(" where " + strWhere);
            //}
            return DbHelperMySQL.Query(strSql.ToString(), library);
        }

        /// <summary>
        /// 获取记录总数
        /// </summary>
        public int GetRecordCount(string strWhere)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(1) FROM user ");
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
                strSql.Append("order by T.id desc");
            }
            strSql.Append(")AS Row, T.*  from user T ");
            if (!string.IsNullOrEmpty(strWhere.Trim()))
            {
                strSql.Append(" WHERE " + strWhere);
            }
            strSql.Append(" ) TT");
            strSql.AppendFormat(" WHERE TT.Row between {0} and {1}", startIndex, endIndex);
            return DbHelperMySQL.Query(strSql.ToString());
        }

        /*
        /// <summary>
        /// 分页获取数据列表
        /// </summary>
        public DataSet GetList(int PageSize,int PageIndex,string strWhere)
        {
            MySqlParameter[] parameters = {
                    new MySqlParameter("@tblName", MySqlDbType.VarChar, 255),
                    new MySqlParameter("@fldName", MySqlDbType.VarChar, 255),
                    new MySqlParameter("@PageSize", MySqlDbType.Int32),
                    new MySqlParameter("@PageIndex", MySqlDbType.Int32),
                    new MySqlParameter("@IsReCount", MySqlDbType.Bit),
                    new MySqlParameter("@OrderType", MySqlDbType.Bit),
                    new MySqlParameter("@strWhere", MySqlDbType.VarChar,1000),
                    };
            parameters[0].Value = "user";
            parameters[1].Value = "id";
            parameters[2].Value = PageSize;
            parameters[3].Value = PageIndex;
            parameters[4].Value = 0;
            parameters[5].Value = 0;
            parameters[6].Value = strWhere;	
            return DbHelperMySQL.RunProcedure("UP_GetRecordByPage",parameters,"ds");
        }*/

        #endregion  BasicMethod
        #region  ExtensionMethod

        #endregion  ExtensionMethod
    }
}

