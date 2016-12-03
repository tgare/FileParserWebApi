using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GR.Files.Import
{
    public class Utils
    {
        public static DataView Sort(DataTable srcDataTable, string[] sortColumns, string[] sortParms)
        {
            DataView dv = new DataView(srcDataTable);

            StringBuilder sbuilder = new StringBuilder();

            for (var i=0; i < sortColumns.Length; i++)
            {
                sbuilder.Append(sortColumns[i] + " " + sortParms[i]);

                if (i < sortColumns.Length - 1)
                {
                    sbuilder.Append(","); 
                }
            }

            dv.Sort = sbuilder.ToString();

            return dv;
        }

        public static void Output(DataView srcTable)
        {
            foreach (DataRowView row in srcTable)
            {
                DataRow currRow = row.Row;

                foreach (var item in currRow.ItemArray)
                {
                    if (item is DateTime)
                    {
                        DateTime date = Convert.ToDateTime(item.ToString());
                        string strDate = String.Format("{0:M/d/yyyy}", date);
                        DateTime newdate = Convert.ToDateTime(strDate);
                        Console.Write(newdate.ToString("M/d/yyyy"));
                    }
                    else
                        Console.Write(item + " ");
                }
                Console.WriteLine();
            }
        }
    }
}
