using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;
using System.Globalization;
using System.Net;

namespace DataLayer
{
    public class DB
    {
        public DB()
        {

        }
        public string ConnectionString
        {
            get
            {
                string connString = ConfigurationManager.ConnectionStrings["connString"].ToString();

                //SqlConnectionStringBuilder sb = new SqlConnectionStringBuilder(connString);
                //sb.ConnectTimeout 
                return connString;
            }
        }

        public void InsertIntoDatabase(CsvClass CsvObject)
        {
            using (SqlConnection conn = new SqlConnection(ConnectionString))
            {
                conn.Open();
                using (SqlCommand cmd = new SqlCommand("instert", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Clear();

                    cmd.Parameters.AddWithValue("@LOGTIME", CsvObject.LogTime);
                    cmd.Parameters.AddWithValue("@ACTION", CsvObject.Action);
                    cmd.Parameters.AddWithValue("@FOLDERPATH", CsvObject.FolderPath);
                    cmd.Parameters.AddWithValue("@FILENAME", CsvObject.Filename);
                    cmd.Parameters.AddWithValue("@USERNAME", CsvObject.Username);
                    cmd.Parameters.AddWithValue("@IPADRESS", CsvObject.IpAdress);
                    cmd.Parameters.AddWithValue("@XFERSIZE", CsvObject.XferSize);
                    cmd.Parameters.AddWithValue("@DURATION", CsvObject.Duration);
                    cmd.Parameters.AddWithValue("@AGENTBRAND", CsvObject.AgentBrand);
                    cmd.Parameters.AddWithValue("@AGENTVERSION", CsvObject.AgentVersion);
                    cmd.Parameters.AddWithValue("@ERROR", CsvObject.Error);

                    cmd.ExecuteNonQuery();
                    conn.Close();
                }
            }
        }

        public List<CsvClass> readDatabase(string starDate, string endDate, string Ip)
        {
            List<CsvClass> listaObiecte = new List<CsvClass>();
            DateTime starDateParsat;
            DateTime endDateParsat;
            bool parseStarDate = DateTime.TryParseExact(starDate, "mm/dd/yyyy",CultureInfo.InvariantCulture,DateTimeStyles.None,out starDateParsat);
            bool parseEndDate = DateTime.TryParseExact(endDate, "mm/dd/yyyy", CultureInfo.InvariantCulture,DateTimeStyles.None,out endDateParsat);

            

            using (SqlConnection conn = new SqlConnection(ConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand("selectProcedure", conn))
                {
                    conn.Open();
                    cmd.CommandType = CommandType.StoredProcedure;
                    if (parseStarDate == true)
                        cmd.Parameters.AddWithValue("@startDate", starDateParsat);
                    else
                        cmd.Parameters.Add("@startDate", SqlDbType.SmallDateTime).Value = DBNull.Value;

                    if (parseEndDate == true)
                        cmd.Parameters.AddWithValue("@endDate", endDateParsat);
                    else
                        cmd.Parameters.Add("@endDate", SqlDbType.SmallDateTime).Value = DBNull.Value;

                    
                    
                    cmd.Parameters.AddWithValue("@Ip", Ip);

                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        CsvClass csv = new CsvClass();
                        csv.Id = Convert.ToInt32(reader["Id"].ToString());
                        csv.LogTime = DateTime.Parse(reader["LogTime"].ToString());
                        csv.FolderPath = reader["FolderPath"].ToString();
                        csv.Filename = reader["Filename"].ToString();
                        csv.Username = reader["Username"].ToString();
                        csv.IpAdress = reader["IpAdress"].ToString();
                        csv.XferSize = Convert.ToInt32(reader["XferSize"].ToString());
                        csv.Duration = Convert.ToDouble(reader["Duration"].ToString());
                        csv.AgentBrand = reader["AgentBrand"].ToString();
                        csv.AgentVersion = reader["AgentVersion"].ToString();
                        csv.Error = Convert.ToInt32(reader["Error"].ToString());
                    
                        listaObiecte.Add(csv);
                    }
                }
            }
            return listaObiecte;

        }


    }
}
