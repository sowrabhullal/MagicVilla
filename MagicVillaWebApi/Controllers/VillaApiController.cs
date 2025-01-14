using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MagicVillaWebApi.Models.Dto;
using MagicVillaWebApi.Models;
using MagicVillaWebApi.Data;
using Microsoft.AspNetCore.JsonPatch;

namespace MagicVillaWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VillaApiController : ControllerBase
    {
        private readonly ILogger<VillaApiController> _logger;
        public VillaApiController(ILogger<VillaApiController> logger) { 
            _logger = logger;
        }

        [HttpGet]
        public ActionResult<IEnumerable<VillaDto>> GetVillas() {
            return VillaStore.villaList;
        }

        [HttpGet("id")]
        public ActionResult<VillaDto> GetVilla(int id) {
            if (id == 0) {
                return BadRequest();
            }
            var villa = VillaStore.villaList.FirstOrDefault(u => u.Id == id);
            if (villa == null)
            {
                return NotFound();
            }
            return Ok(villa);
        }

        [HttpPost]
        public ActionResult<VillaDto> CreateVilla([FromBody]VillaDto villaDto) {

            if (villaDto == null) {
                return BadRequest();
            }

            if (VillaStore.villaList.FirstOrDefault(u => u.Name.ToLower() == villaDto.Name.ToLower())!=null) {
                ModelState.AddModelError("Error", "Name already exists");
                return BadRequest(ModelState);
            }

            if (villaDto.Id > 0) {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }

            villaDto.Id = VillaStore.villaList.OrderByDescending(u => u.Id).FirstOrDefault().Id;
            VillaStore.villaList.Add(villaDto);

            return Ok(villaDto);
        }

        [HttpDelete("id")]
        public IActionResult DeleteVilla(int id) {
            if (id == 0) {
                return BadRequest();
            }

            var villa = VillaStore.villaList.FirstOrDefault(u => u.Id == id);
            if (villa == null)
            {
                return NotFound();
            }

            VillaStore.villaList.Remove(villa);
            return NoContent();

        }

        [HttpPut]
        public ActionResult<VillaDto> UpdateVilla(int id, [FromBody]VillaDto villaDto) {
            if (id != villaDto.Id) {
                return BadRequest(villaDto);
            }

            var villa= VillaStore.villaList.FirstOrDefault(v => v.Id == id);
            villa.Name= villaDto.Name;
            villa.occupancy = villaDto.occupancy;
            villa.sqft = villaDto.sqft;

            return Ok(villa);
        }

        [HttpPatch]
        public ActionResult<VillaDto> PatchVilla(int id, JsonPatchDocument<VillaDto> patch) {
            if (id == 0 || patch == null) {
                return BadRequest();
            }

            var villa=VillaStore.villaList.FirstOrDefault(u => u.Id == id);

            if (villa == null) { return NotFound(); }

            patch.ApplyTo(villa, ModelState);

            if (!ModelState.IsValid) {
                return BadRequest();
            }
            else { 
                return NoContent();
            }
        }
    }
}
