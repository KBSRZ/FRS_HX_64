using DataAngine_Set.BLL;
using FRSServerHttp.Model;
using FRSServerHttp.Server;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FRSServerHttp.Service
{
    /// <summary>
    /// 比对两张图片
    /// </summary>
    class VerifyingService : BaseService
    {
        dataset bll = new dataset();
        public static string Domain
        {
            get
            {
                return "person-database";
            }
        }


        public override void OnGet(HttpRequest request, HttpResponse response)
        {
        }

        /// <summary>
        /// Post时调用
        /// </summary>
        public override void OnPost(HttpRequest request, HttpResponse response)
        {
            bool status = false;
            if (request.Operation == "verify")//添加一条数据
            {
                Log.Debug("比较图片");

                //OneVsOne
                if (request.RestConvention == "0")
                {              
                    VerifyOneVsOne verify = VerifyOneVsOne.CreateInstanceFromJSON(request.PostParams);
                    if (verify != null)
                    {
                       Bitmap Bitmapsrc = BytesToBitmap(verify.PicSrc);
                       Bitmap Bitmapdst = BytesToBitmap(verify.PicDst);
                       double score=fa.Compare(Bitmapsrc, Bitmapdst);

                       response.SetContent(JsonConvert.SerializeObject(score));
                        
                    }

                }
                else
                {                   
                    VerifyOneVsN verify = VerifyOneVsN.CreateInstanceFromJSON(request.PostParams);
                    if (verify != null)
                    {
                        int DatasetId = Convert.ToInt32(request.RestConvention);
                        DataAngine_Set.Model.dataset ds = new DataAngine_Set.Model.dataset();
                        ds = bll.GetModel(DatasetId);

                        Bitmap Bitmapsrc = BytesToBitmap(verify.PicSrc);
                        fa.LoadData(ds.datasetname);
                        FRS.HitAlert[] hits = fa.Search(Bitmapsrc);
                        string msg = JsonConvert.SerializeObject(Model.HitAlert.CreateInstanceFromFRSHitAlert(hits));
                        response.SetContent(msg);
                    }
                }

            }
            response.Send();

        }

        public static Bitmap BytesToBitmap(byte[] Bytes)
        {
            MemoryStream stream = null;
            try
            {
                stream = new MemoryStream(Bytes);
                return new Bitmap((Image)new Bitmap(stream));
            }
            catch (ArgumentNullException ex)
            {
                throw ex;
            }
            catch (ArgumentException ex)
            {
                throw ex;
            }
            finally
            {
                stream.Close();
            }
        }   
  
    }
}
