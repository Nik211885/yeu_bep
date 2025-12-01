// Cấu hình chung cho toastr
const notifications =[];
$.apiHelper.get("/api/notification", {
    success: function(response) {
        response.items.forEach((element) => {
            notifications.push({
                title: element.title,
                text: element.text,
                message: element.body,
                type:'Kiểm duyệt công thức',
                time:  element.time,
                link: element.link,
            });
        })
        renderNotifications();
    }
})
toastr.options = {
    closeButton: true,
    progressBar: true,
    positionClass: "toast-bottom-right",
    timeOut: 3000,
    extendedTimeOut: 1000,
    newestOnTop: true,
    showDuration: 300,
    hideDuration: 500,
    showMethod: "fadeIn",
    hideMethod: "fadeOut"
};

window.toast = {
    success: function(message, title) {
        toastr.success(message, title || "Thành công");
    },
    error: function(message, title) {
        toastr.error(message, title || "Lỗi");
    },
    info: function(message, title) {
        toastr.info(message, title || "Thông tin");
    },
    warning: function(message, title) {
        toastr.warning(message, title || "Cảnh báo");
    }
};
document.addEventListener('click', function(event) {
    const dropdown = document.getElementById('notificationDropdown');
    const headerIcon = document.querySelector('.header-icon');

    if (!dropdown.contains(event.target) && !headerIcon.contains(event.target)) {
        dropdown.classList.remove('active');
    }
});

function toggleNotifications() {
    const dropdown = document.getElementById('notificationDropdown');
    dropdown.classList.toggle('active');
}

function renderNotifications() {
    updateNotificationCount();
    const notificationList = document.getElementById('notificationList');

    if (notifications.length === 0) {
        notificationList.innerHTML = `
                    <div class="empty-notifications">
                        <i class="fas fa-bell-slash"></i>
                        <p>Không có thông báo nào</p>
                    </div>
                `;
        return;
    }

    notificationList.innerHTML = notifications.map(notif => `
                <a href="/${notif.link}" style="text-decoration: beige !important;">
                    <div class="notification-item">
                        <div class="notification-icon ${notif.type}">
                            <i class="fas ${notif.icon}"></i>
                        </div>
                        <div class="notification-content">
                            <div class="notification-title">${notif.title}</div>
                            <div class="notification-message">${notif.message}</div>
                            <div class="notification-time">${notif.time}</div>
                        </div>
                    </div>
                </a>
            `).join('');
    
}

function updateNotificationCount() {
    const unreadCount = notifications.length;
    const badge = document.getElementById('notificationCount');
    if (unreadCount > 0) {
        badge.textContent = unreadCount > 9 ? '9+' : unreadCount;
        badge.style.display = 'flex';
    } else {
        badge.style.display = 'none';
    }
}

const connection = new signalR.HubConnectionBuilder()
    .withUrl("/notification")
    .build();

connection.on("ReceiveMessage", function (message) {
    const element = JSON.parse(message);
    console.log(element);
    notifications.unshift({
        title: element.Title,
        text: element.Text,
        message: element.Body,
        type:'Kiểm duyệt công thức',    
        time:  element.Time,
        link: element.Link
    });
    renderNotifications();
});

connection.start()
    .then(() => {
        console.log("Kết nối SignalR đến Server thành công!");
        $("#sendButton").prop("disabled", false);
    })
    .catch(err => {
        console.error("Lỗi kết nối SignalR: " + err.toString());
    });
