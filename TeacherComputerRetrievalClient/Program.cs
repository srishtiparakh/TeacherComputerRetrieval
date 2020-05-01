namespace TeacherComputerRetrievalClient
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using TeacherComputerRetrievalLibrary;

    class Program
    {
        static async Task Main(string[] args)
        {
            //Test Input
            string input = "AB5, BC4, CD8, DC8, DE6, AD5, CE2, EB3, AE7";
            IRouteOperations<string> routeOperations = new RouteOperationsLogic<string>();
            var result = await routeOperations.CreateAcademiesNetworkAsync(new List<(string start, string end, int distance)> {
            ("A", "B", 5),
            ("A", "E", 7),
            ("A", "D", 5),
            ("B", "C", 4),
            ("C", "D", 8),
            ("D", "C", 8),
            ("D", "E", 6),
            ("C", "E", 2),
            ("E", "B", 3)

            // for trial
           // ("D", "B", 8),
        });

            Console.WriteLine(await routeOperations.TotalDistanceAlongRouteAsync(new List<string> { "A", "B", "C" }, result));
            Console.WriteLine(await routeOperations.TotalDistanceAlongRouteAsync(new List<string> { "A", "E", "B", "C", "D" }, result));
            Console.WriteLine(await routeOperations.TotalDistanceAlongRouteAsync(new List<string> { "A", "E", "D" }, result));


            var routes  = await routeOperations.TotalRoutesBetweenAcademiesWithStopsAsync("C", "C", 3, result);
            foreach (var (path, total) in routes)
            {
                Console.WriteLine($"{String.Join("->", path)} with cost {total}");
            }

            Console.WriteLine();
            var withExactRoute = await routeOperations.TotalRoutesBetweenAcademiesWithStopsAsync("A", "C", 4, result, true);
            foreach (var (path, total) in withExactRoute)
            {
                Console.WriteLine($"{String.Join("->", path)} with cost {total}");
            }

            Console.WriteLine();

            Console.WriteLine(await routeOperations.ShortestRouteBetweenAcademies("A", "C", result));
            Console.WriteLine(await routeOperations.ShortestRouteBetweenAcademies("B", "B", result));

            Console.WriteLine();

            var routeWithRange = await routeOperations.TotalRoutesBetweenAcademiesAsync("C", "C", result);
            foreach (var (path, total) in routeWithRange)
            {
                Console.WriteLine($"{String.Join("->", path)} with cost {total}");
            }
            Console.ReadLine();
        }
    }
}
