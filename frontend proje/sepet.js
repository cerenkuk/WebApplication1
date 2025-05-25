document// Sayfa yüklendiğinde sepeti ekrana getir
document.addEventListener('DOMContentLoaded', function () {
    renderCart(); // renderCart fonksiyonunu çağır
});

// Sepeti ekranda görüntüleyen fonksiyon
function renderCart() {
    const cartItemsDiv = document.getElementById('cart-items'); // Sepet öğelerinin gösterileceği HTML alanı
    const totalPriceDiv = document.getElementById('total-price'); // Toplam fiyatın gösterileceği alan
    const paymentTotalSpan = document.getElementById('payment-total'); // Ödeme bölümündeki toplam fiyat
    const cart = JSON.parse(localStorage.getItem('cart')) || []; // LocalStorage'dan sepet verilerini al
    let totalPrice = 0; // Toplam fiyatı hesaplamak için değişken

    if (cart.length === 0) {
        // Eğer sepet boşsa, kullanıcıya bilgi ver
        cartItemsDiv.innerHTML = '<p id="empty-cart">Sepetiniz boş.</p>';
        totalPriceDiv.textContent = '';
        paymentTotalSpan.textContent = '0';
    } else {
        cartItemsDiv.innerHTML = ''; // Önceki içeriği temizle
        cart.forEach((item, index) => {
            // Her ürün için HTML elemanı oluştur
            cartItemsDiv.innerHTML += `
                <div class="cart-item">
                    <div>
                        <strong>${item.name}</strong> (${item.type}) - ${item.price} TL
                    </div>
                    <button onclick="removeFromCart(${index}, ${item.price})">Kaldır</button>
                </div>
            `;
            totalPrice += item.price; // Fiyatı toplam fiyata ekle
        });

        // Toplam fiyat alanlarını güncelle
        totalPriceDiv.textContent = `Toplam: ${totalPrice} TL`;
        paymentTotalSpan.textContent = totalPrice;
    }
}

// Sepetten bir öğeyi kaldıran fonksiyon
function removeFromCart(index, price) {
    let cart = JSON.parse(localStorage.getItem('cart')) || [];
    cart.splice(index, 1); // Belirtilen index'teki ürünü çıkar
    localStorage.setItem('cart', JSON.stringify(cart)); // Güncellenmiş sepeti kaydet
    renderCart(); // Sepeti yeniden render et
}

// Ödeme işlemini gerçekleştiren fonksiyon
function processPayment() {
    const cart = JSON.parse(localStorage.getItem('cart')) || [];

    if (cart.length > 0) {
        // Sepette ürün varsa her ürün için API'ye istek gönder
        cart.forEach(item => {
            fetch("https://localhost:5001/api/Cart/Add", {
                method: "POST",
                headers: {
                    "Content-Type": "application/json",
                    "Authorization": "Bearer " + localStorage.getItem("token") // Kullanıcı token'ı
                },
                body: JSON.stringify({
                    eventId: item.id // Sepetteki etkinliğin ID'si gönderilir
                })
            })
                .then(response => response.json())
                .then(data => console.log("Sepete ekleme yanıtı:", data))
                .catch(error => console.error("Sepete ekleme hatası:", error));
        });

        // Ödeme başarılı mesajı
        alert(`Toplam ${document.getElementById('payment-total').textContent} TL tutarında ödeme alındı. Etkinliklerinize katılımınız onaylanmıştır!`);

        localStorage.removeItem('cart'); // Sepeti temizle
        renderCart(); // Boş sepeti tekrar göster
    } else {
        alert("Sepetinizde ürün bulunmamaktadır.");
    }
}
