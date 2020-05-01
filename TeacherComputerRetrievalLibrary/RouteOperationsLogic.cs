namespace TeacherComputerRetrievalLibrary
{
    using System;
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    /// <summary>
    /// Interface for route operations
    /// </summary>
    public class RouteOperationsLogic<T> : IRouteOperations<T>
    {
        public async Task<AcademiesNetwork<T>> CreateAcademiesNetworkAsync(List<(T start, T end, int distance)> locations)
        {
            var result = new AcademiesNetwork<T>();
            foreach (var (start, end, distance) in locations)
            {
                await result.AddEdgeAsync(start, end, distance);
            }

            return result;
            
        }

        public int ShortestRouteBetweenAcademies(T start, T end, AcademiesNetwork<T> network)
        {
            throw new System.NotImplementedException();
        }

        public async Task<string> TotalDistanceAlongRouteAsync(List<T> locations, AcademiesNetwork<T> network)
        {
            return await GetPathDistanceAsync(locations, network.academies);
        }

        public async Task<List<(List<T> path, int total)>> TotalRoutesBetweenAcademiesAsync(T start, T end, AcademiesNetwork<T> network)
        {
            List<T> visitedList = new List<T>();

            var path = (new List<T>
            {
                start  // Origin city added by default
            }, 0);


            var paths = new List<(List<T> path, int total)> ();

            GetAllRoutes(start, end, visitedList, path, network.academies, ref paths);
            return paths;
        }

        public async Task<List<(List<T> path, int total)>> TotalRoutesBetweenAcademiesWithStopsAsync(T start, T end, int stops, AcademiesNetwork<T> network)
        {
            List<T> visitedList = new List<T>();

            var path = (new List<T>
            {
                start  // Origin city added by default
            }, 0);


            var paths = new List<(List<T> path, int total)>();

            GetAllRoutesByStops(start, end, visitedList, path, network.academies, ref paths, stops);
            return paths;
        }

        internal async Task<string> GetPathDistanceAsync(List<T> cities, Dictionary<T, City<T>> academies)
        {
            var totalDistance = 0;
            if (cities.Count == 1)
            {
                return "NO SUCH ROUTE";
            }

            for (int i = 1; i < cities.Count; i++)
            {
                var s = academies[cities[i - 1]]?.Connections.FirstOrDefault(t => t.Destination.Name.Equals(cities[i]))?.Distance;
                if (s.HasValue)
                    totalDistance += s.Value;
                else
                    return "NO SUCH ROUTE";
            }

            return await Task.FromResult(totalDistance.ToString());
        }

        private void GetAllRoutes(T start, T destination, List<T> visitedList,
            (List<T> cities, int total) path, Dictionary<T, City<T>> academies, ref List<(List<T> path, int total)> paths)
        {
            //if (path.total != 0 || !start.Equals(destination))
            visitedList.Add(start);

            if (start.Equals(destination) && path.total != 0)
            {
                visitedList.RemoveAt(visitedList.FindLastIndex(x => x.Equals(start)));
               
                paths.Add((path: new List<T>(path.cities), path.total));
                return;
            }

            foreach (var i in academies[start].Connections)
            {
                // avoid cycle of bidirectional cities loop
                int counter = path.cities.Count;
                if (counter < 4 || (counter >= 4 && (!path.cities[counter - 3].Equals(start))
                    || !path.cities[counter - 2].Equals(i.Destination.Name)))
                {
                    path.cities.Add(i.Destination.Name);
                    path.total += i.Distance;

                    GetAllRoutes(i.Destination.Name, destination, visitedList,
                                        path, academies, ref paths);

                    path.total -= i.Distance;
                    path.cities.RemoveAt(path.cities.FindLastIndex(x => x.Equals(i.Destination.Name)));  // This is to make sure that we only visit a city once.
                }

                //path.cities.Add(i.Destination.Name);
                //path.total += i.Distance;

                //GetAllRoutes(i.Destination.Name, destination, visitedList,
                //                    path, academies, ref paths);

                //path.total -= i.Distance;
                //path.cities.RemoveAt(path.cities.FindLastIndex(x => x.Equals(i.Destination.Name)));  // This is to make sure that we only visit a city once.
            }

            // Mark the current node  
            //visitedList.RemoveAt(visitedList.FindLastIndex(x => x.Equals(start)));
        }

        private void GetAllRoutesByStops(T start, T destination, List<T> visitedList,
    (List<T> cities, int total) path, Dictionary<T, City<T>> academies, ref List<(List<T> path, int total)> paths, int stops)
        {
            //if (path.total != 0 || !start.Equals(destination))
            visitedList.Add(start);

            if (start.Equals(destination) && path.total != 0)
            {
                if (visitedList.Count - 1 <= stops)
                {
                    //visitedList.RemoveAt(visitedList.FindLastIndex(x => x.Equals(start)));
                    paths.Add((path: new List<T>(path.cities), path.total));
                    //return;
                }
            }

            foreach (var i in academies[start].Connections)
            {
                if (visitedList.Count - 1 < stops)
                {
                    path.cities.Add(i.Destination.Name);
                    path.total += i.Distance;

                    GetAllRoutesByStops(i.Destination.Name, destination, visitedList,
                                        path, academies, ref paths, stops);

                    //path.total -= i.Distance;
                    //path.cities.RemoveAt(path.cities.FindLastIndex(x => x.Equals(i.Destination.Name)));  // This is to make sure that we only visit a city once.
                }
            }

            // Mark the current node  
            visitedList.RemoveAt(visitedList.FindLastIndex(x => x.Equals(start)));
            path.cities.RemoveAt(path.cities.FindLastIndex(x => x.Equals(start)));
        }
    }
}
