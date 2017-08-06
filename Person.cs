using System.Collections.Generic;

namespace reflection
{
    public class Person
    {
        public string Name {get;set;}
        public Wrapper<Favourites> Favourites { get; set; }
        public Wrapper<List<Education>> Education { get; set; }
    }

    public class Favourites
    {
        public string Comments { get; set; }
    }

    public class Education
    {
        public EducationType Type { get; set; }
        public string Major { get; set; }
        public Wrapper<Grade> Grade { get; set; }
    }

    public class Grade
    {
        public float GPA { get; set; }
        public int MaxGPA { get; set; } = 4;
    }

    public enum EducationType
    {
        Bachelors,
        Masters
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
                },
                Education = new Wrapper<List<Education>>
                {
                    data = new List<Education>(new Education[]{
                        new Education
                        {
                            Type = EducationType.Bachelors,
                            Major = "Chemistry",
                            Grade = new Wrapper<Grade>
                            {
                                data = new Grade
                                {
                                    GPA = 3.5f
                                }
                            }
                        },
                        new Education
                        {
                            Type = EducationType.Masters,
                            Major = "Electronic Absorption Spectroscopy",
                            Grade = new Wrapper<Grade>
                            {
                                data = new Grade
                                {
                                    GPA = 3.1f
                                }
                            }
                        }
                    })
                }

            };
        }
    }
}
