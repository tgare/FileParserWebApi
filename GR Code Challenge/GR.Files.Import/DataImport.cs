using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualBasic.FileIO;

namespace GR.Files.Import
{
    public class DataImport
    {
        private DataTable data;
        private bool hasHeaders;
        static readonly string[] DelimiterValues = { "|", ",", " " };

        public DataImport() 
        {
            data = new DataTable();
            hasHeaders = false;
        }

        public DataTable GetDataTable()
        {
            return data;
        }

        public void ProcessArguments(string[] args)
        {
            foreach (var arg in args)
            {
                var filename = arg;

                try
                {
                    Stream stream = File.Open(filename, FileMode.Open);
                    this.LoadToDataTable(stream);
                    stream.Close();
                }
                catch (FileNotFoundException e)
                {
                    Console.WriteLine("File {0} not found, please check for accurate filename and location", arg);
                }
            }
        }

        public DataTable LoadToDataTable(Stream filename)
        {
            var parser = new TextFieldParser(filename);
            parser.SetDelimiters(DelimiterValues);

            while (!parser.EndOfData)
            {
                var currentRow = parser.ReadFields();

                var isHeaderRow = currentRow[0] == "LastName";

                if (hasHeaders && isHeaderRow)
                {
                    continue;
                }

                if (!hasHeaders)
                {
                    foreach (var field in currentRow)
                    {
                        data.Columns.Add(field, field.ToLower() == "dateofbirth" ? typeof(DateTime) : typeof(string));
                    }
                    hasHeaders = true;
                }
                else
                {
                    //if (isHeaderRow) { continue; }

                    data.Rows.Add(currentRow);
                }
            }

            return data;
        }
    }
}
