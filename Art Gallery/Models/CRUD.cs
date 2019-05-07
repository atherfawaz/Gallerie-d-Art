using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Art_Gallery.Models;


public class CRUD
{

    private static string connectionString = "Data Source=DESKTOP-UEJVQ22;Initial Catalog=ART_GAL;Integrated Security=True;";

    public static int makeAdmin(String identifier)
    {
        SqlConnection con = new SqlConnection(connectionString);

        try
        {
            con.Open();
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
            return -1;
        }

        SqlCommand cmd;

        try
        {
            String fullname = identifier;
            String[] separatednames = fullname.Split(' ');
            cmd = new SqlCommand("makeAdmin", con);

            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("@fname", SqlDbType.VarChar, 500).Value = separatednames[0].ToString();
            cmd.Parameters.Add("@lname", SqlDbType.VarChar, 500).Value = separatednames[1].ToString();

            cmd.ExecuteNonQuery();

            return 1;
        }
        catch (SqlException ex)
        {
            Console.WriteLine("SQL Error" + ex.Message.ToString());
            return -2;
        }
        finally
        {
            con.Close();
        }
    }

    public static int remove_UserDB(String identifier)
    {
        SqlConnection con = new SqlConnection(connectionString);

        try
        {
            con.Open();
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
            return -1;
        }

        SqlCommand cmd;

        try
        {
            String fullname = identifier;
            String[] separatednames = fullname.Split(' ');
            cmd = new SqlCommand("removeUser", con);

            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("@fname", SqlDbType.VarChar, 500).Value = separatednames[0].ToString();
            cmd.Parameters.Add("@lname", SqlDbType.VarChar, 500).Value = separatednames[1].ToString();

            cmd.ExecuteNonQuery();

            return 1;
        }
        catch (SqlException ex)
        {
            Console.WriteLine("SQL Error" + ex.Message.ToString());
            return -2;
        }
        finally
        {
            con.Close();
        }
    }

    public static List<User> get_All_Users()
    {
        SqlConnection con = new SqlConnection(connectionString);

        try
        {
            con.Open();
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
            return null;
        }

        SqlCommand cmd;

        try
        {
            cmd = new SqlCommand("SELECT* FROM [USER]", con);

            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            da.Fill(dt);

            List<User> user_table = new List<User>();

            foreach (DataRow row in dt.Rows)
            {
                user_table.Add(new User(Convert.ToInt32(row["UserID"].ToString()),
                    row["FirstName"].ToString() + " " + row["LastName"].ToString(),
                    row["Email"].ToString(),
                    row["AccountType"].ToString()));
            }
            return user_table;
        }
        catch (SqlException ex)
        {
            Console.WriteLine("SQL Error" + ex.Message.ToString());
            return null;
        }
        finally
        {
            con.Close();
        }
    }

    public static Tuple<int, string, string> get_session_data(String email)
    {
        SqlConnection con = new SqlConnection(connectionString);

        try
        {
            con.Open();
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
            return null;
        }

        SqlCommand cmd;

        try
        {
            cmd = new SqlCommand("SELECT UserID, AccountType, FirstName, LastName FROM [User] WHERE email = " + "'" + email + "'", con);

            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            da.Fill(dt);

            Tuple<int, string, string> ret;

            foreach (DataRow row in dt.Rows)
            {
                ret = new Tuple<int, string, string>(Convert.ToInt32(row["userID"]),
                                                     row["FirstName"].ToString() + " " + row["LastName"],
                                                     row["AccountType"].ToString());

                return ret;
            }
            return null;

        }
        catch (SqlException ex)
        {
            Console.WriteLine("SQL Error" + ex.Message.ToString());
            return null;
        }
        finally
        {
            con.Close();
        }
    }

