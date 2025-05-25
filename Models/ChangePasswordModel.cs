namespace WebApplication1.Models
{
    // Şifre değiştirme işlemi için kullanılan veri modelidir.
    public class ChangePasswordModel
    {
        // Kullanıcının ID'si (kimliği)
        public int UserId { get; set; }

        // Kullanıcının mevcut (eski) şifresi
        public string OldPassword { get; set; }

        // Yeni belirlenmek istenen şifre
        public string NewPassword { get; set; }
    }
}
