using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Net;
using System.Threading;
using System.Windows.Input;
using System.Xml.Linq;
using System.Xml;
using System.Xml.XPath;
using System.Windows;
using System.Reflection;
using System.IO;
using Utilities.Model;
using System.Xml.Serialization;

namespace OddsChecker.DAL
{

  


    public static class XMLFileFactory
    {
        public static List<XMLFile<T>> MakeFileObjects<T>(string folderPath)
        {
            List<XMLFile<T>> fileList = new List<XMLFile<T>>();

            DirectoryInfo fileDirectory = new DirectoryInfo(folderPath);
            FileInfo[] Files = null;
            try
            {
                Files = fileDirectory.GetFiles("*.xml");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return null;
            }

            foreach (FileInfo file in Files)
            {
                //App.Current.Dispatcher.Invoke(() =>
                //{

                //XElement xelement = XElement.Load();

                //var xx = xelement.FirstNode;
                fileList.Add(new XMLFile<T> { Creation = Utilities.TimeStampHelpers.GetDate(file.Name.Replace('_', '-')), FileInfo = file });
                //});

                //AddStatusUpdate("Local Stem File Located. Empty Coupon created.", file.Name);
            }

            return fileList;
        }


        public static XMLFile<T> MakeFileObject<T>(FileInfo file)
        {


            return new XMLFile<T> { Creation = Utilities.TimeStampHelpers.GetDate(file.Name.Replace('_', '-')), FileInfo = file };


        }



     



    }
}

