// Entity Framework tarafından otomatik oluşturulmuş bir sınıftır.
// Bu sınıf, veritabanıyla iletişimi sağlayan DbContext nesnesidir.

namespace WebApplication1.Models
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;

    // Etkinlik sistemine ait veritabanı bağlamı.
    // DbContext sınıfından türetilmiştir ve uygulamanın veri modeliyle veritabanı arasındaki ilişkiyi sağlar.
    public partial class EtkinlikDBEntities1 : DbContext
    {
        // Yapıcı metot, bağlantı string'ini "EtkinlikDBEntities1" olarak ayarlar (Web.config içinde tanımlı).
        public EtkinlikDBEntities1()
            : base("name=EtkinlikDBEntities1")
        {
        }

        // Entity Framework'ün Code-First yapılandırmasıyla uyumsuzluk durumunu belirtir.
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }

        // Aşağıdaki DbSet tanımlamaları, veritabanındaki tabloları temsil eder.
        public virtual DbSet<Announcements> Announcements { get; set; }  // Duyurular tablosu
        public virtual DbSet<CartItems> CartItems { get; set; }          // Kullanıcıların sepetindeki etkinlikler
        public virtual DbSet<Events> Events { get; set; }                // Etkinlik bilgileri
        public virtual DbSet<Tickets> Tickets { get; set; }              // Satın alınan bilet bilgileri
        public virtual DbSet<Users> Users { get; set; }                  // Kullanıcılar
    }
}
