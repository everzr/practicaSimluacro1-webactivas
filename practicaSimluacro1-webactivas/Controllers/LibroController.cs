using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using practicaSimluacro1_webactivas.Models;

namespace practicaSimluacro1_webactivas.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LibroController : ControllerBase
    {
        private readonly bibliotecaContext _bibliotecaContext;

        public LibroController(bibliotecaContext bibliotecaContext)
        {
            _bibliotecaContext = bibliotecaContext;
        }

        [HttpGet]
        [Route("GetAll")]
        public IActionResult Get()
        {
            List<libro> listadoLibro = (from l in _bibliotecaContext.libro select l).ToList();
            if (listadoLibro.Count() == 0)
            {
                return NotFound();
            }
            return Ok(listadoLibro);
        }

        [HttpGet]
        [Route("GetById/{id}")]
        public IActionResult Get(int id)
        {
            var listadoAutores = (from ll in _bibliotecaContext.libro
                                  join a in _bibliotecaContext.autor
                                         on ll.autor_id equals a.id
                                  where ll.id == id
                                  select new
                                  {
                                     ll.id,
                                     ll.titulo,
                                     ll.anio_publicacion,
               
                                     Autor = a.nombre,
                                      
                                  }).ToList();

            if (listadoAutores.Count == 0)
            {
                return NotFound();
            }

            return Ok(listadoAutores);
        }

        [HttpGet]
        [Route("Get")]


        [HttpPost]
        [Route("Add")]
        public IActionResult guardarLibro([FromBody] libro libro)
        {
            try
            {
                _bibliotecaContext.libro.Add(libro);
                _bibliotecaContext.SaveChanges();
                return Ok(libro);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut]
        [Route("actualizar/{id}")]
        public IActionResult Actualizarlibro(int id, [FromBody] libro libroModificar)
        {
            libro? libroActual = (from l in _bibliotecaContext.libro
                                     where l.id == id
                                     select l).FirstOrDefault();

            if (libroActual == null)
            {
                return NotFound();
            }
            libroActual.titulo = libroModificar.titulo;
            libroActual.anio_publicacion = libroModificar.anio_publicacion;
            //libroActual.autor_id = equipoModificar.marca_id;
            //libroActual.categoria_id = equipoModificar.tipo_equipo_id;
            libroActual.resumen = libroModificar.resumen;

            _bibliotecaContext.Entry(libroActual).State = EntityState.Modified;
            _bibliotecaContext.SaveChanges();

            return Ok(libroModificar);
        }

        [HttpDelete]
        [Route("eliminar/{id}")]
        public IActionResult EliminarEquipo(int id)
        {
            libro? libro = (from l in _bibliotecaContext.libro
                               where l.id == id
                               select l).FirstOrDefault();

            if (libro == null)
            {
                return NotFound();
            }

            _bibliotecaContext.libro.Attach(libro);
            _bibliotecaContext.libro.Remove(libro);
            _bibliotecaContext.SaveChanges();

            return Ok(libro);
        }




    }
}
