using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace K_NECT.Models
{
    [Table("TAREA")]
    public class Tarea
    {
        [Key]
        [Column("idTarea")]
        public int IdTarea { get; set; }

        [Required]
        [Column("idAsignatura")]
        public int IdAsignatura { get; set; }

        [Required]
        [Column("nombreTarea")]
        [StringLength(100)]
        public string NombreTarea { get; set; }

        [Column("descripcion")]
        public string Descripcion { get; set; }

        [Required]
        [Column("valorPorcentual")]
        public decimal ValorPorcentual { get; set; }

        [Column("fechaAsignacion")]
        public DateTime FechaAsignacion { get; set; } = DateTime.Now;

        [Required]
        [Column("fechaEntrega")]
        public DateTime FechaEntrega { get; set; }

        [Column("estado")]
        [StringLength(20)]
        public string Estado { get; set; } = "ACTIVA";

        // Navegación
        [ForeignKey("IdAsignatura")]
        public virtual Asignatura Asignatura { get; set; }
    }
}