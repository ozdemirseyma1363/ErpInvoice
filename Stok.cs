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
    
    public partial class Stok
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Stok()
        {
            this.FaturaDetays = new HashSet<FaturaDetay>();
        }
    
        public int No { get; set; }
        public string Kod { get; set; }
        public string Ad { get; set; }
        public string Barkod { get; set; }
        public Nullable<decimal> BirimFiyat { get; set; }
        public Nullable<int> BirimNo { get; set; }
    
        public virtual Birim Birim { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<FaturaDetay> FaturaDetays { get; set; }
    }
}
