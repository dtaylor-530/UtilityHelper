

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UtilityHelper
{

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


      



    }
}


