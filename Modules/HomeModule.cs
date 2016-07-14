using Nancy;
using System;
using System.Collections.Generic;
using BestRestaurants.Objects;

namespace BestRestaurants
{
  public class HomeModule : NancyModule
  {
    public HomeModule()
    {
      Get["/"] = _ =>
      {
        List<Cuisine> allCuisines = Cuisine.GetAll();
        return View["index.cshtml", allCuisines];
      };

      Get["/restaurants/all"] = _ =>
      {
        List<Restaurant> allRestaurants = Restaurant.GetAll();
        allRestaurants.Sort((n1,n2) => n1.GetName().CompareTo(n2.GetName()));
        return View["restaurants.cshtml", allRestaurants];
      };

      Get["/restaurants/cuisine/{id}"] = parameters =>
      {
        Cuisine cuisine = Cuisine.Find(parameters.id);
        List<Restaurant> allRestaurants = cuisine.GetAllRestaurantsByCuisine();
        return View["restaurants.cshtml", allRestaurants];
      };

      Get["/restaurants/{id}"] = parameters =>
      {
        Restaurant restaurant = Restaurant.Find(parameters.id);
        return View["view_restaurant.cshtml", restaurant];
      };

      Get["/restaurants/add"] = _ =>
      {
        List<Cuisine> allCuisines = Cuisine.GetAll();
        return View["add_restaurant.cshtml", allCuisines];
      };

      Post["/restaurants/add"] = _ =>
      {
        string name = Request.Form["name"];
        int cuisineId = Request.Form["cuisine_id"];
        string address = Request.Form["address"];
        string email = Request.Form["email"];
        string phone = Request.Form["phone"];
        string description = Request.Form["description"];
        Restaurant newRestaurant = new Restaurant(name, cuisineId, description, address, phone, email);
        newRestaurant.Save();
        return View["view_restaurant.cshtml", newRestaurant];
      };
      Get["/reviews/add/{id}"] = parameters =>
      {
        int restaurantId = parameters.id;
        return View["add_review", restaurantId];
      };
      Post["/reviews/add"] = _ =>
      {
        string name = Request.Form["name"];
        int stars = Request.Form["stars"];
        DateTime? date = Request.Form["date"];
        string review = Request.Form["review"];
        int restaurantId = Request.Form["restaurantId"];
        Review newReview = new Review(name, review, date, stars, restaurantId);
        newReview.Save();
        Restaurant restaurant = Restaurant.Find(restaurantId);
        return View["view_restaurant.cshtml", restaurant];
      };
    }
  }

}
