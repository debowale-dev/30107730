<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Index.aspx.cs" Inherits="WebApplication2.Index" %>

<!DOCTYPE html>
<html lang="en">

<head>
  <meta charset="utf-8">
  <meta content="width=device-width, initial-scale=1.0" name="viewport">
  <title>Index - Yummy Bootstrap Template</title>
  <meta name="description" content="">
  <meta name="keywords" content="">

  <!-- Favicons -->
  <link href="assets/img/favicon.png" rel="icon">
  <link href="assets/img/apple-touch-icon.png" rel="apple-touch-icon">

  <!-- Fonts -->
  <link href="https://fonts.googleapis.com" rel="preconnect">
  <link href="https://fonts.gstatic.com" rel="preconnect" crossorigin>
  <link href="https://fonts.googleapis.com/css2?family=Roboto:ital,wght@0,100;0,300;0,400;0,500;0,700;0,900;1,100;1,300;1,400;1,500;1,700;1,900&family=Inter:wght@100;200;300;400;500;600;700;800;900&family=Amatic+SC:wght@400;700&display=swap" rel="stylesheet">

  <!-- Vendor CSS Files -->
  <link href="assets/vendor/bootstrap/css/bootstrap.min.css" rel="stylesheet">
  <link href="assets/vendor/bootstrap-icons/bootstrap-icons.css" rel="stylesheet">
  <link href="assets/vendor/aos/aos.css" rel="stylesheet">
  <link href="assets/vendor/glightbox/css/glightbox.min.css" rel="stylesheet">
  <link href="assets/vendor/swiper/swiper-bundle.min.css" rel="stylesheet">

  <!-- Main CSS File -->
  <link href="assets/css/main.css" rel="stylesheet">

  <!-- =======================================================
  * Template Name: Yummy
  * Template URL: https://bootstrapmade.com/yummy-bootstrap-restaurant-website-template/
  * Updated: Aug 07 2024 with Bootstrap v5.3.3
  * Author: BootstrapMade.com
  * License: https://bootstrapmade.com/license/
  ======================================================== -->
 <style>
     .video-container {
       position: relative;
       display: inline-block;
     }

     .video-container img {
       display: block;
       max-width: 100%;
       height: auto;
     }

     .pulsating-play-btn {
       position: absolute;
       top: 50%;
       left: 50%;
       transform: translate(-50%, -50%);
     }

     .pulsating-play-btn:hover {
       transform: translate(-50%, -50%) scale(1.1);
     }

  .or-separator {
    position: relative;
    width: 100%;
  }

  .or-separator span {
    font-family: cursive;
    padding: 0 10px;
    font-size: 1.2rem;
    color: #333;
  }

  .or-separator:before {
    content: "";
    position: absolute;
    top: 50%;
    left: 0;
    right: 0;
    height: 1px;
    background-color: #ccc;
    z-index: -1;
  }
  .recipe-details {
    font-size: 14px; /* Smaller font size */
    line-height: 1.6;
    margin-bottom: 20px;
}

.recipe-details h4 {
    font-size: 16px;
    font-weight: bold;
}

.numbered-list {
    list-style-type: decimal;
    padding-left: 20px;
    margin-bottom: 20px;
}
.allergen-container {
    font-family: cursive;
    position: relative;
    width: 100%;
    height: 60px; /* Adjust height as needed */
    overflow: hidden;
    background: #ffdddd; /* Light red background for visibility */
    color: #900; /* Dark red text color */
    padding: 5px;
    font-weight: bold;
    border: 1px solid #900; /* Dark red border */
    margin-bottom: 20px; /* Space between labels */
    white-space: nowrap;
}

.allergen-container .allergen-label {
    display: inline-block;
    padding-left: 100%;
    animation: scroll-left 15s linear infinite;
}

@keyframes scroll-left {
    from {
        transform: translateX(100%);
    }
    to {
        transform: translateX(-100%);
    }
}
 .rating-container {
        display: inline-block;
        font-size: 2rem;
        color: #d3d3d3; /* Gray for inactive stars */
    }

    .star {
        cursor: pointer;
        transition: color 0.3s;
    }

    .star:hover, .star.active {
        color: gold; /* Gold for active stars */
    }


</style>
  <script src="https://code.jquery.com/jquery-3.6.0.min.js"></script>
