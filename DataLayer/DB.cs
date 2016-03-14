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
                return connString;
            }
        }

        public void InsertIntoDatabase(List<CsvClass> lista)
        {
            using (SqlConnection conn = new SqlConnection(ConnectionString))
            {
                conn.Open();
                foreach (CsvClass CsvObject in lista)
                {
                    using (SqlCommand cmd = new SqlCommand("instert", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.Clear();

                        if (CsvObject.LogTime == null)
                            cmd.Parameters.AddWithValue("@LOGTIME", DBNull.Value);
                        else
                            cmd.Parameters.AddWithValue("@LOGTIME", CsvObject.LogTime);

                        if (string.IsNullOrEmpty(CsvObject.Action) == true)
                            cmd.Parameters.AddWithValue("@ACTION", DBNull.Value);
                        else
                            cmd.Parameters.AddWithValue("@ACTION", CsvObject.Action);

                        if (string.IsNullOrEmpty(CsvObject.FolderPath) == true)
                            cmd.Parameters.AddWithValue("@FOLDERPATH", DBNull.Value);
                        else
                            cmd.Parameters.AddWithValue("@FOLDERPATH", CsvObject.FolderPath);

                        if (string.IsNullOrEmpty(CsvObject.Filename) == true)
                            cmd.Parameters.AddWithValue("@FILENAME", DBNull.Value);
                        else
                            cmd.Parameters.AddWithValue("@FILENAME", CsvObject.Filename);

                        if (string.IsNullOrEmpty(CsvObject.Username) == true)
                            cmd.Parameters.AddWithValue("@USERNAME", DBNull.Value);
                        else
                            cmd.Parameters.AddWithValue("@USERNAME", CsvObject.Username);

                        if (string.IsNullOrEmpty(CsvObject.IpAdress) == true)
                            cmd.Parameters.AddWithValue("@IPADRESS", DBNull.Value);
                        else
                            cmd.Parameters.AddWithValue("@IPADRESS", CsvObject.IpAdress);

                        if (CsvObject.XferSize == null)
                            cmd.Parameters.AddWithValue("@XFERSIZE", DBNull.Value);
                        else
                            cmd.Parameters.AddWithValue("@XFERSIZE", CsvObject.XferSize);

                        if (CsvObject.Duration == null)
                            cmd.Parameters.AddWithValue("@DURATION", DBNull.Value);
                        else
                            cmd.Parameters.AddWithValue("@DURATION", CsvObject.Duration);
                        if (string.IsNullOrEmpty(CsvObject.AgentBrand) == true)                        
                            cmd.Parameters.AddWithValue("@AGENTBRAND", DBNull.Value);
                        else
                            cmd.Parameters.AddWithValue("@AGENTBRAND", CsvObject.AgentBrand);

                        if (string.IsNullOrEmpty(CsvObject.AgentVersion) == true)
                            cmd.Parameters.AddWithValue("@AGENTVERSION", DBNull.Value);
                        else
                            cmd.Parameters.AddWithValue("@AGENTVERSION", CsvObject.AgentVersion);
                        if (CsvObject.Error == null)
                            cmd.Parameters.AddWithValue("@ERROR", DBNull.Value);
                        else
                            cmd.Parameters.AddWithValue("@ERROR", CsvObject.Error);

                        cmd.ExecuteNonQuery();

                    }
                }
                conn.Close();
            }
        }

        public List<CsvClass> readDatabase(string starDate, string endDate, string Ip)
        {
            List<CsvClass> listaObiecte = new List<CsvClass>();
            DateTime starDateParsat;
            DateTime endDateParsat;


            bool parseStarDate = DateTime.TryParse(starDate, out starDateParsat);
            bool parseEndDate = DateTime.TryParse(endDate, out endDateParsat);

            using (SqlConnection conn = new SqlConnection(ConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand("selectProcedure", conn))
                {
                    conn.Open();
                    cmd.CommandType = CommandType.StoredProcedure;
                    if (parseStarDate == true)
                        cmd.Parameters.AddWithValue("@startDate", starDateParsat);
                    else
                        cmd.Parameters.Add("@startDate", SqlDbType.DateTime).Value = DBNull.Value;

                    if (parseEndDate == true)
                        cmd.Parameters.AddWithValue("@endDate", endDateParsat);
                    else
                        cmd.Parameters.Add("@endDate", SqlDbType.DateTime).Value = DBNull.Value;

                    cmd.Parameters.AddWithValue("@Ip", Ip);

                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        CsvClass csv = new CsvClass();
                        csv.Id = Convert.ToInt32(reader["Id"].ToString());
                        csv.LogTime = DateTime.Parse(reader["LogTime"].ToString());
                        csv.Action = reader["Action"].ToString();
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
