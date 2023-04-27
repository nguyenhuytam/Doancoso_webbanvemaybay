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

}////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

    const nembleP = document.querySelector('.btn-number-p')
    const closeNembleP = document.querySelector('body')
    const pTable = document.querySelector('.p-table')
    nembleP.addEventListener('click', function () {
        document.querySelector(".p-table").style.display = "block"
    })
    closeNembleP.addEventListener('click', function () {
        document.querySelector(".p-table").style.display = "none"
    })
    nembleP.addEventListener('click', function (event) {
        event.stopPropagation()
    })
    pTable.addEventListener('click', function (event) {
        event.stopPropagation()
    })
    const locationBtns = document.querySelectorAll('.js-open-location')
    const locationOnly = document.querySelector('.js-location')
    const locationContainer = document.querySelector('.js-location-container')
    const locationClose = document.querySelector('.js-location-close')
    function showLocations() {
        locationOnly.classList.add('location-open')
    }
    function hideLocations() {
        locationOnly.classList.remove('location-open')
    }
    for (const locationBtn of locationBtns) {
        locationBtn.addEventListener('click', showLocations)
    }
    locationClose.addEventListener('click', hideLocations)
    locationOnly.addEventListener('click', hideLocations)
    locationContainer.addEventListener('click', function (event) {
        event.stopPropagation()
    })
    const locationBtns2 = document.querySelectorAll('.js-open-location-2')
    const locationOnly2 = document.querySelector('.js-location-2')
    const locationContainer2 = document.querySelector('.js-location-container-2')
    const locationClose2 = document.querySelector('.js-location-close-2')
    function showLocations2() {
        locationOnly2.classList.add('location-open')
    }
    function hideLocations2() {
        locationOnly2.classList.remove('location-open')
    }
    for (const locationBtn2 of locationBtns2) {
        locationBtn2.addEventListener('click', showLocations2)
    }
    locationClose2.addEventListener('click', hideLocations2)
    locationOnly2.addEventListener('click', hideLocations2)
    locationContainer2.addEventListener('click', function (event) {
        event.stopPropagation()
    })
    document.getElementById('one-way').onclick = function (e) {
        if (this.checked) {
        document.querySelector("#round-trip_day").style = "opacity: 0.3; cursor: no-drop;"
            document.querySelector("#round-trip_day").disabled = true;
        }
    }
    document.getElementById('round-trip').onclick = function (e) {
        if (this.checked) {
        document.querySelector("#round-trip_day").style = "opacity: 1; cursor: pointer;"
            document.querySelector("#round-trip_day").disabled = false;
        }
    }
    const chonDiaDiems = document.querySelectorAll('.locition-body-content li')
    for (const chonDiaDiem of chonDiaDiems) {
        chonDiaDiem.addEventListener('click', function () {
            document.querySelector("#to").value = chonDiaDiem.innerText;
        })
        chonDiaDiem.addEventListener('click', hideLocations)
    }
    const chonDiaDiems2 = document.querySelectorAll('.locition-body-content-2 li')
    for (const chonDiaDiem2 of chonDiaDiems2) {
        chonDiaDiem2.addEventListener('click', function () {
            document.querySelector("#from").value = chonDiaDiem2.innerText;
        })
        chonDiaDiem2.addEventListener('click', hideLocations2)
    }
    //js tinh tong nguoi
    let amountTong = document.getElementById('p-tong')
    let amountT = amountTong.value
    let renderT = (amountT) => {
        amountTong.value = amountT
    }
    amountTong.addEventListener('input', () => {
        amountT = amountTong.value;
    amountT = parseInt(amountT);
    amountT = (isNaN(amountT) || amountT < 1 || amountT > 9) ? 1 : amountT;
    renderT(amountT)
    });
    //js tinh tong nguoi lon
    let amountNguoiLon = document.getElementById('p-nl')
    let amountNL = amountNguoiLon.value
    let renderNL = (amountNL) => {
        amountNguoiLon.value = amountNL
    }
    let handlePlusnl = () => {
        if (amountNL < 9 && amountT < 9) {
        amountNL++
            amountT++
        }
    renderNL(amountNL);
    renderT(amountT);
    }
    let handleMinusnl = () => {
        if (amountNL > 1 && amountT > 1) {
        amountNL--
            amountT--
        }
    renderNL(amountNL);
    renderT(amountT);
    }
    amountNguoiLon.addEventListener('input', () => {
        amountNL = amountNguoiLon.value;
    amountNL = parseInt(amountNL);
    amountNL = (isNaN(amountNL) || amountNL < 1 || amountNL > 9) ? 1 : amountNL;
    renderNL(amountNL)
    });
    //js tinh tong tre em
    let amountTreEm = document.getElementById('p-te')
    let amountTE = amountTreEm.value
    let renderTE = (amountTE) => {
        amountTreEm.value = amountTE
    }
    let handlePluste = () => {
        if (amountTE < 9 && amountT < 9) {
        amountTE++
            amountT++
        }
    renderTE(amountTE);
    renderT(amountT);
    }
    let handleMinuste = () => {
        if (amountTE > 0 && amountT > 1) {
        amountTE--
            amountT--
        }
    renderTE(amountTE);
    renderT(amountT);
    }
    amountTreEm.addEventListener('input', () => {
        amountTE = amountTreEm.value;
    amountTE = parseInt(amountTE);
    amountTE = (isNaN(amountTE) || amountTE < 0 || amountTE > 9) ? 1 : amountTE;
    renderTE(amountTE)
    });
    //js tinh tong em be
    let amountEmBe = document.getElementById('p-eb')
    let amountEB = amountEmBe.value
    let renderEB = (amountEB) => {
        amountEmBe.value = amountEB
    }
    let handlePluseb = () => {
        if (amountEB < 9 && amountT < 9) {
        amountEB++
            amountT++
        }
    renderEB(amountEB);
    renderT(amountT)
    }
    let handleMinuseb = () => {
        if (amountEB > 0 && amountT > 1) {
        amountEB--
            amountT--
        }
    renderEB(amountEB);
    renderT(amountT);
    }
    amountEmBe.addEventListener('input', () => {
        amountEB = amountEmBe.value;
    amountEB = parseInt(amountEB);
    amountEB = (isNaN(amountEB) || amountEB < 0 || amountEB > 9) ? 1 : amountEB;
    renderEB(amountEB)
    });
