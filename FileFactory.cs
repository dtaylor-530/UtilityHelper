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
using UtilityHelper.Model;
using System.Xml.Serialization;


namespace UtilityHelper
{



    public static class FileFactory
    {
        public static IEnumerable<File<T>> MakeFileObjects<T>(string folderPath)
        {
            List<File<T>> fileList = new List<File<T>>();

            DirectoryInfo fileDirectory = new DirectoryInfo(folderPath);
            FileInfo[] Files = null;
            try
            {
                Files = fileDirectory.GetFiles("*.xml");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

            foreach (FileInfo file in Files)
            {
                yield return new File<T> { Creation = DateTimeHelper.TimeStampParse(file.Name.Replace('_', '-')), FileInfo = file };

            }


        }


        public static File<T> MakeFileObject<T>(FileInfo file)
        {


            return new File<T> { Creation = DateTimeHelper.TimeStampParse(file.Name.Replace('_', '-')), FileInfo = file };


        }



     



    }



}

