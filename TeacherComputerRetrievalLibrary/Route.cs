namespace TeacherComputerRetrievalLibrary
{
    public class Route<T>
    {
        public City<T> Destination { get; set; }

        public int Distance { get; set; }

        public Route(City<T> destination, int distance)
        {
            Destination = destination;
            Distance = distance;
        }
    }
}