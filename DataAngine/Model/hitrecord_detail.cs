using System;
namespace DataAngine.Model
{
	/// <summary>
	/// hitrecord_detail:实体类(属性说明自动提取数据库字段的描述信息)
	/// </summary>

	public partial class hitrecord_detail
	{
		public hitrecord_detail()
		{}
		#region Model
		private int _id;
		private int _hit_record_id;
		private int _user_id;
		private int _rank;
		private float _score;
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
		public int hit_record_id
		{
			set{ _hit_record_id=value;}
			get{return _hit_record_id;}
		}
		/// <summary>
		/// 
		/// </summary>
		public int user_id
		{
			set{ _user_id=value;}
			get{return _user_id;}
		}
		/// <summary>
		/// 
		/// </summary>
		public int rank
		{
			set{ _rank=value;}
			get{return _rank;}
		}
		/// <summary>
		/// 
		/// </summary>
		public float score
		{
			set{ _score=value;}
			get{return _score;}
		}
		#endregion Model

	}
}

