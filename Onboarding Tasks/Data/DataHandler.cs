using Microsoft.Data.SqlClient;
using Newtonsoft.Json;
using System.Data;
using Tasks.Models.VidizmoContract;
using Vidizmo.Contract.DataModels.Permissions;
using Vidizmo.Contract.MashupManagement;

namespace Tasks.Data
{
    public class DataHandler
    {
        public async static Task<List<MashupInfo>> GetMashupInfo(string connectionstring , RequestMashupInfo filterObj)
        {
            //List<MashupInfo> mashupsInfo = new List<MashupInfo>();
            var info = "";
            var content = "";

            await using(SqlConnection connection = new SqlConnection(connectionstring))
            {
                connection.Open();

                await using (SqlCommand command = new SqlCommand("MashupInfo", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    if (!String.IsNullOrEmpty(filterObj.Culture))
                    {
                        command.Parameters.AddWithValue("@Culture", filterObj.Culture);
                    }

                    if (!String.IsNullOrEmpty(filterObj.Title))
                    {
                        command.Parameters.AddWithValue("@Title", filterObj.Title);
                    }

                    if (!String.IsNullOrEmpty(filterObj.Description))
                    {
                        command.Parameters.AddWithValue("@Description", filterObj.Description);
                    }

                    if (!String.IsNullOrEmpty(filterObj.Tags))
                    {
                        command.Parameters.AddWithValue("@Tag", filterObj.Tags);
                    }

                    if (!String.IsNullOrEmpty(filterObj.Category))
                    {
                        command.Parameters.AddWithValue("@Category", filterObj.Category);
                    }

                    if (filterObj.UserId != 0 && filterObj.UserId != null) 
                    {
                        command.Parameters.AddWithValue("@UserId", filterObj.UserId);
                    }

                    if (filterObj.MashupId != 0 && filterObj.MashupId != null) 
                    {
                        command.Parameters.AddWithValue("@MashupId", filterObj.MashupId);
                    }

                    if (filterObj.TenantId != 0 && filterObj.TenantId != null) 
                    {
                        command.Parameters.AddWithValue("@TenantId", filterObj.TenantId);
                    }

                    if(filterObj.PublishedDate.HasValue)
                    {
                        command.Parameters.AddWithValue("@PublishedDate", filterObj.PublishedDate);
                    }


                    command.Parameters.AddWithValue("@IsAIProcessed", (object)filterObj.IsAIProcessed ?? DBNull.Value);
                    command.Parameters.AddWithValue("@isTranscode", (object)filterObj.IsTranscoded ?? DBNull.Value);

                    await using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            info += reader.GetString(0);
                        }
                    }

                }

            }

           
            var mashupsInfo = JsonConvert.DeserializeObject<List<MashupInfo>>(info);

            return mashupsInfo;
        }

    }
}