    public static List<Art> get_Specific_Art(int identifier)
    {
        SqlConnection con = new SqlConnection(connectionString);

        try
        {
            con.Open();
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
            return null;
        }

        SqlCommand cmd;

        try
        {
            cmd = new SqlCommand("SELECT * FROM Artwork WHERE ArtworkID = " + identifier.ToString(), con);

            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            da.Fill(dt);

            List<Art> art = new List<Art>();

            foreach (DataRow row in dt.Rows)
            {
                art.Add(new Art(Convert.ToInt32(row["ArtworkID"].ToString()),
                                row["Creator"].ToString(),
                                row["Name"].ToString(),
                                row["Description"].ToString(),
                                row["Medium"].ToString(),
                                row["Origin"].ToString(),
                                row["pictureLink"].ToString(),
                                row["Date"].ToString(),
                                Convert.ToInt32(row["Price"].ToString())));
            }
            return art;
        }
        catch (Exception e)
        {
            Console.WriteLine("SQL Error: " + e.Message);
            return null;
        }
        finally
        {
            con.Close();
        }
    }

    public static int rate_art(int user_id, int art_id, int rating, string extra)
    {
        SqlConnection con = new SqlConnection(connectionString);

        try
        {
            con.Open();
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
            return -2;
        }

        SqlCommand cmd;

        try
        {
            cmd = new SqlCommand("rateArt", con);

            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("@userID", SqlDbType.Int).Value = user_id;
            cmd.Parameters.Add("@artID", SqlDbType.Int).Value = art_id;
            cmd.Parameters.Add("@rating", SqlDbType.Int).Value = rating;
            cmd.Parameters.Add("@additional", SqlDbType.VarChar, 30).Value = extra;

            SqlParameter outputParam = cmd.Parameters.Add("@out", SqlDbType.Int);
            outputParam.Direction = ParameterDirection.Output;
            outputParam.Value = "";

            cmd.ExecuteNonQuery();

            return Convert.ToInt32(outputParam.Value);
        }
        catch (Exception e)
        {
            Console.WriteLine("SQL Error: " + e.Message);
            return -3;
        }
        finally
        {
            con.Close();
        }
    }

    public static float getAvgRating(int art_id)
    {
        SqlConnection con = new SqlConnection(connectionString);

        try
        {
            con.Open();
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
            return -2;
        }

        SqlCommand cmd;

        try
        {
            cmd = new SqlCommand("avgRating", con);

            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("@artID", SqlDbType.Int).Value = art_id;

            SqlParameter outputParam = cmd.Parameters.Add("@out", SqlDbType.BigInt);
            outputParam.Direction = ParameterDirection.Output;
            outputParam.Value = "";

            cmd.ExecuteNonQuery();

            return Convert.ToInt64(outputParam.Value);
        }
        catch (Exception e)
        {
            Console.WriteLine("SQL Error: " + e.Message);
            return -3;
        }
        finally
        {
            con.Close();
        }
    }

    public static int like_art(int user_id, int art_id)
    {
        SqlConnection con = new SqlConnection(connectionString);

        try
        {
            con.Open();
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
            return -2;
        }

        SqlCommand cmd;

        try
        {
            cmd = new SqlCommand("likeArt", con);

            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("@userID", SqlDbType.Int).Value = user_id;
            cmd.Parameters.Add("@artID", SqlDbType.Int).Value = art_id;

            SqlParameter outputParam = cmd.Parameters.Add("@out", SqlDbType.Int);
            outputParam.Direction = ParameterDirection.Output;
            outputParam.Value = "";

            cmd.ExecuteNonQuery();

            return Convert.ToInt32(outputParam.Value);
        }
        catch (Exception e)
        {
            Console.WriteLine("SQL Error: " + e.Message);
            return -3;
        }
        finally
        {
            con.Close();
        }
    }

