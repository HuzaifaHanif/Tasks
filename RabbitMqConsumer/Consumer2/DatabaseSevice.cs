
using Microsoft.Data.SqlClient;
using ServiceContracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KafkaConsumer2
{
    public class DatabaseSevice
    {
        private string _connectionString;
        private static DatabaseSevice _instance;

        private DatabaseSevice()
        {

        }

        public static DatabaseSevice GetInstance()
        {
            if (_instance == null)
                return _instance = new DatabaseSevice();

            return _instance;
        }

        public async Task LogDb(RabbitMq message)
        {
            string query = @"INSERT INTO RabbitMQ (Guid, Queue, Message, Exchange, ConsumerName)
                VALUES (@Guid, @Queue, @Message, @Exchange, @ConsumerName);";


            _connectionString = "Data Source=VID-DT-1051;Database=SoftechWorldWide;Integrated Security=True;Connect Timeout=30;Encrypt=True;Trust Server Certificate=True;Application Intent=ReadWrite;Multi Subnet Failover=False";

            try
            {
                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    connection.Open();

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@Guid", message.Guid);
                        command.Parameters.AddWithValue("@Queue", message.Queue);
                        command.Parameters.AddWithValue("@Message", message.Message);
                        command.Parameters.AddWithValue("@Exchange", message.Exchange);
                        command.Parameters.AddWithValue("@ConsumerName", message.ConsumerName);

                        command.ExecuteNonQuery();
                    }
                }
            }

            catch (SqlException ex)
            {
                Console.WriteLine($"SQL Exception: {ex.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception: {ex.Message}");
            }

        }
    }
}

