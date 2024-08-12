using KafkaConsumer1;
using Microsoft.Data.SqlClient;
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

        public async Task LogDb(Kafka message)
        {
            string query = @"INSERT INTO Kafka (Guid, Topic, Message, ConsumerName, Partition)
                VALUES (@Guid, @Topic, @Message, @ConsumerName, @Partition);";


            _connectionString = "Data Source=VID-DT-1051;Database=SoftechWorldWide;Integrated Security=True;Connect Timeout=30;Encrypt=True;Trust Server Certificate=True;Application Intent=ReadWrite;Multi Subnet Failover=False";

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Guid", message.Id);
                    command.Parameters.AddWithValue("@Topic", message.Topic);
                    command.Parameters.AddWithValue("@Message", message.Message);
                    command.Parameters.AddWithValue("@ConsumerName", message.ConsumerName);
                    command.Parameters.AddWithValue("@Partition", message.Partition);

                    await command.ExecuteNonQueryAsync();
                }
            }

        }
    }
}
