using Xunit;
using System.Collections.Generic;
using System;
using System.Data;
using System.Data.SqlClient;

namespace BestRestaurants.Objects
{
  public class ReviewTest : IDisposable
  {
    public ReviewTest()
    {
      DBConfiguration.ConnectionString = "Data Source=(localdb)\\mssqllocaldb;Initial Catalog=best_restaurants_test;Integrated Security=SSPI;";
    }

    public void Dispose()
    {
      Review.DeleteAll();
    }

    [Fact]
    public void Review_DatabaseEmpty()
    {
      //Arrange
      int result = Review.GetAll().Count;
      Assert.Equal(0, result);
    }

    [Fact]
    public void Review_EqualReviews_True()
    {
      //Arrange, Act
      DateTime newDate = new DateTime(2016,7,13);
      string newText = "This was the best restaurant I've ever eaten at. I intend to eat there every day of my life.";
      Review firstReview = new Review("Carl", newText, newDate, 10, 1);
      Review secondReview = new Review("Carl", newText, newDate, 10, 1);
      //Assert
      Assert.Equal(firstReview, secondReview);
    }

    [Fact]
    public void Review_SavesToDatabase()
    {
      //Arrange
      DateTime newDate = new DateTime(2016,7,13);
      string newText = "This was the best restaurant I've ever eaten at. I intend to eat there every day of my life.";
      Review newReview = new Review("Carl", newText, newDate, 10, 1);
      //Act
      newReview.Save();
      Review savedReview = Review.GetAll()[0];
      //Assert
      Assert.Equal(newReview, savedReview);
    }

    [Fact]
    public void Review_SavesWithId()
    {
      //Arrange
      DateTime newDate = new DateTime(2016,7,13);
      string newText = "This was the best restaurant I've ever eaten at. I intend to eat there every day of my life.";
      Review newReview = new Review("Carl", newText, newDate, 10, 1);
      //Act
      newReview.Save();
      int result = Review.GetAll()[0].GetId();
      int expectedResult = newReview.GetId();
      //Assert
      Assert.Equal(expectedResult, result);
    }

    [Fact]
    public void Review_Updates()
    {
      //Arrange
      DateTime newDate = new DateTime(2016,7,13);
      string newText = "This was the best restaurant I've ever eaten at. I intend to eat there every day of my life.";
      Review newReview = new Review("Carl", newText, newDate, 10, 1);
      newReview.Save();
      //Act
      string otherText = "This was the worst restaurant I've ever eaten at. I will burn it to the ground.";
      newReview.Update("Carl", otherText, newDate, 0, 1);
      string text = newReview.GetText();
      int stars = newReview.GetStars();
      //Assert
      Assert.Equal(otherText, text);
      Assert.Equal(0, stars);

    }

  }
}
