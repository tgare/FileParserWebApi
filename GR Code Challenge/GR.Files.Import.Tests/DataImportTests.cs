using Microsoft.VisualStudio.TestTools.UnitTesting;
using GR.Files.Import;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GR.Files.Import.Tests
{
    [TestClass()]
    public class DataImportTests
    {
        #region Tests: LoadToDataTable

        [TestMethod()]
        public void LoadToDataTable_WithPipeDelimiter_DataTableLoaded()
        {

            var importFile =
                @"LastName|FirstName|Gender|FavoriteColor|DateOfBirth
Sran|Garry|M|Green|10/22/1984
Vran|Angelica|F|Pink|07/27/1985
Bran|Joshua|M|Blue|10/04/2015";

            // convert string to stream
            byte[] byteArray = Encoding.UTF8.GetBytes(importFile);
            //byte[] byteArray = Encoding.ASCII.GetBytes(contents);
            MemoryStream stream = new MemoryStream(byteArray);

            var data = new DataImport();

            data.LoadToDataTable(stream);

            Assert.AreEqual(data.GetDataTable().Rows.Count, 3);

        }

        [TestMethod()]
        public void LoadToDataTable_WithCommaDelimiter_DataTableLoaded()
        {

            var importFile =
                @"LastName,FirstName,Gender,FavoriteColor,DateOfBirth
Aran,Garry,M,Green,10/22/1984
Dran,Angelica,F,Pink,07/27/1985
Cran,Joshua,M,Blue,10/04/2015";

            // convert string to stream
            byte[] byteArray = Encoding.UTF8.GetBytes(importFile);
            //byte[] byteArray = Encoding.ASCII.GetBytes(contents);
            MemoryStream stream = new MemoryStream(byteArray);

            var data = new DataImport();

            data.LoadToDataTable(stream);

            Assert.AreEqual(data.GetDataTable().Rows.Count, 3);

        }

        [TestMethod()]
        public void LoadToDataTable_WithSpaceDelimiter_DataTableLoaded()
        {

            var importFile =
                @"LastName FirstName Gender FavoriteColor DateOfBirth
Aran Garry M Green 10/22/1984
Dran Angelica F Pink 07/27/1985
Cran Joshua M Blue 10/04/2015";

            // convert string to stream
            byte[] byteArray = Encoding.UTF8.GetBytes(importFile);
            //byte[] byteArray = Encoding.ASCII.GetBytes(contents);
            MemoryStream stream = new MemoryStream(byteArray);

            var data = new DataImport();

            data.LoadToDataTable(stream);

            Assert.AreEqual(data.GetDataTable().Rows.Count, 3);

        }

        [TestMethod()]
        public void LoadToDataTable_WithMultipleFilesAndDelimiters_DataTableLoaded()
        {

            var importFile =
                @"LastName|FirstName|Gender|FavoriteColor|DateOfBirth
Sran|Garry|M|Green|10/22/1984
Vran|Angelica|F|Pink|07/27/1985
Bran|Joshua|M|Blue|10/04/2015";

            // convert string to stream
            byte[] byteArray = Encoding.UTF8.GetBytes(importFile);
            //byte[] byteArray = Encoding.ASCII.GetBytes(contents);
            MemoryStream stream = new MemoryStream(byteArray);

            var importFile2 =
                @"LastName,FirstName,Gender,FavoriteColor,DateOfBirth
Aran,Garry,M,Green,10/22/1984
Dran,Angelica,F,Pink,07/27/1985
Cran,Joshua,M,Blue,10/04/2015";

            // convert string to stream
            byte[] byteArray2 = Encoding.UTF8.GetBytes(importFile2);
            //byte[] byteArray = Encoding.ASCII.GetBytes(contents);
            MemoryStream stream2 = new MemoryStream(byteArray);



            var data = new DataImport();

            data.LoadToDataTable(stream);
            data.LoadToDataTable(stream2);

            Assert.AreEqual(data.GetDataTable().Rows.Count, 6);
        }

        #endregion
    }
}