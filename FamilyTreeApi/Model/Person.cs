namespace FamilyTreeApi.Model
{
    public class Person
    {
        public int Id { get; set; }
        public string Nickname { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string Gender { get; set; } = string.Empty;
        public string DOB { get; set; } = string.Empty;
        //public int[] Spouses { get; set; } = new int[0];
        public int? Spouse { get; set; }
        public int[] Children { get; set; } = new int[0];
        public int? OtherParentId { get; set; }
    }
}
