using ServiceContracts;
using Microsoft.Data.SqlClient;
using Confluent.Kafka;

namespace Services
{
    public class RabbitMQServiceDatabase : IRabbitMQServiceDatabase
    {
        public async Task LogConsumerData(ConsumerRabbitMq consumerObj, string connectionString)
        {
            string query = @"INSERT INTO RabbitMQ (Guid, Queue, Message, Exchange, ConsumerName)
                VALUES (@Guid, @Queue, @Message, @Exchange, @ConsumerName);";


            //_connectionString = "Data Source=VID-DT-1051;Database=SoftechWorldWide;Integrated Security=True;Connect Timeout=30;Encrypt=True;Trust Server Certificate=True;Application Intent=ReadWrite;Multi Subnet Failover=False";

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@Guid", consumerObj.Guid);
                        command.Parameters.AddWithValue("@Queue", consumerObj.Queue);
                        command.Parameters.AddWithValue("@Message", consumerObj.Message);
                        command.Parameters.AddWithValue("@Exchange", consumerObj.Exchange);
                        command.Parameters.AddWithValue("@ConsumerName", consumerObj.ConsumerName);

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
