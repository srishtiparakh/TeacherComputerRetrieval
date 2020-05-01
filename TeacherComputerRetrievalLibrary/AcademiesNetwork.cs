namespace TeacherComputerRetrievalLibrary
{
    using System;
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    public class AcademiesNetwork<T>
    {
        public readonly Dictionary<T, City<T>> academies = new Dictionary<T, City<T>>();

        public async Task AddEdgeAsync(T source, T destination, int cost)
        {
            var start = await GetAcademyAsync(source);
            start.Connections.Add(new Route<T>(await GetAcademyAsync(destination), cost));
        }

        private async Task<City<T>> GetAcademyAsync(T cityName)
        {
            academies.TryGetValue(cityName, out City<T> v);
            if (v == null)
            {
                v = new City<T>(cityName);
                academies.Add(cityName, v);
            }
            return v;
        }

        //public async Task PrintGraphWithAdjacencyListAsync()
        //{
        //    foreach (var item in academies.Keys)
        //    {
        //        Console.WriteLine($"Adjacency List for city {item}");
        //        foreach (var city in academies[item].Connections)
        //        {
        //            Console.WriteLine($"{item}  -> {city.Destination.Name} -> cost -> {city.Distance}");
        //        }
        //    }
        //}

        public async Task GetAllPathsAsync(T start, T end)
        {
            List<T> visitedList = new List<T>();

            var path = (new List<T>
            {
                start  // Origin city added by default
            }, 0);


            await GetAllRoutesAsync(start, end, visitedList, path);
        }

        private async Task GetAllRoutesAsync(T start, T destination, List<T> visitedList, (List<T> cities, int total) path)
        {
            if (path.total != 0)
                visitedList.Add(start);

            if (start.Equals(destination) && path.total != 0)
            {
                Console.WriteLine($"{string.Join("->", path.cities)} with cost {path.total}");

                visitedList.RemoveAt(visitedList.FindLastIndex(x => x.Equals(start)));
                return;
            }

            foreach (var i in academies[start].Connections)
            {
                path.cities.Add(i.Destination.Name);
                path.total += i.Distance;

                await GetAllRoutesAsync(i.Destination.Name, destination, visitedList,
                                    path);

                path.total -= i.Distance;
                path.cities.RemoveAt(path.cities.FindLastIndex(x => x.Equals(i.Destination.Name)));  // This is to make sure that we only visit a city once.
            }

            // Mark the current node  
            //visitedList.RemoveAt(visitedList.FindLastIndex(x => x.Equals(start)));
        }
    }
}
