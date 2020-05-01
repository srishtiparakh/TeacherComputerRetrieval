using System.Collections.Generic;

namespace TeacherComputerRetrievalLibrary
{
    public class City<T>
    {
        public T Name { get; set; }

        public List<Route<T>> Connections { get; set; }

        public int Distance { get; set; }

        public City(T Name)
        {
            this.Name = Name;
            Connections = new List<Route<T>>();
            Distance = int.MaxValue;
        }
    }
}