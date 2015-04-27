using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Business_Logic;
using System.IO;
using System.Drawing;

namespace Tests
{
    [TestClass]
    public class PictureProcessTest
    {
        [TestMethod]
        public void TestGetFileExtends()
        {
            PictureProcess process = new PictureProcess();
            string exm1 = "pic1.jpg";
            string exm2 = "pic2.pic2.BMP";
            string exm3 = "pic3";
            Assert.AreEqual("jpg", process.GetFileExtends(exm1));
            Assert.AreEqual("bmp", process.GetFileExtends(exm2));
            Assert.IsNull(process.GetFileExtends(exm3));
        }

        public void TestCheckFileExtends()
        {
            PictureProcess process = new PictureProcess();
            Assert.AreEqual(true, process.CheckFileExtends("jpg"));
            Assert.AreEqual(true, process.CheckFileExtends("BMP"));
            Assert.AreEqual(true, process.CheckFileExtends("cr2"));
            Assert.AreEqual(true, process.CheckFileExtends("NEF"));
            Assert.AreEqual(true, process.CheckFileExtends("arw"));
            Assert.AreEqual(true, process.CheckFileExtends("PEF"));
            Assert.AreEqual(true, process.CheckFileExtends("dng"));
            Assert.AreEqual(false, process.CheckFileExtends("abc"));
        }

        public void TestbyteArrayToImage()
        {
            PictureProcess process = new PictureProcess();

            FileInfo fi = new FileInfo("~/ImageData/Test1.jpg");
            FileStream fs = fi.OpenRead();
            byte[] bytes = new byte[fs.Length];
            fs.Read(bytes, 0, Convert.ToInt32(fs.Length));
            var temp = process.byteArrayToImage(bytes);
            Assert.IsNotNull(temp);
            Assert.AreEqual("Image", temp.GetType().ToString());
        }

        public void TestImageJPGToByteArray()
        {
            PictureProcess process = new PictureProcess();

            Image img = Image.FromFile("~/ImageData/Test1.jpg");
            var temp = process.ImageJPGToByteArray(img);
            Assert.IsNotNull(temp);
            Assert.AreEqual("byte", temp.GetType().ToString());
        }

        public void TestImageBMPToByteArray()
        {
            PictureProcess process = new PictureProcess();

            Image img = Image.FromFile("~/ImageData/Test3.bmp");
            var temp = process.ImageBMPToByteArray(img);
            Assert.IsNotNull(temp);
            Assert.AreEqual("byte", temp.GetType().ToString());
        }

        public void TestValidatePicture()
        {
            PictureProcess process = new PictureProcess();

            string temp1 = process.ValidatePicture("jpg", 4 * 1024 * 1024);
            string temp2 = process.ValidatePicture("jpg", 1024 * 1024);
            string temp3 = process.ValidatePicture("png", 4 * 1024 * 1024);
            Assert.AreEqual("Valid", temp1);
            Assert.AreEqual("File size shoud be bigger than 2MB and less than 20MB", temp2);
            Assert.AreEqual("File type is invalid! We accept jpg, bmp, cr2, nef, arw, pef, dng", temp3);
        }

        public void TestZoomAuto()
        {
            PictureProcess process = new PictureProcess();

            Assert.IsNull(process.ZoomAuto(null));
            FileInfo fi = new FileInfo("~/ImageData/Test1.jpg");
            FileStream fs = fi.OpenRead();
            byte[] bytes = new byte[fs.Length];
            byte[] temp = process.ZoomAuto(bytes);
            Assert.IsNotNull(temp);
            int tempHeight = 600 * 2448 / 3698;
            int tempLength = 600 * tempHeight;
            Assert.AreEqual(tempLength, temp.Length);
        }
    }
}
