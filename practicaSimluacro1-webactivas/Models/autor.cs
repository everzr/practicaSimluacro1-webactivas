using System.ComponentModel.DataAnnotations;
namespace practicaSimluacro1_webactivas.Models
{
    public class autor
    {
        [Key]
        public int id { get; set; }
        public string nombre { get; set; }
        public string nacionalidad { get; set; }


    }
}
