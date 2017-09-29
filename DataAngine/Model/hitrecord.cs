using System;
namespace DataAngine.Model
{
	/// <summary>
	/// hitrecord:实体类(属性说明自动提取数据库字段的描述信息)
	/// </summary>

	public partial class hitrecord
	{
		public hitrecord()
		{}
		#region Model
		private int _id;
		private string _face_query_image_path;
        private float _threshold;
		private DateTime _occur_time;
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
		public string face_query_image_path
		{
			set{ _face_query_image_path=value;}
			get{return _face_query_image_path;}
		}
		/// <summary>
		/// 
		/// </summary>
		public float threshold
		{
			set{ _threshold=value;}
			get{return _threshold;}
		}
		/// <summary>
		/// 
		/// </summary>
		public DateTime occur_time
		{
			set{ _occur_time=value;}
			get{return _occur_time;}
		}
		#endregion Model

	}
}

