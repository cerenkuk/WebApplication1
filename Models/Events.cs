namespace WebApplication1.Models
{
    using System;
    using System.Collections.Generic;

    // Events sýnýfý, veritabanýndaki "Events" (Etkinlikler) tablosunu temsil eder.
    public partial class Events
    {
        // Yapýcý metot: CartItems ve Tickets iliþkisel koleksiyonlarýný baþlatýr.
        public Events()
        {
            this.CartItems = new HashSet<CartItems>(); // Her etkinliðin birden fazla sepet öðesi olabilir
            this.Tickets = new HashSet<Tickets>();     // Her etkinliðe ait birden fazla bilet olabilir
        }

        // Etkinlik birincil anahtarý
        public int EventID { get; set; }

        // Etkinlik baþlýðý (örneðin: "Rock Konseri")
        public string Title { get; set; }

        // Etkinlik tipi (örneðin: Konser, Tiyatro, Sergi vb.)
        public string EventType { get; set; }

        // Etkinlik tarihi
        public System.DateTime EventDate { get; set; }

        // Etkinliðin toplam kontenjaný
        public int Quota { get; set; }

        // Etkinlik bilet fiyatý
        public decimal Price { get; set; }

        // Etkinliðin aktif olup olmadýðýný belirten bayrak (true = göster, false = silinmiþ gibi davran)
        public Nullable<bool> IsActive { get; set; }

        // Bu etkinliðe ait sepet öðeleri (iliþkisel tablo baðlantýsý)
        public virtual ICollection<CartItems> CartItems { get; set; }

        // Bu etkinliðe ait biletler
        public virtual ICollection<Tickets> Tickets { get; set; }
    }
}
