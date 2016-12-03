using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.Http.Hosting;
using System.Web.SessionState;
using Castle.DynamicProxy.Generators;
using GR.Web.Api.Controllers;
using GR.Web.Api.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace GR.Web.Api.Tests
{

    [TestClass]
    public class RecordControllerTest
    {
        private const string CacheKey = "RecordsStore";

        private string GetTestRecords()
        {
            var importFile =
                @"LastName|FirstName|Gender|FavoriteColor|DateOfBirth
Sran|Garry|M|Green|10/22/1984
Vran|Angelica|F|Pink|07/27/1985
Bran|Joshua|M|Blue|10/04/2015
Bootery|Scootery|F|Blue|08/10/1995
Bolivia|Olivia|M|Blue|01/04/2000";

            return importFile;
        }

        [TestInitialize]
        public void Initialize()
        {
            var server = new Mock<HttpServerUtilityBase>(MockBehavior.Loose);
            var response = new Mock<HttpResponseBase>(MockBehavior.Strict);
            var request = new Mock<HttpRequestBase>(MockBehavior.Strict);
            var session = new MockHttpSession();

            var context = new Mock<HttpContextBase>();
            context.SetupGet(x => x.Request).Returns(request.Object);
            context.SetupGet(x => x.Response).Returns(response.Object);
            context.SetupGet(x => x.Server).Returns(server.Object);
            context.SetupGet(x => x.Session).Returns(session);

            HttpContext.Current = MockHelpers.FakeHttpContext();

            IDictionaryEnumerator enumerator = HttpContext.Current.Cache.GetEnumerator();

            while (enumerator.MoveNext())
            {

                HttpContext.Current.Cache.Remove((string)enumerator.Key);

            }
        }

        public void DestroyContext()
        {
            HttpContext.Current = null;
        }

        [TestMethod]
        public void GetAllRecords_ShouldReturnAllRecords()
        {
            var testRecords = GetTestRecords();
            var controller = new RecordsController(testRecords);

            var result = controller.Get() as List<Records>;
            Assert.AreEqual(5, result.Count);
        }

        [TestMethod]
        public void GetAllRecords_SortedByGender_ShouldReturnAllRecordsSortedByGender()
        {
            var testRecords = GetTestRecords();
            var controller = new RecordsController(testRecords);

            var result = controller.GetSorted("gender") as List<Records>;
            Assert.AreEqual(5, result.Count);
            Assert.AreEqual(result[0].FirstName, "Angelica");
            Assert.AreEqual(result[1].FirstName, "Scootery");
            Assert.AreEqual(result[2].FirstName, "Garry");
            Assert.AreEqual(result[3].FirstName, "Joshua");
            Assert.AreEqual(result[4].FirstName, "Olivia");
        }

        [TestMethod]
        public void GetAllRecords_SortedByLastName_ShouldReturnAllRecordsSortedByLastName()
        {
            var testRecords = GetTestRecords();
            var controller = new RecordsController(testRecords);

            var result = controller.GetSorted("lastname") as List<Records>;
            Assert.AreEqual(5, result.Count);
            Assert.AreEqual(result[0].FirstName, "Olivia");
            Assert.AreEqual(result[1].FirstName, "Scootery");
            Assert.AreEqual(result[2].FirstName, "Joshua");
            Assert.AreEqual(result[3].FirstName, "Garry");
            Assert.AreEqual(result[4].FirstName, "Angelica");
        }

        [TestMethod]
        public void GetAllRecords_SortedByName_ShouldReturnAllRecordsSortedByName()
        {
            var testRecords = GetTestRecords();
            var controller = new RecordsController(testRecords);

            var result = controller.GetSorted("name") as List<Records>;
            Assert.AreEqual(5, result.Count);
            Assert.AreEqual(result[0].FirstName, "Angelica");
            Assert.AreEqual(result[1].FirstName, "Garry");
            Assert.AreEqual(result[2].FirstName, "Joshua");
            Assert.AreEqual(result[3].FirstName, "Olivia");
            Assert.AreEqual(result[4].FirstName, "Scootery");
        }

        [TestMethod]
        public void PostRecord_ByPipeDelimited_ShouldAddToCurrentData()
        {
            var testRecords = GetTestRecords();
            var controller = new RecordsController(testRecords)
            {
                Request = new HttpRequestMessage
                {
                    Method = HttpMethod.Post,
                    RequestUri = new Uri("http://localhost/" + "api/Records/Post")
                }
            };
            controller.Request.Properties.Add(HttpPropertyKeys.HttpConfigurationKey, new HttpConfiguration());

            var newRecord = new RecordInput {
                Record = @"Jackson|Michael|M|Black|08/14/1955"
            };

            controller.Post(newRecord);

            var result = controller.Get();

            Assert.AreEqual(6, result.Count);
        }
    }

    #region Private Test Classes

    public class MockHttpSession : HttpSessionStateBase
    {
        Dictionary<string, object> m_SessionStorage = new Dictionary<string, object>();

        public override object this[string name]
        {
            get { return m_SessionStorage[name]; }
            set { m_SessionStorage[name] = value; }
        }

        public override void Abandon()
        {
            // Do nothing
        }
    }

    public class MockHelpers
    {
        public static HttpContext FakeHttpContext()
        {
            var httpRequest = new HttpRequest("", "http://localhost/", "");
            var stringWriter = new StringWriter();
            var httpResponse = new HttpResponse(stringWriter);
            var httpContext = new HttpContext(httpRequest, httpResponse);

            var sessionContainer = new HttpSessionStateContainer(
                "id",
                new SessionStateItemCollection(),
                new HttpStaticObjectsCollection(),
                10,
                true,
                HttpCookieMode.AutoDetect,
                SessionStateMode.InProc,
                false);

            SessionStateUtility.AddHttpSessionStateToContext(httpContext, sessionContainer);

            return httpContext;
        }

        #endregion

    }
}
