namespace reflection
{
    public class Person
    {
        public string Name {get;set;}
        public Wrapper<Favourites> Favourites { get; set; }
    }

    public class Favourites
    {
        public string Comments { get; set; }
    }

    public static class PersonMocker
    {
        public static Person GetMockPerson()
        {
            return new Person
            {
                Name = "Jane Doe",
                Favourites = new Wrapper<Favourites> 
                {
                    data = new Favourites 
                    {
                        Comments = "Not hungry at all"
                    }
                }
            };
        }
    }
}
