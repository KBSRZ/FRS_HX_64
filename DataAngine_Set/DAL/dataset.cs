using System;
using System.Data;
using System.Text;
using MySql.Data.MySqlClient;
using System.Windows.Forms;
using DataAngine_Set.DBUtility;//Please add references
namespace DataAngine_Set.DAL
{
    /// <summary>
    /// 数据访问类:dataset
    /// </summary>
    public partial class dataset
    {
        public dataset()
        { }
        #region  BasicMethod

        /// <summary>
        /// 得到最大ID
        /// </summary>
        public int GetMaxId()
        {
            return DbHelperMySQL.GetMaxID("id", "dataset");
        }

        /// <summary>
        /// 是否存在该记录
        /// </summary>
        public bool Exists(int id)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(1) from dataset");
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
        public bool Add(DataAngine_Set.Model.dataset model)
        {
            //MessageBox.Show("新建数据库");
            //建立相关数据库
            StringBuilder strlibrarySql = new StringBuilder();
            strlibrarySql.Append("CREATE DATABASE `" + model.datasetname + "`;");
            strlibrarySql.Append("USE `" + model.datasetname + "`;");

            strlibrarySql.Append("create table `" + model.datasetname + "`.`user`");
            strlibrarySql.Append("(");
            strlibrarySql.Append("`id` int(11) AUTO_INCREMENT,");
            strlibrarySql.Append("`people_id` varchar(50) NULL,");
            strlibrarySql.Append("`name` nvarchar(50) NULL,");
            strlibrarySql.Append("`gender` char(1) NULL,");
            strlibrarySql.Append("`card_id` varchar(50) NULL,");
            strlibrarySql.Append("`image_id` varchar(60) NULL,");
            strlibrarySql.Append("`face_image_path` varchar(200) NOT NULL,");
            strlibrarySql.Append("`feature_data` LongBlob NOT NULL,");
            strlibrarySql.Append("`type` char NULL,");
            strlibrarySql.Append("`create_time` datetime NOT NULL,");
            strlibrarySql.Append("`modified_time` datetime NOT NULL,");
            strlibrarySql.Append("`quality_score` float,");
            strlibrarySql.Append("PRIMARY KEY (`id`),");
            strlibrarySql.Append("UNIQUE KEY `id` (`id`)");
            strlibrarySql.Append(")ENGINE=InnoDB DEFAULT CHARSET=utf8;");

            strlibrarySql.Append("create table `" + model.datasetname + "`.`hitrecord`");
            strlibrarySql.Append("(");
            strlibrarySql.Append("`id` int(11) AUTO_INCREMENT,");
            strlibrarySql.Append("`face_query_image_path` varchar(200) NOT NULL,");
            strlibrarySql.Append("`threshold` float NOT NULL,");
            strlibrarySql.Append("`occur_time` datetime NOT NULL,");
            strlibrarySql.Append("PRIMARY KEY (`id`),");
            strlibrarySql.Append("UNIQUE KEY `id` (`id`)");
            strlibrarySql.Append(")ENGINE=InnoDB DEFAULT CHARSET=utf8;");

            strlibrarySql.Append("create table `" + model.datasetname + "`.`hitrecord_detail`");
            strlibrarySql.Append("(");
            strlibrarySql.Append("`id` int(11) AUTO_INCREMENT,");
            strlibrarySql.Append("`hit_record_id` int(11) NOT NULL,");
            strlibrarySql.Append("`user_id` int(11) NOT NULL,");
            strlibrarySql.Append("`rank` int(11) NOT NULL,");
            strlibrarySql.Append("`score` float NOT NULL,");
            strlibrarySql.Append("PRIMARY KEY (`id`),");
            strlibrarySql.Append("UNIQUE KEY `id` (`id`)");
            strlibrarySql.Append(")ENGINE=InnoDB DEFAULT CHARSET=utf8;");

            strlibrarySql.Append("create view `" + model.datasetname + "`.`hitalert` as ");
            strlibrarySql.Append("select ");
            strlibrarySql.Append("hit.id,hit.face_query_image_path,hit.threshold,hit.occur_time,detail.id as detail_id,detail.rank,detail.score,usr.id as user_id,usr.name as user_name,usr.gender as user_gender,usr.card_id as user_card_id,usr.people_id as user_people_id,usr.image_id as user_image_id,usr.face_image_path as user_face_image_path,usr.type as user_type,usr.create_time as user_create_time,usr.modified_time as user_modified_time,usr.quality_score as user_quality_score ");
            strlibrarySql.Append("FROM ");
            strlibrarySql.Append("(`" + model.datasetname + "`.`hitrecord_detail` as detail ");
            strlibrarySql.Append("left join `" + model.datasetname + "`.`hitrecord` as hit on detail.hit_record_id=hit.id) ");
            strlibrarySql.Append("left join `" + model.datasetname + "`.`user` as usr on detail.user_id = usr.id;");

            int rows = DbHelperMySQL.ExecuteSql(strlibrarySql.ToString());
            if (rows > 0)
            {
                StringBuilder strSql = new StringBuilder();
                strSql.Append("insert into dataset(");
                strSql.Append("datasetname,type,user,password,ip,port,remark)");
                strSql.Append(" values (");
                strSql.Append("@datasetname,@type,@user,@password,@ip,@port,@remark)");
                MySqlParameter[] parameters = {
					new MySqlParameter("@datasetname", MySqlDbType.VarChar,50),
					new MySqlParameter("@type", MySqlDbType.Int32,11),
					new MySqlParameter("@user", MySqlDbType.VarChar,20),
					new MySqlParameter("@password", MySqlDbType.VarChar,20),
					new MySqlParameter("@ip", MySqlDbType.VarChar,20),
					new MySqlParameter("@port", MySqlDbType.VarChar,20),
					new MySqlParameter("@remark", MySqlDbType.VarChar,50)};
                parameters[0].Value = model.datasetname;
                parameters[1].Value = model.type;
                parameters[2].Value = model.user;
                parameters[3].Value = model.password;
                parameters[4].Value = model.ip;
                parameters[5].Value = model.port;
                parameters[6].Value = model.remark;

                rows = DbHelperMySQL.ExecuteSql(strSql.ToString(), parameters);
                if (rows > 0)
                    return true;
                else
                    return false;
            }
            else
            {
                return false;
            }
        }
        /// <summary>
        /// 更新一条数据
        /// </summary>
        public bool Update(DataAngine_Set.Model.dataset model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update dataset set ");
            strSql.Append("datasetname=@datasetname,");
            strSql.Append("type=@type,");
            strSql.Append("user=@user,");
            strSql.Append("password=@password,");
            strSql.Append("ip=@ip,");
            strSql.Append("port=@port,");
            strSql.Append("remark=@remark");
            strSql.Append(" where id=@id");
            MySqlParameter[] parameters = {
					new MySqlParameter("@datasetname", MySqlDbType.VarChar,50),
					new MySqlParameter("@type", MySqlDbType.Int32,11),
					new MySqlParameter("@user", MySqlDbType.VarChar,20),
					new MySqlParameter("@password", MySqlDbType.VarChar,20),
					new MySqlParameter("@ip", MySqlDbType.VarChar,20),
					new MySqlParameter("@port", MySqlDbType.VarChar,20),
					new MySqlParameter("@remark", MySqlDbType.VarChar,50),
					new MySqlParameter("@id", MySqlDbType.Int32,11)};
            parameters[0].Value = model.datasetname;
            parameters[1].Value = model.type;
            parameters[2].Value = model.user;
            parameters[3].Value = model.password;
            parameters[4].Value = model.ip;
            parameters[5].Value = model.port;
            parameters[6].Value = model.remark;
            parameters[7].Value = model.id;

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
            strSql.Append("delete from dataset ");
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
        /// 删除一条数据
        /// </summary>
        public bool Delete(Model.dataset model)
        {
            //删除相关数据库
            StringBuilder strlibrarySql = new StringBuilder();
            strlibrarySql.Append("DROP DATABASE " + model.datasetname + ";");
            int rows = DbHelperMySQL.ExecuteSql(strlibrarySql.ToString());
            if (rows > 0)
            {
                StringBuilder strSql = new StringBuilder();
                strSql.Append("delete from dataset ");
                strSql.Append(" where id=@id");
                MySqlParameter[] parameters = {
					new MySqlParameter("@id", MySqlDbType.Int32)
			};
                parameters[0].Value = model.id;

                rows = DbHelperMySQL.ExecuteSql(strSql.ToString(), parameters);
                if (rows > 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }
        /// <summary>
        /// 批量删除数据
        /// </summary>
        public bool DeleteList(string idlist)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete from dataset ");
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
        public DataAngine_Set.Model.dataset GetModel(int id)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("select id,datasetname,type,user,password,ip,port,remark from dataset ");
            strSql.Append(" where id=@id");
            MySqlParameter[] parameters = {
					new MySqlParameter("@id", MySqlDbType.Int32)
			};
            parameters[0].Value = id;

            DataAngine_Set.Model.dataset model = new DataAngine_Set.Model.dataset();
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
        public DataAngine_Set.Model.dataset DataRowToModel(DataRow row)
        {
            DataAngine_Set.Model.dataset model = new DataAngine_Set.Model.dataset();
            if (row != null)
            {
                if (row["id"] != null && row["id"].ToString() != "")
                {
                    model.id = int.Parse(row["id"].ToString());
                }
                if (row["datasetname"] != null)
                {
                    model.datasetname = row["datasetname"].ToString();
                }
                if (row["type"] != null && row["type"].ToString() != "")
                {
                    model.type = int.Parse(row["type"].ToString());
                }
                if (row["user"] != null)
                {
                    model.user = row["user"].ToString();
                }
                if (row["password"] != null)
                {
                    model.password = row["password"].ToString();
                }
                if (row["ip"] != null)
                {
                    model.ip = row["ip"].ToString();
                }
                if (row["port"] != null)
                {
                    model.port = row["port"].ToString();
                }
                if (row["remark"] != null)
                {
                    model.remark = row["remark"].ToString();
                }
            }
            return model;
        }

        /// <summary>
        /// 获得数据列表
        /// </summary>
        public DataSet GetList(string strWhere)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select id,datasetname,type,user,password,ip,port,remark ");
            strSql.Append(" FROM dataset ");
            if (strWhere.Trim() != "")
            {
                strSql.Append(" where " + strWhere);
            }
            return DbHelperMySQL.Query(strSql.ToString());
        }

        /// <summary>
        /// 获取记录总数
        /// </summary>
        public int GetRecordCount(string strWhere)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(1) FROM dataset ");
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
            strSql.Append(")AS Row, T.*  from dataset T ");
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
            parameters[0].Value = "dataset";
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

