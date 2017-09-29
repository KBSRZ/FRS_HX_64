using System;
using System.Data;
using System.Text;
using MySql.Data.MySqlClient;
using System.Data.OracleClient;

using DataAngine.DBUtility;//Please add references

namespace DataAngine.DAL
{
	/// <summary>
	/// 数据访问类:hitrecord
	/// </summary>
	public partial class hitrecord
	{
		public hitrecord()
		{}
		#region  BasicMethod

		/// <summary>
		/// 得到最大ID
		/// </summary>
		public int GetMaxId()
		{
		    return DbHelperMySQL.GetMaxID("id", "hitrecord");
            //return DbHelperOracle.GetMaxID("id", "TH_FACE_HITRECORD"); 
		}

		/// <summary>
		/// 是否存在该记录
		/// </summary>
		public bool Exists(int id)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("select count(1) from hitrecord");
			strSql.Append(" where id=@id");
			MySqlParameter[] parameters = {
					new MySqlParameter("@id", MySqlDbType.Int32)
			};
			parameters[0].Value = id;

			return DbHelperMySQL.Exists(strSql.ToString(),parameters);
		}


		/// <summary>
		/// 增加一条数据
		/// </summary>
		public bool Add(DataAngine.Model.hitrecord model)
		{

			StringBuilder strSql=new StringBuilder();


            strSql.Append("insert into hitrecord(");
            strSql.Append("face_query_image_path,threshold,occur_time)");
            strSql.Append(" values (");
            strSql.Append("@face_query_image_path,@threshold,@occur_time)");
            MySqlParameter[] parameters = {
                    new MySqlParameter("@face_query_image_path", MySqlDbType.VarChar,200),
                    new MySqlParameter("@threshold", MySqlDbType.Float),
                    new MySqlParameter("@occur_time", MySqlDbType.DateTime)};
            parameters[0].Value = model.face_query_image_path;
            parameters[1].Value = model.threshold;
            parameters[2].Value = model.occur_time;

            int rows=DbHelperMySQL.ExecuteSql(strSql.ToString(),parameters);

           
            //strSql.Append("insert into TH_FACE_HITRECORD(");
            //strSql.Append("face_query_image_path,threshold,occur_time)");
            //strSql.Append(" values (");
            //strSql.Append(":face_query_image_path,:threshold,:occur_time)");
            //OracleParameter[] parameters = {
            //        new OracleParameter(":face_query_image_path",OracleType.VarChar,200),
            //        new OracleParameter(":threshold", OracleType.Float),
            //        new OracleParameter(":occur_time", OracleType.DateTime)};
            //parameters[0].Value = model.face_query_image_path;
            //parameters[1].Value = model.threshold;
            //parameters[2].Value = model.occur_time;

            //int rows = DbHelperOracle.ExecuteNonQuery(CommandType.Text,strSql.ToString(), parameters);
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
		public bool Update(DataAngine.Model.hitrecord model)
		{
			StringBuilder strSql=new StringBuilder();


            strSql.Append("update hitrecord set ");
            strSql.Append("face_query_image_path=@face_query_image_path,");
            strSql.Append("threshold=@threshold,");
            strSql.Append("occur_time=@occur_time");
            strSql.Append(" where id=@id");
            MySqlParameter[] parameters = {
                    new MySqlParameter("@face_query_image_path", MySqlDbType.VarChar,200),
                    new MySqlParameter("@threshold", MySqlDbType.Float),
                    new MySqlParameter("@occur_time", MySqlDbType.DateTime),
                    new MySqlParameter("@id", MySqlDbType.Int32,11)};
            parameters[0].Value = model.face_query_image_path;
            parameters[1].Value = model.threshold;
            parameters[2].Value = model.occur_time;
            
            int rows=DbHelperMySQL.ExecuteSql(strSql.ToString(),parameters);



            //strSql.Append("update hitrecord set ");
            //strSql.Append("face_query_image_path=:face_query_image_path,");
            //strSql.Append("threshold=:threshold,");
            //strSql.Append("occur_time=:occur_time");
            //strSql.Append(" where id=:id");
            //OracleParameter[] parameters = {
            //        new OracleParameter(":face_query_image_path", OracleType.VarChar,200),
            //        new OracleParameter(":threshold", OracleType.Float),
            //        new OracleParameter(":occur_time", OracleType.DateTime),
            //        new OracleParameter(":id", OracleType.Int32)};
            //parameters[0].Value = model.face_query_image_path;
            //parameters[1].Value = model.threshold;
            //parameters[2].Value = model.occur_time;
            //parameters[3].Value = model.id;

            //int rows = DbHelperOracle.ExecuteNonQuery(CommandType.Text, strSql.ToString(), parameters);

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
			
			StringBuilder strSql=new StringBuilder();
			strSql.Append("delete from hitrecord ");
			strSql.Append(" where id=@id");
			MySqlParameter[] parameters = {
					new MySqlParameter("@id", MySqlDbType.Int32)
			};
			parameters[0].Value = id;

			int rows=DbHelperMySQL.ExecuteSql(strSql.ToString(),parameters);
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
		public bool DeleteList(string idlist )
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("delete from hitrecord ");
			strSql.Append(" where id in ("+idlist + ")  ");
			int rows=DbHelperMySQL.ExecuteSql(strSql.ToString());
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
		public DataAngine.Model.hitrecord GetModel(int id)
		{
            StringBuilder strSql = new StringBuilder();
	
			strSql.Append("select id,face_query_image_path,threshold,occur_time from hitrecord ");
			strSql.Append(" where id=@id");
			MySqlParameter[] parameters = {
					new MySqlParameter("@id", MySqlDbType.Int32)
			};
			parameters[0].Value = id;

			DataAngine.Model.hitrecord model=new DataAngine.Model.hitrecord();
			DataSet ds=DbHelperMySQL.Query(strSql.ToString(),parameters);
			if(ds.Tables[0].Rows.Count>0)
			{
				return DataRowToModel(ds.Tables[0].Rows[0]);
			}
			else
			{
				return null;
			}
 

            //strSql.Append("select id,face_query_image_path,threshold,occur_time from hitrecord ");
            //strSql.Append(" where id=:id");
            //OracleParameter[] parameters = {
            //        new OracleParameter(":id", OracleType.Int32)
            //};
            //parameters[0].Value = id;

            //DataAngine.Model.hitrecord model = new DataAngine.Model.hitrecord();
            //DataSet ds = DbHelperOracle.ExecuteDataSet(CommandType.Text, strSql.ToString(), parameters);
            //if (ds.Tables[0].Rows.Count > 0)
            //{
            //    return DataRowToModel(ds.Tables[0].Rows[0]);
            //}
            //else
            //{
            //    return null;
            //}

		}


		/// <summary>
		/// 得到一个对象实体
		/// </summary>
		public DataAngine.Model.hitrecord DataRowToModel(DataRow row)
		{
			DataAngine.Model.hitrecord model=new DataAngine.Model.hitrecord();
			if (row != null)
			{
				if(row["id"]!=null && row["id"].ToString()!="")
				{
					model.id=int.Parse(row["id"].ToString());
				}
				if(row["face_query_image_path"]!=null)
				{
					model.face_query_image_path=row["face_query_image_path"].ToString();
				}
				if(row["threshold"]!=null && row["threshold"].ToString()!="")
				{
					model.threshold=float.Parse(row["threshold"].ToString());
				}
				if(row["occur_time"]!=null && row["occur_time"].ToString()!="")
				{
					model.occur_time=DateTime.Parse(row["occur_time"].ToString());
				}
			}
			return model;
		}

		/// <summary>
		/// 获得数据列表
		/// </summary>
		public DataSet GetList(string strWhere)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("select id,face_query_image_path,threshold,occur_time ");
			strSql.Append(" FROM hitrecord ");
			if(strWhere.Trim()!="")
			{
				strSql.Append(" where "+strWhere);
			}
			return DbHelperMySQL.Query(strSql.ToString());
		}

		/// <summary>
		/// 获取记录总数
		/// </summary>
		public int GetRecordCount(string strWhere)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("select count(1) FROM hitrecord ");
			if(strWhere.Trim()!="")
			{
				strSql.Append(" where "+strWhere);
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
			StringBuilder strSql=new StringBuilder();
			strSql.Append("SELECT * FROM ( ");
			strSql.Append(" SELECT ROW_NUMBER() OVER (");
			if (!string.IsNullOrEmpty(orderby.Trim()))
			{
				strSql.Append("order by T." + orderby );
			}
			else
			{
				strSql.Append("order by T.id desc");
			}
			strSql.Append(")AS Row, T.*  from hitrecord T ");
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
			parameters[0].Value = "hitrecord";
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

