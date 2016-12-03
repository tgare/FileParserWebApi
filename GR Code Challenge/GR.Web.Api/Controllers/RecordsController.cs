using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using GR.Files.Import;
using GR.Web.Api.Models;
using GR.Web.Api.Services;

namespace GR.Web.Api.Controllers
{
    public class RecordsController : ApiController
    {
        private RecordsRepository recordsRepository;

        public RecordsController()
        {
            this.recordsRepository = new RecordsRepository();
        }

        public RecordsController(string testRecords)
        {
            this.recordsRepository = new RecordsRepository();
            recordsRepository.LoadData(testRecords);
        }

        public List<Records> Get()
        {
            return recordsRepository.GetAllRecords();
        }

        public List<Records> GetSorted(string id)
        {
            return recordsRepository.GetAllRecordsSorted(id);
        }

        public HttpResponseMessage Post(RecordInput input)
        {
            this.recordsRepository.SaveRecord(input.Record);

            var response = Request.CreateResponse<string>(System.Net.HttpStatusCode.Created, input.Record);

            return response;
        }
    }
}
