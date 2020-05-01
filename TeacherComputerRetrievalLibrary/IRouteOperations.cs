namespace TeacherComputerRetrievalLibrary
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    /// <summary>
    /// Interface for route operations
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IRouteOperations<T>
    {
        /// <summary>
        /// Creates the academies network asynchronous.
        /// </summary>
        /// <param name="locations">The locations.</param>
        /// <returns></returns>
        Task<AcademiesNetwork<T>> CreateAcademiesNetworkAsync(List<(T start, T end, int distance)> locations);

        /// <summary>
        /// Calculates the total distance along route.
        /// </summary>
        /// <param name="locations">The locations.</param>
        /// <param name="network">The network.</param>
        /// <returns>
        /// a distance value in number or 'No SUCH ROUTE' as message.
        /// </returns>
        Task<string> TotalDistanceAlongRouteAsync(List<T> locations, AcademiesNetwork<T> network);

        /// <summary>
        /// Total routes between academies.
        /// </summary>
        /// <param name="start">The start.</param>
        /// <param name="end">The end.</param>
        /// <param name="network">The network.</param>
        /// <returns>
        /// total routes between 2 academies
        /// </returns>
        Task<List<(List<T> path, int total)>> TotalRoutesBetweenAcademiesAsync(T start, T end, AcademiesNetwork<T> network);

        /// <summary>
        /// Totals the routes between academies with stops asynchronous.
        /// </summary>
        /// <param name="start">The start.</param>
        /// <param name="end">The end.</param>
        /// <param name="stops">The stops.</param>
        /// <param name="network">The network.</param>
        /// <returns></returns>
        Task<List<(List<T> path, int total)>> TotalRoutesBetweenAcademiesWithStopsAsync(T start, T end, int stops, AcademiesNetwork<T> network);


        /// <summary>
        /// Shortests route between academies.
        /// </summary>
        /// <param name="start">The start.</param>
        /// <param name="end">The end.</param>
        /// <param name="network">The network.</param>
        /// <returns></returns>
        int ShortestRouteBetweenAcademies(T start, T end, AcademiesNetwork<T> network);
    }
}
