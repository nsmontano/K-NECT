using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace K_NECT.Models
{
    [Table("ESTUDIANTE")]
    public class Estudiante
    {
        [Key]
        [Column("idEstudiante")]
        public int IdEstudiante { get; set; }

        [Required]
        [Column("codigoEstudiante")]
        [StringLength(20)]
        public string CodigoEstudiante { get; set; }

        [Required]
        [Column("nombres")]
        [StringLength(80)]
        public string Nombres { get; set; }

        [Required]
        [Column("apellidos")]
        [StringLength(80)]
        public string Apellidos { get; set; }

        [Required]
        [Column("correoEstudiante")]
        [StringLength(100)]
        public string CorreoEstudiante { get; set; }

        [Required]
        [Column("contraseña")]
        [StringLength(255)]
        public string Contraseña { get; set; }

        [Column("telefono")]
        [StringLength(15)]
        public string Telefono { get; set; }

        [Column("fechaNacimiento")]
        public DateTime? FechaNacimiento { get; set; }

        [Column("carrera")]
        [StringLength(100)]
        public string Carrera { get; set; }

        [Column("semestre")]
        public int? Semestre { get; set; }

        [Column("activo")]
        public bool Activo { get; set; } = true;

        // Propiedad calculada
        [NotMapped]
        public string NombreCompleto => $"{Nombres} {Apellidos}";
    }
}