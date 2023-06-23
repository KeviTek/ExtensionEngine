using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExtensionEngine.Models
{
    class Dbservice
    {
        SqlConnection conSql = null;
        MySqlConnection con = null;

        SqlCommand cmdSql = null;
        MySqlDataReader dr = null;

        MySqlCommand cmd = null;

        void ConnectionString()
        {
            //con = new MySqlConnection(ConfigurationManager.AppSettings["CIP_connexionString"].ToString());
            con = new MySqlConnection(ConfigurationManager.AppSettings["PM_mysqlconnect"].ToString());

        }

        // fetch data from database

        public List<ExtensionEntry> FetchData(string APPROVED)
        {
            List<ExtensionEntry> list = new List<ExtensionEntry>();
            ExtensionEntry extensionEntry = null;

            string query = "select APP_STATUS app_status, STAFF_NAME_LABEL staff_name_label, STATUS status , USERNAME username , DEADLINE_LABEL deadline_label from PMT_EXTENSION_REQUEST where status = '" + APPROVED + "'" + ";";
            //string query = "select ID id, CUS_NUM cus_num, CHEQ_NO cheq_no, AMOUNT amount, CASE_NUM case_num, STATUS status, REASON reason, DATE_EMIT date_emit, COMMUNE issuer_place, to_company to_company, signatory SIGNATORY from PMT_CIP_LETTER_GEN where status = '" + pending + "'" + ";";
            try
            {
                ConnectionString();
                con.Open();
                cmd = new MySqlCommand(query, con);
                dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    extensionEntry = new ExtensionEntry()
                    {                   
                        app_status = dr["app_status"].ToString(),
                        staff_name_label = dr["staff_name_label"].ToString(),
                        status = dr["status"].ToString(),
                        username = dr["username"].ToString(),
                        deadline = dr["deadline_label"].ToString()
                    };
                    list.Add(extensionEntry);
                    
                }

                if (extensionEntry != null)
                {                 
                    
                    string logPath = ConfigurationManager.AppSettings["LogPath"];

                    using (StreamWriter writer = new StreamWriter(logPath, true))
                    {
                        foreach (var item in list)
                        {
                            var c = item.deadline;

                            var a = c.Split('h');

                            var dead = a[0];
                            writer.WriteLine(item.username+","+dead);
                        }
                    }
                }              

                //con.Close();
            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {
                con.Close();
            }
            return list;
        }
      
        public void SendEmail(List<ExtensionEntry> list)
        {
            //Send Mail

            var emailFrom = "contact.ci@gtbank.com";
            var emailTo = "it.ci@gtbank.com";
            var emailTo1 = "loic.kone@gtbank.com";
            var emailTo2 = "gertrude.yobonou@gtbank.com";
            //    ccEmail = ConfigurationManager.AppSettings["gtconnectEmail"];

            var msg = "Cher Team " + Environment.NewLine + "";

            msg += "" + Environment.NewLine + "";
            msg += "Nous esperons que ce mail vous trouve en condition favorable," + Environment.NewLine + "";
            msg += "Nous vous envoyons la liste des exceptions pris aujourd'hui," + Environment.NewLine + "";
            msg += "Veuillez passer une excellente fin de soirée" + Environment.NewLine + "";
            msg += "-------------------------------------------------------------------------" + Environment.NewLine + "";
            foreach (var item in list)
            {
                msg += item.staff_name_label + " : " + item.username + Environment.NewLine + "";
            }

            msg += "" + Environment.NewLine + "";
            msg += "" + Environment.NewLine + "";
            msg += " Cordialement ";


            EmailSender emailSender = new EmailSender();
            string resp = emailSender.SendEmail(emailFrom, emailTo, "Extension Request Information", msg);

            EmailSender emailSender1 = new EmailSender();
            string resp1 = emailSender1.SendEmail(emailFrom, emailTo1, "Extension Request Information", msg);

            EmailSender emailSender2 = new EmailSender();
            string resp2 = emailSender2.SendEmail(emailFrom, emailTo2, "Extension Request Information", msg);
        }

        public void mail()
        {
            var emailFrom = "contact.ci@gtbank.com";
            var emailTo = "it.ci@gtbank.com";
            //    ccEmail = ConfigurationManager.AppSettings["gtconnectEmail"];

            var msg = "Cher Team IT " + Environment.NewLine + "";

            msg += "" + Environment.NewLine + "";
            msg += "Nous esperons que ce mail vous trouve en condition favorable," + Environment.NewLine + "";
            msg += "Vous avez aucune extension prise ," + Environment.NewLine + "";
            msg += "Veuillez passer une excellente fin de soirée" + Environment.NewLine + "";
            msg += "" + Environment.NewLine + "";
            msg += "" + Environment.NewLine + "";
            msg += " Cordialement ";


            EmailSender emailSender = new EmailSender();
            string resp = emailSender.SendEmail(emailFrom, emailTo, "Extension Request Information", msg);
        }

        public bool UpdateStatus(string TREATED, string APPROVED)
        {

            string query = "update PMT_EXTENSION_REQUEST set STATUS = '" + TREATED + "'" + " where status = '" + APPROVED + "';";
            //string query1 = "DELETE FROM PMT_EXTENSION_REQUEST WHERE STATUS ='PENDING';";
            //string query = "update PMT_CIP_LETTER_GEN set STATUS = '" + value + "'" + " where id = '" + id + "';";
            try
            {
                ConnectionString();
                con.Open();
                cmd = new MySqlCommand(query, con);
                int resp = cmd.ExecuteNonQuery();
                con.Close();
                if (resp > 0) return true;
                else return false;

            }
            catch (Exception ex)
            {
                return false;
            }
            finally
            {
                con.Close();
            }
        }

        public bool DropPending(string PENDING)
        {

            //string query = "update PMT_EXTENSION_REQUEST set STATUS = '" + TREATED + "'" + " where status = '" + APPROVED + "';";
            string query = "DELETE FROM PMT_EXTENSION_REQUEST WHERE STATUS ='PENDING';";
            //string query = "update PMT_CIP_LETTER_GEN set STATUS = '" + value + "'" + " where id = '" + id + "';";
            try
            {
                ConnectionString();
                con.Open();
                cmd = new MySqlCommand(query, con);
                int resp = cmd.ExecuteNonQuery();
                con.Close();
                if (resp > 0) return true;
                else return false;

            }
            catch (Exception ex)
            {
                return false;
            }
            finally
            {
                con.Close();
            }
        }

    }
}
