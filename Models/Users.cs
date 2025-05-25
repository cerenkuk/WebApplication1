namespace WebApplication1.Models
{
    using System;
    using System.Collections.Generic;

    // Users sýnýfý, sistemdeki kullanýcýlarý temsil eder.
    public partial class Users
    {
        // Kurucu metod: Kullanýcý ile iliþkili sepet ve bilet nesneleri baþlatýlýr.
        public Users()
        {
            this.CartItems = new HashSet<CartItems>();
            this.Tickets = new HashSet<Tickets>();
        }

        // Kullanýcýnýn benzersiz kimliði (Primary Key)
        public int UserID { get; set; }

        // Kullanýcýnýn e-posta adresi
        public string Email { get; set; }

        // Þifre hash deðeri (þifre güvenli bir þekilde saklanýr)
        public string PasswordHash { get; set; }

        // Kullanýcýnýn sistem tarafýndan onaylý olup olmadýðýný belirtir
        public Nullable<bool> IsApproved { get; set; }

        // Kullanýcýnýn rolü (örn: "User", "Admin")
        public string Role { get; set; }

        // Kullanýcýnýn hesap oluþturma tarihi
        public Nullable<System.DateTime> CreatedAt { get; set; }

        // Kullanýcýnýn sepete eklediði ürünler ile olan iliþkisi
        public virtual ICollection<CartItems> CartItems { get; set; }

        // Kullanýcýnýn satýn aldýðý biletler ile olan iliþkisi
        public virtual ICollection<Tickets> Tickets { get; set; }
    }
}