    public static List<Art> get_Art()
    {
        SqlConnection con = new SqlConnection(connectionString);

        try
        {
            con.Open();
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
            return null;
        }

        SqlCommand cmd;

        try
        {

            cmd = new SqlCommand("SELECT * FROM Artwork", con);

            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            da.Fill(dt);

            List<Art> art = new List<Art>();
            foreach (DataRow row in dt.Rows)
            {
                art.Add(new Art(Convert.ToInt32(row["ArtworkID"].ToString()),
                                row["Creator"].ToString(),
                                row["Name"].ToString(),
                                row["Description"].ToString(),
                                row["Medium"].ToString(),
                                row["Origin"].ToString(),
                                row["pictureLink"].ToString(),
                                row["Date"].ToString(),
                                Convert.ToInt32(row["Price"].ToString())));
            }
            return art;
        }
        catch (Exception e)
        {
            Console.WriteLine("SQL Error: " + e.Message);
            return null;
        }
        finally
        {
            con.Close();
        }
    }

    public static List<Art> get_liked(int user_id)
    {
        SqlConnection con = new SqlConnection(connectionString);

        try
        {
            con.Open();
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
            return null;
        }

        SqlCommand cmd;

        try
        {

            cmd = new SqlCommand("SELECT * FROM get_liked(@userID)", con);
            cmd.Parameters.AddWithValue("@userID", user_id);

            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            da.Fill(dt);

            List<Art> art = new List<Art>();
            foreach (DataRow row in dt.Rows)
            {
                art.Add(new Art(Convert.ToInt32(row["ArtworkID"].ToString()),
                                row["Creator"].ToString(),
                                row["Name"].ToString(),
                                row["Description"].ToString(),
                                row["Medium"].ToString(),
                                row["Origin"].ToString(),
                                row["pictureLink"].ToString(),
                                row["Date"].ToString(),
                                Convert.ToInt32(row["Price"].ToString())));
            }
            return art;
        }
        catch (Exception e)
        {
            Console.WriteLine("SQL Error: " + e.Message);
            return null;
        }
        finally
        {
            con.Close();
        }
    }

    public static List<Art> get_sales(int user_id)
    {
        SqlConnection con = new SqlConnection(connectionString);

        try
        {
            con.Open();
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
            return null;
        }

        SqlCommand cmd;

        try
        {

            cmd = new SqlCommand("SELECT * FROM get_sale(@userID)", con);
            cmd.Parameters.AddWithValue("@userID", user_id);

            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            da.Fill(dt);

            List<Art> art = new List<Art>();
            foreach (DataRow row in dt.Rows)
            {
                art.Add(new Art(Convert.ToInt32(row["ArtworkID"].ToString()),
                                row["Creator"].ToString(),
                                row["Name"].ToString(),
                                row["Description"].ToString(),
                                row["Medium"].ToString(),
                                row["Origin"].ToString(),
                                row["pictureLink"].ToString(),
                                row["Date"].ToString(),
                                Convert.ToInt32(row["Price"].ToString())));
            }
            return art;
        }
        catch (Exception e)
        {
            Console.WriteLine("SQL Error: " + e.Message);
            return null;
        }
        finally
        {
            con.Close();
        }
    }

    public static List<Art> get_purchases(int user_id)
    {
        SqlConnection con = new SqlConnection(connectionString);

        try
        {
            con.Open();
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
            return null;
        }

        SqlCommand cmd;

        try
        {

            cmd = new SqlCommand("SELECT * FROM get_pur(@userID)", con);
            cmd.Parameters.AddWithValue("@userID", user_id);

            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            da.Fill(dt);

            List<Art> art = new List<Art>();
            foreach (DataRow row in dt.Rows)
            {
                art.Add(new Art(Convert.ToInt32(row["ArtworkID"].ToString()),
                                row["Creator"].ToString(),
                                row["Name"].ToString(),
                                row["Description"].ToString(),
                                row["Medium"].ToString(),
                                row["Origin"].ToString(),
                                row["pictureLink"].ToString(),
                                row["Date"].ToString(),
                                Convert.ToInt32(row["Price"].ToString())));
            }
            return art;
        }
        catch (Exception e)
        {
            Console.WriteLine("SQL Error: " + e.Message);
            return null;
        }
        finally
        {
            con.Close();
        }
    }

