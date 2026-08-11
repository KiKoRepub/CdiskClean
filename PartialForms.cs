using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CdiskClean;


partial class FuncTestForm
{
    class Person
    {
        public string Name { get; set; }
        public string Sex { get; set; }

        public Person(string name, string sex)
        {
            Name = name;
            Sex = sex;
        }

        public static List<Person> GetSampleData()
        {
            return new List<Person>
            {
                new Person("Alice", "Female"),
                new Person("Bob", "Male"),
                new Person("Charlie", "Male"),
            };
        }

    }
}

