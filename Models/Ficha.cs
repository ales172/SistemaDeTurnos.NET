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
    
    public partial class Ficha
    {

        public int Id_Ficha { get; set; }
        public int Id_Paciente { get; set; }
        public Nullable<System.DateTime> Fecha_Ingreso { get; set; }
        public string Diagnostico { get; set; }
        public string Antecedentes { get; set; }
        public string Contraindicaciones { get; set; }
    
        public virtual Paciente Paciente { get; set; }
    }
}
