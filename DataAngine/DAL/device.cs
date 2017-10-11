using System;
using System.Data;
using System.Text;
using System.Windows.Forms;
using MySql.Data.MySqlClient;
using System.Data.OracleClient;

using DataAngine.DBUtility;//Please add references

namespace DataAngine.DAL
{
    public partial class device
    {
        public device()
        { }
        #region  BasicMethod
        
        /// <summary>
        /// 是否存在该记录
        /// </summary>
        public bool Exists(int id)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(1) from frs_database_set.device");
            strSql.Append(" where id=@id");
            MySqlParameter[] parameters = {
					new MySqlParameter("@id", MySqlDbType.Int32)
			};
            parameters[0].Value = id;

            return DbHelperMySQL.Exists(strSql.ToString(), false, parameters);
        }


        /// <summary>
        /// 增加一条数据
        /// </summary>
        public bool Add(DataAngine.Model.device model)
        {
            StringBuilder strSql = new StringBuilder();


            strSql.Append("insert into frs_database_set.device(");
            strSql.Append("name,ip,port,user,password)");
            strSql.Append(" values (");
            strSql.Append("@name,@ip,@port,@user,@password)");
            MySqlParameter[] parameters = {                   
					new MySqlParameter("@name", MySqlDbType.VarChar,50),
                    new MySqlParameter("@ip", MySqlDbType.VarChar,50),
                    new MySqlParameter("@port", MySqlDbType.VarChar,50),
                    new MySqlParameter("@user", MySqlDbType.VarChar,50),
                    new MySqlParameter("@password", MySqlDbType.VarChar,50)};

            parameters[0].Value = model.name;
            parameters[1].Value = model.ip;
            parameters[2].Value = model.port;
            parameters[3].Value = model.user;
            parameters[4].Value = model.password;

            int rows = DbHelperMySQL.ExecuteSql(strSql.ToString(), false, parameters);

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
            strSql.Append("delete from frs_database_set.device ");
            strSql.Append(" where id=@id");
            MySqlParameter[] parameters = {
					new MySqlParameter("@id", MySqlDbType.Int32)
			};
            parameters[0].Value = id;

            int rows = DbHelperMySQL.ExecuteSql(strSql.ToString(), false, parameters);
            if (rows > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool DeleteByName(string name)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete from frs_database_set.device ");
            strSql.Append(" where name=@name");
            MySqlParameter[] parameters = {
					new MySqlParameter("@name", MySqlDbType.Int32)
			};
            parameters[0].Value = name;

            int rows = DbHelperMySQL.ExecuteSql(strSql.ToString(), false, parameters);
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
        public DataAngine.Model.device GetModel(int id)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("select id,name,ip,port,user,password from frs_database_set.device ");
            strSql.Append(" where id=@id");
            MySqlParameter[] parameters = {
					new MySqlParameter("@id", MySqlDbType.Int32)
			};
            parameters[0].Value = id;

            DataAngine.Model.device model = new DataAngine.Model.device();
            DataSet ds = DbHelperMySQL.Query(strSql.ToString(), false, parameters);
            if (ds.Tables[0].Rows.Count > 0)
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

            return DbHelperMySQL.Query(strSql.ToString(),false);
        }


        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public DataAngine.Model.device DataRowToModel(DataRow row)
        {
            DataAngine.Model.device model = new DataAngine.Model.device();
            if (row != null)
            {
                if (row["id"] != null && row["id"].ToString() != "")
                {
                    model.id = int.Parse(row["id"].ToString());
                }
                if (row["name"] != null)
                {
                    model.name = row["name"].ToString();
                }
                if (row["ip"] != null)
                {
                    model.name = row["ip"].ToString();
                }
                if (row["port"] != null)
                {
                    model.name = row["port"].ToString();
                }
                if (row["user"] != null)
                {
                    model.name = row["user"].ToString();
                }
                if (row["password"] != null)
                {
                    model.name = row["password"].ToString();
                }
            }
            return model;
        }

        /// <summary>
        /// 获得数据列表
        /// </summary>
        public DataSet GetAllDevice()
        {
            StringBuilder strSql = new StringBuilder();

            strSql.Append("select id,name,ip,port,user,password ");
            strSql.Append(" FROM frs_database_set.device ");

            return DbHelperMySQL.Query(strSql.ToString(), false);

        }

        #endregion  BasicMethod
        #region  ExtensionMethod

        #endregion  ExtensionMethod
    }
}
