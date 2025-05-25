namespace WebApplication1.Models
{
    using System;
    using System.Collections.Generic;

    // CartItems s�n�f�, her kullan�c�n�n sepetine ekledi�i etkinlikleri temsil eder.
    public partial class CartItems
    {
        // Sepet ��esi benzersiz ID'si (Primary Key)
        public int CartItemID { get; set; }

        // Sepet ��esinin hangi kullan�c�ya ait oldu�unu belirten foreign key
        public Nullable<int> UserID { get; set; }

        // Hangi etkinli�e ait oldu�unu belirten foreign key
        public Nullable<int> EventID { get; set; }

        // Bilet tipi (�rne�in: Standart, VIP vs.)
        public string TicketType { get; set; }

        // Ka� adet bilet al�nd���n� tutar
        public Nullable<int> Quantity { get; set; }

        // Etkinlik ile olan ili�ki (Navigation property)
        public virtual Events Events { get; set; }

        // Kullan�c� ile olan ili�ki (Navigation property)
        public virtual Users Users { get; set; }
    }
}
