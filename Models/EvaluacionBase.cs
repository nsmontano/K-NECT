using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace K_NECT.Models
{
    [Table("EVALUACION_BASE")]
    public class EvaluacionBase
    {
        [Key]
        [Column("idEvaluacionBase")]
        public int IdEvaluacionBase { get; set; }

        [Required]
        [Column("idEstudianteAsignatura")]
        public int IdEstudianteAsignatura { get; set; }

        [Required]
        [Column("tipoEvaluacion")]
        [StringLength(20)]
        public string TipoEvaluacion { get; set; }

        [Required]
        [Column("notaObtenida")]
        public decimal NotaObtenida { get; set; }

        [Required]
        [Column("pesoPorcentual")]
        public decimal PesoPorcentual { get; set; }

        [Column("comentario")]
        public string Comentario { get; set; }

        [Column("fechaRegistro")]
        public DateTime FechaRegistro { get; set; } = DateTime.Now;

        [Column("fechaModificacion")]
        public DateTime? FechaModificacion { get; set; }

        // Navegación
        [ForeignKey("IdEstudianteAsignatura")]
        public virtual EstudianteAsignatura EstudianteAsignatura { get; set; }
    }
}