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
    public class UtilsTests
    {
        #region Tests: Sort

        [TestMethod]
        public void Sort_ByLastNameDesc_DataTableSorted()
        {
            var data = new DataTable();

            data.Columns.Add("LastName", typeof (string));
            data.Columns.Add("FirstName", typeof (string));
            data.Columns.Add("Gender", typeof (string));
            data.Columns.Add("FavoriteColor", typeof (string));
            data.Columns.Add("DateOfBirth", typeof (DateTime));

            data.Rows.Add(new object[] {"Okai", "Rino", "F", "Green", "01/01/2016"});
            data.Rows.Add(new object[] {"Gates", "Bill", "M", "Blue", "02/01/2016"});
            data.Rows.Add(new object[] {"Jobs", "Steve", "M", "Red", "01/15/2016"});
            data.Rows.Add(new object[] {"Vandross", "Luther", "F", "Black", "10/22/2016"});
            data.Rows.Add(new object[] {"Jackson", "Michael", "F", "Green", "10/04/2015"});

            var sortColumns = new string[] {"LastName"};
            var sortParams = new string[] {"DESC"};

            var result = Utils.Sort(data, sortColumns, sortParams);

            Assert.AreEqual(result[0].Row.ItemArray[0].ToString(), "Vandross");
            Assert.AreEqual(result[1].Row.ItemArray[0].ToString(), "Okai");
            Assert.AreEqual(result[2].Row.ItemArray[0].ToString(), "Jobs");
            Assert.AreEqual(result[3].Row.ItemArray[0].ToString(), "Jackson");
            Assert.AreEqual(result[4].Row.ItemArray[0].ToString(), "Gates");
        }

        [TestMethod]
        public void Sort_ByGenderAscAndByLastNameAsc_DataTableSorted()
        {
            var data = new DataTable();

            data.Columns.Add("LastName", typeof (string));
            data.Columns.Add("FirstName", typeof (string));
            data.Columns.Add("Gender", typeof (string));
            data.Columns.Add("FavoriteColor", typeof (string));
            data.Columns.Add("DateOfBirth", typeof (DateTime));

            data.Rows.Add(new object[] {"Okai", "Rino", "F", "Green", "01/01/2016"});
            data.Rows.Add(new object[] {"Gates", "Bill", "M", "Blue", "02/01/2016"});
            data.Rows.Add(new object[] {"Jobs", "Steve", "M", "Red", "01/15/2016"});
            data.Rows.Add(new object[] {"Vandross", "Luther", "F", "Black", "10/22/2016"});
            data.Rows.Add(new object[] {"Jackson", "Michael", "F", "Green", "10/04/2015"});

            var sortColumns = new string[] {"Gender", "LastName"};
            var sortParams = new string[] {"ASC", "ASC"};

            var result = Utils.Sort(data, sortColumns, sortParams);

            Assert.AreEqual(result[0].Row.ItemArray[0].ToString(), "Jackson");
            Assert.AreEqual(result[1].Row.ItemArray[0].ToString(), "Okai");
            Assert.AreEqual(result[2].Row.ItemArray[0].ToString(), "Vandross");
            Assert.AreEqual(result[3].Row.ItemArray[0].ToString(), "Gates");
            Assert.AreEqual(result[4].Row.ItemArray[0].ToString(), "Jobs");
        }

        #endregion

        #region Tests: Output

        [TestMethod()]
        public void Output_DataTableView_ToConsole()
        {
            using (StringWriter sw = new StringWriter())
            {
                Console.SetOut(sw);

                var data = new DataTable();

                data.Columns.Add("LastName", typeof (string));
                data.Columns.Add("FirstName", typeof (string));
                data.Columns.Add("Gender", typeof (string));
                data.Columns.Add("FavoriteColor", typeof (string));
                data.Columns.Add("DateOfBirth", typeof (DateTime));

                data.Rows.Add(new object[] {"Okai", "Rino", "F", "Green", "01/01/2016"});
                data.Rows.Add(new object[] {"Gates", "Bill", "M", "Blue", "02/01/2016"});
                data.Rows.Add(new object[] {"Jobs", "Steve", "M", "Red", "01/15/2016"});

                DataView dataView = new DataView(data);

                Utils.Output(dataView);

                string expected = @"Okai Rino F Green 1/1/2016
Gates Bill M Blue 2/1/2016
Jobs Steve M Red 1/15/2016
";

                Assert.AreEqual<string>(expected, sw.ToString());
            }
        }

        #endregion

    }
}