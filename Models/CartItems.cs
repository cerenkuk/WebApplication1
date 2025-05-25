namespace WebApplication1.Models
{
    using System;
    using System.Collections.Generic;

    // CartItems sýnýfý, her kullanýcýnýn sepetine eklediði etkinlikleri temsil eder.
    public partial class CartItems
    {
        // Sepet öðesi benzersiz ID'si (Primary Key)
        public int CartItemID { get; set; }

        // Sepet öðesinin hangi kullanýcýya ait olduðunu belirten foreign key
        public Nullable<int> UserID { get; set; }

        // Hangi etkinliðe ait olduðunu belirten foreign key
        public Nullable<int> EventID { get; set; }

        // Bilet tipi (örneðin: Standart, VIP vs.)
        public string TicketType { get; set; }

        // Kaç adet bilet alýndýðýný tutar
        public Nullable<int> Quantity { get; set; }

        // Etkinlik ile olan iliþki (Navigation property)
        public virtual Events Events { get; set; }

        // Kullanýcý ile olan iliþki (Navigation property)
        public virtual Users Users { get; set; }
    }
}
