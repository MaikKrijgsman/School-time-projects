using Game_Website.Model;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Game_Website
{
    public partial class Profile : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            var sessionName = Session["Name"];
            var NameSession = Session["GameName"];
            Lbl1.Text = Convert.ToString(sessionName);

            if (sessionName == null)
            {
                Response.Redirect("Default.aspx");
            }
            IList<MemScore> scores = new List<MemScore>();
            string connection = Properties.Settings.Default.MySqlConnectionString;
            MySqlConnection dbcon = new MySqlConnection(connection);
            dbcon.Open();
            MySqlCommand cmd;
            cmd = dbcon.CreateCommand();

            cmd.CommandText = "select * from test.scores where Name = '" + NameSession + "' order by Score asc limit 1;";
            MySqlDataReader rdr = cmd.ExecuteReader();

            while (rdr.Read())
            {
                MemScore d = new MemScore();
                d.DBscore = (string)rdr["Score"];
                scores.Add(d);
            }
            foreach (MemScore Score in scores)
            {
                PersonalScores.InnerText = "Memory: " + Score.DBscore;
            }
            if (scores.Count == 0)
            {
                PersonalScores.InnerText = "No highscore set for memory";
            }

            rdr.Close();

        }

        protected void BtnLogout_ServerClick(object sender, EventArgs e)
        {
            Session.Abandon();
            Response.Redirect("Default.aspx");
        }

        protected void SaveChangesPF_ServerClick(object sender, EventArgs e)
        {
            var NameSession = Session["GameName"];


            if (TxtNickNamePF == null)
            {
                try
                {
                    string connstring = Properties.Settings.Default.MySqlConnectionString;
                    MySqlConnection myconn = new MySqlConnection(connstring);
                    string query = "update test.users set Nick_Name = '" + TxtNickNamePF.Value + "' where Nick_Name = '" + NameSession.ToString() + "';";
                    MySqlCommand cmd = new MySqlCommand(query, myconn);
                    MySqlDataReader MyReader;
                    myconn.Open();
                    MyReader = cmd.ExecuteReader();
                    myconn.Close();
                }
                catch (Exception)
                {   

                }

            }
        }
    }
}