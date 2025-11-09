using K_NECT.Models;
using System.Data.Entity;

namespace K_NECT.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext() : base("K_NECTConnection")
        {
            Configuration.LazyLoadingEnabled = false;
            Configuration.ProxyCreationEnabled = false;
            Database.SetInitializer<ApplicationDbContext>(null);
        }

        // Tablas principales
        public DbSet<Estudiante> ESTUDIANTE { get; set; }
        public DbSet<Docente> DOCENTE { get; set; }
        public DbSet<Asignatura> ASIGNATURA { get; set; }
        public DbSet<EstudianteAsignatura> ESTUDIANTE_ASIGNATURA { get; set; }
        public DbSet<DocenteAsignatura> DOCENTE_ASIGNATURA { get; set; }

        // Actividades académicas
        public DbSet<Parcial> PARCIAL { get; set; }
        public DbSet<Tarea> TAREA { get; set; }
        public DbSet<Proyecto> PROYECTO { get; set; }

        // Evaluaciones
        public DbSet<EvaluacionBase> EVALUACION_BASE { get; set; }
        public DbSet<EvaluacionParcial> EVALUACION_PARCIAL { get; set; }

        // Registro de horas
        public DbSet<RegistroHorasEstudio> REGISTRO_HORAS_ESTUDIO { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
    }
}