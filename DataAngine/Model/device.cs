using System;


namespace DataAngine.Model
{
    /// <summary>
    /// user:实体类(属性说明自动提取数据库字段的描述信息)
    /// </summary>
    [Serializable]
    public partial class device
    {
        public device()
        { }
        #region Model
        private int _id;
        private string _name;
        private string _ip;
        private string _port;
        private string _user;
        private string _password;

        /// <summary>
        /// auto_increment
        /// </summary>
        public int id
        {
            set { _id = value; }
            get { return _id; }
        }

        /// <summary>
        /// 
        /// </summary>
        public string name
        {
            set { _name = value; }
            get { return _name; }
        }

        /// <summary>
        /// 
        /// </summary>
        public string ip
        {
            set { _ip = value; }
            get { return _ip; }
        }

        /// <summary>
        /// 
        /// </summary>
        public string port
        {
            set { _port = value; }
            get { return _port; }
        }

        /// <summary>
        /// 
        /// </summary>
        public string user
        {
            set { _user = value; }
            get { return _user; }
        }

        /// <summary>
        /// 
        /// </summary>
        public string password
        {
            set { _password = value; }
            get { return _password; }
        }
        #endregion Model

    }
}
