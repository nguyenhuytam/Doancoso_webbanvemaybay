const headerSticky = document.querySelector('#header')
window.addEventListener("scroll", function () {
    x = window.pageYOffset;
    if (x > 0) {
        header.classList.add("sticky")
    }
    else {
        header.classList.remove("sticky")
    }
})
window.onscroll = function () { scrollFunction() };
function scrollFunction() {
    if (document.body.scrollTop > 20 || document.documentElement.scrollTop > 20) {
        document.getElementById("myBtn").style.display = "block";
        document.getElementById("myZalo").style.display = "block";
        document.getElementById("myCall").style.display = "block";
    } else {
        document.getElementById("myBtn").style.display = "none";
    }
}
function topFunction() {
    document.body.scrollTop = 0;
    document.documentElement.scrollTop = 0;
}
//var header = document.getElementById('header');
//var mobileMenu = document.getElementById('mobile-menu');
//var headerHeigt = header.clientHeight;
////dong/mo mobile menu
//mobileMenu.onclick = function () {
//    var isClose = header.clientHeight === headerHeigt;
//    if (isClose) {
//        header.style.height = 'auto';
//    }
//    else {
//        header.style.height = null;
//    }
//}
////tu dong dong khi chon menu
//var menuItems = document.querySelectorAll('#nav li a[href*="#"]');
//for (var i = 0; i < menuItems.length; i++) {
//    var menuItem = menuItems[i];
//    menuItem.onclick = function (event) {
//        var isParentMenu = this.nextElementSibling && menuItem.nextElementSibling.classList.contains('subnav');
//        if (isParentMenu) {
//            event.preventDefault();
//        }
//        else {
//            header.style.height = null;
//        }
//    }
//}
//const nembleP = document.querySelector('.btn-number-p')
//const closeNembleP = document.querySelector('#backgroud')
//const closeNembleP2 = document.querySelector('#main')
//const pTable = document.querySelector('.p-table')
//nembleP.addEventListener('click', function () {
//    document.querySelector(".p-table").style.display = "block"
//})
//closeNembleP.addEventListener('click', function () {
//    document.querySelector(".p-table").style.display = "none"
//})
//closeNembleP2.addEventListener('click', function () {
//    document.querySelector(".p-table").style.display = "none"
//})
//nembleP.addEventListener('click', function (event) {
//    event.stopPropagation()
//})
//pTable.addEventListener('click', function (event) {
//    event.stopPropagation()
//})
//const locationBtns = document.querySelectorAll('.js-open-location')
//const locationOnly = document.querySelector('.js-location')
//const locationContainer = document.querySelector('.js-location-container')
//const locationClose = document.querySelector('.js-location-close')
//function showLocations() {
//    locationOnly.classList.add('location-open')
//}
//function hideLocations() {
//    locationOnly.classList.remove('location-open')
//}
//for (const locationBtn of locationBtns) {
//    locationBtn.addEventListener('click', showLocations)
//}
//locationClose.addEventListener('click', hideLocations)
//locationOnly.addEventListener('click', hideLocations)
//locationContainer.addEventListener('click', function (event) {
//    event.stopPropagation()
//})
//const locationBtns2 = document.querySelectorAll('.js-open-location-2')
//const locationOnly2 = document.querySelector('.js-location-2')
//const locationContainer2 = document.querySelector('.js-location-container-2')
//const locationClose2 = document.querySelector('.js-location-close-2')
//function showLocations2() {
//    locationOnly2.classList.add('location-open')
//}
//function hideLocations2() {
//    locationOnly2.classList.remove('location-open')
//}
//for (const locationBtn2 of locationBtns2) {
//    locationBtn2.addEventListener('click', showLocations2)
//}
//locationClose2.addEventListener('click', hideLocations2)
//locationOnly2.addEventListener('click', hideLocations2)
//locationContainer2.addEventListener('click', function (event) {
//    event.stopPropagation()
//})
//document.getElementById('one-way').onclick = function (e) {
//    if (this.checked) {
//        document.querySelector("#round-trip_day").style = "opacity: 0.3; cursor: no-drop;"
//        document.querySelector("#round-trip_day").disabled = true;
//    }
//}
//document.getElementById('round-trip').onclick = function (e) {
//    if (this.checked) {
//        document.querySelector("#round-trip_day").style = "opacity: 1; cursor: pointer;"
//        document.querySelector("#round-trip_day").disabled = false;
//    }
//}
//const chonDiaDiems = document.querySelectorAll('.locition-body-content li')
//for (const chonDiaDiem of chonDiaDiems) {
//    chonDiaDiem.addEventListener('click', function () {
//        document.querySelector("#to").value = chonDiaDiem.innerText;
//    })
//    chonDiaDiem.addEventListener('click', hideLocations)
//}
//const chonDiaDiems2 = document.querySelectorAll('.locition-body-content-2 li')
//for (const chonDiaDiem2 of chonDiaDiems2) {
//    chonDiaDiem2.addEventListener('click', function () {
//        document.querySelector("#form").value = chonDiaDiem2.innerText;
//    })
//    chonDiaDiem2.addEventListener('click', hideLocations2)
//} 