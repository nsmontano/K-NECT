using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace K_NECT.Models
{
    [Table("DOCENTE_ASIGNATURA")]
    public class DocenteAsignatura
    {
        [Key]
        [Column("idDocenteAsignatura")]
        public int IdDocenteAsignatura { get; set; }

        [Required]
        [Column("idDocente")]
        public int IdDocente { get; set; }

        [Required]
        [Column("idAsignatura")]
        public int IdAsignatura { get; set; }

        [Column("fechaAsignacion")]
        public DateTime FechaAsignacion { get; set; } = DateTime.Now;

        [Required]
        [Column("cicloAcademico")]
        [StringLength(20)]
        public string CicloAcademico { get; set; }

        [Column("grupo")]
        [StringLength(10)]
        public string Grupo { get; set; }

        // Propiedades de navegación
        [ForeignKey("IdDocente")]
        public virtual Docente Docente { get; set; }

        [ForeignKey("IdAsignatura")]
        public virtual Asignatura Asignatura { get; set; }
    }
}