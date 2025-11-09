using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace K_NECT.Models
{
    [Table("PROYECTO")]
    public class Proyecto
    {
        [Key]
        [Column("idProyecto")]
        public int IdProyecto { get; set; }

        [Required]
        [Column("idAsignatura")]
        public int IdAsignatura { get; set; }

        [Required]
        [Column("nombreProyecto")]
        [StringLength(100)]
        public string NombreProyecto { get; set; }

        [Column("descripcion")]
        public string Descripcion { get; set; }

        [Required]
        [Column("fechaInicio")]
        public DateTime FechaInicio { get; set; }

        [Required]
        [Column("fechaFin")]
        public DateTime FechaFin { get; set; }

        [Column("requisitos")]
        public string Requisitos { get; set; }

        // Navegación
        [ForeignKey("IdAsignatura")]
        public virtual Asignatura Asignatura { get; set; }
    }
}