using System;
using System.Data;
using System.Collections.Generic;
//using Maticsoft.Common;
using DataAngine_Set.Model;
namespace DataAngine_Set.BLL
{
	/// <summary>
	/// dataset
	/// </summary>
	public partial class dataset
	{
		private readonly DataAngine_Set.DAL.dataset dal=new DataAngine_Set.DAL.dataset();
		public dataset()
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
		public bool Add(DataAngine_Set.Model.dataset model)
		{
			return dal.Add(model);
		}

		/// <summary>
		/// 更新一条数据
		/// </summary>
		public bool Update(DataAngine_Set.Model.dataset model)
		{
			return dal.Update(model);
		}

        ///// <summary>
        ///// 删除一条数据
        ///// </summary>
        //public bool Delete(int id)
        //{
			
        //    return dal.Delete(id);
        //}

        /// <summary>
        /// 删除一条数据
        /// </summary>
        public bool Delete(int id)
        {
            Model.dataset ds = new DataAngine_Set.Model.dataset();
            dataset datasetbll = new dataset();
            ds = datasetbll.GetModel(id);
            return dal.Delete(ds);
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
		public DataAngine_Set.Model.dataset GetModel(int id)
		{
			
			return dal.GetModel(id);
		}

        ///// <summary>
        ///// 得到一个对象实体，从缓存中
        ///// </summary>
        //public DataAngine_Set.Model.dataset GetModelByCache(int id)
        //{
			
        //    string CacheKey = "datasetModel-" + id;
        //    object objModel = Maticsoft.Common.DataCache.GetCache(CacheKey);
        //    if (objModel == null)
        //    {
        //        try
        //        {
        //            objModel = dal.GetModel(id);
        //            if (objModel != null)
        //            {
        //                int ModelCache = Maticsoft.Common.ConfigHelper.GetConfigInt("ModelCache");
        //                Maticsoft.Common.DataCache.SetCache(CacheKey, objModel, DateTime.Now.AddMinutes(ModelCache), TimeSpan.Zero);
        //            }
        //        }
        //        catch{}
        //    }
        //    return (DataAngine_Set.Model.dataset)objModel;
        //}

		/// <summary>
		/// 获得数据列表
		/// </summary>
		public DataSet GetList(string strWhere)
		{
			return dal.GetList(strWhere);
		}
		/// <summary>
		/// 获得数据列表
		/// </summary>
		public List<DataAngine_Set.Model.dataset> GetModelList(string strWhere)
		{
			DataSet ds = dal.GetList(strWhere);
			return DataTableToList(ds.Tables[0]);
		}
		/// <summary>
		/// 获得数据列表
		/// </summary>
		public List<DataAngine_Set.Model.dataset> DataTableToList(DataTable dt)
		{
			List<DataAngine_Set.Model.dataset> modelList = new List<DataAngine_Set.Model.dataset>();
			int rowsCount = dt.Rows.Count;
			if (rowsCount > 0)
			{
				DataAngine_Set.Model.dataset model;
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
		public DataSet GetListByPage(string strWhere, string orderby, int startIndex, int endIndex)
		{
			return dal.GetListByPage( strWhere,  orderby,  startIndex,  endIndex);
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