<script>
    document.addEventListener('DOMContentLoaded', function () {
        const stars = document.querySelectorAll('.rating-container .star');

        stars.forEach(star => {
            star.addEventListener('click', function (event) {
                event.preventDefault(); // Prevent default form behavior
                const rating = this.getAttribute('data-value');
                const predictionId = this.closest('form').querySelector('input[name="predictionId"]').value;
                const recipeName = this.closest('form').querySelector('input[name="recipeName"]').value;

                // Set the value of the hidden input
                this.closest('form').querySelector('input[name="ratingValue"]').value = rating;

                const formData = new FormData();
                formData.append('ratingValue', rating);
                formData.append('predictionId', predictionId);
                formData.append('recipeName', recipeName); // Append the recipe name

                fetch('Index', {
                    method: 'POST',
                    headers: {
                        'Content-Type': 'application/x-www-form-urlencoded',
                    },
                    body: new URLSearchParams(formData)
                })
                    .then(response => response.text())
                    .then(result => {
                        console.log('Success:', result);
                    })
                    .catch(error => {
                        console.error('Error:', error);
                    });
            });
        });
    });
</script>


</head>
     
<body class="index-page">
   <form id ="form1" runat ="server" method="post">
  <header id="header" class="header d-flex align-items-center sticky-top">
    <div class="container position-relative d-flex align-items-center justify-content-between">

      <a href="index.html" class="logo d-flex align-items-center me-auto me-xl-0">
        <h1 class="sitename">FoodRec</h1>
        <span>.</span>
      </a>

      <nav id="navmenu" class="navmenu">
        <ul>
          <li><a href="#hero" class="active">Home</a></li>
          <li><a href="#about">About</a></li>
          <li><a href="#menu">Menu</a></li>
          <li><a href="#contact">Contact</a></li>
        </ul>
        <i class="mobile-nav-toggle d-xl-none bi bi-list"></i>
      </nav>

      <a class="btn-getstarted" href="index.html#book-a-table">Login/Register</a>

    </div>
  </header>

  <main class="main">

    <!-- Hero Section -->
 <section id="hero" class="hero section light-background" runat="server">
  <div class="container">
    <div class="row gy-4 justify-content-center justify-content-lg-between">
      <div class="col-lg-5 order-2 order-lg-1 d-flex flex-column justify-content-center">
        <h1 data-aos="fade-up">Unleash the Chef in You</h1>
        <p data-aos="fade-up" data-aos-delay="100">Snap a photo, and let our AI-powered system recognize your ingredients and generate mouth-watering recipes just for you. Discover the joy of cooking with what you have at hand.</p>
<div class="d-flex align-items-center flex-column" data-aos="fade-up" data-aos-delay="200">
  <!-- File upload and upload button -->
  <div class="d-flex align-items-center w-100">
    <asp:FileUpload ID="FileUpload1" runat="server" CssClass="form-control" />
    <asp:Button ID="Button1" class="btn-get-started ms-2" runat="server" Text="Upload Image" OnClick="Button1_Click" />
  </div>

  <!-- OR separator -->
  <div class="my-3 text-center or-separator">
    <span>OR</span>
  </div>

  <!-- Search input and search button -->
  <div class="d-flex align-items-center w-100">
    <asp:TextBox ID="SearchRecipeTextBox" runat="server" CssClass="form-control" placeholder="Search Recipe" />
    <asp:Button ID="SearchRecipeButton" class="btn-get-started ms-2" runat="server" Text="Search" OnClick="SearchRecipeButton_Click" />
  </div>
</div>

      </div>
      <div class="col-lg-5 order-1 order-lg-2" data-aos="zoom-out">
  <div class="position-relative mt-4 video-container">
    <img src="assets/img/about-2.jpg" class="img-fluid" alt="">
    <a href="https://www.youtube.com/watch?v=Y7f98aduVJ8" class="glightbox pulsating-play-btn">
      <i class="bi bi-play-circle-fill"></i>
    </a>
  </div>
</div>
    </div>
  </div>
</section>

   <!-- About Section -->
