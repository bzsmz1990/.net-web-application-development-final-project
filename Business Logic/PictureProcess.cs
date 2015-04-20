using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.IO;

namespace Business_Logic
{
    public class PictureProcess
    {
        //Valid file type
        //Since different companies has different file extension for their RAW file
        //We only conside Nikon(.nef), Canon(.cr2), SONY(.arw), PENTAX(.pef) and LEICA(.dng)
        public enum ValidFileType
        {
            jpg, bmp, cr2, nef, arw, pef, dng
        }

        //Extract file extension
        public string GetFileExtends(string filename)
        {
            string ext = null;
            if (filename.IndexOf('.') > 0)
            {
                string[] fs = filename.Split('.');
                ext = fs[fs.Length - 1];
            }
            return ext;
        }

        //Check whether file's extension is valid
        public bool CheckFileExtends(string fileExtends)
        {
            bool status = false;
            fileExtends = fileExtends.ToLower();
            string[] fe = Enum.GetNames(typeof(ValidFileType));
            for (int i = 0; i < fe.Length; i++)
            {
                if (fe[i].ToLower() == fileExtends)
                {
                    status = true;
                    break;
                }
            }
            return status;
        }

        public Image byteArrayToImage(byte[] byteArrayIn)
        {
            MemoryStream ms = new MemoryStream(byteArrayIn);
            Image returnImage = Image.FromStream(ms);
            return returnImage;
        }

        public byte[] imageToByteArray(Image imageIn)
        {
            MemoryStream ms = new MemoryStream();
            imageIn.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);
            return ms.ToArray();
        }

        //Apply validation here
        public string ValidatePicture(string picType, int picSize)
        {
            PictureProcess picPro = new PictureProcess();
            string validation = "Valid";
            if (picType != "image/jpeg")
            {
                if (!picPro.CheckFileExtends(picType))
                {
                    validation = "File type is invalid! We accept jpg, bmp, cr2, nef, arw, pef, dng";
                }
            }

            if (picSize < (2 * 1024 * 1024))
            {
                validation = "File size shoud be bigger than 2MB";
            }
            return validation;
        }
    }
}
