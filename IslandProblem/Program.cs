using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace IslandProblem
{
    class Program
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="args"></param>
        static void Main(string[] args)
        {
            do
            {
                Map map = new Map();
                Random rnd = new Random();
                int tick = Environment.TickCount;
                Console.WriteLine("Press '0' for test island or '1' for procedurally generated island");
                if (Console.ReadLine() == "0")
                {
                    map.Size = 5;
                    map.GameMap = new Unit[map.Size][];
                    for (int i = 0; i < map.Size; i++)
                    {
                        map.GameMap[i] = new Unit[map.Size];
                        for (int j = 0; j < map.Size; j++)
                        {
                            if ((i == 0 && j == 0) || (i == 1 && j == 0) || (i == 2 && j == 0) || (i == 2 && j == 2) || (i == 2 && j == 1) || (i == 1 && j == 3) || (i == 1 && j == 4) ||
                                (i == 2 && j == 4) || (i == 3 && j == 4) || (i == 4 && j == 4) || (i == 4 && j == 2))
                                map.GameMap[i][j] = new Unit(1, null, i, j, Double.MaxValue, Int32.MaxValue, null);
                            else
                                map.GameMap[i][j] = new Unit(0, null, i, j, Double.MaxValue, Int32.MaxValue, null);
                        }
                    }
                    for (int i = 0; i < map.Size; i++)
                    {
                        for (int j = 0; j < map.Size; j++)
                        {
                            map.GameMap[i][j].Neighbours = new List<Unit>();
                            if (i > 0)
                                map.GameMap[i][j].Neighbours.Add(map.GameMap[i - 1][j]);
                            if (i < map.GameMap.Length - 1)
                                map.GameMap[i][j].Neighbours.Add(map.GameMap[i + 1][j]);
                            if (j > 0)
                                map.GameMap[i][j].Neighbours.Add(map.GameMap[i][j - 1]);
                            if (j < map.GameMap[i].Length - 1)
                                map.GameMap[i][j].Neighbours.Add(map.GameMap[i][j + 1]);
                        }
                    }
                    map.Seed = tick.ToString();
                    map.Islands = FindIslands(map);
                    AddBridges(map, rnd);
                }
                else
                {
                    Console.WriteLine("How many islands do you want?");
                    map.InitIslandCount = int.Parse(Console.ReadLine());
                    Console.WriteLine("How large do you want the array?");
                    map.Size = int.Parse(Console.ReadLine());
                    Console.WriteLine("Seed? (enter \"?\" for random seed)");
                    map.Seed = Console.ReadLine();
                    if (map.Seed == "?")
                    {
                        rnd = new Random();
                        map.Seed = Environment.TickCount.ToString();
                    }
                    else if (int.TryParse(map.Seed, out int iSeed))
                    {
                        rnd = new Random(iSeed);
                    }
                    else
                        continue;
                    map.GameMap = GenerateIsland(map, rnd);
                }
                
                List<Unit> path = new List<Unit>();
                    
                int bridges = PrintMap(map, null, null, path);                                
                Console.WriteLine("Map size: " + map.Size);
                Console.Write("Number of islands (connected blocks of '");
                Console.ForegroundColor = ConsoleColor.Green;
                Console.Write("██");
                Console.ResetColor();
                Console.WriteLine("'): " + map.Islands.Count);
                Console.Write("Number of land blocks ('");
                Console.ForegroundColor = ConsoleColor.Green;
                Console.Write("██");
                Console.ResetColor();
                Console.WriteLine("'): " + CountLandSpots(map));
                Console.WriteLine("Number of bridges ('¦¦' or '=='): " + bridges);
                Console.WriteLine("Seed: " + map.Seed);                         
                Console.WriteLine("Start Index(Row)?");
                int startX = int.Parse(Console.ReadLine());
                Console.WriteLine("Start Index(Col)?");
                int startY = int.Parse(Console.ReadLine());
                Console.WriteLine("End Index(Row)?");
                int goalX = int.Parse(Console.ReadLine());
                Console.WriteLine("End Index(Col)?");
                int goalY = int.Parse(Console.ReadLine());
                path = AStarAlgo.Algo(map.GameMap[startX][startY], map.GameMap[goalX][goalY], map);
                PrintMap(map, map.GameMap[startX][startY], map.GameMap[goalX][goalY], path);
                Console.WriteLine(path != null ? "Success! Path created." : "Failure");
                Console.WriteLine("\r\nWould you like to create a new map? (Y/N)");
            } while (Console.ReadLine().ToUpper() == "Y");
            
        }        
        /// <summary>
        /// Prints out the map.
        /// Each piece of land is represented as "██", each vertical bridge is represented as "¦¦", each horizontal bridge is represented as "==", and each empy block "  " represents water.
        /// </summary>
        /// <param name="GameMap"></param>
        /// <returns></returns>
        static int PrintMap(Map map, Unit start, Unit goal, List<Unit> path)
        {
            Console.BackgroundColor = ConsoleColor.Blue;
            int bridges = 0;
            for (int i = 0; i < map.GameMap.Length; i++)
            {
                for (int j = 0; j < map.GameMap[i].Length; j++)
                {
                    
                    if (map.GameMap[i][j].Contents == 1)
                    {
                        if (start == map.GameMap[i][j])
                            Console.ForegroundColor = ConsoleColor.Yellow;
                        else if (goal == map.GameMap[i][j])
                            Console.ForegroundColor = ConsoleColor.Red;
                        else if (path.Contains(map.GameMap[i][j]))
                            Console.ForegroundColor = ConsoleColor.Magenta;
                        else
                            Console.ForegroundColor = ConsoleColor.Green;
                        Console.Write("██");
                    }                    
                    else if (map.GameMap[i][j].Contents == 3)
                    {
                        if (path.Contains(map.GameMap[i][j]))
                            Console.ForegroundColor = ConsoleColor.Magenta;
                        else
                            Console.ForegroundColor = ConsoleColor.DarkYellow;
                        Console.Write("¦¦"); bridges++;
                    }
                    else if (map.GameMap[i][j].Contents == 4)
                    {
                        if(path.Contains(map.GameMap[i][j]))
                            Console.ForegroundColor = ConsoleColor.Magenta;
                        else
                            Console.ForegroundColor = ConsoleColor.DarkYellow;
                        Console.Write("=="); bridges++; 
                    }
                    else
                    { Console.Write("  "); }
                        
                }
                Console.WriteLine();
            }
            Console.ResetColor();
            return bridges;
        }
        /// <summary>
        /// Depth-First Search method
        ///     initialise list of type list <int[,]>, Islands
        ///     foreach array in mtrx, i
        ///         foreach int in mtrx[i]
        ///             if number is 1
        ///                 Add index to List
        ///                 for (int i = 0; i LT List.Count; i++)
        ///                     look at neighbour indexes that do not exist in list
        ///                     add them to list if they contain 1
        ///                 endfor
        ///                 add List to Islands
        ///             endif
        ///         endfor
        ///     endfor
        ///     return Islands count
        /// </summary>
        /// <param name="GameMap"></param>
        /// <returns></returns>
        static List<Island> FindIslands(Map map)
        {
            map.Islands = new List<Island>();
            for (int i = 0; i < map.GameMap.Length; i++)
                for (int j = 0; j < map.GameMap.Length; j++)
                    map.GameMap[i][j].Island = null;

            for (int i = 0; i < map.GameMap.Length; i++)
            {
                for (int j = 0; j < map.GameMap[i].Length; j++)
                {
                    if (map.GameMap[i][j].Contents == 1 && map.GameMap[i][j].Island == null)
                    {

                        Island island = new Island
                        {
                            Id = map.Islands.Count + 1,
                            LandUnits = new List<Unit>() { map.GameMap[i][j] },
                            Connections = new List<Island>()
                        };
                        map.GameMap[i][j].Island = island;
                        map.Islands.Add(island);                        
                        for (int k = 0; k < island.LandUnits.Count; k++)
                        {
                            if (island.LandUnits[k].X > 0)
                                if (map.GameMap[island.LandUnits[k].X - 1][island.LandUnits[k].Y].Contents == 1 && map.GameMap[island.LandUnits[k].X - 1][island.LandUnits[k].Y].Island != map.GameMap[i][j].Island)
                                {
                                    map.GameMap[island.LandUnits[k].X - 1][island.LandUnits[k].Y].Island = island;
                                    island.LandUnits.Add(map.GameMap[island.LandUnits[k].X - 1][island.LandUnits[k].Y]);
                                }
                            if (island.LandUnits[k].X < map.GameMap.Length-1)
                                if (map.GameMap[island.LandUnits[k].X + 1][island.LandUnits[k].Y].Contents == 1 && map.GameMap[island.LandUnits[k].X + 1][island.LandUnits[k].Y].Island != map.GameMap[i][j].Island)
                                {
                                    map.GameMap[island.LandUnits[k].X + 1][island.LandUnits[k].Y].Island = island;
                                    island.LandUnits.Add(map.GameMap[island.LandUnits[k].X + 1][island.LandUnits[k].Y]);
                                }
                            if (island.LandUnits[k].Y > 0)
                                if (map.GameMap[island.LandUnits[k].X][island.LandUnits[k].Y - 1].Contents == 1 && map.GameMap[island.LandUnits[k].X][island.LandUnits[k].Y - 1].Island != map.GameMap[i][j].Island)
                                {
                                    map.GameMap[island.LandUnits[k].X][island.LandUnits[k].Y - 1].Island = island;
                                    island.LandUnits.Add(map.GameMap[island.LandUnits[k].X][island.LandUnits[k].Y - 1]);
                                }
                            if (island.LandUnits[k].Y < map.GameMap[island.LandUnits[k].X].Length - 1)
                                if (map.GameMap[island.LandUnits[k].X][island.LandUnits[k].Y + 1].Contents == 1 && map.GameMap[island.LandUnits[k].X][island.LandUnits[k].Y + 1].Island != map.GameMap[i][j].Island)
                                {
                                    map.GameMap[island.LandUnits[k].X][island.LandUnits[k].Y + 1].Island = island;
                                    island.LandUnits.Add(map.GameMap[island.LandUnits[k].X][island.LandUnits[k].Y + 1]);
                                }
                        }
                    }
                }
            }
            return map.Islands;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="numOfIslands"></param>
        /// <param name="size"></param>
        /// <param name="rnd"></param>
        /// <returns></returns>
        static Unit[][] GenerateIsland(Map map, Random rnd)
        {
            map.Islands = new List<Island>();
            map.GameMap = new Unit[map.Size][];
            for (int i = 0; i < map.Size; i++)
            {
                map.GameMap[i] = new Unit[map.Size];
                for (int j = 0; j < map.Size; j++)
                {
                    map.GameMap[i][j] = new Unit(1, null, i, j, Double.MaxValue, Int32.MaxValue, null);
                }
            }
            for (int i = 0; i < map.Size; i++)
            {
                for (int j = 0; j < map.Size; j++)
                {
                    map.GameMap[i][j].Neighbours = new List<Unit>();
                    if (i > 0)
                        if (map.GameMap[i - 1][j].Contents != 0) 
                            map.GameMap[i][j].Neighbours.Add(map.GameMap[i - 1][j]);
                    if (i < map.GameMap.Length - 1)
                        if (map.GameMap[i + 1][j].Contents != 0)
                            map.GameMap[i][j].Neighbours.Add(map.GameMap[i + 1][j]);
                    if (j > 0)
                        if (map.GameMap[i][j - 1].Contents != 0)
                            map.GameMap[i][j].Neighbours.Add(map.GameMap[i][j - 1]);
                    if (j < map.GameMap[i].Length - 1)
                        if (map.GameMap[i][j + 1].Contents != 0)
                            map.GameMap[i][j].Neighbours.Add(map.GameMap[i][j + 1]);
                }
            }
            while (map.Islands.Count < map.InitIslandCount && CountLandSpots(map) > map.InitIslandCount)
            {
                map.GameMap[rnd.Next(0,map.Size)][rnd.Next(0, map.Size)].Contents = 0;
                map.Islands = FindIslands(map);
            }
            AddBridges(map, rnd);
            return map.GameMap;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="gameMap"></param>
        /// <param name="size"></param>
        /// <param name="rnd"></param>
        static void AddBridges(Map map, Random rnd)
        {
            for (int i = 0; i < map.Islands.Count; i++)
            {
                foreach (Unit unit in map.Islands[i].LandUnits)
                {
                    if (unit.X > 1 && rnd.Next(0, 3) == 1)
                        if (map.GameMap[unit.X - 1][unit.Y].Contents == 0 && map.GameMap[unit.X - 2][unit.Y].Contents == 1 
                            && map.GameMap[unit.X - 2][unit.Y].Island != map.GameMap[unit.X][unit.Y].Island 
                            && !map.GameMap[unit.X - 2][unit.Y].Island.Connections.Contains(map.Islands[i])
                            && !CheckSurrounded(map.GameMap[unit.X - 1][unit.Y], map))
                        {
                            map.GameMap[unit.X - 1][unit.Y].Contents = 3;
                            map.GameMap[unit.X - 2][unit.Y].Island.Connections.Add(unit.Island);
                            unit.Island.Connections.Add(map.GameMap[unit.X - 2][unit.Y].Island);
                        }
                    if (unit.X < map.Size - 2 && rnd.Next(0, 3) == 1)
                        if (map.GameMap[unit.X + 1][unit.Y].Contents == 0 && map.GameMap[unit.X + 2][unit.Y].Contents == 1 
                            && map.GameMap[unit.X + 2][unit.Y].Island != map.GameMap[unit.X][unit.Y].Island
                            && !map.GameMap[unit.X + 2][unit.Y].Island.Connections.Contains(map.Islands[i])
                            && !CheckSurrounded(map.GameMap[unit.X + 1][unit.Y], map))
                        {
                            map.GameMap[unit.X + 1][unit.Y].Contents = 3;
                            map.GameMap[unit.X + 2][unit.Y].Island.Connections.Add(unit.Island);
                            unit.Island.Connections.Add(map.GameMap[unit.X + 2][unit.Y].Island);
                        }
                    if (unit.Y > 1 && rnd.Next(0, 3) == 1)
                        if (map.GameMap[unit.X][unit.Y - 1].Contents == 0 && map.GameMap[unit.X][unit.Y - 2].Contents == 1 
                            && map.GameMap[unit.X][unit.Y - 2].Island != map.GameMap[unit.X][unit.Y].Island
                            && !map.GameMap[unit.X][unit.Y - 2].Island.Connections.Contains(map.Islands[i])
                            && !CheckSurrounded(map.GameMap[unit.X][unit.Y - 1], map))
                        {
                            map.GameMap[unit.X][unit.Y - 1].Contents = 4;
                            map.GameMap[unit.X][unit.Y - 2].Island.Connections.Add(unit.Island);
                            unit.Island.Connections.Add(map.GameMap[unit.X][unit.Y - 2].Island);
                        }
                    if (unit.Y < map.Size - 2 && rnd.Next(0, 3) == 1)
                        if (map.GameMap[unit.X][unit.Y + 1].Contents == 0 && map.GameMap[unit.X][unit.Y + 2].Contents == 1 
                            && map.GameMap[unit.X][unit.Y + 2].Island != map.GameMap[unit.X][unit.Y].Island
                            && !map.GameMap[unit.X][unit.Y + 2].Island.Connections.Contains(map.Islands[i])
                            && !CheckSurrounded(map.GameMap[unit.X][unit.Y + 1], map))
                        {
                            map.GameMap[unit.X][unit.Y + 1].Contents = 4;
                            map.GameMap[unit.X][unit.Y + 2].Island.Connections.Add(unit.Island);
                            unit.Island.Connections.Add(map.GameMap[unit.X][unit.Y + 2].Island);
                        }
                }
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="unit"></param>
        /// <param name="map"></param>
        /// <returns></returns>
        static bool CheckSurrounded(Unit unit, Map map)
        {
            if (unit.X > 0 && unit.X < map.Size - 1 && unit.Y > 0 && unit.Y < map.Size - 1)
                if (map.GameMap[unit.X - 1][unit.Y].Contents == 1 && map.GameMap[unit.X + 1][unit.Y].Contents == 1
                    && map.GameMap[unit.X][unit.Y - 1].Contents == 1 && map.GameMap[unit.X][unit.Y + 1].Contents == 1)
                    return true;
            return false;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="gameMap"></param>
        /// <returns></returns>
        static int CountLandSpots(Map map)
        {
            int landSpots = 0;
            for (int i = 0; i < map.GameMap.Length; i++)
            {
                for (int j = 0; j < map.GameMap[i].Length; j++)
                {
                    if (map.GameMap[i][j].Contents == 1)
                    {
                        landSpots++;
                    }
                }
            }
            return landSpots;
        }
    }
}
