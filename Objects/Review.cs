using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace BestRestaurants.Objects
{
  public class Review
  {
    private int _id;
    private string _reviewerName;
    private string _reviewText;
    private DateTime? _date;
    private int _starsNumber;
    private int _restaurantId;

    public Review(string reviewerName, string reviewText, DateTime? date, int starsNumber, int restaurantId, int Id = 0)
    {
      _id = Id;
      _reviewerName = reviewerName;
      _reviewText = reviewText;
      _date = date;
      _starsNumber = starsNumber;
      _restaurantId = restaurantId;
    }

    public int GetId()
    {
      return _id;
    }
    public string GetName()
    {
      return _reviewerName;
    }
    public string GetText()
    {
      return _reviewText;
    }
    public DateTime? GetDate()
    {
      return _date;
    }
    public int GetStars()
    {
      return _starsNumber;
    }
    public int GetRestaurantId()
    {
      return _restaurantId;
    }

    public static void DeleteAll()
    {
      SqlConnection conn = DB.Connection();
      conn.Open();
      SqlCommand cmd = new SqlCommand("DELETE FROM reviews;", conn);
      cmd.ExecuteNonQuery();
    }
    public override bool Equals(System.Object otherReview)
    {
      if(!(otherReview is Review))
      {
        return false;
      }
      else
      {
        Review newReview = (Review) otherReview;
        bool idEquality = (_id == newReview.GetId());
        bool reviewerNameEquality = (_reviewerName == newReview.GetName());
        bool reviewTextEquality = (_reviewText == newReview.GetText());
        bool dateEquality = (_date == newReview.GetDate());
        bool starsNumberEquality = (_starsNumber == newReview.GetStars());
        bool restaurantIdEquality = (_restaurantId == newReview.GetRestaurantId());

        return (idEquality && reviewerNameEquality && reviewTextEquality && dateEquality && starsNumberEquality && restaurantIdEquality);
      }
    }

    public static List<Review> GetAll()
    {
      List<Review> allReviews = new List<Review>{};
      SqlConnection conn = DB.Connection();
      conn.Open();
      SqlDataReader rdr = null;

      SqlCommand cmd = new SqlCommand("SELECT * FROM reviews ORDER BY restaurant_id;", conn);

      rdr = cmd.ExecuteReader();

      while(rdr.Read())
      {
        int newId = rdr.GetInt32(0);
        string newName = rdr.GetString(1);
        string newText = rdr.GetString(2);
        DateTime? newDate = rdr.GetDateTime(3);
        int newStars = rdr.GetInt32(4);
        int newRestaurantId = rdr.GetInt32(5);
        Review newReview = new Review(newName, newText, newDate, newStars, newRestaurantId, newId);
        allReviews.Add(newReview);
      }

      if (rdr != null) rdr.Close();
      if (conn != null) conn.Close();

      return allReviews;
    }

    public void Save()
    {
      SqlConnection conn = DB.Connection();
      conn.Open();
      SqlDataReader rdr = null;

      SqlCommand cmd = new SqlCommand("INSERT INTO reviews (reviewer_name, review_text, review_date, number_stars, restaurant_id) OUTPUT INSERTED.id VALUES (@NewName, @NewText, @NewDate, @NewStars, @NewRestaurantId);", conn);

      SqlParameter newNameParameter = new SqlParameter();
      newNameParameter.ParameterName = "@NewName";
      newNameParameter.Value = _reviewerName;
      cmd.Parameters.Add(newNameParameter);

      SqlParameter newTextParameter = new SqlParameter();
      newTextParameter.ParameterName = "@NewText";
      newTextParameter.Value = _reviewText;
      cmd.Parameters.Add(newTextParameter);

      SqlParameter newDateParameter = new SqlParameter();
      newDateParameter.ParameterName = "@NewDate";
      newDateParameter.Value = _date;
      cmd.Parameters.Add(newDateParameter);

      SqlParameter newStarsParameter = new SqlParameter();
      newStarsParameter.ParameterName = "@NewStars";
      newStarsParameter.Value = _starsNumber;
      cmd.Parameters.Add(newStarsParameter);

      SqlParameter newRestaurantIdParameter = new SqlParameter();
      newRestaurantIdParameter.ParameterName = "@NewRestaurantId";
      newRestaurantIdParameter.Value = _restaurantId;
      cmd.Parameters.Add(newRestaurantIdParameter);

      rdr = cmd.ExecuteReader();
      while(rdr.Read())
      {
        _id = rdr.GetInt32(0);
      }
      if (rdr != null) rdr.Close();
      if (conn != null) rdr.Close();
    }

    public void Update(string newName, string newText, DateTime? newDate, int newStars, int newRestaurantId)
    {
      SqlConnection conn = DB.Connection();
      conn.Open();
      SqlDataReader rdr = null;

      SqlCommand cmd = new SqlCommand("UPDATE reviews SET reviewer_name = @NewName, review_text = @NewText, review_date = @NewDate, number_stars = @NewStars, restaurant_id = @NewRestaurantId OUTPUT INSERTED.reviewer_name, INSERTED.review_text, INSERTED.review_date, INSERTED.number_stars, INSERTED.restaurant_id WHERE id = @RestaurantId;", conn);

      SqlParameter newNameParameter = new SqlParameter();
      newNameParameter.ParameterName = "@NewName";
      newNameParameter.Value = newName;
      cmd.Parameters.Add(newNameParameter);

      SqlParameter newTextParameter = new SqlParameter();
      newTextParameter.ParameterName = "@NewText";
      newTextParameter.Value = newText;
      cmd.Parameters.Add(newTextParameter);

      SqlParameter newDateParameter = new SqlParameter();
      newDateParameter.ParameterName = "@NewDate";
      newDateParameter.Value = newDate;
      cmd.Parameters.Add(newDateParameter);

      SqlParameter newStarsParameter = new SqlParameter();
      newStarsParameter.ParameterName = "@NewStars";
      newStarsParameter.Value = newStars;
      cmd.Parameters.Add(newStarsParameter);

      SqlParameter newRestaurantIdParameter = new SqlParameter();
      newRestaurantIdParameter.ParameterName = "@NewRestaurantId";
      newRestaurantIdParameter.Value = newRestaurantId;
      cmd.Parameters.Add(newRestaurantIdParameter);

      SqlParameter restaurantIdParameter = new SqlParameter();
      restaurantIdParameter.ParameterName = "@RestaurantId";
      restaurantIdParameter.Value = _id;
      cmd.Parameters.Add(restaurantIdParameter);

      rdr = cmd.ExecuteReader();
      while(rdr.Read())
      {
        _reviewerName = rdr.GetString(0);
        _reviewText = rdr.GetString(1);
        _date = rdr.GetDateTime(2);
        _starsNumber = rdr.GetInt32(3);
        _restaurantId = rdr.GetInt32(4);
      }

      if (rdr != null) rdr.Close();
      if (conn != null) conn.Close();

    }

  }
}
