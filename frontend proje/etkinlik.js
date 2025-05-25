// Başlangıç verisi olarak örnek etkinlikler tanımlanır (gerçek projede API'den alınır)
let events = [
    { id: 1, name: "Rock Gecesi", type: "Konser", date: "2025-05-20", capacity: 50, price: 500 },
    { id: 2, name: "Hamlet", type: "Tiyatro", date: "2025-06-10", capacity: 40, price: 300 },
    { id: 3, name: "Modern Sanat", type: "Sergi", date: "2025-07-01", capacity: 60, price: 150 }
];

let announcements = []; // Duyurular dizisi
let usersToApprove = ["ali123", "ayse456"]; // Onay bekleyen kullanıcılar

// Sayfa içindeki bölümleri gösteren fonksiyon
function showSection(id) {
    document.querySelectorAll("section").forEach(section => section.classList.remove("active"));
    document.getElementById(id).classList.add("active");

    if (id === "admin") {
        renderAdminEventList();
        renderUserApprovalList();
    }
    if (id === "events") {
        renderEvents();
    }
}

// Giriş formu gönderildiğinde çalışır
document.getElementById("loginForm").addEventListener("submit", async function (e) {
    e.preventDefault();

    const email = document.getElementById("loginEmail").value;
    const password = document.getElementById("loginPassword").value;

    try {
        const response = await fetch("https://localhost:5001/api/Auth/Login", {
            method: "POST",
            headers: { "Content-Type": "application/json" },
            body: JSON.stringify({ email, password })
        });

        if (!response.ok) {
            const errorData = await response.json();
            throw new Error(errorData.message || "Giriş başarısız");
        }

        const data = await response.json();
        localStorage.setItem("token", data.token); // Token'ı localStorage'a kaydet
        alert("Giriş başarılı!");
        showSection("events"); // Etkinlik sayfasına yönlendir
    } catch (error) {
        alert("Giriş başarısız: " + error.message);
    }
});

// Kayıt formu gönderildiğinde çalışır
document.getElementById("registerForm").addEventListener("submit", async function (e) {
    e.preventDefault();

    const name = document.getElementById("registerName").value;
    const surname = document.getElementById("registerSurname").value;
    const email = document.getElementById("registerEmail").value;
    const password = document.getElementById("registerPassword").value;
    const registerMessage = document.getElementById("registerMessage");

    try {
        const response = await fetch("https://localhost:5001/api/Auth/register", {
            method: "POST",
            headers: { "Content-Type": "application/json" },
            body: JSON.stringify({ name, surname, email, password })
        });

        if (response.ok) {
            registerMessage.textContent = "Kayıt başarılı! Yönetici onayı bekleniyor.";
            registerMessage.style.color = "green";
        } else {
            const error = await response.json();
            registerMessage.textContent = error.message || "Kayıt başarısız oldu.";
            registerMessage.style.color = "red";
        }
    } catch (err) {
        registerMessage.textContent = "Sunucuya bağlanılamadı.";
        registerMessage.style.color = "red";
    }
});

// Şifre değiştirme işlemi
document.getElementById("changePasswordForm").addEventListener("submit", async function (e) {
    e.preventDefault();

    const oldPassword = document.getElementById("currentPassword").value;
    const newPassword = document.getElementById("newPassword").value;
    const confirmNewPassword = document.getElementById("confirmPassword").value;
    const passwordChangeMessage = document.getElementById("passwordChangeMessage");

    if (newPassword !== confirmNewPassword) {
        passwordChangeMessage.textContent = "Yeni şifreler uyuşmuyor.";
        passwordChangeMessage.style.color = "red";
        return;
    }

    try {
        const response = await fetch("https://localhost:5001/api/Auth/ChangePassword", {
            method: "POST",
            headers: {
                "Content-Type": "application/json",
                "Authorization": "Bearer " + localStorage.getItem("token")
            },
            body: JSON.stringify({ oldPassword, newPassword, confirmNewPassword })
        });

        if (response.ok) {
            passwordChangeMessage.textContent = "Şifre başarıyla değiştirildi.";
            passwordChangeMessage.style.color = "green";
            document.getElementById("changePasswordForm").reset(); // Formu temizle
        } else {
            const errorData = await response.json();
            passwordChangeMessage.textContent = `Şifre değişikliği başarısız: ${errorData.message || 'Bir sorun oluştu.'}`;
            passwordChangeMessage.style.color = "red";
        }
    } catch (error) {
        passwordChangeMessage.textContent = "Sunucuya bağlanılamadı: " + error.message;
        passwordChangeMessage.style.color = "red";
    }
});

// Etkinlikleri listeleme
function renderEvents() {
    const area = document.getElementById("eventList");
    area.innerHTML = "";
    events.forEach(ev => {
        area.innerHTML += `
            <div class="event">
                <strong>${ev.type}: ${ev.name}</strong><br>
                Tarih: ${ev.date}<br>
                Kontenjan: ${ev.capacity}<br>
                Fiyat: ${ev.price} TL<br>
                ${ev.capacity > 0 ? `<button class="form-button" onclick="addToCart(${ev.id}, '${ev.name}', '${ev.type}', ${ev.price})">Sepete Ekle</button>` : '<span style="color:red;">Kontenjan Dolu</span>'}
            </div>
        `;
    });
}

