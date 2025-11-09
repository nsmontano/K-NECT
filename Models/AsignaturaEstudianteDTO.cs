using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace K_NECT.Models.DTOs
{
    public class AsignaturaEstudianteDTO
    {
        public int IdAsignatura { get; set; }
        public string CodigoAsignatura { get; set; }
        public string NombreAsignatura { get; set; }
        public string CodigoDocente { get; set; }
        public string NombreDocente { get; set; }
    }
}

