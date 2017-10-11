using System;
using System.Data;
using System.Text;
using System.Windows.Forms;
using MySql.Data.MySqlClient;
using System.Data.OracleClient;

using DataAngine.DBUtility;//Please add references

namespace DataAngine.DAL
{
    public partial class table
    {
        public table()
        {}
        #region  BasicMethod
        /// <summary>
        /// 得到最大ID
        /// </summary>
        public int GetMaxId()
        {
            return DbHelperMySQL.GetMaxID("id", "table",false);
        }

        /// <summary>
        /// 是否存在该记录
        /// </summary>
        public bool Exists(int id)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(1) from frs_database_set.table");
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
        public bool Add(DataAngine.Model.table model)
        {
            StringBuilder strSql = new StringBuilder();


            strSql.Append("insert into frs_database_set.table(");
            strSql.Append("name)");
            strSql.Append(" values (");
            strSql.Append("@name)");
            MySqlParameter[] parameters = {                   
					new MySqlParameter("@name", MySqlDbType.VarChar,50)};
          
            parameters[0].Value = model.name;           

            int rows = DbHelperMySQL.ExecuteSql(strSql.ToString(),false, parameters);          

            if (rows > 0){
                //建立相关数据库
                StringBuilder strlibrarySql = new StringBuilder();


                strlibrarySql.Append("CREATE DATABASE `"+ model.name +"`;");
                strlibrarySql.Append("USE `" + model.name + "`;");

                strlibrarySql.Append("create table `" + model.name + "`.`user`");
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

                strlibrarySql.Append("create table `" + model.name + "`.`hitrecord`");
                strlibrarySql.Append("(");
                strlibrarySql.Append("`id` int(11) AUTO_INCREMENT,");
                strlibrarySql.Append("`face_query_image_path` varchar(200) NOT NULL,");
                strlibrarySql.Append("`threshold` float NOT NULL,");
                strlibrarySql.Append("`occur_time` datetime NOT NULL,");
                strlibrarySql.Append("PRIMARY KEY (`id`),");
                strlibrarySql.Append("UNIQUE KEY `id` (`id`)");
                strlibrarySql.Append(")ENGINE=InnoDB DEFAULT CHARSET=utf8;");

                strlibrarySql.Append("create table `" + model.name + "`.`hitrecord_detail`");
                strlibrarySql.Append("(");
                strlibrarySql.Append("`id` int(11) AUTO_INCREMENT,");
                strlibrarySql.Append("`hit_record_id` int(11) NOT NULL,");
                strlibrarySql.Append("`user_id` int(11) NOT NULL,");
                strlibrarySql.Append("`rank` int(11) NOT NULL,");
                strlibrarySql.Append("`score` float NOT NULL,");
                strlibrarySql.Append("PRIMARY KEY (`id`),");
                strlibrarySql.Append("UNIQUE KEY `id` (`id`)");
                strlibrarySql.Append(")ENGINE=InnoDB DEFAULT CHARSET=utf8;");

                strlibrarySql.Append("create view `" + model.name + "`.`hitalert` as ");
                strlibrarySql.Append("select ");
                strlibrarySql.Append("hit.id,hit.face_query_image_path,hit.threshold,hit.occur_time,detail.id as detail_id,detail.rank,detail.score,usr.id as user_id,usr.name as user_name,usr.gender as user_gender,usr.card_id as user_card_id,usr.people_id as user_people_id,usr.image_id as user_image_id,usr.face_image_path as user_face_image_path,usr.type as user_type,usr.create_time as user_create_time,usr.modified_time as user_modified_time,usr.quality_score as user_quality_score ");
                strlibrarySql.Append("FROM ");
                strlibrarySql.Append("(`" + model.name + "`.`hitrecord_detail` as detail ");
                strlibrarySql.Append("left join `" + model.name + "`.`hitrecord` as hit on detail.hit_record_id=hit.id) ");
                strlibrarySql.Append("left join `" + model.name + "`.`user` as usr on detail.user_id = usr.id;");

                rows = DbHelperMySQL.ExecuteSql(strlibrarySql.ToString(), parameters);
                return true;
            }
            else{
                return false;
            }
        }
       

        /// <summary>
        /// 删除一条数据
        /// </summary>
        public bool Delete(int id)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete from frs_database_set.table ");
            strSql.Append(" where id=@id");
            MySqlParameter[] parameters = {
					new MySqlParameter("@id", MySqlDbType.Int32)
			};
            parameters[0].Value = id;

            int rows = DbHelperMySQL.ExecuteSql(strSql.ToString(),false, parameters);
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
            strSql.Append("delete from frs_database_set.table ");
            strSql.Append(" where name=@name");
            MySqlParameter[] parameters = {
					new MySqlParameter("@name", MySqlDbType.Int32)
			};
            parameters[0].Value = name;

            int rows = DbHelperMySQL.ExecuteSql(strSql.ToString(), false,parameters);
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
        public DataAngine.Model.table GetModel(int id)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("select id,name from frs_database_set.table ");
            strSql.Append(" where id=@id");
            MySqlParameter[] parameters = {
					new MySqlParameter("@id", MySqlDbType.Int32)
			};
            parameters[0].Value = id;

            DataAngine.Model.table model = new DataAngine.Model.table();
            DataSet ds = DbHelperMySQL.Query(strSql.ToString(), false,parameters);
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
        public DataAngine.Model.table DataRowToModel(DataRow row)
        {
            DataAngine.Model.table model = new DataAngine.Model.table();
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
            }
            return model;
        }
       
        /// <summary>
        /// 获得数据列表
        /// </summary>
        public DataSet GetAllTable()
        {            
            StringBuilder strSql = new StringBuilder();

            strSql.Append("select id,name ");
            strSql.Append(" FROM frs_database_set.table ");
           
            return DbHelperMySQL.Query(strSql.ToString(),false);
           
        }           

        #endregion  BasicMethod
        #region  ExtensionMethod

        #endregion  ExtensionMethod
    }
}
