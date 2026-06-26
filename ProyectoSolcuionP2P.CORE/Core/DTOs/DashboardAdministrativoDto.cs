using System;
using System.Collections.Generic;
using System.Text;

namespace ProyectoSolucionP2P.CORE.Core.DTOs
{
    public class DashboardAdministrativoDto
    {
        public int TotalUsuarios { get; set; }
        public int TotalOperaciones { get; set; }
        public int TotalDisputas { get; set; }
        public int VerificacionesPendientes { get; set; }
    }
}
