using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace HumanIK.BUSINESS.Concrete
{
    public static class ImageSaver
    { 
        private static Account account = new Account(
               "drgl2pfow",
               "331271952486897",
                "bcSuOwtq7ap2gMzqODAV1xIHa6k");

        private static Cloudinary _cloudinary = new Cloudinary(account);

        public static async Task<string> SaveImage(IFormFile image)
        {
            if (image == null)
                return null;
            
            var filePath = Path.GetTempFileName();//gecici dosya yolu oluşturur ve yolunu verir.

            using (var stream = File.Create(filePath))
            {
                await image.CopyToAsync(stream);//gelen image'i gecici açtığı dosyanın stream'ine kopyalıyor.
            }

            var uploadParams = new ImageUploadParams()
            {
                File = new FileDescription(filePath),//dosya yolunu veriyor
                PublicId = image.FileName//dosya ismi
            };

            var uploadResult = _cloudinary.Upload(uploadParams);//resmi yükler
            return uploadResult.Url.ToString();//resmin sonucunda json içindeki url'i çeker.
        }

       


    }
}
