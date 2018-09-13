using System;
using MySql.Data.MySqlClient;
using System.Data;
using System.Net.Mail;
using System.Net;
using System.Data.SqlClient;
using System.Configuration;
using System.Timers;

namespace Game_Website
{
    public partial class Login : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
        }
        protected void BtnLogin_Click(object sender, EventArgs e)
        {
            string strcon = Properties.Settings.Default.MySqlConnectionString;
            string str;
            MySqlCommand com;
            object obj;
            MySqlConnection con = new MySqlConnection(strcon);

            con.Open();

            str = "select count(*) from test.users where Email=@Email and Password =@Password";

            com = new MySqlCommand(str, con);
            com.CommandType = CommandType.Text;
            com.Parameters.AddWithValue("@Email", TxTemail.Text);
            com.Parameters.AddWithValue("@Password", TxTpassword.Text);
            obj = com.ExecuteScalar();

            if (Convert.ToInt32(obj) != 0)
            {
                string sqlConn = Properties.Settings.Default.MySqlConnectionString;
                MySqlConnection connec = new MySqlConnection(sqlConn);
                MySqlCommand sqlCmd = new MySqlCommand();
                string sSql = "SELECT Nick_Name FROM test.users WHERE Email Like '%" + TxTemail.Text + "%'";

                sqlCmd.CommandText = sSql;
                sqlCmd.CommandType = CommandType.Text;
                connec.Open();
                sqlCmd.Connection = connec;
                MySqlDataReader reader = sqlCmd.ExecuteReader();
                while (reader.Read())
                {
                    string LoginMessage = "Login successfull!";
                    SpanLabelLogin.Text = LoginMessage;
                    string result = reader["Nick_Name"].ToString();

                    string WelcomeMessage = "Welcome:" + result;
                    Session["GameName"] = result;
                    Session["Name"] = WelcomeMessage;

                    Response.Redirect("Default.aspx");
                    //Response.AddHeader("REFRESH", "1;URL=/Default.aspx");
                }
                reader.Close();
            }
            else
            {
                string LoginError = "Wrong password or email!";
                SpanLabelLogin.Text = LoginError;
            }
            con.Close();
        }
        protected void btnGoToRegister_Click(object sender, EventArgs e)
        {
            Response.Redirect("Create_Account.aspx");
        }
        protected void btnforgot_Click(object sender, EventArgs e)
        {
            try
            {
                string constring = Properties.Settings.Default.MySqlConnectionString;
                MySqlConnection con = new MySqlConnection(constring);
                MySqlCommand cmd = new MySqlCommand("Select count(*) from test.users where Email= @Email");
                cmd.Parameters.AddWithValue("@Email", this.TxtEmailRequest.Text);
                cmd.Connection = con;
                con.Open();
                int TotalRows = 0;
                TotalRows = Convert.ToInt32(cmd.ExecuteScalar());
                if (TotalRows > 0)
                {
                    try
                    {
                        MailMessage mail = new MailMessage();
                        SmtpClient SmtpServer = new SmtpClient("smtp.gmail.com");

                        mail.From = new MailAddress("maik.bbct@gmail.com");
                        mail.To.Add(TxtEmailRequest.Text);
                        mail.Subject = "Password recovery";
                        mail.Body = "This is for testing SMTP mail from GMAIL";

                        SmtpServer.Port = 587;
                        SmtpServer.Credentials = new System.Net.NetworkCredential(Properties.Settings.Default.EmailAdress, Properties.Settings.Default.EmailPassword);
                        SmtpServer.EnableSsl = true;

                        SmtpServer.Send(mail);
                        SpanLabelLogin.Text = "Mail send!";
                    }
                    catch (Exception)
                    {
                        SpanLabelLogin.Text = "Oops something went wrong try again or contact a admin!";
                    }
                }
                else
                {
                    SpanLabelLogin.Text = ("Email is not registered!");
                    TxtEmailRequest.Text = "";
                }
            }
            catch (Exception)
            {
                SpanLabelLogin.Text = ("Oops something went wrong try again or contact a admin!");
            }
        }
    }
}

