namespace WebApplication1.Models
{
    using System;
    using System.Collections.Generic;

    // Users s�n�f�, sistemdeki kullan�c�lar� temsil eder.
    public partial class Users
    {
        // Kurucu metod: Kullan�c� ile ili�kili sepet ve bilet nesneleri ba�lat�l�r.
        public Users()
        {
            this.CartItems = new HashSet<CartItems>();
            this.Tickets = new HashSet<Tickets>();
        }

        // Kullan�c�n�n benzersiz kimli�i (Primary Key)
        public int UserID { get; set; }

        // Kullan�c�n�n e-posta adresi
        public string Email { get; set; }

        // �ifre hash de�eri (�ifre g�venli bir �ekilde saklan�r)
        public string PasswordHash { get; set; }

        // Kullan�c�n�n sistem taraf�ndan onayl� olup olmad���n� belirtir
        public Nullable<bool> IsApproved { get; set; }

        // Kullan�c�n�n rol� (�rn: "User", "Admin")
        public string Role { get; set; }

        // Kullan�c�n�n hesap olu�turma tarihi
        public Nullable<System.DateTime> CreatedAt { get; set; }

        // Kullan�c�n�n sepete ekledi�i �r�nler ile olan ili�kisi
        public virtual ICollection<CartItems> CartItems { get; set; }

        // Kullan�c�n�n sat�n ald��� biletler ile olan ili�kisi
        public virtual ICollection<Tickets> Tickets { get; set; }
    }
}
