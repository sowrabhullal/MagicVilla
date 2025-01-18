using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MagicVillaWebApi.Models.Dto;
using MagicVillaWebApi.Models;
using MagicVillaWebApi.Data;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.EntityFrameworkCore;
using AutoMapper;
using MagicVillaWebApi.Repository.IRepository;
using System.Net;
using Microsoft.AspNetCore.Authorization;

namespace MagicVillaWebApi.Controllers.V2
{
    [Route("api/v{version:apiVersion}/MagicVillaWebApi")]
    [ApiController]
    [ApiVersion("2.0")]

    public class VillaApiController : ControllerBase
    {

        IVillaRepository repository;
        private readonly IMapper _mapper;
        protected APIResponse _response;

        public VillaApiController(IVillaRepository repo, IMapper mapper) {
            repository = repo;
            _mapper = mapper;
            _response = new();
        }

        
        [HttpGet]
        public async Task<ActionResult<APIResponse>> Get()
        {
            return Ok("Hello from v2 api");
        }
    }
}
