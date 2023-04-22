// -------------------------------Phần autoplay phần index.html

var slideIndex = 1;

// Next/previous controls
function plusSlides(n) {
  showSlides(slideIndex += n);
}

// Thumbnail image controls
function currentSlide(n) {
  showSlides(slideIndex = n);
}

function showSlides(n) {
  var i;
  var slides = document.getElementsByClassName("mySlides");
  var dots = document.getElementsByClassName("dot");
  if (n > slides.length) {slideIndex = 1} 
  if (n < 1) {slideIndex = slides.length}
  for (i = 0; i < slides.length; i++) {
      slides[i].style.display = "none"; 
  }
  for (i = 0; i < dots.length; i++) {
      dots[i].className = dots[i].className.replace(" active", "");
  }
  slides[slideIndex-1].style.display = "block"; 
  dots[slideIndex-1].className += " active";
}

// Tự động chuyển slide
setInterval(function () {
  plusSlides(1);
}, 3000); // 3000 là thời gian chuyển slide tính bằng mili giây

// Phần nút next page trong CTphim.html------------------------------------------------------------------------------------------

const next = document.querySelectorAll(".pagenext");
const prev = document.querySelectorAll(".pageprev");
const movieList = document.querySelectorAll(".movie-list");

next.forEach((pagenext ,i) => {
  const itemNumber = movieList[i].querySelectorAll("img").length;
  let clickCounter = 0 ;
      pagenext.addEventListener("click", () => {
      clickCounter++;
      if(itemNumber-(6 + clickCounter) >=0){
        movieList[i].style.transform = `translateX(${
          movieList[i].computedStyleMap().get("transform")[0].x.value
          -210}px)`;
      }else{
        movieList[i].style.transform="translateX(0)";
        clickCounter=0;
      } 
      console.log(movieList[i].querySelectorAll("img").length);
    });
});
prev.forEach((pageprev ,i) => {
  let clickCounter = 0 ;
  pageprev.addEventListener("click", () => {
    if ( clickCounter >=0) {
      clickCounter--;
      movieList[i].style.transform = `translateX(${
        movieList[i].computedStyleMap().get("transform")[0].x.value
        +210}px)`;
    } else {
      movieList[i].style.transform = `translateX(${
        movieList[i.length-1]}`;
      clickCounter=0;
    }
    console.log(movieList[i].querySelectorAll("img").length);
  });
});



/////////////////////////////////////////////////// đăng nhập 
const wrapper = document.querySelector('.wrapper');

function registerActive(){
    wrapper.classList.toggle('active');
}
function loginActive(){
    wrapper.classList.toggle('active');
    ////////////////////////////////////////////// tìm kiếm 
    function searchMovies() {
        // Lấy giá trị từ ô tìm kiếm
        var input = document.getElementById("search-input");
        var filter = input.value.toUpperCase();

        // Lấy danh sách phim
        var movies = document.getElementsByClassName("movie");

        // Lặp qua danh sách phim và hiển thị các phim thỏa mãn điều kiện tìm kiếm
        for (var i = 0; i < movies.length; i++) {
            var title = movies[i].getElementsByClassName("title")[0];
            if (title.innerHTML.toUpperCase().indexOf(filter) > -1) {
                movies[i].style.display = "";
            } else {
                movies[i].style.display = "none";
            }
        }
    }

    // Thêm sự kiện cho ô tìm kiếm
    var input = document.getElementById("search-input");
    input.addEventListener("keyup", function (event) {
        // Bắt đầu tìm kiếm khi người dùng nhấn Enter
        if (event.keyCode === 13) {
            event.preventDefault();
            searchMovies();
        }
    });

    // Thêm sự kiện cho nút tìm kiếm
    var searchBtn = document.querySelector(".bx-search");
    searchBtn.addEventListener("click", searchMovies);

} 


//// xem sau 
