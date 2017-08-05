namespace reflection
{
    public class Person
    {
        public string Name {get;set;}
        public Wrapper<Favourites> Favourites { get; set; }

        public static Person Mock()
        {
            return new Person
            {
                Name = "Jane Doe",
                Favourites = new Wrapper<Favourites> 
                {
                    data = new Favourites 
                    {
                        List = "Not hungry at all"
                    }
                }
            };
        }
    }

    public class Favourites
    {
        public string List { get; set; }
    }
}
