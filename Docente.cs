using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace K_NECT.Models
{
    [Table("DOCENTE")]
    public class Docente
    {
        [Key]
        [Column("idDocente")]
        public int IdDocente { get; set; }

        [Required]
        [Column("codigoDocente")]
        [StringLength(20)]
        public string CodigoDocente { get; set; }

        [Required]
        [Column("nombres")]
        [StringLength(80)]
        public string Nombres { get; set; }

        [Required]
        [Column("apellidos")]
        [StringLength(80)]
        public string Apellidos { get; set; }

        [Required]
        [Column("correoDocente")]
        [StringLength(100)]
        public string CorreoDocente { get; set; }

        [Required]
        [Column("contraseña")]
        [StringLength(255)]
        public string Contraseña { get; set; }

        [Column("especialidad")]
        [StringLength(100)]
        public string Especialidad { get; set; }

        [Column("activo")]
        public bool Activo { get; set; } = true;

        [NotMapped]
        public string NombreCompleto => $"{Nombres} {Apellidos}";

        [NotMapped]
        public int CantidadAsignaturas { get; set; }

        [NotMapped]
        public int CantidadEstudiantes { get; set; }

        [NotMapped]
        public int EvaluacionesPendientes { get; set; }

    }
}