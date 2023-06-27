using Dapper;
using Infrastructure.DataModels;
using Npgsql;

namespace Infrastructure.Repositories;

public class UserRepository
{
    private readonly NpgsqlDataSource _dataSource;

    public UserRepository(NpgsqlDataSource dataSource)
    {
        _dataSource = dataSource;
    }


    public bool IsThisEmailTaken(string email)
    {
        var query = "SELECT COUNT(*) FROM library.end_users WHERE library.end_users.email = @email;";

        using (var conn = _dataSource.OpenConnection())
        {
            return conn.ExecuteScalar<int>(query, new { email }) == 1;
        }
    }

    public EndUser GetUserByEmailOrNull(string email)
    {
        var query = @"
SELECT email, status, end_user_id as enduserid, password_hash as passwordhash, salt, role, pravatar_id as profileimgurl
FROM library.end_users WHERE library.end_users.email = @email;";

        using (var conn = _dataSource.OpenConnection())
        {
            return conn.QueryFirstOrDefault<EndUser>(query, new { email });
        }
    }

    public EndUser CreateAndReturnUser(string email, string salt, string hash, string role)
    {
        var sql = @"
INSERT INTO library.end_users (email, password_hash, salt, role, status) 
VALUES (@email, @hash, @salt, @role, 'active') 
RETURNING email, password_hash as passwordhash, salt, role, status, end_user_id as enduserid;";
        using (var conn = _dataSource.OpenConnection())
        {
            return conn.QueryFirst<EndUser>(sql, new { email, hash, salt, role });
        }
    }

    public EndUser GetUserById(int userId)
    {
        var sql = @"
SELECT email, status, end_user_id as enduserid, password_hash as passwordhash, salt, role, pravatar_id as profileimgurl
FROM library.end_users WHERE library.end_users.end_user_id = @userId;";


        using (var conn = _dataSource.OpenConnection())
        {
            return conn.QueryFirst<EndUser>(sql, new { userId });
        }
    }


    public bool DeleteUserById(int userId)
    {
        var sql = "DELETE FROM library.end_users WHERE library.end_users.end_user_id = @userId;";

        using (var conn = _dataSource.OpenConnection())
        {
            return conn.Execute(sql, new { userId }) == 1;
        }
    }
    
    public bool UpdateUsersAvatar(int userId, int pravatarId)
    {
        var sql = @"UPDATE library.end_users SET pravatar_id = @pravatarId WHERE library.end_users.end_user_id = @userId;";
        using (var conn = _dataSource.OpenConnection())
        {
            return conn.Execute(sql, new { pravatarId, userId }) == 1;
        }
    }
}