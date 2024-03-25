using FLiu__Auth.Models;
using FLiu__Auth.Models.Dto.NewFolder;
using System.Data.SqlClient;

namespace FLiu__Auth.Repository
{
    public class FriendShipRepo : IFriendShipRepo
    {
        private readonly string _connectionString;

        public FriendShipRepo(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection")!;
        }

        private async Task<SqlConnection> GetOpenConnectionAsync()
        {
            var connection = new SqlConnection(_connectionString);
            await connection.OpenAsync();
            return connection;
        }

        public enum FriendshipStatus
        {
            Pending,
            Accepted,
            Rejected,
            None // Represents no friendship request
        }

        public async Task<(bool Exists, FriendshipStatus Status)> FriendshipExists(SqlConnection connection, SqlTransaction transaction, int id1, int id2)
        {
            // Ensure that the command uses the existing transaction
            string query = "SELECT Status FROM Friends WHERE (UserID1 = @UserID1 AND UserID2 = @UserID2) OR (UserID1 = @UserID2 AND UserID2 = @UserID1)";
            var command = new SqlCommand(query, connection, transaction);
            command.Parameters.AddWithValue("@UserID1", id1);
            command.Parameters.AddWithValue("@UserID2", id2);

            var status = await command.ExecuteScalarAsync();

            if (status != null && Enum.TryParse(status.ToString(), out FriendshipStatus friendshipStatus))
            {
                return (true, friendshipStatus);
            }
            else
            {
                return (false, FriendshipStatus.None);
            }
        }

        public async Task<FriendShip> Add(int id1, int id2)
        {
            FriendShip friendship = new FriendShip();
            using (var connection = await GetOpenConnectionAsync())
            {
                using (var transaction = connection.BeginTransaction())
                {
                    var (exists, status) = await FriendshipExists(connection, transaction, id1, id2);

                    if (exists)
                    {
                        // Friendship request already exists
                        if (status == FriendshipStatus.Pending)
                        {
                            friendship.Message = "Friendship request is pending.";

                        }
                        else if (status == FriendshipStatus.Accepted)
                        {
                            friendship.Message = "Friendship request is already accepted.";

                        }
                        else if (status == FriendshipStatus.Rejected)
                        {
                            friendship.Message = "Friendship request is pending.";

                        }
                    }
                    else
                    {

                        // Proceed with inserting the new friendship record
                        string query = @"INSERT INTO Friends (UserID1, UserID2, Status, ActionUserID, CreatedAt, UpdatedAt)
                                        VALUES (@UserID1, @UserID2, @Status, @ActionUserID, GETDATE(), GETDATE());
                                        SELECT SCOPE_IDENTITY();"; // Retrieve the ID of the inserted row

                        // Create SqlCommand object with the query and connection
                        var command = new SqlCommand(query, connection);
                        // Set parameters for the query
                        command.Parameters.AddWithValue("@UserID1", id1);
                        command.Parameters.AddWithValue("@UserID2", id2);
                        command.Parameters.AddWithValue("@ActionUserID", id1);
                        command.Parameters.AddWithValue("@Status", "Pending"); // Assuming default status is "Pending"

                        command.Transaction = transaction;
                        // Execute the command and retrieve the generated identity (FriendshipID)
                        int friendshipId = Convert.ToInt32(await command.ExecuteScalarAsync());

                        // Commit the transaction
                        transaction.Commit();

                        // Create and return the FriendShip object with the inserted data

                        friendship.Type = true;
                        friendship.FriendShipID = friendshipId;
                        friendship.Message = "Request sent successfully";
                        friendship.UserID1 = id1;
                        friendship.UserID2 = id2;
                        friendship.ActionUserID = id1;
                        friendship.Status = "Pending"; // Assuming default status is "Pending"
                        friendship.CreatedAt = DateTime.Now;
                        friendship.UpdatedAt = DateTime.Now;

                    }
                }
                return friendship;
            }
        }

