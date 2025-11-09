using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace K_NECT.Models
{
    [Table("ASIGNATURA")]
    public class Asignatura
    {
        [Key]
        [Column("idAsignatura")]
        public int IdAsignatura { get; set; }

        [Required]
        [Column("codigoAsignatura")]
        [StringLength(20)]
        public string CodigoAsignatura { get; set; }

        [Required]
        [Column("nombreAsignatura")]
        [StringLength(100)]
        public string NombreAsignatura { get; set; }

        [Required]
        [Column("creditos")]
        public int Creditos { get; set; }

        [Column("horasTeoricas")]
        public int? HorasTeoricas { get; set; }

        [Column("horasPracticas")]
        public int? HorasPracticas { get; set; }

        [Column("descripcion")]
        public string Descripcion { get; set; }

        [Column("activa")]
        public bool Activa { get; set; } = true;
    }
}