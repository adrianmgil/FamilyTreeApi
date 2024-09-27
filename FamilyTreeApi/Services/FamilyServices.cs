using FamilyTreeApi.DataAccess;
using FamilyTreeApi.Extensions;
using FamilyTreeApi.Model;

namespace FamilyTreeApi.Services
{
    public interface IFamilyServices
    {
        Task<IEnumerable<Person>> GetAll();
        Task<Person> GetPerson(int Id);
        Task<int> Update(int personId, Person person);
        Task<int> CreatePerson(Person person);
        Task<int> CreateChild(int parentId, Person person);
        Task<int> CreateParent(int childId, Person person);
        Task DeletePerson(int personId);
        Task CreateSpouse(int personId, Person person);
    }

    public class FamilyServices : IFamilyServices
    {
        private readonly IFamilyDataAccess familyDataAccess;

        public FamilyServices(IFamilyDataAccess familyDataAccess)
        {
            this.familyDataAccess = familyDataAccess;
        }

        public async Task<IEnumerable<Person>> GetAll()
        {
            return (await familyDataAccess.GetAll()).ToPerson();
        }

        public async Task<Person> GetPerson(int Id)
        {
            return (await familyDataAccess.GetPerson(Id)).ToPerson();
        }

        public async Task<int> Update(int personId, Person person)
        {
            return await familyDataAccess.Update(personId, person.ToPersonQueryModel());
        }

        public async Task<int> CreateChild(int parentId, Person person)
        {
            int childId = 0;
            var parent = await familyDataAccess.GetPerson(parentId);
            if (parent != null) {
                childId = await CreatePerson(person);
                if (childId > 0) {
                    await UpdateParentChildren(parent, childId);
                }
            }
            var otherParent = person.OtherParentId.HasValue ? await familyDataAccess.GetPerson(person.OtherParentId.Value) : null;
            if (otherParent != null) {
                await UpdateParentChildren(otherParent, childId);
            }

            return childId;
        }

        private async Task UpdateParentChildren(PersonQueryModel parent, int childId)
        {
            parent.Children = string.IsNullOrEmpty(parent.Children) ? $";{childId};" : $"{parent.Children}{childId};";
            await familyDataAccess.UpdateChildren(parent.Id, parent.Children);
        }

        public async Task<int> CreateParent(int childId, Person person)
        {
            var child = await familyDataAccess.GetPerson(childId);
            if (child != null)
            {
                person.Children = new int[] { childId };
                return await CreatePerson(person);
            }
            return 0;
        }

        public async Task<int> CreatePerson(Person person)
        {
            return await familyDataAccess.CreatePerson(person.ToPersonQueryModel());
        }

        public async Task DeletePerson(int personId)
        {
            var parent = await familyDataAccess.FindParent(personId);
            parent.ForEach(p =>
            {
                if (p.Children != null)
                {
                    familyDataAccess.UpdateChildren(p.Id, p.Children.Replace($";{personId};", ";"));
                }
            });
            var spouseId = await familyDataAccess.FindSpouse(personId);
            if (spouseId > 0)
            {
                await familyDataAccess.UpdateSpouse(spouseId.Value, "");
            }
            await familyDataAccess.DeletePerson(personId);
        }

        public async Task CreateSpouse(int personId, Person person)
        {
            var spouseId = await familyDataAccess.CreatePerson(person.ToPersonQueryModel());
            if (spouseId > 0)
            {
                await familyDataAccess.UpdateSpouse(personId, spouseId.ToString());
            }
        }
    }
}