// Sepete etkinlik ekleme
function addToCart(eventId, name, type, price) {
    let cart = JSON.parse(localStorage.getItem('cart')) || [];
    const existingItemIndex = cart.findIndex(item => item.id === eventId);

    if (existingItemIndex === -1) {
        const eventIndex = events.findIndex(ev => ev.id === eventId);
        if (events[eventIndex].capacity > 0) {
            const newItem = { id: eventId, name, type, price };
            cart.push(newItem);
            localStorage.setItem('cart', JSON.stringify(cart));
            events[eventIndex].capacity--;
            renderEvents();
            renderAdminEventList();
            alert(`${name} sepetinize eklendi! Kalan kontenjan: ${events[eventIndex].capacity}`);
        } else {
            alert(`${name} etkinliğinin kontenjanı dolmuştur.`);
        }
    } else {
        alert(`${name} zaten sepetinizde.`);
    }
}

// Sepetim sayfasına git
function goToCart() {
    alert("Sepetim sayfasına yönlendiriliyor... (Bu fonksiyon henüz implemente edilmedi.)");
    // window.location.href = 'sepet.html';
}

// Yeni etkinlik ekleme
function addEvent() {
    const name = document.getElementById("newEventName").value;
    const date = document.getElementById("newEventDate").value;
    const type = document.getElementById("newEventType").value;
    const capacity = parseInt(document.getElementById("newEventCapacity").value);
    const price = parseFloat(document.getElementById("newEventPrice").value);

    if (!name || !date || !type || isNaN(capacity) || isNaN(price)) {
        alert("Lütfen tüm alanları doldurun ve geçerli bir fiyat girin.");
        return;
    }

    const newId = events.length ? events[events.length - 1].id + 1 : 1;
    events.push({ id: newId, name, type, date, capacity, price });
    alert("Etkinlik eklendi!");
    renderEvents();
    renderAdminEventList();
}

// Etkinlik silme
function deleteEvent(id) {
    events = events.filter(ev => ev.id !== id);
    alert("Etkinlik silindi!");
    renderEvents();
    renderAdminEventList();
}

// Admin panelinde etkinlik listesi
function renderAdminEventList() {
    const area = document.getElementById("adminEventList");
    area.innerHTML = "";
    events.forEach(ev => {
        area.innerHTML += `
            <div class="event">
                <strong>${ev.name}</strong> (${ev.type}) - ${ev.price} TL<br>
                Tarih: ${ev.date} | Kontenjan: ${ev.capacity}<br>
                <button class="form-button" onclick="deleteEvent(${ev.id})">Sil</button>
            </div>
        `;
    });
}

// Duyuru ekleme
function addAnnouncement() {
    const text = document.getElementById("newAnnouncement").value;
    if (text.trim() === "") return alert("Boş duyuru eklenemez.");
    announcements.push(text);
    alert("Duyuru eklendi!");
    document.getElementById("newAnnouncement").value = "";
}

// Yönetici onay paneli
function renderUserApprovalList() {
    const area = document.getElementById("userApprovalList");
    area.innerHTML = "";
    if (usersToApprove.length === 0) {
        area.innerHTML = "<i>Onay bekleyen kullanıcı yok.</i>";
    } else {
        usersToApprove.forEach((user, index) => {
            area.innerHTML += `
                <div>
                    Kullanıcı: <strong>${user}</strong>
                    <button class="form-button" onclick="approveUser(${index})">Onayla</button>
                </div>
            `;
        });
    }
}

// Kullanıcıyı onayla
function approveUser(index) {
    const approvedUser = usersToApprove.splice(index, 1);
    alert(`${approvedUser} kullanıcısı onaylandı!`);
    renderUserApprovalList();
}

// Sayfa yüklendiğinde çalışacak başlangıç işlemleri
document.addEventListener('DOMContentLoaded', function () {
    // Kullanıcı giriş yapmışsa biletlerini getir
    if (localStorage.getItem("token")) {
        fetch("https://localhost:5001/api/User/Tickets", {
            headers: {
                "Authorization": "Bearer " + localStorage.getItem("token")
            }
        })
            .then(res => res.json())
            .then(data => console.log("Biletlerim:", data))
            .catch(err => console.error("Biletler alınırken hata:", err));
    }

    // Etkinlikleri API'den al (veya local verileri kullan)
    fetch("https://localhost:5001/api/Events")
        .then(res => {
            if (!res.ok) throw new Error(`HTTP error! status: ${res.status}`);
            return res.json();
        })
        .then(data => {
            console.log("Etkinlikler API'den alındı:", data);
            // events = data; // API verisi kullanılacaksa aktif et
            renderEvents();
        })
        .catch(err => {
            console.warn("API başarısız. Yerel veriler gösteriliyor:", err);
            renderEvents();
        });

    showSection('login'); // Başlangıçta giriş ekranı göster
});
