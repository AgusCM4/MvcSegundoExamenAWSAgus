using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MvcSegundoExamenAWSAgus.Models
{
    public class Evento
    {
        public int IdEvento { get; set; }

        public string Nombre { get; set; }

        public string Artista { get; set; }

        public int IdCategoria { get; set; }
    }
}
