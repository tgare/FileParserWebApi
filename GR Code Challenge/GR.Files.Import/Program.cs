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
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length == 0)
            {
                Console.WriteLine("No files to import, please press any key to exit");
                Console.ReadKey();
                Environment.Exit(0);
            }
            var dt = new DataImport();

            dt.ProcessArguments(args);
            
            // Sort the data
            DataView dv = Utils.Sort(dt.GetDataTable(), new[] {"Gender", "LastName"}, new[] {"ASC", "ASC"});
            DataView dv2 = Utils.Sort(dt.GetDataTable(), new[] { "DateOfBirth" }, new[] { "ASC" });
            DataView dv3 = Utils.Sort(dt.GetDataTable(), new[] { "LastName" }, new[] { "DESC" });
            
            // Output to console
            Utils.Output(dv);
            Utils.Output(dv2);
            Utils.Output(dv3);

            Console.WriteLine("Process complete");
            Console.ReadKey();
        }
    }
}
