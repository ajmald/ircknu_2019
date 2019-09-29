using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using IrkcnuApi.Services;
using IrkcnuApi.Models;

namespace IrkcnuApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ArtikelsController : ControllerBase
    {
        private readonly ArtikelService _artikelService;

        public ArtikelsController(ArtikelService artikelService)
        {
            _artikelService = artikelService;
        }

        [HttpGet]
        public ActionResult<List<Artikel>> Get() =>
            _artikelService.Get();

        [HttpGet("{id:length(24)}", Name = "GetArtikel")]
        public ActionResult<Artikel> Get(string id)
        {
            var artikel = _artikelService.Get(id);

            if (artikel == null)
            {
                return NotFound();
            }

            return artikel;
        }

        [HttpPost]
        public ActionResult<Artikel> Create(Artikel artikel)
        {
            _artikelService.Create(artikel);

            return CreatedAtRoute("GetArtikel", new { id = artikel.Id.ToString() }, artikel);
        }

        [HttpPost("createArtikels")]
        public ActionResult<string> CreateArtikels(List<Artikel> artikels)
        {
            string s = _artikelService.CreateArtikels(artikels);
            return s;
        }

      

        [HttpPut("{id:length(24)}")]
        public IActionResult Update(string id, Artikel artikelIn)
        {
            var artikel = _artikelService.Get(id);

            if (artikel == null)
            {
                return NotFound();
            }

            _artikelService.Update(id, artikelIn);

            return NoContent();
        }

        /// <summary>
        /// Deletes a specific Artikel.
        /// </summary>
        /// <param name="id"></param>  

        [HttpDelete("{id:length(24)}")]
        public IActionResult Delete(string id)
        {
            var artikel = _artikelService.Get(id);

            if (artikel == null)
            {
                return NotFound();
            }

            _artikelService.Remove(artikel.Id);

            return NoContent();
        }
    }
}
