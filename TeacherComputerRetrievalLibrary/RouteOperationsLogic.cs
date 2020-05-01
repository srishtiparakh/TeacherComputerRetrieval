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

        public async Task<int> ShortestRouteBetweenAcademies(T start, T end, AcademiesNetwork<T> network)
        {
            List<T> visitedList = new List<T>();

            var path = (new List<T>
            {
                start
            }, total: 0);

            int routeTotal = int.MaxValue;
            GetShortestPath(start, end, visitedList, ref path, network.academies, ref routeTotal);
            return await Task.FromResult(routeTotal);
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
                start 
            }, 0);

            var paths = new List<(List<T> path, int total)>();

            GetAllRoutes(start, end, visitedList, path, network.academies, ref paths);
            return await Task.FromResult(paths);
        }

        public async Task<List<(List<T> path, int total)>> TotalRoutesBetweenAcademiesWithStopsAsync(T start, T end, int stops, AcademiesNetwork<T> network, bool matchExact = false)
        {
            List<T> visitedList = new List<T>();

            var path = (new List<T>
            {
                start  // Origin city added by default
            }, 0);


            var paths = new List<(List<T> path, int total)>();

            GetAllRoutesByStops(start, end, visitedList, path, network.academies, ref paths, stops, matchExact);
            return await Task.FromResult(paths);
        }

        private void GetAllRoutes(T start, T destination, List<T> visitedList,
            (List<T> cities, int total) path, Dictionary<T, City<T>> academies, ref List<(List<T> path, int total)> paths)
        {
            visitedList.Add(start);

            if (start.Equals(destination) && path.total != 0)
            {
                visitedList.RemoveAt(visitedList.FindLastIndex(x => x.Equals(start)));

                paths.Add((path: new List<T>(path.cities), path.total));
                return;
            }

            foreach (var i in academies[start].Connections)
            {
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
            }
        }

        private void GetAllRoutesByStops(T start, T destination, List<T> visitedList,
    (List<T> cities, int total) path, Dictionary<T, City<T>> academies, ref List<(List<T> path, int total)> paths, int stops, bool matchExact)
        {
            visitedList.Add(start);

            if (start.Equals(destination) && path.total != 0)
            {
                if (matchExact ? visitedList.Count - 1 == stops : visitedList.Count - 1 <= stops)
                {
                    paths.Add((path: new List<T>(path.cities), path.total));
                }
            }

            foreach (var i in academies[start].Connections)
            {
                if (visitedList.Count <= stops)
                {
                    path.cities.Add(i.Destination.Name);
                    path.total += i.Distance;

                    GetAllRoutesByStops(i.Destination.Name, destination, visitedList,
                                        path, academies, ref paths, stops, matchExact);
                }
            }

            visitedList.RemoveAt(visitedList.FindLastIndex(x => x.Equals(start)));
            path.cities.RemoveAt(path.cities.FindLastIndex(x => x.Equals(start)));
        }


        private void GetShortestPath(T start, T destination, List<T> visitedList,
            ref (List<T> cities, int total) path, Dictionary<T, City<T>> academies, ref int routeCost)
        {
            visitedList.Add(start);

            if (start.Equals(destination) && path.total != 0)
            {
                routeCost = path.total;
            }

            foreach (var i in academies[start].Connections)
            {
                if (!visitedList.Contains(i.Destination.Name) || i.Destination.Name.Equals(destination))
                {
                    if (routeCost > (path.total + i.Distance))
                    {
                        path.cities.Add(i.Destination.Name);
                        path.total += i.Distance;

                        GetShortestPath(i.Destination.Name, destination, visitedList,
                                            ref path, academies, ref routeCost);

                        path.total -= i.Distance;
                    }
                }
            }

            visitedList.RemoveAt(visitedList.FindLastIndex(x => x.Equals(start)));
            path.cities.RemoveAt(path.cities.FindLastIndex(x => x.Equals(start)));
        }
        private async Task<string> GetPathDistanceAsync(List<T> cities, Dictionary<T, City<T>> academies)
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
    }
}
