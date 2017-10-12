using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DataAngine.BLL
{
    public partial  class hitalert
    {

        private readonly DataAngine.DAL.hitalert dal=new DataAngine.DAL.hitalert();
        #region  BasicMethod
        /// <summary>
        /// 添加一条数据
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public bool Add(DataAngine.Model.hitalert model)
        {
           return dal.Add(model);
           
        }

        /// <summary>
        /// 更新一条数据
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public bool Update(DataAngine.Model.hitalert model)
        {
            return dal.Update(model);

        }
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
        public List<DataAngine.Model.hitalert> DataTableToList(DataTable table)
        {
            List<DataAngine.Model.hitalert> modelList = new List<Model.hitalert>();
            List<DataTable> newTables = new List<DataTable>();
            HashSet<int> groupIds = new HashSet<int>();
            foreach (DataRow row in table.Rows)
            {
                int groupId = int.Parse(row["id"].ToString());
                if (!groupIds.Contains(groupId))
                {
                    groupIds.Add(groupId);
                    DataTable newTable = table.Clone();
                    newTable.TableName = groupId.ToString();
                    newTable.ImportRow(row);
                    newTables.Add(newTable);
                }
                else
                {
                    DataTable newTable = newTables.Find(x => x.TableName == groupId.ToString());
                    newTable.ImportRow(row);
                }
            }

            foreach (var tb in newTables)
            {
                modelList.Add(dal.DataTableToModel(tb));
            }
            return modelList;
        }
   
		#endregion  BasicMethod
		#region  ExtensionMethod
        /// <summary>
        /// 通过时间得到对象实体
        /// </summary>
        public List<DataAngine.Model.hitalert> GetModelByTime(DateTime startTime, DateTime endTime)
        {
            DataSet ds = dal.GetModelByTime(startTime, endTime);
         
            return DataTableToList(ds.Tables[0]);

        }

        public DataSet GetListByTime(DateTime startTime, DateTime endTime)
        {
            string strWhere = string.Format("occur_time between '{0}' and '{1}'", startTime.ToString("yyyy-MM-dd HH:mm:ss"), endTime.ToString("yyyy-MM-dd HH:mm:ss"));
            return dal.GetList(strWhere);
        }

        public DataSet GetListByTime(DateTime startTime, DateTime endTime, string library)
        {
            string strWhere = string.Format("occur_time between '{0}' and '{1}'", startTime.ToString("yyyy-MM-dd HH:mm:ss"), endTime.ToString("yyyy-MM-dd HH:mm:ss"));
            return dal.GetList(strWhere,library);
        }

        //分页时间查询
        public DataSet GetListByTime(DateTime startTime, DateTime endTime, int startIndex, int pageSize)
        {
            string strWhere = string.Format("occur_time between '{0}' and '{1}'", startTime.ToString("yyyy-MM-dd HH:mm:ss"), endTime.ToString("yyyy-MM-dd HH:mm:ss"));
            return dal.GetList(strWhere, startIndex, pageSize);
        }

        //分页时间查询
        public DataSet GetListByTime(DateTime startTime, DateTime endTime, int startIndex, int pageSize, string library)
        {
            string strWhere = string.Format("occur_time between '{0}' and '{1}'", startTime.ToString("yyyy-MM-dd HH:mm:ss"), endTime.ToString("yyyy-MM-dd HH:mm:ss"));
            return dal.GetList(strWhere, startIndex, pageSize, library);
        }  
        
		#endregion  ExtensionMethod

    }
}
