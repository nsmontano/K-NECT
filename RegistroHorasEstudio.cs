using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace K_NECT.Models
{
    [Table("REGISTRO_HORAS_ESTUDIO")]
    public class RegistroHorasEstudio
    {
        [Key]
        [Column("idRegistro")]
        public int IdRegistro { get; set; }

        [Required]
        [Column("idEstudianteAsignatura")]
        public int IdEstudianteAsignatura { get; set; }

        [Required]
        [Column("fecha")]
        public DateTime Fecha { get; set; }

        [Required]
        [Column("horasEstudio")]
        public decimal HorasEstudio { get; set; }

        [Column("temaEstudiado")]
        [StringLength(200)]
        public string TemaEstudiado { get; set; }

        [Column("observaciones")]
        public string Observaciones { get; set; }

        // Navegación
        [ForeignKey("IdEstudianteAsignatura")]
        public virtual EstudianteAsignatura EstudianteAsignatura { get; set; }
    }
}