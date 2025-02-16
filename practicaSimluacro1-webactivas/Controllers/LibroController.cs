using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using practicaSimluacro1_webactivas.Models;
using System.Diagnostics.Metrics;

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
        public IActionResult GetAll(int pageNumber = 1)
        {
           
            int totalRegistros = _bibliotecaContext.libro.Count();
            int cantidadPorPagina = totalRegistros <= 10 ? totalRegistros : 10;  // Si hay 8 registros o menos, mostrar todo, sino, limitar a 10

            // Si hay más de 10 registros, paginamos, de lo contrario, mostramos todos
            var listadoLibro = _bibliotecaContext.libro
                                                 .Skip((pageNumber - 1) * cantidadPorPagina)
                                                 .Take(cantidadPorPagina)
                                                 .ToList();

            if (listadoLibro.Count == 0)
            {
                return NotFound("No se encontraron libros");
            }

            return Ok(listadoLibro);
        }


        [HttpGet]
        [Route("GetById/{id}")]
        public IActionResult GetById(int id)
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
        [Route("GetLibroAfterYear2000")]
        public IActionResult GetLibros2000() {

            var listadoLibros = (from ll in _bibliotecaContext.libro
                                 where ll.anio_publicacion >2000
                                 select ll).ToList();
            if(listadoLibros.Count == 0) 
            {
                return NotFound(); 
            }

            return Ok(listadoLibros);
        
        }

        [HttpGet]
        [Route("GetLibroByTitulo/{titulo}")]
        public IActionResult GetLibroByTitulo(string titulo)
        {
            var listadoLibros = (from ll in _bibliotecaContext.libro
                                 where ll.titulo.Contains(titulo)
                                 select ll).ToList();

            if (listadoLibros.Count == 0)
            {
                return NotFound(new { Message = "No se encontraron libros con el título proporcionado" });
            }

            return Ok(listadoLibros);
        }

        [HttpGet]
        [Route("GetLibrosMasRecientes")]
        public IActionResult GetLibrosMasRecientes()
        {
            

            var librosMasRecientes = (from ll in _bibliotecaContext.libro
                                      select ll
                                      )
                                    .OrderByDescending(x => x.anio_publicacion)
                                    .ToList();

            if (librosMasRecientes == null)
            {
                return NotFound();
            }



            return Ok(librosMasRecientes);
        }

        [HttpGet]
        [Route("GetCantLibrosByYear")]
        public IActionResult GetCantLibrosByYear()
        {


            var librosMasRecientes = (from ll in _bibliotecaContext.libro
                                      group ll by ll.anio_publicacion into grupo
                                      select new
                                      {
                                          anio = grupo.Key,
                                          cant_libros = grupo.Count(),
                                      }
                                      
                                      )
                                    .OrderByDescending(x => x.anio)
                                    .ToList();

            if (librosMasRecientes == null)
            {
                return NotFound();
            }



            return Ok(librosMasRecientes);
        }

        [HttpGet]
        [Route("GetFirstLibroAutor/{id}")]
        public IActionResult GetFirstLibroAutor(int id)
        {
            var libro = (from ll in _bibliotecaContext.libro
                         join aa in _bibliotecaContext.autor
                         on ll.autor_id equals aa.id
                         select new {
                         
                             autor = aa.nombre,
                             primero_libro = ll.titulo,
                             ll.anio_publicacion

                         }).OrderBy(x => x.anio_publicacion).Take(1).ToList();

            return Ok(libro);

        }

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
