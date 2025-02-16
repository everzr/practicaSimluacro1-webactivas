using System.ComponentModel.DataAnnotations;

namespace practicaSimluacro1_webactivas.Models
{
    public class libro
    {
        [Key]
        public int id { get; set; }
        public string titulo { get; set; }
        public int? anio_publicacion { get; set; }
        public int autor_id { get; set; }
        public int? categoria_id { get; set; }
        public string resumen {  get; set; }

    }
}
