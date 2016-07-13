using Xunit;
using System.Collections.Generic;
using System;
using System.Data;
using System.Data.SqlClient;

namespace BestRestaurants.Objects
{
  public class RestaurantTest : IDisposable
  {
    private string restaurantDescription = "A festive environment with friendly wait staff.";
    private string restaurantAddress = "2015 SE 11th Ave";
    private string restaurantPhone = "434-444-4434";
    private string restaurantEmail = "none@none.com";
    public RestaurantTest()
    {
      DBConfiguration.ConnectionString = "Data Source=(localdb)\\mssqllocaldb;Initial Catalog=best_restaurants_test;Integrated Security=SSPI;";
    }
    public void Dispose()
    {
      Restaurant.DeleteAll();
      Review.DeleteAll();
    }

    [Fact]
    public void Restaurant_DatabaseEmpty()
    {
      //Arrange, Act
      int result = Restaurant.GetAll().Count;
      //Assert
      Assert.Equal(0, result);
    }

    [Fact]
    public void Restaurant_RestaurantsAreSame_True()
    {
      //Arrange, Act

      Restaurant firstRestaurant = new Restaurant("El Nutritaco", 1, restaurantDescription, restaurantAddress, restaurantPhone, restaurantEmail);
      Restaurant secondRestaurant = new Restaurant("El Nutritaco", 1, restaurantDescription, restaurantAddress, restaurantPhone, restaurantEmail);
      //Assert
      Assert.Equal(firstRestaurant, secondRestaurant);
    }

    [Fact]
    public void Restaurant_SavesToDatabase()
    {
      //Arrange
      Restaurant testRestaurant = new Restaurant("El Nutritaco", 1, restaurantDescription, restaurantAddress, restaurantPhone, restaurantEmail);
      //Act
      testRestaurant.Save();
      Restaurant savedRestaurant = Restaurant.GetAll()[0];
      //Assert
      Assert.Equal(testRestaurant, savedRestaurant);
    }

    [Fact]
    public void Restaurant_SavesWithId()
    {
      //Arrange
      Restaurant testRestaurant = new Restaurant("El Nutritaco", 1, restaurantDescription, restaurantAddress, restaurantPhone, restaurantEmail);
      //Act
      testRestaurant.Save();
      int result = Restaurant.GetAll()[0].GetId();
      int expectedResult = testRestaurant.GetId();
      //Assert
      Assert.Equal(expectedResult, result);
    }
    [Fact]
    public void Restaurant_Find()
    {
      //Arrange
      Restaurant newRestaurant = new Restaurant("Los Gorditos", 1, restaurantDescription, restaurantAddress, restaurantPhone, restaurantEmail);
      newRestaurant.Save();
      //Act
      Restaurant foundRestaurant = Restaurant.Find(newRestaurant.GetId());
      //Assert
      Assert.Equal(newRestaurant, foundRestaurant);
    }

    [Fact]
    public void Restaurant_Update()
    {
      //Arrange
      Restaurant newRestaurant = new Restaurant("Let's Eat Thai", 2, restaurantDescription, restaurantAddress, restaurantPhone, restaurantEmail);
      string expectedName = "Los Gorditos";
      int expectedCuisineId = 1;
      newRestaurant.Save();
      //Act
      newRestaurant.Update("Los Gorditos", 1, restaurantDescription, restaurantAddress, restaurantPhone, restaurantEmail);
      string actualName = newRestaurant.GetName();
      int actualCuisineId = newRestaurant.GetCuisineId();
      //Assert
      Assert.Equal(expectedName, actualName);
      Assert.Equal(expectedCuisineId, actualCuisineId);
    }

    [Fact]
    public void Restaurant_ReviewsByRestaurant()
    {
      //Arrange
      Restaurant testRestaurant = new Restaurant("El Nutritaco", 1, restaurantDescription, restaurantAddress, restaurantPhone, restaurantEmail);
      testRestaurant.Save();
      DateTime newDate = new DateTime(2016,7,13);
      string newText = "This was the best restaurant I've ever eaten at. I intend to eat there every day of my life.";
      Review testReview = new Review("Carl", newText, newDate, 10, testRestaurant.GetId());
      testReview.Save();
      List<Review> expectedResult = new List<Review>{testReview};
      //Act
      List<Review> result = testRestaurant.GetReviews();
      //Assert
      Assert.Equal(expectedResult, result);
    }
  }
}