    public static int Login(string email, string password)
    {

        SqlConnection con = new SqlConnection(connectionString);

        try
        {
            con.Open();
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
            return -1;
        }

        SqlCommand cmd;

        try
        {
            cmd = new SqlCommand("logOn", con);
            cmd.CommandType = CommandType.StoredProcedure;


            cmd.Parameters.Add("@email", SqlDbType.VarChar, 30).Value = email;
            cmd.Parameters.Add("@password", SqlDbType.NVarChar, 64).Value = password;

            SqlParameter outputParam = cmd.Parameters.Add("@returnValue", SqlDbType.Int);
            outputParam.Direction = ParameterDirection.Output;
            outputParam.Value = "";

            cmd.ExecuteNonQuery();
            Console.WriteLine(outputParam.Value);

            return Convert.ToInt32(outputParam.Value);

        }
        catch (SqlException ex)
        {
            Console.WriteLine("SQL Error " + ex.Message.ToString());
            return -2;
        }
        finally
        {
            con.Close();
        }

    }

    public static int Register(string fname, string lname, string pass, char gender, string email, string dob, string which, string address)
    {

        SqlConnection con = new SqlConnection(connectionString);
        try
        {
            con.Open();
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
            return -1;
        }

        SqlCommand cmd;

        try
        {
            cmd = new SqlCommand("signUp", con);

            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("@firstName", SqlDbType.VarChar, 15).Value = fname;
            cmd.Parameters.Add("@lastName", SqlDbType.VarChar, 15).Value = lname;
            cmd.Parameters.Add("@accountType", SqlDbType.VarChar, 6).Value = which;
            cmd.Parameters.Add("@email", SqlDbType.VarChar, 30).Value = email;
            cmd.Parameters.Add("@gender", SqlDbType.Char).Value = gender;
            cmd.Parameters.Add("@password", SqlDbType.VarChar, 64).Value = pass;
            cmd.Parameters.Add("@DOB", SqlDbType.Date).Value = Convert.ToDateTime(dob);
            cmd.Parameters.Add("@address", SqlDbType.VarChar, 50).Value = address;

            SqlParameter outputParam = cmd.Parameters.Add("@out", SqlDbType.Int);
            outputParam.Direction = ParameterDirection.Output;
            outputParam.Value = "";
            outputParam.Direction = ParameterDirection.Output;

            cmd.ExecuteNonQuery();

            return Convert.ToInt32(outputParam.Value);
        }
        catch (SqlException ex)
        {
            Console.WriteLine("SQL Error" + ex.Message.ToString());
            return -2;
        }
        finally
        {
            con.Close();
        }

    }

    public static Tuple<int, int> delete_art(int art_id)
    {

        SqlConnection con = new SqlConnection(connectionString);
        try
        {
            con.Open();
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
            return new Tuple<int, int>(-1, 0);
        }

        SqlCommand cmd;

        try
        {
            cmd = new SqlCommand("removeArt", con);

            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("@artID", SqlDbType.Int).Value = art_id;

            SqlParameter outputParam = cmd.Parameters.Add("@out", SqlDbType.Int);
            SqlParameter outputParam1 = cmd.Parameters.Add("@index", SqlDbType.Int);
            outputParam.Direction = ParameterDirection.Output;
            outputParam.Value = "";
            outputParam1.Direction = ParameterDirection.Output;
            outputParam1.Value = ParameterDirection.Output;

            cmd.ExecuteNonQuery();

            return new Tuple<int, int>(Convert.ToInt32(outputParam.Value),
                                       Convert.ToInt32(outputParam1.Value));

        }
        catch (SqlException ex)
        {
            Console.WriteLine("SQL Error" + ex.Message.ToString());
            return new Tuple<int, int>(-3, 0);
        }
        finally
        {
            con.Close();
        }

    }

