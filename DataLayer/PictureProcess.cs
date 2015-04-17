using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer
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
        public static string GetFileExtends(string filename)
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
        public static bool CheckFileExtends(string fileExtends)
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
    }
}
