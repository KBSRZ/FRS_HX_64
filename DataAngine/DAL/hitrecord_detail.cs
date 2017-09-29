using System;
using System.Data;
using System.Text;
using System.Collections.Generic;
using MySql.Data.MySqlClient;
using System.Data.OracleClient;

using DataAngine.DBUtility;//Please add references

namespace DataAngine.DAL
{
	/// <summary>
	/// 数据访问类:hitrecord_detail
	/// </summary>
	public partial class hitrecord_detail
	{
		public hitrecord_detail()
		{}
		#region  BasicMethod

		/// <summary>
		/// 得到最大ID
		/// </summary>
		public int GetMaxId()
		{
		return DbHelperMySQL.GetMaxID("id", "hitrecord_detail"); 
		}

		/// <summary>
		/// 是否存在该记录
		/// </summary>
		public bool Exists(int id)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("select count(1) from hitrecord_detail");
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
        public bool Add(DataAngine.Model.hitrecord_detail model)
		{
			StringBuilder strSql=new StringBuilder();


			strSql.Append("insert into hitrecord_detail(");
			strSql.Append("hit_record_id,user_id,rank,score)");
			strSql.Append(" values (");
			strSql.Append("@hit_record_id,@user_id,@rank,@score)");
			MySqlParameter[] parameters = {
					new MySqlParameter("@hit_record_id", MySqlDbType.Int32,11),
					new MySqlParameter("@user_id", MySqlDbType.Int32,11),
					new MySqlParameter("@rank", MySqlDbType.Int32,11),
					new MySqlParameter("@score", MySqlDbType.Float)};
			parameters[0].Value = model.hit_record_id;
			parameters[1].Value = model.user_id;
			parameters[2].Value = model.rank;
			parameters[3].Value = model.score;

			int rows=DbHelperMySQL.ExecuteSql(strSql.ToString(),parameters);
 

            //strSql.Append("insert into TH_FACE_HITRECORD_DETAIL(");
            //strSql.Append("hit_record_id,user_id,rank,score)");
            //strSql.Append(" values (");
            //strSql.Append(":hit_record_id,:user_id,:rank,:score)");

            //OracleParameter[] parameters = {
            //        new OracleParameter(":hit_record_id", OracleType.Int32),
            //        new OracleParameter(":user_id", OracleType.Int32),
            //        new OracleParameter(":rank", OracleType.Int32),
            //        new OracleParameter(":score", OracleType.Float)};

            //parameters[0].Value = model.hit_record_id;
            //parameters[1].Value = model.user_id;
            //parameters[2].Value = model.rank;
            //parameters[3].Value = model.score;

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
		/// 更新一条数据
		/// </summary>
        public bool Update(DataAngine.Model.hitrecord_detail model)
		{
			StringBuilder strSql=new StringBuilder();

			strSql.Append("update hitrecord_detail set ");
			strSql.Append("hit_record_id=@hit_record_id,");
			strSql.Append("user_id=@user_id,");
			strSql.Append("rank=@rank,");
			strSql.Append("score=@score");
			strSql.Append(" where id=@id");
			MySqlParameter[] parameters = {
					new MySqlParameter("@hit_record_id", MySqlDbType.Int32,11),
					new MySqlParameter("@user_id", MySqlDbType.Int32,11),
					new MySqlParameter("@rank", MySqlDbType.Int32,11),
					new MySqlParameter("@score", MySqlDbType.Float),
					new MySqlParameter("@id", MySqlDbType.Int32,11)};
			parameters[0].Value = model.hit_record_id;
			parameters[1].Value = model.user_id;
			parameters[2].Value = model.rank;
			parameters[3].Value = model.score;
			parameters[4].Value = model.id;

			int rows=DbHelperMySQL.ExecuteSql(strSql.ToString(),parameters);
 

            //strSql.Append("update hitrecord_detail set ");
            //strSql.Append("hit_record_id=:hit_record_id,");
            //strSql.Append("user_id=:user_id,");
            //strSql.Append("rank=:rank,");
            //strSql.Append("score=:score");
            //strSql.Append(" where id=:id");
            //OracleParameter[] parameters = {
            //        new OracleParameter(":hit_record_id", OracleType.Int32),
            //        new OracleParameter(":user_id", OracleType.Int32),
            //        new OracleParameter(":rank", OracleType.Int32),
            //        new OracleParameter(":score", OracleType.Float),
            //        new OracleParameter(":id", OracleType.Int32)};
            //parameters[0].Value = model.hit_record_id;
            //parameters[1].Value = model.user_id;
            //parameters[2].Value = model.rank;
            //parameters[3].Value = model.score;
            //parameters[4].Value = model.id;

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
		/// 更新一条数据
		/// </summary>
        public bool UpdateByUserId(DataAngine.Model.hitrecord_detail model)
        {
            StringBuilder strSql = new StringBuilder();

            strSql.Append("update hitrecord_detail set ");
            strSql.Append("hit_record_id=:hit_record_id,");
            strSql.Append("rank=:rank,");
            strSql.Append("score=:score");
            strSql.Append(" where user_id=:user_id");
            OracleParameter[] parameters = {
					new OracleParameter(":hit_record_id", OracleType.Int32),
					new OracleParameter(":user_id", OracleType.Int32),
					new OracleParameter(":rank", OracleType.Int32),
					new OracleParameter(":score", OracleType.Float),
					new OracleParameter(":id", OracleType.Int32)};
            parameters[0].Value = model.hit_record_id;
            parameters[1].Value = model.user_id;
            parameters[2].Value = model.rank;
            parameters[3].Value = model.score;
            parameters[4].Value = model.id;

            int rows = DbHelperOracle.ExecuteNonQuery(CommandType.Text, strSql.ToString(), parameters);

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
			strSql.Append("delete from hitrecord_detail ");
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
			strSql.Append("delete from hitrecord_detail ");
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
		public DataAngine.Model.hitrecord_detail GetModel(int id)
		{	
			StringBuilder strSql=new StringBuilder();
			strSql.Append("select id,hit_record_id,user_id,rank,score from hitrecord_detail ");
			strSql.Append(" where id=@id");
			MySqlParameter[] parameters = {
					new MySqlParameter("@id", MySqlDbType.Int32)
			};
			parameters[0].Value = id;

			DataAngine.Model.hitrecord_detail model=new DataAngine.Model.hitrecord_detail();
			DataSet ds=DbHelperMySQL.Query(strSql.ToString(),parameters);
			if(ds.Tables[0].Rows.Count>0)
			{
				return DataRowToModel(ds.Tables[0].Rows[0]);
			}
			else
			{
				return null;
			}
		}

        /// <summary>
        /// 根据HitrecordId 获得 hitrecord_detail
        /// </summary>
        /// <param name="HitrecordId"></param>
        /// <returns></returns>
        public List<DataAngine.Model.hitrecord_detail> GetModelByHitrecordId(int HitrecordId)
        {
            List<DataAngine.Model.hitrecord_detail> modelList = new List<Model.hitrecord_detail>();
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select id,hit_record_id,user_id,rank,score from hitrecord_detail ");
            strSql.Append(" where hit_record_id=@hit_record_id");
            MySqlParameter[] parameters = {
					new MySqlParameter("@hit_record_id", MySqlDbType.Int32)
			};
            parameters[0].Value = HitrecordId;

            DataAngine.Model.hitrecord_detail model = new DataAngine.Model.hitrecord_detail();
            DataSet ds = DbHelperMySQL.Query(strSql.ToString(), parameters);
            foreach (var row in ds.Tables[0].Rows)
            {
                modelList.Add(DataRowToModel((DataRow)row));
            }
            return modelList;
           
        }


                /// <summary>
        /// 根据HitrecordId 获得 hitrecord_detail
        /// </summary>
        /// <param name="HitrecordId"></param>
        /// <returns></returns>
        public List<DataAngine.Model.hitrecord_detail> GetModelByHitUserId(int hitUserId)
        {
            List<DataAngine.Model.hitrecord_detail> modelList = new List<Model.hitrecord_detail>();
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select id,hit_record_id,user_id,rank,score from hitrecord_detail ");
            strSql.Append(" where user_id=@user_id");
            OracleParameter[] parameters = {
					new OracleParameter("@user_id", OracleType.Int32)
			};
            parameters[0].Value = hitUserId;

            DataAngine.Model.hitrecord_detail model = new DataAngine.Model.hitrecord_detail();
            DataSet ds = DbHelperOracle.ExecuteDataSet(CommandType.Text, strSql.ToString(), parameters);
            foreach (var row in ds.Tables[0].Rows)
            {
                modelList.Add(DataRowToModel((DataRow)row));
            }
            return modelList;

        }


		/// <summary>
		/// 得到一个对象实体
		/// </summary>
		public DataAngine.Model.hitrecord_detail DataRowToModel(DataRow row)
		{
			DataAngine.Model.hitrecord_detail model=new DataAngine.Model.hitrecord_detail();
			if (row != null)
			{
				if(row["id"]!=null && row["id"].ToString()!="")
				{
					model.id=int.Parse(row["id"].ToString());
				}
				if(row["hit_record_id"]!=null && row["hit_record_id"].ToString()!="")
				{
					model.hit_record_id=int.Parse(row["hit_record_id"].ToString());
				}
				if(row["user_id"]!=null && row["user_id"].ToString()!="")
				{
					model.user_id=int.Parse(row["user_id"].ToString());
				}
				if(row["rank"]!=null && row["rank"].ToString()!="")
				{
					model.rank=int.Parse(row["rank"].ToString());
				}
				if(row["score"]!=null && row["score"].ToString()!="")
				{
					model.score=float.Parse(row["score"].ToString());
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
			strSql.Append("select id,hit_record_id,user_id,rank,score ");
			strSql.Append(" FROM hitrecord_detail ");
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
			strSql.Append("select count(1) FROM hitrecord_detail ");
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
			strSql.Append(")AS Row, T.*  from hitrecord_detail T ");
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
			parameters[0].Value = "hitrecord_detail";
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

