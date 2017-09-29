using System;
namespace DataAngine.Model
{
	/// <summary>
	/// user:实体类(属性说明自动提取数据库字段的描述信息)
	/// </summary>
	[Serializable]
	public partial class user
	{
		public user()
		{}
		#region Model
		private int _id;
        private string _people_id;
		private string _name;
		private string _gender;
        private string _card_id;
        private string _image_id;
		private string _face_image_path;
		private byte[] _feature_data;
		private string _type;
		private DateTime _create_time=DateTime.Now;
		private DateTime _modified_time=DateTime.Now;
		private float? _quality_score;
		/// <summary>
		/// auto_increment
		/// </summary>
		public int id
		{
			set{ _id=value;}
			get{return _id;}
		}

        public string people_id
		{
            set { _people_id = value; }
            get { return _people_id; }
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
		public string gender
		{
			set{ _gender=value;}
			get{return _gender;}
		}

        public string card_id
        {
            set { _card_id = value; }
            get { return _card_id; }
        }

        public string image_id
        {
            set { _image_id = value; }
            get { return _image_id; }
        }

		/// <summary>
		/// 
		/// </summary>
		public string face_image_path
		{
			set{ _face_image_path=value;}
			get{return _face_image_path;}
		}
		/// <summary>
		/// 
		/// </summary>
        public byte[] feature_data
		{
			set{ _feature_data=value;}
			get{return _feature_data;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string type
		{
			set{ _type=value;}
			get{return _type;}
		}
		/// <summary>
		/// 
		/// </summary>
		public DateTime create_time
		{
			set{ _create_time=value;}
			get{return _create_time;}
		}
		/// <summary>
		/// 
		/// </summary>
		public DateTime modified_time
		{
			set{ _modified_time=value;}
			get{return _modified_time;}
		}
		/// <summary>
		/// 
		/// </summary>
		public float? quality_score
		{
			set{ _quality_score=value;}
			get{return _quality_score;}
		}
		#endregion Model

	}
}

