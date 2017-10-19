using System;
namespace DataAgine_Set.Model
{
	/// <summary>
	/// task:实体类(属性说明自动提取数据库字段的描述信息)
	/// </summary>
	[Serializable]
	public partial class task
	{
		public task()
		{}
		#region Model
		private int _id;
		private int _databaseid;
		private int _deviceid;
		/// <summary>
		/// auto_increment
		/// </summary>
		public int id
		{
			set{ _id=value;}
			get{return _id;}
		}
		/// <summary>
		/// 
		/// </summary>
		public int databaseid
		{
			set{ _databaseid=value;}
			get{return _databaseid;}
		}
		/// <summary>
		/// 
		/// </summary>
		public int deviceid
		{
			set{ _deviceid=value;}
			get{return _deviceid;}
		}
		#endregion Model

	}
}

