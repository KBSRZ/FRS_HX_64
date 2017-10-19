using System;
using System.Data;
using System.Text;
using MySql.Data.MySqlClient;
using DataAngine_Set.DBUtility;//Please add references
namespace DataAngine_Set.DAL
{
	/// <summary>
	/// 数据访问类:device
	/// </summary>
	public partial class device
	{
		public device()
		{}
		#region  BasicMethod

		/// <summary>
		/// 得到最大ID
		/// </summary>
		public int GetMaxId()
		{
		return DbHelperMySQL.GetMaxID("id", "device"); 
		}

		/// <summary>
		/// 是否存在该记录
		/// </summary>
		public bool Exists(int id)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("select count(1) from device");
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
		public bool Add(DataAngine_Set.Model.device model)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("insert into device(");
			strSql.Append("name,address,departmentmentid,longitude,latitude,locationtype,remark)");
			strSql.Append(" values (");
			strSql.Append("@name,@address,@departmentmentid,@longitude,@latitude,@locationtype,@remark)");
			MySqlParameter[] parameters = {
					new MySqlParameter("@name", MySqlDbType.VarChar,50),
					new MySqlParameter("@address", MySqlDbType.VarChar,50),
					new MySqlParameter("@departmentmentid", MySqlDbType.VarChar,50),
					new MySqlParameter("@longitude", MySqlDbType.Double,5),
					new MySqlParameter("@latitude", MySqlDbType.Double,5),
					new MySqlParameter("@locationtype", MySqlDbType.Int32,11),
					new MySqlParameter("@remark", MySqlDbType.VarChar,50)};
			parameters[0].Value = model.name;
			parameters[1].Value = model.address;
			parameters[2].Value = model.departmentmentid;
			parameters[3].Value = model.longitude;
			parameters[4].Value = model.latitude;
			parameters[5].Value = model.locationtype;
			parameters[6].Value = model.remark;

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
		/// 更新一条数据
		/// </summary>
		public bool Update(DataAngine_Set.Model.device model)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("update device set ");
			strSql.Append("name=@name,");
			strSql.Append("address=@address,");
			strSql.Append("departmentmentid=@departmentmentid,");
			strSql.Append("longitude=@longitude,");
			strSql.Append("latitude=@latitude,");
			strSql.Append("locationtype=@locationtype,");
			strSql.Append("remark=@remark");
			strSql.Append(" where id=@id");
			MySqlParameter[] parameters = {
					new MySqlParameter("@name", MySqlDbType.VarChar,50),
					new MySqlParameter("@address", MySqlDbType.VarChar,50),
					new MySqlParameter("@departmentmentid", MySqlDbType.VarChar,50),
					new MySqlParameter("@longitude", MySqlDbType.Double,5),
					new MySqlParameter("@latitude", MySqlDbType.Double,5),
					new MySqlParameter("@locationtype", MySqlDbType.Int32,11),
					new MySqlParameter("@remark", MySqlDbType.VarChar,50),
					new MySqlParameter("@id", MySqlDbType.Int32,11)};
			parameters[0].Value = model.name;
			parameters[1].Value = model.address;
			parameters[2].Value = model.departmentmentid;
			parameters[3].Value = model.longitude;
			parameters[4].Value = model.latitude;
			parameters[5].Value = model.locationtype;
			parameters[6].Value = model.remark;
			parameters[7].Value = model.id;

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
		/// 删除一条数据
		/// </summary>
		public bool Delete(int id)
		{
			
			StringBuilder strSql=new StringBuilder();
			strSql.Append("delete from device ");
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
			strSql.Append("delete from device ");
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
		public DataAngine_Set.Model.device GetModel(int id)
		{
			
			StringBuilder strSql=new StringBuilder();
			strSql.Append("select id,name,address,departmentmentid,longitude,latitude,locationtype,remark from device ");
			strSql.Append(" where id=@id");
			MySqlParameter[] parameters = {
					new MySqlParameter("@id", MySqlDbType.Int32)
			};
			parameters[0].Value = id;

			DataAngine_Set.Model.device model=new DataAngine_Set.Model.device();
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

        public DataSet GetDevice(string name)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("select id,name,address,departmentmentid,longitude,latitude,locationtype,remark ");
            strSql.Append(" FROM device ");
            strSql.Append(" where name='" + name + "'");

            return DbHelperMySQL.Query(strSql.ToString());
        }


		/// <summary>
		/// 得到一个对象实体
		/// </summary>
		public DataAngine_Set.Model.device DataRowToModel(DataRow row)
		{
			DataAngine_Set.Model.device model=new DataAngine_Set.Model.device();
			if (row != null)
			{
				if(row["id"]!=null && row["id"].ToString()!="")
				{
					model.id=int.Parse(row["id"].ToString());
				}
				if(row["name"]!=null)
				{
					model.name=row["name"].ToString();
				}
				if(row["address"]!=null)
				{
					model.address=row["address"].ToString();
				}
				if(row["departmentmentid"]!=null)
				{
					model.departmentmentid=row["departmentmentid"].ToString();
				}
					//model.longitude=row["longitude"].ToString();
					//model.latitude=row["latitude"].ToString();
				if(row["locationtype"]!=null && row["locationtype"].ToString()!="")
				{
					model.locationtype=int.Parse(row["locationtype"].ToString());
				}
				if(row["remark"]!=null)
				{
					model.remark=row["remark"].ToString();
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
			strSql.Append("select id,name,address,departmentmentid,longitude,latitude,locationtype,remark ");
			strSql.Append(" FROM device ");
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
			strSql.Append("select count(1) FROM device ");
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
			strSql.Append(")AS Row, T.*  from device T ");
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
			parameters[0].Value = "device";
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

