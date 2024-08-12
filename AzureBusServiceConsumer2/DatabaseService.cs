

using Microsoft.Data.SqlClient;

namespace AzureBusServiceConsumer2
{
    public class DatabaseService
    {
        private string _connectionString;
        private static DatabaseService _instance;

        private DatabaseService()
        {
            
        }

        public static DatabaseService GetInstance()
        {
            if( _instance == null )    
                return _instance = new DatabaseService();

            return _instance;
        }

        public async Task LogDb(AzureBus message)
        {
            string query = @"INSERT INTO AzureBus (Guid, Topic, Message , ConsumerName)
                VALUES (@Guid, @Topic, @Message , @ConsumerName);";


            _connectionString = "Data Source=VID-DT-1051;Database=SoftechWorldWide;Integrated Security=True;Connect Timeout=30;Encrypt=True;Trust Server Certificate=True;Application Intent=ReadWrite;Multi Subnet Failover=False";

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Guid", message.Guid);
                    command.Parameters.AddWithValue("@Topic", message.Topic);
                    command.Parameters.AddWithValue("@Message", message.Message);
                    command.Parameters.AddWithValue("@ConsumerName", message.ConsumerName);

                    await command.ExecuteNonQueryAsync();
                }
            }

        }


    }

}

