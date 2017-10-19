using System;
using System.Data;
using System.Text;
using MySql.Data.MySqlClient;
using DataAgine_Set.DBUtility;//Please add references
namespace DataAgine_Set.DAL
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
		public bool Add(DataAgine_Set.Model.device model)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("insert into device(");
			strSql.Append("name,ip,port,user,password,info)");
			strSql.Append(" values (");
			strSql.Append("@name,@ip,@port,@user,@password,@info)");
			MySqlParameter[] parameters = {
					new MySqlParameter("@name", MySqlDbType.VarChar,50),
					new MySqlParameter("@ip", MySqlDbType.VarChar,20),
					new MySqlParameter("@port", MySqlDbType.VarChar,20),
					new MySqlParameter("@user", MySqlDbType.VarChar,20),
					new MySqlParameter("@password", MySqlDbType.VarChar,50),
					new MySqlParameter("@info", MySqlDbType.VarChar,20)};
			parameters[0].Value = model.name;
			parameters[1].Value = model.ip;
			parameters[2].Value = model.port;
			parameters[3].Value = model.user;
			parameters[4].Value = model.password;
			parameters[5].Value = model.info;

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
		public bool Update(DataAgine_Set.Model.device model)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("update device set ");
			strSql.Append("name=@name,");
			strSql.Append("ip=@ip,");
			strSql.Append("port=@port,");
			strSql.Append("user=@user,");
			strSql.Append("password=@password,");
			strSql.Append("info=@info");
			strSql.Append(" where id=@id");
			MySqlParameter[] parameters = {
					new MySqlParameter("@name", MySqlDbType.VarChar,50),
					new MySqlParameter("@ip", MySqlDbType.VarChar,20),
					new MySqlParameter("@port", MySqlDbType.VarChar,20),
					new MySqlParameter("@user", MySqlDbType.VarChar,20),
					new MySqlParameter("@password", MySqlDbType.VarChar,50),
					new MySqlParameter("@info", MySqlDbType.VarChar,20),
					new MySqlParameter("@id", MySqlDbType.Int32,11)};
			parameters[0].Value = model.name;
			parameters[1].Value = model.ip;
			parameters[2].Value = model.port;
			parameters[3].Value = model.user;
			parameters[4].Value = model.password;
			parameters[5].Value = model.info;
			parameters[6].Value = model.id;

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
		public DataAgine_Set.Model.device GetModel(int id)
		{
			
			StringBuilder strSql=new StringBuilder();
			strSql.Append("select id,name,ip,port,user,password,info from device ");
			strSql.Append(" where id=@id");
			MySqlParameter[] parameters = {
					new MySqlParameter("@id", MySqlDbType.Int32)
			};
			parameters[0].Value = id;

			DataAgine_Set.Model.device model=new DataAgine_Set.Model.device();
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
            strSql.Append("select id,name,ip,port,user,password ");
            strSql.Append(" FROM device ");
            strSql.Append(" where name='" + name + "'");

            return DbHelperMySQL.Query(strSql.ToString());
        }


		/// <summary>
		/// 得到一个对象实体
		/// </summary>
		public DataAgine_Set.Model.device DataRowToModel(DataRow row)
		{
			DataAgine_Set.Model.device model=new DataAgine_Set.Model.device();
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
				if(row["ip"]!=null)
				{
					model.ip=row["ip"].ToString();
				}
				if(row["port"]!=null)
				{
					model.port=row["port"].ToString();
				}
				if(row["user"]!=null)
				{
					model.user=row["user"].ToString();
				}
				if(row["password"]!=null)
				{
					model.password=row["password"].ToString();
				}
				if(row["info"]!=null)
				{
					model.info=row["info"].ToString();
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
			strSql.Append("select id,name,ip,port,user,password,info ");
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

