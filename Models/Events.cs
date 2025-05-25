namespace WebApplication1.Models
{
    using System;
    using System.Collections.Generic;

    // Events s�n�f�, veritaban�ndaki "Events" (Etkinlikler) tablosunu temsil eder.
    public partial class Events
    {
        // Yap�c� metot: CartItems ve Tickets ili�kisel koleksiyonlar�n� ba�lat�r.
        public Events()
        {
            this.CartItems = new HashSet<CartItems>(); // Her etkinli�in birden fazla sepet ��esi olabilir
            this.Tickets = new HashSet<Tickets>();     // Her etkinli�e ait birden fazla bilet olabilir
        }

        // Etkinlik birincil anahtar�
        public int EventID { get; set; }

        // Etkinlik ba�l��� (�rne�in: "Rock Konseri")
        public string Title { get; set; }

        // Etkinlik tipi (�rne�in: Konser, Tiyatro, Sergi vb.)
        public string EventType { get; set; }

        // Etkinlik tarihi
        public System.DateTime EventDate { get; set; }

        // Etkinli�in toplam kontenjan�
        public int Quota { get; set; }

        // Etkinlik bilet fiyat�
        public decimal Price { get; set; }

        // Etkinli�in aktif olup olmad���n� belirten bayrak (true = g�ster, false = silinmi� gibi davran)
        public Nullable<bool> IsActive { get; set; }

        // Bu etkinli�e ait sepet ��eleri (ili�kisel tablo ba�lant�s�)
        public virtual ICollection<CartItems> CartItems { get; set; }

        // Bu etkinli�e ait biletler
        public virtual ICollection<Tickets> Tickets { get; set; }
    }
}
