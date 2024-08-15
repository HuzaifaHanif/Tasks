using ServiceContracts;
using Microsoft.Data.SqlClient;

namespace Services
{
    public class AzureBusServiceDatabase : IAzureBusServiceDatabase
    {
        public async Task LogConsumerData(ConsumerAzureBus consumerObj, string connectionString)
        {
            string query = @"INSERT INTO AzureBus (Guid, Topic, Message , ConsumerName)
                VALUES (@Guid, @Topic, @Message , @ConsumerName);";


            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                await connection.OpenAsync();

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Guid", consumerObj.Guid);
                    command.Parameters.AddWithValue("@Topic", consumerObj.Topic);
                    command.Parameters.AddWithValue("@Message", consumerObj.Message);
                    command.Parameters.AddWithValue("@ConsumerName", consumerObj.ConsumerName);

                    await command.ExecuteNonQueryAsync();
                }
            }
        }
    }
}
