

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UtilityHelper
{
    using DataAccess;
    using Microsoft.VisualBasic.FileIO;
    using System.Data;
    using System.IO;

    public static class CsvParser
    {


        // Consider using Deedle nuget library instead

        public static IEnumerable<string[]> Parse(string path)
        {


            using (TextFieldParser csvParser = new TextFieldParser(path))
            {
                //csvParser.CommentTokens = new string[] { "#" };
                csvParser.SetDelimiters(new string[] { "," });
                csvParser.HasFieldsEnclosedInQuotes = false;

                // Skip the row with the column names
                csvParser.ReadLine();

                while (!csvParser.EndOfData)
                {
                    // Read current line fields, pointer moves to the next line.
                    yield return csvParser.ReadFields();

                }
            }



        }



        public static string[] ParseHeader(string path)
        {

            using (var stream = System.IO.File.Open(path, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
            {
                using (var reader = new StreamReader(stream))
                {
                    var line = reader.ReadLine();
                    return line.Split();

                }
            }
        }


        // removes columns with empty data
        public static string CleanByColumns(string filename, params string[] ignoreproperties)
        {
            MutableDataTable dt = DataAccess.DataTable.New.ReadCsv(filename);

            var colsdlt = dt.Columns.Select(col =>
             {
                 var v = col.Values.Any(_ => _ == "");
                 return v ? col.Name : null;
             })
                .Where(_ => _ != null)
                .ToArray();


            var a = colsdlt.Except(ignoreproperties);

            dt.DeleteColumns(a.ToArray());

            return dt.SaveToString();

        }




        // removes rows with empty data
        public static string CleanByRows(string filename, params string[] ignorecolumns)
        {
            MutableDataTable dt = DataAccess.DataTable.New.ReadCsv(filename);
            var xx = dt.ColumnNames
                .Select((_, i) => { return ignorecolumns.Contains(_) ? (int?)i : null; })
                .Where(cv => cv != null);

            dt.KeepRows((_) =>
            {
                return _.Values.Select((__, j) => __ != "" || xx.Contains(j)).All(g => g);
            });

            return dt.SaveToString();

        }


        public static bool CleanFolderByRows(string path, string outpath, params string[] ignorecolumns)
        {
            if (!Directory.Exists(outpath))
                Directory.CreateDirectory(outpath);

            foreach (var file in Directory.GetFiles(path, "*.csv"))
            {
                var s = CleanByRows(file, ignorecolumns);

                File.WriteAllText(Path.Combine(outpath, Path.GetFileName(file)), s);

            }

            return true;
        }


    }
}


