using System;
using System.Data;
using System.Collections.Generic;
//using DataAngine_Set.Common;
using DataAngine.Model;

namespace DataAngine.BLL
{
	/// <summary>
	/// user
	/// </summary>
	public partial class user
	{
		private readonly DataAngine.DAL.user dal=new DataAngine.DAL.user();
		public user()
		{}
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
		public bool Add(DataAngine.Model.user model)
		{
			return dal.Add(model);
           
		}

        /// <summary>
        /// 增加一条数据
        /// </summary>
        public bool Add(DataAngine.Model.user model, string library)
        {
            return dal.Add(model, library);

        }

		/// <summary>
		/// 更新一条数据
		/// </summary>
		public bool Update(DataAngine.Model.user model)
		{
			return dal.Update(model);
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
		/// 删除一条数据
		/// </summary>
		public bool DeleteList(string idlist )
		{
			return dal.DeleteList(idlist );
		}

		/// <summary>
		/// 得到一个对象实体
		/// </summary>
		public DataAngine.Model.user GetModel(int id)
		{
			
			return dal.GetModel(id);
		}

        public List<Model.user> GetAllUser()
        {

            DataSet ds = dal.GetList(string.Empty);
            List<Model.user> allUsers;
            DataTable dt = ds.Tables[0];

            allUsers = DataTableToList(dt);
        
            return allUsers;
        }


        public List<Model.user> GetAllUser(string libraryname)
        {

            DataSet ds = dal.GetList(string.Empty,libraryname);
            List<Model.user> allUsers;
            DataTable dt = ds.Tables[0];

            allUsers = DataTableToList(dt);

            return allUsers;
        }
        ///// <summary>
        ///// 得到一个对象实体，从缓存中
        ///// </summary>
        //public FRS.Model.user GetModelByCache(int id)
        //{
			
        //    string CacheKey = "userModel-" + id;
        //    object objModel = DataAngine_Set.Common.DataCache.GetCache(CacheKey);
        //    if (objModel == null)
        //    {
        //        try
        //        {
        //            objModel = dal.GetModel(id);
        //            if (objModel != null)
        //            {
        //                int ModelCache = DataAngine_Set.Common.ConfigHelper.GetConfigInt("ModelCache");
        //                DataAngine_Set.Common.DataCache.SetCache(CacheKey, objModel, DateTime.Now.AddMinutes(ModelCache), TimeSpan.Zero);
        //            }
        //        }
        //        catch{}
        //    }
        //    return (FRS.Model.user)objModel;
        //}

		/// <summary>
		/// 获得数据列表
		/// </summary>
		public DataSet GetList(string strWhere)
		{
			return dal.GetList(strWhere);
		}

        public DataSet GetPicPathList(string strWhere)
        {
            return dal.GetPicPathList(strWhere);
        }
        public DataSet GetPicPathList(string strWhere,string library)
        {
            return dal.GetPicPathList(strWhere, library);
        }
        //分页查询
        public DataSet GetPicPathList(string strWhere, int startIndex, int endIndex)
        {
            return dal.GetPicPathList(strWhere,startIndex,endIndex);
        }
        public DataSet GetPicPathList(string strWhere, int startIndex, int endIndex,string library)
        {
            return dal.GetPicPathList(strWhere, startIndex, endIndex, library);
        }
		/// <summary>
		/// 获得数据列表
		/// </summary>
		public List<DataAngine.Model.user> GetModelList(string strWhere)
		{
			DataSet ds = dal.GetList(strWhere);
			return DataTableToList(ds.Tables[0]);
		}
		/// <summary>
		/// 获得数据列表
		/// </summary>
		public List<DataAngine.Model.user> DataTableToList(DataTable dt)
		{
			List<DataAngine.Model.user> modelList = new List<DataAngine.Model.user>();
			int rowsCount = dt.Rows.Count;
			if (rowsCount > 0)
			{
				DataAngine.Model.user model;
				for (int n = 0; n < rowsCount; n++)
				{
					model = dal.DataRowToModel(dt.Rows[n]);
					if (model != null)
					{
						modelList.Add(model);
					}
				}
			}
			return modelList;
		}

		/// <summary>
		/// 获得数据列表
		/// </summary>
		public DataSet GetAllList()
		{
			return GetList("");
		}

		/// <summary>
		/// 分页获取数据列表
		/// </summary>
		public int GetRecordCount(string strWhere)
		{
			return dal.GetRecordCount(strWhere);
		}
		/// <summary>
		/// 分页获取数据列表
		/// </summary>
		public DataSet GetListByPage(string strWhere, string orderby, int startIndex, int pageSize)
		{
            return dal.GetListByPage(strWhere, orderby, startIndex, pageSize);
		}
		/// <summary>
		/// 分页获取数据列表
		/// </summary>
		//public DataSet GetList(int PageSize,int PageIndex,string strWhere)
		//{
			//return dal.GetList(PageSize,PageIndex,strWhere);
		//}

		#endregion  BasicMethod
		#region  ExtensionMethod

		#endregion  ExtensionMethod
	}
}

