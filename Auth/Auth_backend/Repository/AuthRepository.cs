using FLiu__Auth.Models;
using FLiu__Auth.Models.Dto;
using FLiu__Auth.Models.Dto.NewFolder;
using System.Data.SqlClient;
using System.Security.Cryptography;
using System.Text;
using System.Transactions;

namespace FLiu__Auth.Repository
{
    public class AuthRepository : IAuthRepositoy
    {

        private readonly string _connectionString;

        public AuthRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection")!;
        }

        private async Task<SqlConnection> GetOpenConnectionAsync()
        {
            var connection = new SqlConnection(_connectionString);
            await connection.OpenAsync();
            return connection;
        }



        public async Task<RepoDto> login(Credentials cd)
        {
            try
            {
                // Ensure proper resource disposal using 'using' statements
                using (var conn = await GetOpenConnectionAsync())
                {
                    // Begin transaction
                    using (var transaction = conn.BeginTransaction())
                    {
                        try
                        {
                            // Query to select the user based on the provided email
                            string query = "SELECT Email, [PasswordSalt], [PasswordHash] FROM [UserCredentials] WHERE Email = @Email";

                            using (SqlCommand command = new SqlCommand(query, conn))
                            {
                                command.Parameters.AddWithValue("@Email", cd.Email);
                                command.Transaction = transaction;

                                // Execute the query and read the results
                                using (SqlDataReader reader = await command.ExecuteReaderAsync())
                                {
                                    // Check if a user with the provided email exists
                                    if (await reader.ReadAsync())
                                    {
                                        // Retrieve user's password salt and hashed password from the database
                                        byte[] passwordSalt = (byte[])reader["PasswordSalt"];
                                        byte[] passwordHash = (byte[])reader["PasswordHash"];

                                        // Hash the provided password using the retrieved salt
                                        byte[] hashedPassword = null;

                                        using (HMACSHA512? hmac = new HMACSHA512(passwordSalt))
                                        {
                                            hashedPassword = hmac.ComputeHash(Encoding.UTF8.GetBytes(cd.Password));
                                        }

                                        // Compare the hashed passwords
                                        if (hashedPassword.SequenceEqual(passwordHash))
                                        {
                                            // Passwords match, login successful
                                            // Update IsActive status to true in the Users table
                                            reader.Close();
                                            string updateQuery = "UPDATE Users SET IsActive = 1 WHERE Email = @Email";
                                            using (SqlCommand updateCommand = new SqlCommand(updateQuery, conn))
                                            {
                                                updateCommand.Parameters.AddWithValue("@Email", cd.Email);
                                                updateCommand.Transaction = transaction;
                                                await updateCommand.ExecuteNonQueryAsync();
                                            }

                                            // Commit transaction
                                            transaction.Commit();

                                            var temp = await getidentity(cd.Email);
                                            
                                            return new RepoDto
                                            {
                                                StatusCode = "200",
                                                Message = "Login successful",
                                                User = new User
                                                {
                                                    Email = cd.Email
                                                },
                                                Identity = new Identity
                                                {
                                                    Email = temp.Email,
                                                    FirstName = temp.FirstName,
                                                    LastName = temp.LastName,
                                                    UserName = temp.UserName

                                                }
                                                // You may include additional user information here if needed
                                            };
                                        }
                                        else
                                        {
                                            // Passwords don't match, login failed
                                            return new RepoDto
                                            {
                                                StatusCode = "203",
                                                Message = "Invalid password"
                                            };
                                        }
                                    }
                                    else
                                    {
                                        // No user found with the provided email
                                        return new RepoDto
                                        {
                                            StatusCode = "203",
                                            Message = "Email not found"
                                        };
                                    }
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            // Rollback transaction in case of exception
                            transaction.Rollback();
                            return new RepoDto
                            {
                                StatusCode = "401",
                                Message = "Email not found"
                            };
                        }
                    }
                }
            }
            finally
            {

            }
            
        }


        public async Task<Identity> getidentity(string email)
        {
            string query = "SELECT * FROM Users WHERE Email = @Email";
            using(var conn = await GetOpenConnectionAsync()) 
            using (SqlCommand command = new SqlCommand(query, conn))
            {
                // Add parameter for email
                command.Parameters.AddWithValue("@Email", email);

                // Assign transaction if needed


                // Execute the query asynchronously
                using (var reader = await command.ExecuteReaderAsync())
                {
                    if (reader.HasRows && await reader.ReadAsync())
                    {
                        return new Identity
                        {
                            FirstName = reader.GetString(reader.GetOrdinal("FirstName")),
                            LastName = reader.GetString(reader.GetOrdinal("LastName")),
                            Email = reader.GetString(reader.GetOrdinal("Email")),
                            UserName = reader.GetString(reader.GetOrdinal("UserName"))
                        };
                    }
                    else
                    {
                        return null;
                    }
                }
            }

        }


        public async Task<RepoDto> logout(string email)
        {
            try
            {
                using (var conn = await GetOpenConnectionAsync())
                {
                    // Begin a new SQL transaction
                    using (var transaction = conn.BeginTransaction())
                    {
                        string updateQuery = "UPDATE Users SET IsActive = 0 WHERE Email = @Email";
                        using (SqlCommand updateCommand = new SqlCommand(updateQuery, conn))
                        {
                            // Assign the transaction to the command
                            updateCommand.Transaction = transaction;

                            updateCommand.Parameters.AddWithValue("@Email", email);
                            await updateCommand.ExecuteNonQueryAsync();

                            // Commit the transaction if no exceptions occurred
                            transaction.Commit();
                        }
                    }

                    return new RepoDto
                    {
                        StatusCode = "200",
                        Message = "Successfully Logged Out"
                    };
                }
            }
            catch (Exception ex)
            {
                // Handle any exceptions that occur during the transaction
                // Rollback the transaction to ensure data consistency
                // Log the exception or take appropriate actions
                Console.WriteLine($"An error occurred during logout: {ex.Message}");

                return new RepoDto
                {
                    StatusCode = "500",
                    Message = "An error occurred during logout"
                };
            }
        }





        public async Task<RepoDto> register(Register rg)
        {
            try
            {
                using (var conn = await GetOpenConnectionAsync())
                {
                    // Begin a new SQL transaction
                    using (var transaction = conn.BeginTransaction())
                    {
                        // Queries for checking existing user and inserting user details
                        string selectQuery = "SELECT COUNT(*) FROM [chitchat].[dbo].[Users] WHERE [Email] = @Email OR [UserName] = @UserName";
                        string insertUserQuery = "INSERT INTO [chitchat].[dbo].[Users] ([FirstName], [LastName], [UserName], [Email], [IsActive]) VALUES (@FirstName, @LastName, @UserName, @Email, 0);  SELECT SCOPE_IDENTITY();";
                        string insertPassQuery = "INSERT INTO [chitchat].[dbo].[UserCredentials] ([UserId], [Email], [PasswordSalt], [PasswordHash]) VALUES (@UserId, @Email, @PasswordSalt, @PasswordHash)";

                        // Check if the user already exists
                        using (SqlCommand selectCommand = new SqlCommand(selectQuery, conn, transaction))
                        {
                            selectCommand.Parameters.AddWithValue("@Email", rg.Email);
                            selectCommand.Parameters.AddWithValue("@UserName", rg.UserName);
                            int existingUsersCount = (int)await selectCommand.ExecuteScalarAsync();

                            if (existingUsersCount > 0)
                            {
                                // User already exists, rollback the transaction and return an error response
                                transaction.Rollback();
                                return new RepoDto
                                {
                                    Message = "UserName or Email Already In Use",
                                    StatusCode = "401"
                                };
                            }
                        }

                        // Insert user details
                        int userId;
                        using (SqlCommand insertUserCommand = new SqlCommand(insertUserQuery, conn, transaction))
                        {
                            insertUserCommand.Parameters.AddWithValue("@FirstName", rg.FirstName);
                            insertUserCommand.Parameters.AddWithValue("@LastName", rg.LastName);
                            insertUserCommand.Parameters.AddWithValue("@UserName", rg.UserName);
                            insertUserCommand.Parameters.AddWithValue("@Email", rg.Email);
                            userId = Convert.ToInt32(await insertUserCommand.ExecuteScalarAsync());
                        }

                        // Insert user credentials
                        using (HMACSHA512 hmac = new HMACSHA512())
                        {
                            byte[] passwordSalt = hmac.Key;
                            byte[] passwordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(rg.Password));

                            using (SqlCommand insertPassCommand = new SqlCommand(insertPassQuery, conn, transaction))
                            {
                                insertPassCommand.Parameters.AddWithValue("@Email", rg.Email);
                                insertPassCommand.Parameters.AddWithValue("@PasswordSalt", passwordSalt);
                                insertPassCommand.Parameters.AddWithValue("@PasswordHash", passwordHash);
                                insertPassCommand.Parameters.AddWithValue("@UserId", userId);
                                await insertPassCommand.ExecuteNonQueryAsync();
                            }
                        }

                        // Commit the transaction if all operations succeed
                        transaction.Commit();

                        return new RepoDto
                        {
                            Message = "Successfully Registered",
                            StatusCode = "200"
                        };
                    }
                }
            }
            catch (Exception ex)
            {
                // Log the exception or handle it appropriately
                return new RepoDto
                {
                    Message = $"An error occurred during registration: {ex.Message}",
                    StatusCode = "400"
                };
            }
        }



    }
}


        





