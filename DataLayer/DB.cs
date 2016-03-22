using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;


namespace DataLayer
{
    public class DB
    {
        public DB()
        {

        }

        //Proprietate a clasei DB care preia din config connection string-ul;
        public string ConnectionString
        {
            get
            {
                string connString = ConfigurationManager.ConnectionStrings["connString"].ToString();
                return connString;
            }
        }

        //Metoda care insereaza in baza lista de obiecte. 
        public void InsertIntoDatabase(CsvClass CsvObject)
        {
            using (SqlConnection conn = new SqlConnection(ConnectionString))
            {
                //Deschid conexiunea cu baza de date inainte de foreach pentru a nu deschide conexiunea pentru fiecare obiect inserat
                conn.Open();
                using (SqlCommand cmd = new SqlCommand("instert", conn))
                {

                    //Folosesc o procedura stocata pentru insert
                    cmd.CommandType = CommandType.StoredProcedure;

                    //Sterg parametrii folositi anterior pentru a adauga noi parametri
                    cmd.Parameters.Clear();

                    //Adaug parametrii. Daca proprietatea este null sau empty pun null in baza(DBNull)
                    if (CsvObject.LogTime == null)
                        cmd.Parameters.AddWithValue("@LOGTIME", DBNull.Value);
                    else
                        cmd.Parameters.AddWithValue("@LOGTIME", CsvObject.LogTime);

                    if (string.IsNullOrEmpty(CsvObject.Action))
                        cmd.Parameters.AddWithValue("@ACTION", DBNull.Value);
                    else
                        cmd.Parameters.AddWithValue("@ACTION", CsvObject.Action);

                    if (string.IsNullOrEmpty(CsvObject.FolderPath))
                        cmd.Parameters.AddWithValue("@FOLDERPATH", DBNull.Value);
                    else
                        cmd.Parameters.AddWithValue("@FOLDERPATH", CsvObject.FolderPath);

                    if (string.IsNullOrEmpty(CsvObject.Filename))
                        cmd.Parameters.AddWithValue("@FILENAME", DBNull.Value);
                    else
                        cmd.Parameters.AddWithValue("@FILENAME", CsvObject.Filename);

                    if (string.IsNullOrEmpty(CsvObject.Username))
                        cmd.Parameters.AddWithValue("@USERNAME", DBNull.Value);
                    else
                        cmd.Parameters.AddWithValue("@USERNAME", CsvObject.Username);

                    if (string.IsNullOrEmpty(CsvObject.IpAdress))
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

                    if (string.IsNullOrEmpty(CsvObject.AgentBrand))
                        cmd.Parameters.AddWithValue("@AGENTBRAND", DBNull.Value);
                    else
                        cmd.Parameters.AddWithValue("@AGENTBRAND", CsvObject.AgentBrand);

                    if (string.IsNullOrEmpty(CsvObject.AgentVersion))
                        cmd.Parameters.AddWithValue("@AGENTVERSION", DBNull.Value);
                    else
                        cmd.Parameters.AddWithValue("@AGENTVERSION", CsvObject.AgentVersion);

                    if (CsvObject.Error == null)
                        cmd.Parameters.AddWithValue("@ERROR", DBNull.Value);
                    else
                        cmd.Parameters.AddWithValue("@ERROR", CsvObject.Error);

                        cmd.ExecuteNonQuery();
                    

                }
                //Inchid conexiunea cu baza de date
                conn.Close();
            }
        }

        //Metoda care citeste inregistrarile din baza si le returneaza intr-o lista de obiecte
        public List<CsvClass> readDatabase(string starDate, string endDate, string Ip)
        {
            //Instantiez o lista de obiecte de tip CsvClass
            List<CsvClass> listaObiecte = new List<CsvClass>();

            DateTime starDateParsat;
            DateTime endDateParsat;


            using (SqlConnection conn = new SqlConnection(ConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand("selectProcedure", conn))
                {
                    //Deschid conexiunea cu baza de date
                    conn.Open();

                    //Pentru citire folosesc o procedura stocata
                    cmd.CommandType = CommandType.StoredProcedure;

                    //Adaug parametrii procedurii stocate
                    if (DateTime.TryParse(starDate, out starDateParsat))
                        cmd.Parameters.AddWithValue("@startDate", starDateParsat);
                    else
                        cmd.Parameters.Add("@startDate", SqlDbType.DateTime).Value = DBNull.Value;

                    if (DateTime.TryParse(endDate, out endDateParsat))
                        cmd.Parameters.AddWithValue("@endDate", endDateParsat);
                    else
                        cmd.Parameters.Add("@endDate", SqlDbType.DateTime).Value = DBNull.Value;

                    cmd.Parameters.AddWithValue("@Ip", Ip);

                    //Pentru citirea datelor din baza de date foloses SqlDataReader
                    SqlDataReader reader = cmd.ExecuteReader();

                    //Un while in care citesc fiecare linie din baza de date
                    while (reader.Read())
                    {
                        //Declar variabilele de care am nevoie pentru parsarea datelor citite din baza
                        DateTime logTime;
                        int xferSize;
                        double duration;
                        int error;

                        //Instantiez un obiect de tip CsvClass
                        CsvClass csv = new CsvClass();

                        //Pentru fiecare proprietate a obiectului asignez elementul citit din baza
                        csv.Id = Convert.ToInt32(reader["Id"].ToString());
                        if (DateTime.TryParse(reader["LogTime"].ToString(), out logTime))
                            csv.LogTime = logTime;
                        else
                            csv.LogTime = null;

                        csv.Action = reader["Action"].ToString();
                        csv.FolderPath = reader["FolderPath"].ToString();
                        csv.Filename = reader["Filename"].ToString();
                        csv.Username = reader["Username"].ToString();
                        csv.IpAdress = reader["IpAdress"].ToString();

                        if (Int32.TryParse(reader["XferSize"].ToString(), out xferSize))
                            csv.XferSize = xferSize;
                        else
                            csv.XferSize = null;

                        if (Double.TryParse(reader["Duration"].ToString(), out duration))
                            csv.Duration = duration;
                        else
                            csv.Duration = null;

                        csv.AgentBrand = reader["AgentBrand"].ToString();
                        csv.AgentVersion = reader["AgentVersion"].ToString();

                        if (Int32.TryParse(reader["Error"].ToString(), out error))
                            csv.Error = error;
                        else
                            csv.Error = null;

                        //Adaug obiectul citit in lista
                        listaObiecte.Add(csv);
                    }
                }
            }
            //Returnez lista de obiecte
            return listaObiecte;

        }


    }
}
