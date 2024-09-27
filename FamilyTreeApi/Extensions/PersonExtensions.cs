using FamilyTreeApi.DataAccess;
using FamilyTreeApi.Model;

namespace FamilyTreeApi.Extensions
{
    public static class PersonExtensions
    {
        public static Person ToPerson(this PersonQueryModel model)
        {
            return new Person
            {
                Id = model.Id,
                Name = model.Name,
                Nickname = model.Nickname,
                Gender = model.Gender,
                DOB = model.DOB,
                //Spouses = model.Spouses.ConvertStringToArrayInt(),
                Children = model.Children.ConvertStringToArrayInt(),
                Spouse = model.Spouses?.Length > 0 ? Convert.ToInt32(model.Spouses.Split(';')[0]) : null,
            };
        }

        public static IEnumerable<Person> ToPerson(this IEnumerable<PersonQueryModel> model)
        {
            return model.Select(x => x.ToPerson());
        }

        public static PersonQueryModel ToPersonQueryModel(this Person item)
        {
            return new PersonQueryModel
            {
                Id = item.Id,
                Name = item.Name,
                Nickname = item.Nickname,
                Gender = item.Gender,
                DOB = item.DOB,
                //Spouses = item.Spouse?.ToString(),
                //Children = item.Children.ConvertArrayInttoString(),
            };
        }

        public static int[] ConvertStringToArrayInt(this string? str)
        {
            return !string.IsNullOrEmpty(str?.Trim(';')) ? Array.ConvertAll(str.Trim(';').Split(';'), int.Parse) : [];
        }

        public static string? ConvertArrayInttoString(this int[] arr)
        {
            return arr != null ? string.Join(";", arr) : null;
        }
    }
}