<section id="upload" class="about section" runat="server" visible="false">
  <div class="container section-title" data-aos="fade-down">
    <p><span>Discover Your </span><span class="description-title">>Ingredients</span></p>
  </div>

  <div class="container">
    <div class="row gy-4">
      <div class="col-lg-7" data-aos="fade-up" data-aos-delay="100">
        <!-- Image placeholder where the uploaded image will be displayed -->
        <asp:Image ID="UploadedImg" runat="server" CssClass="img-fluid mb-4" AlternateText="Uploaded Ingredient Image" />
      </div>
      <div class="col-lg-5" data-aos="fade-up" data-aos-delay="250">
        <div class="content ps-0 ps-lg-5">
          <h3>Steps to Prepare Your Dish</h3>
          <asp:BulletedList ID="IngredientList" runat="server" DisplayMode="Text" CssClass="ingredient-list" />
        </div>
      </div>
    </div>
  </div>
</section>
<!-- Why Us Section -->
<!-- About Section -->
<section id="Section1" class="about section" runat="server" visible="false">
  <div class="container section-title" data-aos="fade-down">
    <p><span>Discover Your </span><span class="description-title">>Ingredients</span></p>
  </div>

  <div class="container">
    <div class="row gy-4">
      <div class="col-lg-7" data-aos="fade-up" data-aos-delay="100">
        <!-- Image placeholder where the uploaded image will be displayed -->
        <asp:Image ID="Image1" runat="server" CssClass="img-fluid mb-4" AlternateText="Uploaded Ingredient Image" />
      </div>
      <div class="col-lg-5" data-aos="fade-up" data-aos-delay="250">
        <div class="content ps-0 ps-lg-5">
          <h3>Steps to Prepare Your Dish</h3>
          <asp:BulletedList ID="BulletedList1" runat="server" DisplayMode="Text" CssClass="ingredient-list" />
        </div>
      </div>
    </div>
  </div>
</section>

<!-- Why Us Section -->
<section id="searchresult" runat="server" class="why-us section light-background" style="font-family: cursive">
  <div class="container">
    <div class="row gy-4">
      <!-- First Prediction -->
      <!-- First Prediction -->
<div class="col-lg-4" data-aos="fade-up" data-aos-delay="100">
  <div class="why-box">
    <h3 style="font-family:Candara;color:white;font-weight:800">
      <asp:Label ID="FirstPrediction" runat="server" Text="Label"></asp:Label>
    </h3>
    <div class="allergen-container">
      <asp:Label ID="AllergenFirst" runat="server" Text="Label"></asp:Label>
    </div>
    <div class="recipe-details">
      <h2>Ingredients</h2>
      <ol class="numbered-list">
        <asp:Literal ID="FirstPredictionIngredients" runat="server"></asp:Literal>
      </ol>
      <h2>Steps</h2>
      <ol class="numbered-list">
        <asp:Literal ID="FirstPredictionSteps" runat="server"></asp:Literal>
      </ol>
    </div>

    <!-- Form for First Prediction Rating -->
    <div class="rating-container">
      <span class="star" data-value="1">&#9733;</span>
      <span class="star" data-value="2">&#9733;</span>
      <span class="star" data-value="3">&#9733;</span>
      <span class="star" data-value="4">&#9733;</span>
      <span class="star" data-value="5">&#9733;</span>

      <!-- Hidden fields for rating, predictionId, and recipeName -->
      <input type="hidden" id="ratingValue1" name="ratingValue" value="0" />
      <input type="hidden" id="predictionId1" name="predictionId" value="1" />
      <input type="hidden" id="recipeName1" name="recipeName" value='<%= FirstPrediction.Text %>' />
    </div>
  </div>
