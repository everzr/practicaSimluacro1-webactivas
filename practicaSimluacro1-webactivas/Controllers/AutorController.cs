using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using practicaSimluacro1_webactivas.Models;

namespace practicaSimluacro1_webactivas.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AutorController : ControllerBase
    {
        private readonly bibliotecaContext _bibliotecaContext;

        public AutorController(bibliotecaContext bibliotecaContext)
        {
            _bibliotecaContext = bibliotecaContext;
        }

        [HttpGet]
        [Route("GetAll")]
        public IActionResult Get()
        {
            List<autor> listadoAutor = (from a in _bibliotecaContext.autor select a).ToList();
            if (listadoAutor.Count() == 0)
            {
                return NotFound();
            }
            return Ok(listadoAutor);
        }

        [HttpGet]
        [Route("GetById/{id}")]
        public IActionResult Get(int id)
        {
            var listadoAutores = (from a in _bibliotecaContext.autor
                                 join ll in _bibliotecaContext.libro
                                        on a.id equals ll.autor_id
                                   where a.id == id
                                 select new
                                 {
                                    a.id,
                                    a.nombre,
                                    ll.titulo
                                 }).ToList();

            if (listadoAutores.Count == 0)
            {
                return NotFound();
            }

            return Ok(listadoAutores);
        }


        [HttpPost]
        [Route("Add")]
        public IActionResult guardarLibro([FromBody] autor autor)
        {
            try
            {
                _bibliotecaContext.autor.Add(autor);
                _bibliotecaContext.SaveChanges();
                return Ok(autor);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut]
        [Route("actualizar/{id}")]
        public IActionResult ActualizarAutor(int id, [FromBody] autor autorModificar)
        {
            autor? autorActual = (from a in _bibliotecaContext.autor
                                  where a.id == id
                                  select a).FirstOrDefault();

            if (autorActual == null)
            {
                return NotFound();
            }
            autorActual.nombre = autorModificar.nombre;
            autorActual.nacionalidad = autorModificar.nacionalidad;
        

            _bibliotecaContext.Entry(autorActual).State = EntityState.Modified;
            _bibliotecaContext.SaveChanges();

            return Ok(autorModificar);
        }

        [HttpDelete]
        [Route("eliminar/{id}")]
        public IActionResult EliminarEquipo(int id)
        {
            autor? autor = (from a in _bibliotecaContext.autor
                            where a.id == id
                            select a).FirstOrDefault();

            if (autor == null)
            {
                return NotFound();
            }

            _bibliotecaContext.autor.Attach(autor);
            _bibliotecaContext.autor.Remove(autor);
            _bibliotecaContext.SaveChanges();

            return Ok(autor);
        }

    }
}
