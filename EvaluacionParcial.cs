using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace K_NECT.Models
{
    [Table("EVALUACION_PARCIAL")]
    public class EvaluacionParcial
    {
        [Key]
        [Column("idEvaluacionBase")]
        [ForeignKey("EvaluacionBase")]
        public int IdEvaluacionBase { get; set; }

        [Required]
        [Column("idParcial")]
        public int IdParcial { get; set; }

        [Column("esRecuperacion")]
        public bool EsRecuperacion { get; set; } = false;

        [Column("observacionesSupervisor")]
        [StringLength(255)]
        public string ObservacionesSupervisor { get; set; }

        // Navegación
        public virtual EvaluacionBase EvaluacionBase { get; set; }

        [ForeignKey("IdParcial")]
        public virtual Parcial Parcial { get; set; }
    }
}