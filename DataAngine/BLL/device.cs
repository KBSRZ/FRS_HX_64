using System;
using System.Data;
using System.Collections.Generic;
//using Maticsoft.Common;
using DataAngine.Model;

namespace DataAngine.BLL
{
    public partial class device
    {
        private readonly DataAngine.DAL.device dal = new DataAngine.DAL.device();
        public device()
        { }
        #region  BasicMethod

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
        public bool Add(DataAngine.Model.device model)
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
        public DataAngine.Model.device GetModel(int id)
        {

            return dal.GetModel(id);
        }

        /// <summary>
        /// 获得数据列表
        /// </summary>
        public DataSet GetDevice(string name)
        {
            return dal.GetDevice(name);
        }



        /// <summary>
        /// 获得数据列表
        /// </summary>
        public DataSet GetAllDevice()
        {
            return dal.GetAllDevice();
        }



        #endregion  BasicMethod
        #region  ExtensionMethod

        #endregion  ExtensionMethod
    }
}