</div>


      <!-- Second Prediction -->
      <div class="col-lg-4" data-aos="fade-up" data-aos-delay="100">
        <div class="why-box">
          <h3 style="font-family: cursive;color:white;font-weight:800">
            <asp:Label ID="SecondPrediction" runat="server" Text="Label"></asp:Label>
          </h3>
          <div class="allergen-container">
            <asp:Label ID="AllergenSecond" runat="server" Text="Label"></asp:Label>
          </div>
          <div class="recipe-details">
            <h2>Ingredients</h2>
            <ol class="numbered-list">
              <asp:Literal ID="SecondPredictionIngredients" runat="server"></asp:Literal>
            </ol>
            <h2>Steps</h2>
            <ol class="numbered-list">
              <asp:Literal ID="SecondPredictionSteps" runat="server"></asp:Literal>
            </ol>
          </div>
            <!-- Form for First Prediction Rating -->

    <div class="rating-container">
        <span class="star" data-value="1">&#9733;</span>
        <span class="star" data-value="2">&#9733;</span>
        <span class="star" data-value="3">&#9733;</span>
        <span class="star" data-value="4">&#9733;</span>
        <span class="star" data-value="5">&#9733;</span>
        <input type="hidden" id="ratingValue2" name="ratingValue" value="0" />
        <input type="hidden" id="predictionId2" name="predictionId" value="1" />
    </div>


        </div>
      </div>

      <!-- Third Prediction -->
      <div class="col-lg-4" data-aos="fade-up" data-aos-delay="100">
        <div class="why-box">
          <h2 style="font-family: cursive;color:white;font-weight:800">
            <asp:Label ID="ThirdPrediction" runat="server" Text="Label"></asp:Label>
          </h2>
          <div class="allergen-container">
            <asp:Label ID="AllergenThird" runat="server" Text="Label"></asp:Label>
          </div>
          <div class="recipe-details">
            <h2>Ingredients</h2>
            <ol class="numbered-list">
              <asp:Literal ID="ThirdPredictionIngredients" runat="server"></asp:Literal>
            </ol>
            <h2>Steps</h2>
            <ol class="numbered-list">
              <asp:Literal ID="ThirdPredictionSteps" runat="server"></asp:Literal>
            </ol>
          </div>
           <!-- Form for First Prediction Rating -->
    <div class="rating-container">
        <span class="star" data-value="1">&#9733;</span>
        <span class="star" data-value="2">&#9733;</span>
        <span class="star" data-value="3">&#9733;</span>
        <span class="star" data-value="4">&#9733;</span>
        <span class="star" data-value="5">&#9733;</span>
        <input type="hidden" id="ratingValue3" name="ratingValue" value="0" />
        <input type="hidden" id="predictionId3" name="predictionId" value="1" />
    </div>

        </div>
      </div>
    </div>
  </div>
</section>


  
  </main>

  <footer id="footer" class="footer dark-background">
    <div class="container">
      <div class="row gy-3">
        <div class="col-lg-3 col-md-6 d-flex">
          <i class="bi bi-geo-alt icon"></i>
          <div class="address">
            <h4>Address</h4>
            <p>A108 Adam Street</p>
            <p>New York, NY 535022</p>
          </div>
        </div>

        <div class="col-lg-3 col-md-6 d-flex">
          <i class="bi bi-telephone icon"></i>
          <div>
            <h4>Contact</h4>
            <p>
              <strong>Phone:</strong> +1 5589 55488 55<br>
              <strong>Email:</strong> info@example.com<br>
            </p>
          </div>
        </div>

        <div class="col-lg-3 col-md-6 d-flex">
          <i class="bi bi-clock icon"></i>
          <div>
            <h4>Opening Hours</h4>
            <p>
              <strong>Mon-Sat:</strong> 11AM - 23PM<br>
              <strong>Sunday:</strong> Closed
            </p>
          </div>
        </div>

        <div class="col-lg-3 col-md-6">
          <h4>Follow Us</h4>
          <div class="social-links d-flex">
            <a href="#" class="twitter"><i class="bi bi-twitter"></i></a>
            <a href="#" class="facebook"><i class="bi bi-facebook"></i></a>
            <a href="#" class="instagram"><i class="bi bi-instagram"></i></a>
            <a href="#" class="linkedin"><i class="bi bi-linkedin"></i></a>
          </div>
        </div>
      </div>
    </div>

    <div class="container copyright text-center mt-4">
      <p>© Copyright <strong class="px-1 sitename">Yummy</strong> All Rights Reserved</p>
      <div class="credits">
        Designed by <a href="https://bootstrapmade.com/">BootstrapMade</a>
      </div>
    </div>
  </footer>

  <!-- Scroll Top -->
  <a href="#" id="scroll-top" class="scroll-top d-flex align-items-center justify-content-center"><i class="bi bi-arrow-up-short"></i></a>

  <!-- Preloader -->
  <div id="preloader"></div>

  <!-- Vendor JS Files -->
  <script src="assets/vendor/bootstrap/js/bootstrap.bundle.min.js"></script>
  <script src="assets/vendor/php-email-form/validate.js"></script>
  <script src="assets/vendor/aos/aos.js"></script>
  <script src="assets/vendor/glightbox/js/glightbox.min.js"></script>
  <script src="assets/vendor/purecounter/purecounter_vanilla.js"></script>
  <script src="assets/vendor/swiper/swiper-bundle.min.js"></script>

  <!-- Main JS File -->
  <script src="assets/js/main.js"></script>
    </form>  
</body>

</html>
