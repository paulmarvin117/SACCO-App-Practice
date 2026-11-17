using SACCO.Models;
using SACCO.Models;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace SACCO.Services
{
    public static class StaffRepository
    {
        // Change Server/Password to match your machine
        private const string ConnectionString =
    "Server=localhost,1433;Database=SaccoDB;User Id=sacco_app;Password=Sacco@12345;TrustServerCertificate=True;";
      
        public static async Task<Staff> AuthenticateAsync(string email, string password)
        {
            const string sql = @"
                SELECT StaffId, FullName, Email, Role
                FROM   Staff
                WHERE  Email = @Email
                  AND  PasswordHash = HASHBYTES('SHA2_256', @Password)
                  AND  IsActive = 1";

            using (var connection = new SqlConnection(ConnectionString))
            using (var command = new SqlCommand(sql, connection))
            {
                command.Parameters.Add("@Email", SqlDbType.NVarChar, 150).Value = email;
                command.Parameters.Add("@Password", SqlDbType.NVarChar, 100).Value = password;

                await connection.OpenAsync();

                using (var reader = await command.ExecuteReaderAsync())
                {
                    if (await reader.ReadAsync())
                    {
                        return new Staff
                        {
                            StaffId = reader.GetInt32(0),
                            FullName = reader.GetString(1),
                            Email = reader.GetString(2),
                            Role = reader.GetString(3)
                        };
                    }
                }
            }
            return null;   // no match = wrong email or password
        }
    }
}