using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using GR.Files.Import;
using GR.Web.Api.Models;

namespace GR.Web.Api.Services
{
    public class RecordsRepository
    {
        private const string CacheKey = "RecordsStore";

        public RecordsRepository()
        {
            var ctx = HttpContext.Current;

            if (ctx != null)
            {
                if (ctx.Cache[CacheKey] == null)
                {
                    var records = new DataImport();
                    
                    ctx.Cache[CacheKey] = records;
                }
            }
        }

        public void LoadData(string inputData)
        {
            var ctx = HttpContext.Current;

            if (ctx != null)
            {
                var dataObject = (DataImport)ctx.Cache[CacheKey];

                // convert string to stream
                byte[] byteArray = Encoding.UTF8.GetBytes(@String.Concat(inputData));
                //byte[] byteArray = Encoding.ASCII.GetBytes(contents);
                MemoryStream stream = new MemoryStream(byteArray);

                dataObject.LoadToDataTable(stream);
            }
        }

        public List<Records> GetAllRecords()
        {
            var ctx = HttpContext.Current;

            if (ctx != null)
            {
                var dataObject = (DataImport) ctx.Cache[CacheKey];

                var data = dataObject.GetDataTable();

                List<Records> records = new List<Records>();

                foreach (DataRow dataRow in data.Rows)
                {
                    var currRecord = new Records
                    {
                        LastName = dataRow["LastName"].ToString(),
                        FirstName = dataRow["FirstName"].ToString(),
                        Gender = dataRow["Gender"].ToString(),
                        FavoriteColor = dataRow["FavoriteColor"].ToString(),
                        DateOfBirth = dataRow["DateOfBirth"].ToString(),
                    };

                    records.Add(currRecord);
                }
                
                return (records);
            }

            return null;

        }

        public List<Records> GetAllRecordsSorted(string sortBy)
        {
            var ctx = HttpContext.Current;

            if (ctx != null)
            {
                switch (sortBy)
                {
                    case "birthdate":
                        sortBy = "dateofbirth";
                        break;
                    case "name":
                        sortBy = "firstname";
                        break;
                }

                var dataObject = (DataImport)ctx.Cache[CacheKey];

                var data = dataObject.GetDataTable();

                var sortColumns = new string[] { sortBy };
                var sortParams = new string[] { "ASC" };

                var dv = Utils.Sort(data, sortColumns, sortParams);

                List<Records> records = new List<Records>();

                foreach (DataRowView dataRow in dv)
                {
                    var currRecord = new Records
                    {
                        LastName = dataRow["LastName"].ToString(),
                        FirstName = dataRow["FirstName"].ToString(),
                        Gender = dataRow["Gender"].ToString(),
                        FavoriteColor = dataRow["FavoriteColor"].ToString(),
                        DateOfBirth = dataRow["DateOfBirth"].ToString(),
                    };

                    records.Add(currRecord);
                }

                return (records);
            }

            return null;
        }

        public bool SaveRecord(string record)
        {
            var ctx = HttpContext.Current;

            if (ctx != null)
            {
                try
                {
                    var dataObject = (DataImport)ctx.Cache[CacheKey];

                    
                    // Add headers
                    var headers = @"LastName|FirstName|Gender|FavoriteColor|DateOfBirth
";
                    // convert string to stream
                    byte[] byteArray = Encoding.UTF8.GetBytes(@String.Concat(headers, record));
                    //byte[] byteArray = Encoding.ASCII.GetBytes(contents);
                    MemoryStream stream = new MemoryStream(byteArray);

                    dataObject.LoadToDataTable(stream);

                    return true;
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.ToString());
                    return false;
                }
            }

            return false;
        }
    }
}