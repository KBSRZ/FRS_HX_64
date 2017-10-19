using System;
namespace DataAngine_Set.Model
{
	/// <summary>
	/// device:实体类(属性说明自动提取数据库字段的描述信息)
	/// </summary>
	[Serializable]
	public partial class device
	{
		public device()
		{}
		#region Model
		private int _id;
		private string _name;
		private string _address;
		private string _departmentmentid;
		private double? _longitude;
		private double? _latitude;
		private int? _locationtype;
		private string _remark;
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
		public string name
		{
			set{ _name=value;}
			get{return _name;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string address
		{
			set{ _address=value;}
			get{return _address;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string departmentmentid
		{
			set{ _departmentmentid=value;}
			get{return _departmentmentid;}
		}
		/// <summary>
		/// 
		/// </summary>
		public double? longitude
		{
			set{ _longitude=value;}
			get{return _longitude;}
		}
		/// <summary>
		/// 
		/// </summary>
		public double? latitude
		{
			set{ _latitude=value;}
			get{return _latitude;}
		}
		/// <summary>
		/// 
		/// </summary>
		public int? locationtype
		{
			set{ _locationtype=value;}
			get{return _locationtype;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string remark
		{
			set{ _remark=value;}
			get{return _remark;}
		}
		#endregion Model

	}
}

