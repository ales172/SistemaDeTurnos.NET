//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace SistemaDeTurnos.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class Observacion
    {
        public int Id_Observacion { get; set; }
        public Nullable<System.DateTime> Fecha { get; set; }
        public int Id_Paciente { get; set; }
        public int Id_Medico { get; set; }
        public string Detalle { get; set; }
    
        public virtual Medico Medico { get; set; }
        public virtual Paciente Paciente { get; set; }
    }
}