    public static int sell_art(int user_id, Art art)
    {

        SqlConnection con = new SqlConnection(connectionString);
        try
        {
            con.Open();
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
            return -1;
        }

        SqlCommand cmd;

        try
        {
            cmd = new SqlCommand("sellArt", con);

            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("@userID", SqlDbType.Int).Value = user_id;
            cmd.Parameters.Add("@name", SqlDbType.VarChar, 50).Value = art.Title;
            cmd.Parameters.Add("@creator", SqlDbType.VarChar, 15).Value = art.Creator;
            cmd.Parameters.Add("@price", SqlDbType.Int).Value = art.Price;
            cmd.Parameters.Add("@date", SqlDbType.Date).Value = Convert.ToDateTime(art.Date);
            cmd.Parameters.Add("@origin", SqlDbType.VarChar, 20).Value = art.Origin;
            cmd.Parameters.Add("@medium", SqlDbType.VarChar, 8000).Value = art.Medium;
            cmd.Parameters.Add("@description", SqlDbType.VarChar, 8000).Value = art.Description;
            cmd.Parameters.Add("@link", SqlDbType.VarChar, 8000).Value = art.Link;

            cmd.ExecuteNonQuery();

            return 0;

        }
        catch (SqlException ex)
        {
            Console.WriteLine("SQL Error" + ex.Message.ToString());
            return -2;
        }
        finally
        {
            con.Close();
        }

    }

    public static int purchase(int u_id, int art_id, string sub_type)
    {

        SqlConnection con = new SqlConnection(connectionString);
        try
        {
            con.Open();
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
            return -2;
        }

        SqlCommand cmd;

        try
        {
            cmd = new SqlCommand("purchaseArt", con);

            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("@userID", SqlDbType.Int).Value = u_id;
            cmd.Parameters.Add("@sub_type", SqlDbType.VarChar, 30).Value = sub_type;
            cmd.Parameters.Add("@artID", SqlDbType.Int).Value = art_id;

            if (cmd.ExecuteNonQuery() == 1)
                return 0;

            return -1;

        }
        catch (SqlException ex)
        {
            Console.WriteLine("SQL Error" + ex.Message.ToString());
            return -1;
        }
        finally
        {
            con.Close();
        }

    }

    public static List<Art> get_Search_Name(String identifier)
    {

        SqlConnection con = new SqlConnection(connectionString);
        try
        {
            con.Open();
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
            return null;
        }

        SqlCommand cmd;

        try
        {
            cmd = new SqlCommand("SELECT * FROM searchArt(@identifier)", con);
            cmd.Parameters.AddWithValue("@identifier", identifier);

            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            da.Fill(dt);

            List<Art> art = new List<Art>();
            foreach (DataRow row in dt.Rows)
            {
                art.Add(new Art(Convert.ToInt32(row["ArtworkID"].ToString()),
                                row["Creator"].ToString(),
                                row["Name"].ToString(),
                                row["Description"].ToString(),
                                row["Medium"].ToString(),
                                row["Origin"].ToString(),
                                row["pictureLink"].ToString(),
                                row["Date"].ToString(),
                                Convert.ToInt32(row["Price"].ToString())));
            }
            return art;
        }
        catch (SqlException ex)
        {
            Console.WriteLine("SQL Error" + ex.Message.ToString());
            return null;
        }
        finally
        {
            con.Close();
        }

    }

    public static List<Art> get_Search_Origin(String identifier)
    {
        SqlConnection con = new SqlConnection(connectionString);
        try
        {
            con.Open();
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
            return null;
        }

        SqlCommand cmd;

        try
        {
            cmd = new SqlCommand("SELECT * FROM searchArtOrigin(@identifier)", con);
            cmd.Parameters.AddWithValue("@identifier", identifier);

            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            da.Fill(dt);

            List<Art> art = new List<Art>();
            foreach (DataRow row in dt.Rows)
            {
                art.Add(new Art(Convert.ToInt32(row["ArtworkID"].ToString()),
                                row["Creator"].ToString(),
                                row["Name"].ToString(),
                                row["Description"].ToString(),
                                row["Medium"].ToString(),
                                row["Origin"].ToString(),
                                row["pictureLink"].ToString(),
                                row["Date"].ToString(),
                                Convert.ToInt32(row["Price"].ToString())));
            }
            return art;
        }
        catch (SqlException ex)
        {
            Console.WriteLine("SQL Error" + ex.Message.ToString());
            return null;
        }
        finally
        {
            con.Close();
        }
    }

