using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace K_NECT.Models
{
    [Table("ESTUDIANTE_ASIGNATURA")]
    public class EstudianteAsignatura
    {
        [Key]
        [Column("idEstudianteAsignatura")]
        public int IdEstudianteAsignatura { get; set; }

        [Required]
        [Column("idEstudiante")]
        public int IdEstudiante { get; set; }

        [Required]
        [Column("idAsignatura")]
        public int IdAsignatura { get; set; }

        [Column("fechaInscripcion")]
        public DateTime FechaInscripcion { get; set; } = DateTime.Now;

        [Column("estado")]
        [StringLength(20)]
        public string Estado { get; set; } = "CURSANDO";

        [Required]
        [Column("cicloAcademico")]
        [StringLength(20)]
        public string CicloAcademico { get; set; }

        // Propiedades de navegación
        [ForeignKey("IdEstudiante")]
        public virtual Estudiante Estudiante { get; set; }

        [ForeignKey("IdAsignatura")]
        public virtual Asignatura Asignatura { get; set; }
    }
}