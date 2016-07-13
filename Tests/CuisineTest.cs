using Xunit;
using System.Collections.Generic;
using System;
using System.Data;
using System.Data.SqlClient;

namespace BestRestaurants.Objects
{
  public class CuisineTest : IDisposable
  {
    private string restaurantDescription = "A festive environment with friendly wait staff.";
    private string restaurantAddress = "2015 SE 11th Ave";
    private string restaurantPhone = "434-444-4434";
    private string restaurantEmail = "none@none.com";
    public CuisineTest()
    {
      DBConfiguration.ConnectionString = "Data Source=(localdb)\\mssqllocaldb;Initial Catalog=best_restaurants_test;Integrated Security=SSPI;";
    }

    public void Dispose()
    {
      Cuisine.DeleteAll();
      Restaurant.DeleteAll();
    }

    [Fact]
    public void Cuisine_DatabaseEmptyAtFirst()
    {
      //Arrange, Act
      int result = Cuisine.GetAll().Count;
      //Assert
      Assert.Equal(0, result);
    }

    [Fact]
    public void Cuisine_EqualCuisines_True()
    {
      //Arrange, Act
      Cuisine firstCuisine = new Cuisine("Mexican");
      Cuisine secondCuisine = new Cuisine("Mexican");
      //Assert
      Assert.Equal(firstCuisine, secondCuisine);
    }

    [Fact]
    public void Cuisine_SavesToDatabase()
    {
      //Arrange
      Cuisine testCuisine = new Cuisine("Mexican");
      //Act
      testCuisine.Save();
      Cuisine savedCuisine = Cuisine.GetAll()[0];
      //Assert
      Assert.Equal(testCuisine, savedCuisine);
    }

    [Fact]
    public void Cuisine_SavesWithId()
    {
      //Arrange
      Cuisine testCuisine = new Cuisine("Mexican");
      //Act
      testCuisine.Save();
      int result = Cuisine.GetAll()[0].GetId();
      int expectedResult = testCuisine.GetId();
      //Assert
      Assert.Equal(expectedResult, result);
    }

    [Fact]
    public void Cuisine_FindsInDatabase()
    {
      //Arrange
      Cuisine testCuisine = new Cuisine("Mexican");
      //Act
      testCuisine.Save();
      Cuisine foundCuisine = Cuisine.Find(1);
      //Assert
      Assert.Equal(testCuisine, foundCuisine);
    }

    [Fact]
    public void Cuisine_UpdateSavedCuisine()
    {
      //Arrange
      Cuisine testCuisine = new Cuisine("Mexican");
      string expectedResult = "Thai";
      testCuisine.Save();
      //Act
      testCuisine.Update("Thai");
      string result = testCuisine.GetCuisineType();
      //Assert
      Assert.Equal(expectedResult, result);
    }
    [Fact]
    public void Cuisine_ReturnRestaurantsByCuisine()
    {
      //Arrange
      Cuisine testCuisine = new Cuisine("Mexican");
      testCuisine.Save();
      Restaurant testRestaurant = new Restaurant("El Nutritaco", testCuisine.GetId(), restaurantDescription, restaurantAddress, restaurantPhone, restaurantEmail);
      testRestaurant.Save();
      List<Restaurant> expectedResult = new List<Restaurant>{testRestaurant};
      //Act
      List<Restaurant> result = testCuisine.GetAllRestaurantsByCuisine();
      //Assert
      Assert.Equal(expectedResult, result);
    }

  }
}
