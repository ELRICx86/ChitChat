using FLiu__Auth.Models.Dto.NewFolder;
using FLiu__Auth.Models;
using FLiu__Auth.Models.DTO_Message;
using Microsoft.Extensions.Configuration;
using System.Data.SqlClient;

namespace FLiu__Auth.Repository
{
    public class PrivateMessagesRepo : IPrivateMessagesRepo
    {

        private readonly string _connectionString;
        public PrivateMessagesRepo(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection")!;
        }

        private async Task<SqlConnection> GetOpenConnectionAsync()
        {
            var connection = new SqlConnection(_connectionString);
            await connection.OpenAsync();
            return connection;
        }



        public async Task<List<Connections>> GetConnection(Connections conn)
        {
            List<Connections> connections = new List<Connections>();
            List<int> ls = new List<int>();

            using (var connection = await GetOpenConnectionAsync())
            {
                string selectQuery = "SELECT COUNT(*) FROM CONN_ID WHERE [UserId] = @Id";
                SqlCommand selectCommand = new SqlCommand(selectQuery, connection);
                selectCommand.Parameters.AddWithValue("@Id", conn.UserId);

                // ExecuteScalarAsync returns the first column of the first row as an object
                var count = await selectCommand.ExecuteScalarAsync();

                if (Convert.ToInt32(count) > 0)
                {
                    // Data exists, perform update
                    string updateQuery = "UPDATE CONN_ID SET connId = @Data WHERE [UserId] = @Id";
                    SqlCommand updateCommand = new SqlCommand(updateQuery, connection);
                    updateCommand.Parameters.AddWithValue("@Data", conn.ConnectionId);
                    updateCommand.Parameters.AddWithValue("@Id", conn.UserId);
                    updateCommand.ExecuteNonQuery();
                }
                else
                {
                    // Data doesn't exist, perform insert
                    string insertQuery = "INSERT INTO CONN_ID ([UserId], connId) VALUES (@Id, @Data)";
                    SqlCommand insertCommand = new SqlCommand(insertQuery, connection);
                    insertCommand.Parameters.AddWithValue("@Id", conn.UserId);
                    insertCommand.Parameters.AddWithValue("@Data", conn.ConnectionId);
                    insertCommand.ExecuteNonQuery();
                }

                string query = "SELECT * FROM GetAllFriends(@UserId)";

                

                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@UserId", conn.UserId);

                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        while (reader.Read())
                        {
                            ls.Add(Convert.ToInt32(reader["UserId"]));
                        }
                    }
                }

                string select;
                if (ls.Count > 0)
                {
                    select = "SELECT UserId, connId FROM CONN_ID WHERE UserId IN (" + string.Join(",", ls) + ")";
                }
                else
                {
                    // Handle the case where ls is empty (no elements)
                    // For example, return an empty result set
                    select = "SELECT UserId, connId FROM CONN_ID WHERE 1 = 0"; // This query will return no rows
                }

                using (var command = new SqlCommand(select, connection))
                {

                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        while (reader.Read())
                        {
                            Connections temp = new Connections
                            {
                                UserId = Convert.ToInt32(reader["UserId"]),
                                ConnectionId = Convert.ToString(reader["connId"])
                            };
                            connections.Add(temp);
                        }
                    }
                }



            }
            return connections;
        }
        
        

    
    }
}
