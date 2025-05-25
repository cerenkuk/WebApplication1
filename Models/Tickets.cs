namespace WebApplication1.Models
{
    using System;
    using System.Collections.Generic;

    // Tickets s�n�f�, kullan�c�lar�n sat�n ald��� etkinlik biletlerini temsil eder.
    public partial class Tickets
    {
        // Biletin benzersiz kimli�i (Primary Key)
        public int TicketID { get; set; }

        // Bu biletin hangi kullan�c�ya ait oldu�unu belirtir (foreign key)
        public Nullable<int> UserID { get; set; }

        // Hangi etkinlik i�in al�nd���n� belirtir (foreign key)
        public Nullable<int> EventID { get; set; }

        // Bilet tipi (�rnek: VIP, Standart vb.)
        public string TicketType { get; set; }

        // Biletin fiyat�
        public Nullable<decimal> Price { get; set; }

        // Biletin sat�n al�nd��� tarih ve saat
        public Nullable<System.DateTime> PurchasedAt { get; set; }

        // Etkinlik ile ili�ki
        public virtual Events Events { get; set; }

        // Kullan�c� ile ili�ki
        public virtual Users Users { get; set; }
    }
}
