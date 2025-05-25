namespace WebApplication1.Models
{
    using System;
    using System.Collections.Generic;

    // Tickets sýnýfý, kullanýcýlarýn satýn aldýðý etkinlik biletlerini temsil eder.
    public partial class Tickets
    {
        // Biletin benzersiz kimliði (Primary Key)
        public int TicketID { get; set; }

        // Bu biletin hangi kullanýcýya ait olduðunu belirtir (foreign key)
        public Nullable<int> UserID { get; set; }

        // Hangi etkinlik için alýndýðýný belirtir (foreign key)
        public Nullable<int> EventID { get; set; }

        // Bilet tipi (örnek: VIP, Standart vb.)
        public string TicketType { get; set; }

        // Biletin fiyatý
        public Nullable<decimal> Price { get; set; }

        // Biletin satýn alýndýðý tarih ve saat
        public Nullable<System.DateTime> PurchasedAt { get; set; }

        // Etkinlik ile iliþki
        public virtual Events Events { get; set; }

        // Kullanýcý ile iliþki
        public virtual Users Users { get; set; }
    }
}
