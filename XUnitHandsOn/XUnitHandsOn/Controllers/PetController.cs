using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Results;
using XUnitHandsOn.Commands;
using XUnitHandsOn.Handlers;
using XUnitHandsOn.Models;
using XUnitHandsOn.Querys;

namespace XUnitHandsOn.Controllers
{
    [Route("api/[controller]")]
    public class PetController : ApiController
    {
        private readonly PetQuery Query;
        private readonly CreatePetCommandHandler CreateHandler;
        private readonly DeletePetCommandHandler DeleteHandler;


        public PetController(PetQuery query, CreatePetCommandHandler create, DeletePetCommandHandler delete)
        {
            Query = query;
            CreateHandler = create;
            DeleteHandler = delete;
        }

        [HttpGet]
        public async Task<OkNegotiatedContentResult<List<PetModel>>> Get() => Ok(await Query.GetAllPetAsync());


        [HttpGet]
        public async Task<OkNegotiatedContentResult<PetModel>> Get(int id) => Ok(await Query.GetPetAsync(id));

        [HttpPut]
        public async Task<IHttpActionResult> Put([FromBody] CreatePetCommand create)
        {
            var commandResult = await CreateHandler.Handle(create);
            return commandResult ? (IHttpActionResult) Ok() : BadRequest();
        }

        [HttpDelete]
        public async Task<IHttpActionResult> Delete(DeletePetCommand delete)
        {
            var commandResult = await DeleteHandler.Handle(delete);
            return commandResult ? (IHttpActionResult) Ok() : BadRequest();
        }

    }
}