    public static List<Art> get_Similar(String origin, int id)
    {

        SqlConnection con = new SqlConnection(connectionString);
        try
        {
            con.Open();
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
            return null;
        }

        SqlCommand cmd;

        try
        {
            cmd = new SqlCommand("SELECT * FROM get_sim(@origin, @id)", con);
            cmd.Parameters.AddWithValue("@origin", origin);
            cmd.Parameters.AddWithValue("@id", id);

            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            da.Fill(dt);

            List<Art> art = new List<Art>();
            foreach (DataRow row in dt.Rows)
            {
                art.Add(new Art(Convert.ToInt32(row["ArtworkID"].ToString()),
                                row["Creator"].ToString(),
                                row["Name"].ToString(),
                                row["Description"].ToString(),
                                row["Medium"].ToString(),
                                row["Origin"].ToString(),
                                row["pictureLink"].ToString(),
                                row["Date"].ToString(),
                                Convert.ToInt32(row["Price"].ToString())));
            }
            return art;
        }
        catch (SqlException ex)
        {
            Console.WriteLine("SQL Error" + ex.Message.ToString());
            return null;
        }
        finally
        {
            con.Close();
        }

    }

    public static int owns_art(int user_id, int art_id)
    {

        SqlConnection con = new SqlConnection(connectionString);
        try
        {
            con.Open();
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
            return -2;
        }

        SqlCommand cmd;

        try
        {
            cmd = new SqlCommand("owns_art", con);

            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("@userID", SqlDbType.Int).Value = user_id;
            cmd.Parameters.Add("@artID", SqlDbType.Int).Value = art_id;

            SqlParameter outputParam = cmd.Parameters.Add("@out", SqlDbType.Int);
            outputParam.Direction = ParameterDirection.Output;
            outputParam.Value = "";

            cmd.ExecuteNonQuery();

            return Convert.ToInt32(outputParam.Value);
        }
        catch (SqlException ex)
        {
            Console.WriteLine("SQL Error" + ex.Message.ToString());
            return -3;
        }
        finally
        {
            con.Close();
        }

    }

    public static int checkPurchase(int art_id)
    {

        SqlConnection con = new SqlConnection(connectionString);
        try
        {
            con.Open();
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
            return -2;
        }

        SqlCommand cmd;

        try
        {
            cmd = new SqlCommand("checkPur", con);

            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("@artID", SqlDbType.Int).Value = art_id;

            SqlParameter outputParam = cmd.Parameters.Add("@out", SqlDbType.Int);
            outputParam.Direction = ParameterDirection.Output;
            outputParam.Value = "";

            cmd.ExecuteNonQuery();

            return Convert.ToInt32(outputParam.Value);
        }
        catch (SqlException ex)
        {
            Console.WriteLine("SQL Error" + ex.Message.ToString());
            return -3;
        }
        finally
        {
            con.Close();
        }

    }

    public static int countArt()
    {

        SqlConnection con = new SqlConnection(connectionString);
        try
        {
            con.Open();
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
            return -2;
        }

        SqlCommand cmd;

        try
        {
            cmd = new SqlCommand("countArt", con);

            cmd.CommandType = CommandType.StoredProcedure;

            SqlParameter outputParam = cmd.Parameters.Add("@out", SqlDbType.Int);
            outputParam.Direction = ParameterDirection.Output;
            outputParam.Value = "";

            cmd.ExecuteNonQuery();

            return Convert.ToInt32(outputParam.Value);
        }
        catch (SqlException ex)
        {
            Console.WriteLine("SQL Error" + ex.Message.ToString());
            return -3;
        }
        finally
        {
            con.Close();
        }

    }
}