        public async Task<FriendShip> Delete(int id1, int id2)
        {
            using (var connection = await GetOpenConnectionAsync())
            {

                string query = "DELETE FROM Friends OUTPUT DELETED.* WHERE UserId1 = @UserId1 AND UserId2 = @UserId2";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@UserId1", id1);
                    command.Parameters.AddWithValue("@UserId2", id2);

                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        if (reader.HasRows)
                        {
                            FriendShip friendShip = new FriendShip();
                            while (reader.Read())
                            {
                                friendShip.Type = true;
                                friendShip.Message = "Successfully Deleted";
                                friendShip.FriendShipID = Convert.ToInt32(reader["FriendshipID"]);
                                friendShip.UserID1 = Convert.ToInt32(reader["UserID1"]);
                                friendShip.UserID2 = Convert.ToInt32(reader["UserID2"]);
                                friendShip.Status = reader["Status"].ToString();
                                friendShip.ActionUserID = id1;
                                friendShip.CreatedAt = Convert.ToDateTime(reader["CreatedAt"]);
                                // Assuming UpdatedAt is also DateTime column
                                friendShip.UpdatedAt = (DateTime)reader["UpdatedAt"]; // Use the value from the database
                            }

                            return friendShip;
                        }
                        else
                        {

                            return new FriendShip { Type = false, Message = "Data Dont exist" };
                        }
                    }
                }
            }
        }

        public async Task<IEnumerable<FriendShipDetails>> GetAllFriends(int userId)
        {
            List<FriendShipDetails> friendships = new List<FriendShipDetails>();

            using (var connection = await GetOpenConnectionAsync())
            {
                string query = "SELECT * FROM GetAllFriends(@UserId)";

                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@UserId", userId);

                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        while (reader.Read())
                        {
                            FriendShipDetails friendShip = new FriendShipDetails
                            {
                                FriendshipID = Convert.ToInt32(reader["FriendshipID"]),
                                UserID = Convert.ToInt32(reader["UserID"]),
                                FirstName = reader["FirstName"].ToString(),
                                LastName = reader["LastName"].ToString(),
                                Status = reader["Status"].ToString(),
                                Email = reader["Email"].ToString(),
                                Since = Convert.ToDateTime(reader["UpdatedAt"])
                            };

                            friendships.Add(friendShip);
                        }
                    }
                }
            }

            return friendships;
        }



        public async Task<FriendShip> GetFriendById(int userId)
        {
            FriendShip friendShip = new FriendShip();

            using (var connection = await GetOpenConnectionAsync())
            {
                string query = "SELECT * FROM Friends WHERE UserID2 = @UserId2";

                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@UserId2", userId);

                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        if (await reader.ReadAsync())
                        {
                            return new FriendShip
                            {
                                Type = true,
                                FriendShipID = Convert.ToInt32(reader["FriendshipID"]),
                                UserID1 = Convert.ToInt32(reader["UserID1"]),
                                UserID2 = Convert.ToInt32(reader["UserID2"]),
                                Status = reader["Status"].ToString(),
                                ActionUserID = Convert.ToInt32(reader["ActionUserId"]),
                                CreatedAt = Convert.ToDateTime(reader["CreatedAt"]),
                                UpdatedAt = Convert.ToDateTime(reader["UpdatedAt"])
                            };
                        }
                    }
                }
            }

            return friendShip;
        }

        public async Task<IEnumerable<FriendShipDetails>> GetRequest(int id)
        {
            List<FriendShipDetails> friendships = new List<FriendShipDetails>();

            using (var connection = await GetOpenConnectionAsync())
            {
                string query = @"select * from GetAllFriendShipDetails(@UserID)";

                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@UserId", id);

                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        while (reader.Read())
                        {
                            FriendShipDetails friendShip = new FriendShipDetails
                            {
                                FriendshipID = Convert.ToInt32(reader["FriendshipID"]),
                                UserID = Convert.ToInt32(reader["UserID"]),
                                FirstName = reader["FirstName"].ToString(),
                                LastName = reader["LastName"].ToString(),
                                Status = reader["Status"].ToString(),
                                Email = reader["Email"].ToString(),
                                Since = Convert.ToDateTime(reader["CreatedAt"])
                            };

                            friendships.Add(friendShip);
                        }
                    }
                }
            }

            return friendships;

        }

        public async Task<FriendShip> Response(int user, int friend, string action)
        {
            using (var conn = await GetOpenConnectionAsync())
            {
                string query;
                if (action.ToLower() == "accept")
                {
                    query = "UPDATE Friends SET Status = 'Accepted' OUTPUT inserted.* WHERE UserID2 = @user AND UserID1 = @friend";
                }
                else
                {
                    query = "UPDATE Friends SET Status = 'Rejected' OUTPUT inserted.* WHERE UserID2 = @user AND UserID1 = @friend";
                }

                using (var command = new SqlCommand(query, conn))
                {
                    command.Parameters.AddWithValue("@user", user);
                    command.Parameters.AddWithValue("@friend", friend);

                    // Execute the SQL command asynchronously and get the updated row
                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        // Check if any rows were returned
                        if (await reader.ReadAsync())
                        {
                            // If a row was returned, construct a FriendShip object with details
                            return new FriendShip
                            {
                                Type = true,
                                FriendShipID = Convert.ToInt32(reader["FriendshipID"]),
                                UserID1 = Convert.ToInt32(reader["UserID1"]),
                                UserID2 = Convert.ToInt32(reader["UserID2"]),
                                Status = reader["Status"].ToString(),
                                ActionUserID = Convert.ToInt32(reader["ActionUserId"]),
                                CreatedAt = Convert.ToDateTime(reader["CreatedAt"]),
                                UpdatedAt = Convert.ToDateTime(reader["UpdatedAt"])
                            };
                        }
                    }
                }
            }

            // If no rows were affected or an error occurred, return null
            return null;
        }

    }
}
