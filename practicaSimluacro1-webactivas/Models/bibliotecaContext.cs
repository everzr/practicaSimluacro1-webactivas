using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.SqlServer;

namespace practicaSimluacro1_webactivas.Models
{
    public class bibliotecaContext : DbContext
    {
        public bibliotecaContext(DbContextOptions<bibliotecaContext> options) : base(options)
        {

        }

        public DbSet<autor> autor { get; set; }

        public DbSet<libro> libro { get; set; }

        //public DbSet<tipo_equipos> tipo_equipos { get; set; }

        //public DbSet<marcas> marcas { get; set; }
    }
}
