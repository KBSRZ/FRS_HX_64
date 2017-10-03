using System;
using System.Data;
using System.Collections.Generic;
//using Maticsoft.Common;
using DataAngine.Model;

namespace DataAngine.BLL
{
    public partial class table
    {
        private readonly DataAngine.DAL.table dal = new DataAngine.DAL.table();
        public table()
        { }
        #region  BasicMethod

        /// <summary>
        /// 得到最大ID
        /// </summary>
        public int GetMaxId()
        {
            return dal.GetMaxId();
        }

        /// <summary>
        /// 是否存在该记录
        /// </summary>
        public bool Exists(int id)
        {
            return dal.Exists(id);
        }

        /// <summary>
        /// 增加一条数据
        /// </summary>
        public bool Add(DataAngine.Model.table model)
        {
            return dal.Add(model);

        }
       

        /// <summary>
        /// 删除一条数据
        /// </summary>
        public bool Delete(int id)
        {

            return dal.Delete(id);
        }
        public bool DeleteByName(string name)
        {
            return dal.DeleteByName(name);
        }
        

        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public DataAngine.Model.table GetModel(int id)
        {

            return dal.GetModel(id);
        }

       

        /// <summary>
        /// 获得数据列表
        /// </summary>
        public DataSet GetAllTable()
        {
            return dal.GetAllTable();
        }

              

        #endregion  BasicMethod
        #region  ExtensionMethod

        #endregion  ExtensionMethod
    }
}
