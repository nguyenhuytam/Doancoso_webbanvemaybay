//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace web_banvemaybay.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class Ve
    {
        public int IDve { get; set; }
        public Nullable<int> IDdattruoc { get; set; }
        public Nullable<int> IDchuyenbay { get; set; }
        public Nullable<int> IDhanhkhach { get; set; }
        public string Tinhtrang { get; set; }
        public Nullable<int> IDlienhe { get; set; }
        public Nullable<int> IDhanhli { get; set; }
        public DateTime Ngaydatve { get; set; }
        public double Gia { get; set; }
        public Nullable<int> IDhangve { get; set; }
        public double Tienphat { get; set; }
        public Nullable<int> Sove { get; set; }
        public double Hoangtra { get; set; }
        public Nullable<int> IDchuyenbayve { get; set; }
    
        public virtual Chuyenbay Chuyenbay { get; set; }
        public virtual Dattruoc Dattruoc { get; set; }
        public virtual Hangve Hangve { get; set; }
        public virtual Hanhkhach Hanhkhach { get; set; }
        public virtual Hanhli Hanhli { get; set; }
        public virtual TTlienhe TTlienhe { get; set; }
    }
}
