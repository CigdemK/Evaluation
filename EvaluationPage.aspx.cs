using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;

public partial class EvaluationPage : System.Web.UI.Page
{

    protected void Page_Load(object sender, EventArgs e)
    {

        if (!Page.IsPostBack){

            int numberOfSchemes = SelectNumberOfSchemes();

            var tempUserInfo = SelectLastAddedUser();
            int lastSubjectID = tempUserInfo.Item1;
            int lastScheme = tempUserInfo.Item2;

            int currentScheme = lastScheme % numberOfSchemes +1;
            var tempVideoInfo = SelectNextComparison(currentScheme, 1); // comparison nr 1 for the first page load
            String video1 = tempVideoInfo.Item1;
            String video2 = tempVideoInfo.Item2;
            String originalVideo = tempVideoInfo.Item3;

            ViewState.Add("scheme", currentScheme);
            ViewState.Add("comparison", 2); // start comparison from 2 since we have already loaded the first one
            ViewState.Add("subjectID", ++lastSubjectID);
            ViewState.Add("video1", video1);
            ViewState.Add("video2", video2);
            ViewState.Add("originalVideo", originalVideo);

            InsertNewUser(currentScheme);

            originalVideoID.Attributes["src"] = originalVideo;
            video1ID.Attributes["src"] = video1;
            video2ID.Attributes["src"] = video2;

        }
       
    }

    public void SelectButtonOnClick(object sender, EventArgs e)
    {
        LinkButton clickedButton = (LinkButton)sender;
  
        String selection = "";
        if (clickedButton.ID == "videoButton1")
            selection = ViewState["video1"].ToString();
        else 
            selection = ViewState["video2"].ToString();

        InsertSelection(selection);

        var tempVideoInfo = SelectNextComparison(Convert.ToInt32(ViewState["scheme"]), Convert.ToInt32(ViewState["comparison"]));
        if (tempVideoInfo == null){
            Response.Redirect(Page.ResolveClientUrl("/Goodbye.html"));
            return;
        }
        
        int comparison = Convert.ToInt32(ViewState["comparison"]);

        ViewState.Add("comparison", ++comparison);
        ViewState.Add("video1", tempVideoInfo.Item1);
        ViewState.Add("video2", tempVideoInfo.Item2);
        ViewState.Add("originalVideo", tempVideoInfo.Item3);

        video1ID.Attributes["src"] =  tempVideoInfo.Item1;
        video2ID.Attributes["src"] =  tempVideoInfo.Item2;
        originalVideoID.Attributes["src"] =  tempVideoInfo.Item3;

    }

    private bool InsertSelection(String selection){

        SqlConnection conn = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["SQLAzureConnection"].ConnectionString);
        SqlCommand queryCommand = conn.CreateCommand();

        try {
            conn.Open();
            queryCommand.CommandText = "INSERT INTO Results (SubjectID,Video1,Video2,Selection) VALUES (@subjectid,@video1,@video2,@selection)";
            queryCommand.Parameters.AddWithValue("@subjectid", ViewState["subjectID"]);
            queryCommand.Parameters.AddWithValue("@video1", ViewState["video1"]);
            queryCommand.Parameters.AddWithValue("@video2", ViewState["video2"]);
            queryCommand.Parameters.AddWithValue("@selection", selection);
            queryCommand.ExecuteNonQuery();
        }
        catch (SqlException ex) { return false; }
        finally { conn.Close(); }

        return true;
    }

    private Tuple<String,String,String> SelectNextComparison(int scheme, int comparison) {

        SqlConnection conn = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["SQLAzureConnection"].ConnectionString);
        SqlCommand queryCommand = conn.CreateCommand();
        SqlDataReader reader = null;

        String video1 = "";
        String video2 = "";
        String originalVideo = "";

        try
        {
            conn.Open();
            queryCommand.CommandText = "SELECT Video1,Video2,OriginalVideo FROM Schemes WHERE SchemeNumber=@Scheme AND ComparisonNumber=@Comparison";
            queryCommand.Parameters.AddWithValue("@Scheme", scheme);
            queryCommand.Parameters.AddWithValue("@Comparison", comparison);
            reader = queryCommand.ExecuteReader();
            reader.Read();

            if (!reader.HasRows) return null;

            video1 = reader.GetString(0);
            video2 = reader.GetString(1);
            originalVideo = reader.GetString(2);
        }
        catch (SqlException ex) { return null; }
        finally { conn.Close(); }

        return Tuple.Create(video1, video2, originalVideo); ;
        
    }

    private Tuple<int, int> SelectLastAddedUser() {

        SqlConnection conn = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["SQLAzureConnection"].ConnectionString);
        SqlCommand queryCommand = conn.CreateCommand();
        SqlDataReader reader = null;

        int lastID, lastScheme = 0;

        try{
            conn.Open();
            queryCommand.CommandText = "SELECT * FROM Subjects where ID=(SELECT max(ID) FROM Subjects)";
            reader = queryCommand.ExecuteReader();
            reader.Read();
            lastID = reader.GetInt32(0);
            lastScheme = reader.GetInt32(1);
            conn.Close();
        }
        catch (SqlException ex) { return null; }
        finally { conn.Close(); }

        return Tuple.Create(lastID, lastScheme); ;
    }

    private int SelectNumberOfSchemes() {

        SqlConnection conn = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["SQLAzureConnection"].ConnectionString);
        SqlCommand queryCommand = conn.CreateCommand();
        SqlDataReader reader = null;

        int numberOfSchemes = 0;

        try{
            conn.Open();
            queryCommand.CommandText = "SELECT max(SchemeNumber) FROM Schemes";
            reader = queryCommand.ExecuteReader();
            reader.Read();

            if (!reader.HasRows) return 0;

            numberOfSchemes = reader.GetInt32(0);
            conn.Close();
        }
        catch (SqlException ex) { return 0; }
        finally { conn.Close(); }

        return numberOfSchemes;
    }

    private bool InsertNewUser(int scheme){

        SqlConnection conn = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["SQLAzureConnection"].ConnectionString);
        SqlCommand queryCommand = conn.CreateCommand();

        try {
            conn.Open();
            queryCommand.CommandText = "INSERT INTO Subjects (scheme) VALUES (@currentScheme)";
            queryCommand.Parameters.AddWithValue("@currentScheme", Convert.ToString(scheme));
            queryCommand.ExecuteNonQuery();    
        }
        catch (SqlException ex) { return false; }
        finally { conn.Close(); }

        return true;
    }
     
}