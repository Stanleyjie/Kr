using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using DataModel.Other;
using Microsoft.AspNetCore.Mvc;
using ServiceExt;

namespace PeHubCore.Controllers
{
    /// <summary>
    ///通用
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class CommonApiController : BaseApiController
    {
        /// <summary>
        /// 上传文件
        /// </summary>
        /// <returns></returns>
        [HttpPost("FilePicSave")]
        public async Task<IActionResult> FilePicSave()
        {
            try
            {
                var datetime = DateTime.Now.ToFileTimeUtc().ToString();
                //var imgmodel = new { img_url};
                string url="";
                List<int> listid = new List<int>();
                //var imgtype = Request.Form["imgtype"].ToString();
                //var rid = Request.Form["rid"].ToString();
                //var fff = Request.Form;
                //if (string.IsNullOrEmpty(imgtype) || string.IsNullOrEmpty(rid))
                //{
                //    return Content("请传输正确的参数");
                //}

                var files = Request.Form.Files;
                long size = files.Sum(f => f.Length);
                string webRootPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot");

                foreach (var formFile in files)
                {
                    if (formFile.Length > 0)
                    {
                        string fileExt = formFile.FileName.Split('.').LastOrDefault(); //文件扩展名，不含“.”
                        long fileSize = formFile.Length; //获得文件大小，以字节为单位
                        //string newFileName = System.Guid.NewGuid().ToString() + "." + fileExt; //随机生成新的文件名
                        //var filePath = webRootPath + "/ImgHub/" + imgtype + "/" + newFileName;
                        // var filePath = webRootPath + "/ImgHub/" + formFile.FileName;
                        var filePath = webRootPath + "/ImgHub/" +datetime +formFile.FileName;

                        if (!Directory.Exists(webRootPath + "/ImgHub"))//判断上传文件夹是否存在，若不存在，则创建
                            Directory.CreateDirectory(webRootPath + "/ImgHub");//创建文件夹

                        //if (!Directory.Exists(webRootPath + "/ImgHub/" + imgtype))//判断上传文件夹是否存在，若不存在，则创建
                        //if (!Directory.Exists(webRootPath + "/ImgHub/"))//判断上传文件夹是否存在，若不存在，则创建
                        //        //Directory.CreateDirectory(webRootPath + "/ImgHub/" + imgtype);//创建文件夹
                        //        Directory.CreateDirectory(webRootPath + "/ImgHub/");//创建文件夹

                        using (var stream = new FileStream(filePath, FileMode.Create))
                        {
                            await formFile.CopyToAsync(stream);
                        }
                        //imgmodel = new imagelink();

                        //imgmodel.create_time = DateTime.Now;
                        //imgmodel.update_time = DateTime.Now;
                        //if (imgtype == "product")
                        //{
                        //    imgmodel.img_type = (int)imgEnum.product;
                        //}
                        //else if (imgtype == "customer")
                        //{
                        //    imgmodel.img_type = (int)imgEnum.customer;
                        //}
                        //else if (imgtype == "rotation")
                        //{
                        //    imgmodel.img_type = (int)imgEnum.rotation;
                        //}
                        //else
                        //{
                        //    return Content("请传输正确的参数");
                        //}
                        //imgmodel.relation_id = int.Parse(rid.ToString());
                        //imgmodel.state = 1;
                        //imgmodel.img_url = "/ImgHub/" + imgtype + "/" + newFileName;
                        //_commonService.AddImgInfo(imgmodel);
                        //listid.Add(imgmodel.id);
                        
                        url = "/ImgHub/"+ datetime + formFile.FileName; 
                        // url = "/ImgHub/"+formFile.FileName;
                    }
                }
                //return Ok(new { count = files.Count, ids = listid, url = imgmodel.img_url });
                result.returnData = url;
                return Ok(result);
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }

 


        /// <summary>
        /// 获取最新版本发布更新前端缓存使用
        /// </summary>
        /// <param name="pData"></param>
        /// <returns></returns>
        [HttpGet("getApiVersion")]
        public async Task<IActionResult> getApiVersion()
        {
            string model = await Task.Factory.StartNew(() =>
            {
                return Appsettings.GetSectionValue("WxHub:Version1");
            });
            if (string.IsNullOrEmpty(model))
            {
                return NotFound();
            }
            result.returnData = model;
            return Ok(result);
        }

    }
}