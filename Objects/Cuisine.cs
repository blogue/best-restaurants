using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace BestRestaurants.Objects
{
  public class Cuisine
  {
    private int _id;
    private string _cuisineType;

    public Cuisine(string cuisineType, int id = 0)
    {
      _id = id;
      _cuisineType = cuisineType;
    }

    public string GetCuisineType()
    {
      return _cuisineType;
    }

    public int GetId()
    {
      return _id;
    }

    public void SetCuisineType(string cuisineType)
    {
      _cuisineType = cuisineType;
    }

    public void SetId(int id)
    {
      _id = id;
    }

    public override bool Equals(System.Object otherCuisine)
    {
      if (!(otherCuisine is Cuisine)) return false;
      else
      {
        Cuisine newCuisine = (Cuisine) otherCuisine;
        bool cuisineIdEquality = _id == newCuisine.GetId();
        bool cuisineTypeEquality = _cuisineType == newCuisine.GetCuisineType();
        return (cuisineIdEquality == cuisineTypeEquality);
      }
    }

    public static void DeleteAll()
    {
      SqlConnection conn = DB.Connection();
      conn.Open();
      SqlCommand cmd = new SqlCommand("DELETE FROM cuisines;", conn);
      cmd.ExecuteNonQuery();
    }

    public static List<Cuisine> GetAll()
    {
      List<Cuisine> allCuisines = new List<Cuisine> {};

      SqlConnection conn = DB.Connection();
      SqlDataReader rdr = null;
      conn.Open();

      SqlCommand cmd = new SqlCommand("SELECT * FROM cuisines;", conn);
      rdr = cmd.ExecuteReader();

      while(rdr.Read())
      {
        int newCuisineId = rdr.GetInt32(0);
        string newCuisineType = rdr.GetString(1);

        Cuisine newCuisine = new Cuisine(newCuisineType, newCuisineId);
        allCuisines.Add(newCuisine);
      }

      if (rdr != null)
      {
        rdr.Close();
      }
      if (conn != null)
      {
        conn.Close();
      }
      return allCuisines;
    }

    public List<Restaurant> GetAllRestaurantsByCuisine()
    {
      List<Restaurant> allRestaurants = new List<Restaurant> {};

      SqlConnection conn = DB.Connection();
      SqlDataReader rdr = null;
      conn.Open();

      SqlCommand cmd = new SqlCommand("SELECT * FROM restaurants WHERE cuisine_id = @CuisineId;", conn);

      SqlParameter newCuisineIdParameter = new SqlParameter();
      newCuisineIdParameter.ParameterName = "@CuisineId";
      newCuisineIdParameter.Value = _id;
      cmd.Parameters.Add(newCuisineIdParameter);

      rdr = cmd.ExecuteReader();

      while(rdr.Read())
      {
        int newId = rdr.GetInt32(0);
        string newName = rdr.GetString(1);
        int newCuisineId = rdr.GetInt32(2);
        string newDescription = rdr.GetString(3);
        string newAddress = rdr.GetString(4);
        string newPhone = rdr.GetString(5);
        string newEmail = rdr.GetString(6);
        Restaurant newRestaurant = new Restaurant(newName, newCuisineId, newDescription, newAddress, newPhone, newEmail, newId);
        allRestaurants.Add(newRestaurant);
      }

      if (rdr != null)
      {
        rdr.Close();
      }
      if (conn != null)
      {
        conn.Close();
      }
      return allRestaurants;
    }


    public void Save()
    {
      SqlConnection conn = DB.Connection();
      conn.Open();
      SqlDataReader rdr = null;

      SqlCommand cmd = new SqlCommand("INSERT INTO cuisines (cuisine_type) OUTPUT INSERTED.id VALUES(@CuisineType);", conn);
      SqlParameter cuisineTypeParameter = new SqlParameter();
      cuisineTypeParameter.ParameterName = "@CuisineType";
      cuisineTypeParameter.Value = _cuisineType;
      cmd.Parameters.Add(cuisineTypeParameter);

      rdr = cmd.ExecuteReader();

      while (rdr.Read())
      {
        _id = rdr.GetInt32(0);
      }

      if (rdr != null) rdr.Close();
      if (conn != null) conn.Close();
    }

    public static Cuisine Find(int id)
    {
      SqlConnection conn = DB.Connection();
      conn.Open();
      SqlDataReader rdr = null;

      SqlCommand cmd = new SqlCommand("SELECT * FROM cuisines WHERE id = @CuisineId;", conn);
      SqlParameter cuisineIdParameter = new SqlParameter();
      cuisineIdParameter.ParameterName = "@cuisineId";
      cuisineIdParameter.Value = id.ToString();
      cmd.Parameters.Add(cuisineIdParameter);
      rdr = cmd.ExecuteReader();

      int foundCuisineId = 0;
      string foundCuisineType = null;

      while (rdr.Read())
      {
        foundCuisineId = rdr.GetInt32(0);
        foundCuisineType = rdr.GetString(1);
      }
      Cuisine foundCuisine = new Cuisine(foundCuisineType, foundCuisineId);

      if (rdr != null) rdr.Close();
      if (conn != null) conn.Close();

      return foundCuisine;
    }

    public void Update(string newType)
    {
      SqlConnection conn = DB.Connection();
      conn.Open();
      SqlDataReader rdr = null;

      SqlCommand cmd = new SqlCommand("UPDATE cuisines SET cuisine_type = @NewType OUTPUT INSERTED.cuisine_type WHERE id = @CuisineId;", conn);

      SqlParameter cuisineTypeParameter = new SqlParameter();
      cuisineTypeParameter.ParameterName = "@NewType";
      cuisineTypeParameter.Value = newType;
      cmd.Parameters.Add(cuisineTypeParameter);

      SqlParameter cuisineIdParameter = new SqlParameter();
      cuisineIdParameter.ParameterName = "@CuisineId";
      cuisineIdParameter.Value = _id;
      cmd.Parameters.Add(cuisineIdParameter);

      rdr = cmd.ExecuteReader();

      while(rdr.Read())
      {
        _cuisineType = rdr.GetString(0);
      }

      if (rdr != null) rdr.Close();
      if (conn != null) conn.Close();

    }

  }
}
