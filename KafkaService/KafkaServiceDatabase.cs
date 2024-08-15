using ServiceContracts;
using Microsoft.Data.SqlClient;
using Confluent.Kafka;

namespace Services
{
    public class KafkaServiceDatabase : IKafkaServiceDatabase
    {
        public async Task LogConsumerData(ConsumerKafka consumerObj, string connectionString)
        {
            string query = @"INSERT INTO Kafka (Guid, Topic, Message, ConsumerName, Partition)
                VALUES (@Guid, @Topic, @Message, @ConsumerName, @Partition);";


            //_connectionString = "Data Source=VID-DT-1051;Database=SoftechWorldWide;Integrated Security=True;Connect Timeout=30;Encrypt=True;Trust Server Certificate=True;Application Intent=ReadWrite;Multi Subnet Failover=False";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                await connection.OpenAsync();

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Guid", consumerObj.Id);
                    command.Parameters.AddWithValue("@Topic", consumerObj.Topic);
                    command.Parameters.AddWithValue("@Message", consumerObj.Message);
                    command.Parameters.AddWithValue("@ConsumerName", consumerObj.ConsumerName);
                    command.Parameters.AddWithValue("@Partition", consumerObj.Partition);

                    await command.ExecuteNonQueryAsync();
                }
            }
        }
    }
}
