// custom nav
{
    var navItems = document.getElementsByName("nav-item")
    navItems.forEach(item => {
        var isPage = false;
        var currentPath = window.location.pathname;
        var itemName = item.innerText;
        item.classList.remove("active");
        switch (itemName) {
            case "Home":
                if (currentPath === "/") isPage = true;
                break;
            case "Shop":
                if (currentPath.toLowerCase().includes("product")) isPage = true;
                break;
            case "Voucher":
                if (currentPath.toLowerCase().includes("voucher")) isPage = true;
                break;
            case "Blog":
                if (currentPath.toLowerCase().includes("blog")) isPage = true;
                break;
            case "Order":
                if (currentPath.toLowerCase().includes("order")) isPage = true;
                break;
        }

        isPage && item.classList.add("active");
    })
}

// custom star rate comment product
{
    var starValue = 1;
    var stars = document.getElementsByName("star");
    var inputStar = document.getElementById("star-input");
    inputStar.value = starValue;

    var handleChooseStar = (e) => {
        starValue = e.target.dataset.value;
        inputStar.value = starValue;
        stars.forEach(star => {
            star.classList.remove("text-warning");
            star.dataset.value <= starValue && star.classList.add("text-warning");
        });
    }

    stars.forEach(star => {
        star.onclick = handleChooseStar;
    });
}

// handle choose sub-image of product
var thumbnails = document.querySelectorAll('.sub-image img');
var mainImage = document.getElementById('main-image');

thumbnails.forEach(function (thumbnail) {
    thumbnail.addEventListener('click', function () {
        // Thay đổi src của ảnh lớn thành src của ảnh nhỏ được nhấp vào
        mainImage.src = this.src;
    });
});