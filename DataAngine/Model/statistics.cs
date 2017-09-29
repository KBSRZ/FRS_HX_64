using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DataAngine.Model
{
    [Serializable]
    public partial class statistics
    {
        private int id;
        private int catchFaceImgCount;
        private int matchFaceCount;
        private DateTime startTime = DateTime.Now;
        private DateTime endTime = DateTime.Now;

        public int Id
        {
            set { id = value; }
            get { return id; }
        }


        public int CatchFaceImgCount
        {
            set { catchFaceImgCount = value; }
            get { return catchFaceImgCount; }
        }


        public int MatchFaceCount
        {
            set { matchFaceCount = value; }
            get { return matchFaceCount; }
        }

        public DateTime StartTime
        {
            set { startTime = value; }
            get { return startTime; }
        }

        public DateTime EndTime
        {
            set { endTime = value; }
            get { return endTime; }
        }


    }
}
