using Microsoft.Data.SqlClient;
using Newtonsoft.Json;
using System.Data;
using Tasks.Models.VidizmoContract;

namespace Tasks.Data
{
    public class DataHandler
    {

        public async static Task<List<MashupInfo>> GetMashupInfo(string connectionstring)
        {
            List<MashupInfo> mashupsInfo = new List<MashupInfo>();
            var info = "";

            await using(SqlConnection connection = new SqlConnection(connectionstring))
            {
                connection.Open();

                await using (SqlCommand command = new SqlCommand("MashupInfo", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    await using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            info += reader.GetString(0);

                        }
                    }

                }

            }

            mashupsInfo = JsonConvert.DeserializeObject<List<MashupInfo>>(info);

            return mashupsInfo;
        }

    }
}
