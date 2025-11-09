using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace K_NECT.Models
{
    [Table("PARCIAL")]
    public class Parcial
    {
        [Key]
        [Column("idParcial")]
        public int IdParcial { get; set; }

        [Required]
        [Column("idAsignatura")]
        public int IdAsignatura { get; set; }

        [Required]
        [Column("nombreParcial")]
        [StringLength(100)]
        public string NombreParcial { get; set; }

        [Column("tipo")]
        [StringLength(50)]
        public string Tipo { get; set; }

        [Required]
        [Column("valorPorcentual")]
        public decimal ValorPorcentual { get; set; }

        [Column("fechaRealizacion")]
        public DateTime? FechaRealizacion { get; set; }

        [Column("descripcion")]
        public string Descripcion { get; set; }

        // Navegación
        [ForeignKey("IdAsignatura")]
        public virtual Asignatura Asignatura { get; set; }
    }
}