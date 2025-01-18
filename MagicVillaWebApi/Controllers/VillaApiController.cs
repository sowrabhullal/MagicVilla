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

namespace MagicVillaWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
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

        [Authorize]
        [HttpGet]
        public async Task<ActionResult<APIResponse>> GetVillas() {
            try
            {
                IEnumerable<Villa> villaList = await repository.GetAllAsync();

                _response.Result = _mapper.Map<List<VillaDto>>(villaList);
                _response.StatusCode = HttpStatusCode.OK;
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.ErrorMessages
                     = new List<string>() { ex.ToString() };
            }
            

            return Ok(_response);
        }

        [HttpGet("id")]
        [Authorize(Roles = "admin")]
        public async Task<ActionResult<APIResponse>> GetVilla(int id) {
            try
            {
                if (id == 0)
                {
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    return BadRequest();
                }
                var villa = await repository.GetAsync(u => u.Id == id);

                if (villa == null)
                {
                    _response.StatusCode = HttpStatusCode.NotFound;
                    return NotFound();
                }
                _response.Result = _mapper.Map<VillaDto>(villa);
                _response.StatusCode = HttpStatusCode.OK;
            }
            catch (Exception ex) { 
            
            }
            
            return Ok(_response);
        }

        [HttpPost]
        [Authorize(Roles ="admin")]
        public async Task<ActionResult<APIResponse>> CreateVilla([FromBody]VillaCreateDto createvillaDto) {
            try
            {
                if (createvillaDto == null)
                {
                    return BadRequest();
                }

                if (await repository.GetAsync(u => u.Name.ToLower() == createvillaDto.Name.ToLower()) != null)
                {
                    ModelState.AddModelError("Error", "Name already exists");
                    return BadRequest(ModelState);
                }


                Villa model = _mapper.Map<Villa>(createvillaDto);
                await repository.CreateAsync(model);
                _response.Result = _mapper.Map<VillaDto>(model);
                _response.StatusCode = HttpStatusCode.Created;
            }
            catch (Exception ex) {
                _response.IsSuccess = false;
                _response.ErrorMessages
                     = new List<string>() { ex.ToString() };
            }
           
            return Ok(_response);
        }

        [HttpDelete("id")]
        [Authorize(Roles = "CUSTOM")]
        public async Task<ActionResult<APIResponse>> DeleteVilla(int id) {
            try {
                if (id == 0)
                {
                    return BadRequest();
                }

                var villa = await repository.GetAsync(u => u.Id == id);
                if (villa == null)
                {
                    return NotFound();
                }
                repository.RemoveAsync(villa);
                _response.StatusCode = HttpStatusCode.NoContent;
                _response.IsSuccess = true;
            }
            catch (Exception ex) {
                _response.IsSuccess = false;
                _response.ErrorMessages
                     = new List<string>() { ex.ToString() };
            }
            
            return _response;

        }

        [HttpPut]
        public async Task<ActionResult<APIResponse>> UpdateVilla(int id, [FromBody]VillaUpdateDto updatevillaDto) {
            try {
                if (id != updatevillaDto.Id)
                {
                    return BadRequest(updatevillaDto);
                }

                Villa model = _mapper.Map<Villa>(updatevillaDto);
                repository.UpdateAsync(model);
                _response.StatusCode = HttpStatusCode.NoContent;
                _response.IsSuccess = true;
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.ErrorMessages
                     = new List<string>() { ex.ToString() };
            }


            return _response;
        }

        [HttpPatch]
        public async Task<ActionResult<VillaDto>> PatchVilla(int id, JsonPatchDocument<VillaUpdateDto> patch) {
            if (id == 0 || patch == null) {
                return BadRequest();
            }

            var villa= await repository.GetAsync(u => u.Id == id, tracked:false);
            VillaUpdateDto villaDto = _mapper.Map<VillaUpdateDto>(villa);

            if (villaDto == null) { return NotFound(); }

            patch.ApplyTo(villaDto, ModelState);

            Villa model1 = _mapper.Map<Villa>(villaDto);

            repository.UpdateAsync(model1);

            if (!ModelState.IsValid) {
                return BadRequest();
            }
            else { 
                return NoContent();
            }
        }
    }
}
