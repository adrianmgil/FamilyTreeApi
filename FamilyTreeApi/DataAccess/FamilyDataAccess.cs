using Dapper;
using Microsoft.Data.SqlClient;

namespace FamilyTreeApi.DataAccess
{
    public class PersonQueryModel
    {
        public int Id { get; set; }
        public string Nickname { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string Gender { get; set; } = string.Empty;
        public string DOB { get; set; } = string.Empty;
        public string? Spouses { get; set; } = string.Empty;
        public string? Children { get; set; } = string.Empty;
    }

    public interface IFamilyDataAccess
    {
        Task<IEnumerable<PersonQueryModel>> GetAll();
        Task<int> Update(int personId, PersonQueryModel person);
        Task<int> CreatePerson(PersonQueryModel person);
        Task<string> GetChildren(int personId);
        Task<PersonQueryModel> GetPerson(int personId);
        Task DeletePerson(int personId);
        Task<List<PersonQueryModel>> FindParent(int personId);
        Task<int> UpdateChildren(int personId, string children);
        Task<int?> FindSpouse(int spouseId);
        Task<int> UpdateSpouse(int personId, string spouses);
    }

    public class FamilyDataAccess : IFamilyDataAccess
    {
        private readonly SqlConnection connection;

        public FamilyDataAccess(SqlConnection connection)
        {
            this.connection = connection;
        }

        public async Task<IEnumerable<PersonQueryModel>> GetAll()
        {
            const string sql = "SELECT Id, Nickname, Name, Gender, DOB, Spouses, Children FROM Persons";
            return await connection.QueryAsync<PersonQueryModel>(sql);
        }

        public async Task<int> Update(int personId, PersonQueryModel person)
        {
            const string sql = "UPDATE Persons SET Nickname = @Nickname, Name = @Name, Gender = @Gender, DOB = @DOB WHERE Id = @personId";
            return await connection.ExecuteAsync(sql, new { personId, person.Nickname, person.Name, person.Gender, person.DOB });
        }

        public async Task<int> CreatePerson(PersonQueryModel person)
        {
            const string sql = @"
                INSERT INTO Persons (Nickname, Name, Gender, DOB)
                    VALUES (@Nickname, @Name, @Gender, @DOB);
                SELECT CAST(SCOPE_IDENTITY() AS INT)";

            return await connection.QueryFirstAsync<int>(sql, new { person.Nickname, person.Name, person.Gender, person.DOB });
        }

        public async Task<string> GetChildren(int personId)
        {
            const string sql = "SELECT Children FROM Persons WHERE Id = @personId";
            return await connection.QuerySingleAsync<string>(sql, new { personId });
        }

        public async Task<PersonQueryModel> GetPerson(int personId)
        {
            const string sql = "SELECT Id, Nickname, Name, Gender, DOB, Spouses, Children FROM Persons WHERE Id = @personId";
            return await connection.QuerySingleAsync<PersonQueryModel>(sql, new { personId });
        }

        public async Task DeletePerson(int personId)
        {
            const string sql = "DELETE FROM Persons WHERE Id = @personId";
            await connection.ExecuteAsync(sql, new { personId });
        }

        public async Task<List<PersonQueryModel>> FindParent(int personId)
        {
            const string sql = "SELECT Id, Children FROM Persons WHERE Children LIKE @personId";
            return (await connection.QueryAsync<PersonQueryModel>(sql, new { personId = $"%;{personId};%" })).ToList();
        }

        public async Task<int> UpdateChildren(int personId, string children)
        {
            const string sql = "UPDATE Persons SET Children = @children WHERE Id = @personId";
            return await connection.ExecuteAsync(sql, new { personId, children });
        }

        public async Task<int?> FindSpouse(int spouseId)
        {
            const string sql = "SELECT Id FROM Persons WHERE Spouses = @spouses";
            return (await connection.QueryAsync<int>(sql, new { spouses = spouseId.ToString() })).FirstOrDefault();
        }

        public async Task<int> UpdateSpouse(int personId, string spouses)
        {
            const string sql = "UPDATE Persons SET Spouses = @spouses WHERE Id = @personId";
            return await connection.ExecuteAsync(sql, new { personId, spouses });
        }
    }
}
