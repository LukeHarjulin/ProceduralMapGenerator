using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IslandProblem
{
    public class AStarAlgo
    {
        public static List<Unit> Algo(Unit start, Unit goal, Map map)
        {
            
            List<Unit> openList = new List<Unit>() { start };
            Dictionary<Unit, Unit> closedList = new Dictionary<Unit, Unit>();
            start.G = 0;
            start.F = Heuristic(start, goal);
            while (openList.Count > 0)
            {
                Unit current = openList[0];
                for (int i = 1; i < openList.Count; i++)
                {
                    if (openList[i].F < current.F)
                    {
                        current = openList[i];
                    }
                }
                if (current == goal)
                    return ConstructPath(closedList, current);
                openList.Remove(current);
                foreach (Unit neighbour in current.Neighbours)
                {
                    if (neighbour.Contents != 0)
                    {
                        int tempGCost = current.G + 1;
                        if (tempGCost < neighbour.G)
                        {
                            closedList.Add(neighbour, current);
                            neighbour.G = tempGCost;
                            neighbour.F = neighbour.G + Heuristic(neighbour, goal);
                            if (!openList.Contains(neighbour))
                            {
                                openList.Add(neighbour);
                            }
                        }
                    }                    
                }
            }
            return null;
        }
        public static List<Unit> ConstructPath(Dictionary<Unit,Unit> closedList, Unit current)
        {
            List<Unit> path = new List<Unit>() { current };
            while (closedList.ContainsKey(current))
            {
                current = closedList[current];
                path.Insert(0, current);
            }
            return path;
        }
        /// <summary>
        /// Euclidean Distance calculator
        /// </summary>
        /// <returns></returns>
        public static double Heuristic(Unit a, Unit b) => Math.Sqrt(Math.Abs(((a.X - b.X) * 2) + ((a.Y - b.Y) * 2)));
    }
}
