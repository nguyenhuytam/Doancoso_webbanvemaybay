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