using System;
using System.Net.Http;
using System.Timers;
using Newtonsoft.Json;
using Microsoft.Data.SqlClient;
using Electric_Prices.Interfaces;



namespace Electric_Prices.Services
{

    public class RetrieveData : IRetrieveData
    {
        // Initiliase Timer 
        private static System.Timers.Timer APITimer = new System.Timers.Timer(3600000);
        public void Main()
        {
            SetTimer();
            RetrieveAPIData();
            Console.WriteLine("Retrieve API Data has started");
            Console.WriteLine("Press Enter to stop the process");
            Console.ReadLine();
            APITimer.Stop();
            APITimer.Dispose();
        }

        public async void RetrieveAPIData()
        {

            // Get data from Elering API
            HttpClient client = new HttpClient();

            HttpResponseMessage response = await client.GetAsync("https://dashboard.elering.ee/api/nps/price/EE/current");

            response.EnsureSuccessStatusCode();

            var responseBody = await response.Content.ReadAsStringAsync();

            //Test API output


            // Try to connect to SQL Server
            try
            {
                SqlConnectionStringBuilder sql_builder = new SqlConnectionStringBuilder();

                sql_builder.ConnectionString = "Data Source=LAPTOP-TH8704MP\\ARTEMISJEM;Database=electricpriceDB;Integrated Security=sspi;Encrypt=False;";
                

                using (SqlConnection connection = new SqlConnection(sql_builder.ConnectionString))
                {

                    connection.Open();       

                    string insert = "InsertAPIData";

                    // Insert data retrieved from the API by calling "dbo.InsertAPIData" Stored Procedure on SQL Server
                    using (SqlCommand command = new SqlCommand(insert, connection))
                    {
                        Console.WriteLine("InsertAPIData has started");
                        command.CommandType = System.Data.CommandType.StoredProcedure;

                        command.Parameters.Add(new SqlParameter("@json", responseBody));

                        command.ExecuteReader();

                        Console.WriteLine("InsertAPIData is finished");
                    }

                }
            }
            catch (SqlException e)
            {
                Console.WriteLine(e.ToString());
            }
        }

        // Set timer and reset it after OnTimedEvent has been called

        #pragma warning disable CS1998
        private async void SetTimer()
        {
            #pragma warning disable CS8622
            APITimer.Elapsed += OnTimedEvent;
            APITimer.AutoReset = true;
            APITimer.Enabled = true;
        }

        private async void OnTimedEvent(object source,ElapsedEventArgs e)
        {
            Console.WriteLine("RetrieveData func");
            RetrieveAPIData();
        }
    }
}