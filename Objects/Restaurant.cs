using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace BestRestaurants.Objects
{
  public class Restaurant
  {
    private int _id;
    private string _name;
    private int _cuisineId;

    public Restaurant(string name, int cuisineId, int Id = 0)
    {
      _name = name;
      _cuisineId = cuisineId;
      _id = Id;
    }

    public string GetName()
    {
      return _name;
    }
    public int GetId()
    {
      return _id;
    }
    public int GetCuisineId()
    {
      return _cuisineId;
    }
    public void SetName(string newName)
    {
      _name = newName;
    }
    public void SetCuisineId(int newId)
    {
      _cuisineId = newId;
    }

    public static void DeleteAll()
    {
      SqlConnection conn = DB.Connection();
      conn.Open();
      SqlCommand cmd = new SqlCommand("DELETE FROM restaurants;", conn);
      cmd.ExecuteNonQuery();
    }

    public static List<Restaurant> GetAll()
    {
      List<Restaurant> allRestaurants = new List<Restaurant>{};
      SqlConnection conn = DB.Connection();
      conn.Open();
      SqlDataReader rdr = null;

      SqlCommand cmd = new SqlCommand("SELECT * FROM restaurants;", conn);
      rdr = cmd.ExecuteReader();

      while(rdr.Read())
      {
        int newId = rdr.GetInt32(0);
        string newName = rdr.GetString(1);
        int newCuisineId = rdr.GetInt32(2);
        Restaurant newRestaurant = new Restaurant(newName, newCuisineId, newId);
        allRestaurants.Add(newRestaurant);
      }
      if (rdr != null) rdr.Close();
      if (conn != null) conn.Close();

      return allRestaurants;
    }

    public override bool Equals(System.Object otherRestaurant)
    {
      if(!(otherRestaurant is Restaurant))
      {
        return false;
      }
      else
      {
        Restaurant newRestaurant = (Restaurant) otherRestaurant;
        bool nameEquality = (_name == newRestaurant.GetName());
        bool idEquality = (_id == newRestaurant.GetId());
        bool cuisineEquality = (_cuisineId == newRestaurant.GetCuisineId());
        return (nameEquality && idEquality && cuisineEquality);
      }
    }

    public void Save()
    {
      SqlConnection conn = DB.Connection();
      conn.Open();
      SqlDataReader rdr = null;

      SqlCommand cmd = new SqlCommand("INSERT INTO restaurants (name, cuisine_id) OUTPUT INSERTED.id VALUES(@NewName, @NewCuisine);", conn);

      SqlParameter newNameParameter = new SqlParameter();
      newNameParameter.ParameterName = "@NewName";
      newNameParameter.Value = _name;
      cmd.Parameters.Add(newNameParameter);

      SqlParameter newCuisineParameter = new SqlParameter();
      newCuisineParameter.ParameterName = "@NewCuisine";
      newCuisineParameter.Value = _cuisineId;
      cmd.Parameters.Add(newCuisineParameter);

      rdr = cmd.ExecuteReader();
      while (rdr.Read())
      {
        _id = rdr.GetInt32(0);
      }

      if (rdr != null) rdr.Close();
      if (conn != null) conn.Close();
    }
    public static Restaurant Find(int id)
    {
      SqlConnection conn = DB.Connection();
      conn.Open();
      SqlDataReader rdr = null;

      SqlCommand cmd = new SqlCommand("SELECT * FROM restaurants WHERE id = @RestaurantId;", conn);

      SqlParameter newIdParameter = new SqlParameter();
      newIdParameter.ParameterName = "@RestaurantId";
      newIdParameter.Value = id;
      cmd.Parameters.Add(newIdParameter);

      int foundId = 0;
      string foundName = null;
      int foundCuisineId= 0;

      rdr = cmd.ExecuteReader();
      while (rdr.Read())
      {
        foundId = rdr.GetInt32(0);
        foundName = rdr.GetString(1);
        foundCuisineId = rdr.GetInt32(2);
      }
      Restaurant foundRestaurant = new Restaurant(foundName, foundCuisineId, foundId);

      if (rdr != null) rdr.Close();
      if (conn != null) conn.Close();

      return foundRestaurant;
    }

    public void Update(string newName, int newCuisineId)
    {
      SqlConnection conn = DB.Connection();
      conn.Open();
      SqlDataReader rdr = null;

      SqlCommand cmd = new SqlCommand("UPDATE restaurants SET name = @NewName, cuisine_id = @NewCuisineId OUTPUT INSERTED.name, INSERTED.cuisine_id WHERE id = @RestaurantId;", conn);

      SqlParameter newNameParameter = new SqlParameter();
      newNameParameter.ParameterName = "@NewName";
      newNameParameter.Value = newName;
      cmd.Parameters.Add(newNameParameter);

      SqlParameter newCuisineParameter = new SqlParameter();
      newCuisineParameter.ParameterName = "@NewCuisineId";
      newCuisineParameter.Value = newCuisineId;
      cmd.Parameters.Add(newCuisineParameter);

      SqlParameter idParameter = new SqlParameter();
      idParameter.ParameterName = "@RestaurantId";
      idParameter.Value = _id;
      cmd.Parameters.Add(idParameter);

      rdr = cmd.ExecuteReader();

      while(rdr.Read())
      {
        _name = rdr.GetString(0);
        _cuisineId = rdr.GetInt32(1);
      }

      if (rdr != null) rdr.Close();
      if (conn != null) conn.Close();
    }

    public List<Review> GetReviews()
    {
      List<Review> allReviews = new List<Review>{};
      SqlConnection conn = DB.Connection();
      conn.Open();
      SqlDataReader rdr = null;

      SqlCommand cmd = new SqlCommand("SELECT * FROM reviews WHERE restaurant_id = @RestaurantId ORDER BY restaurant_id;", conn);
      SqlParameter idParameter = new SqlParameter();
      idParameter.ParameterName = "@RestaurantId";
      idParameter.Value = _id;
      cmd.Parameters.Add(idParameter);
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

  }
}
