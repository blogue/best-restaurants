
$(document).ready(function() {
  var stars = Math.round($("#starvalue").val());
  for (i = 0 ; i < stars ; i++) {
    $("#stars").append('<span class="glyphicon glyphicon-star" aria-hidden="true"></span>');
  }

  var reviewCount = $("#review-count").val();
  for (i = 1; i <= reviewCount; i++)
  {
    var otherstars = $("#"+i+"review").val();
    for (j = 0 ; j < otherstars ; j++) {
      $("#" + i+"star").append('<span class="glyphicon glyphicon-star" aria-hidden="true"></span>');
    }
  }
});
