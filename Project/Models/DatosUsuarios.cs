using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Project.Models
{
    public class DatosUsuarios
    {
        public int Id { get; set; }
        [Required]
        public String Nombre { set; get; }
        [Required]
        public String Apellidos { set; get; }

        [Required]
        public DateTime FechaNac { set; get; }

        [Required]
        public String FotoUsuario { set; get; }
        [Required]
        public int EstadoCivil { set; get; }
        [Required]
        public Boolean TieneHermanos { set; get; }
    }
}
