//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace ErpFatura
{
    using System;
    using System.Collections.Generic;
    
    public partial class FaturaDetay
    {
        public int No { get; set; }
        public Nullable<int> FaturaNo { get; set; }
        public Nullable<int> StokNo { get; set; }
        public Nullable<decimal> Miktar { get; set; }
        public Nullable<decimal> BirimFiyat { get; set; }
        public Nullable<decimal> KdvOrani { get; set; }
        public Nullable<decimal> Tutar { get; set; }
        public Nullable<decimal> KdvTutar { get; set; }
        public Nullable<int> BirimNo { get; set; }
    
        public virtual Fatura Fatura { get; set; }
        public virtual Stok Stok { get; set; }
    }
}
