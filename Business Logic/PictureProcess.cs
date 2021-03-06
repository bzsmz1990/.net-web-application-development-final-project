﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.IO;
using DataLayer;

namespace Business_Logic
{
    public class PictureProcess
    {
        private static int targetWidth = 600;
        private static int targetHeight = 400;

        //Extract file extension
        public string GetFileExtends(string filename)
        {
            string ext = null;
            if (filename.IndexOf('.') > 0)
            {
                string[] fs = filename.Split('.');
                ext = fs[fs.Length - 1];
                return ext.ToLower();
            }
            else
                return null;
        }

        //Check whether file's extension is valid
        public bool CheckFileExtends(string fileExtends)
        {
            bool status = false;
            fileExtends = fileExtends.ToLower();
            string[] fe = Enum.GetNames(typeof(Picture.ValidFileType));
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
            if (byteArrayIn == null)
                return null;

            MemoryStream ms = new MemoryStream(byteArrayIn);
            Image returnImage = Image.FromStream(ms);
            return returnImage;
        }

        public byte[] ImageJPGToByteArray(Image imageIn)
        {
            if (imageIn == null)
                return null;

            MemoryStream ms = new MemoryStream();
            imageIn.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);
            return ms.ToArray();
        }

        public byte[] ImageBMPToByteArray(Image imageIn)
        {
            if (imageIn == null)
                return null;

            MemoryStream ms = new MemoryStream();
            imageIn.Save(ms, System.Drawing.Imaging.ImageFormat.Bmp);
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

            if (picSize < (2 * 1024 * 1024) || picSize>(20*1024*1024))
            {
                validation = "File size shoud be bigger than 2MB and less than 20MB";
            }
            return validation;
        }

        //Compress picture
        public byte[] ZoomAuto(byte[] originalImg)
        {
            if (originalImg == null)
                return null;

            Image initImage = byteArrayToImage(originalImg);

            //Calculat compressImg's width and height
            double newWidth = initImage.Width;
            double newHeight = initImage.Height;

            //Original image is too small. Actually, this situation cannot happen
            //Since I have verify the image size in ValidatePicture
            if (initImage.Width <= targetWidth && initImage.Height <= targetHeight)
            {
                return originalImg;
            }
            else
            {
                //width > height
                if (initImage.Width > initImage.Height || initImage.Width == initImage.Height)
                {
                    if (initImage.Width > targetWidth)
                    {
                        newWidth = targetWidth;
                        newHeight = (double)initImage.Height * ((double)targetWidth / (double)initImage.Width);
                    }
                }
                else
                {
                    if (initImage.Height > targetHeight)
                    {
                        newHeight = targetWidth;
                        newWidth = (double)initImage.Width * ((double)targetHeight / (double)initImage.Height);
                    }
                }

                //Create new compressed image
                Image newImage = new System.Drawing.Bitmap((int)newWidth, (int)newHeight);
                System.Drawing.Graphics newG = System.Drawing.Graphics.FromImage(newImage);
                newG.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
                newG.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
                newG.Clear(Color.White);
                newG.DrawImage(initImage, new System.Drawing.Rectangle(0, 0, newImage.Width, newImage.Height), new System.Drawing.Rectangle(0, 0, initImage.Width, initImage.Height), System.Drawing.GraphicsUnit.Pixel);

                System.Drawing.Image compressImg = new System.Drawing.Bitmap(newImage);

                newG.Dispose();
                newImage.Dispose();
                initImage.Dispose();

                return ImageBMPToByteArray(compressImg);
            }
        }
    }
}
