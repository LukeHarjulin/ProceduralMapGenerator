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
        /// Contents equals 1 if the position contains land, and 0 if water
        /// IslandNum represents the island ID essentially
        /// X and Y are coordinates, position identifiers if you will
        /// </summary>
        public struct Position
        {
            public byte Contents;
            public int IslandNum;
            public int X;
            public int Y;
        }
        static void Main(string[] args)
        {
            
            do
            {
                Position[][] map;
                Random rnd = new Random();
                int tick = Environment.TickCount;
                int size = 0;
                string seed = "";
                int islandNums = 0;
                Console.WriteLine("Press '0' for test island or '1' for procedurally generated island");
                if (Console.ReadLine() == "0")
                {
                    size = 5;
                    map = new Position[size][];
                    for (int i = 0; i < size; i++)
                    {
                        map[i] = new Position[size];
                        for (int j = 0; j < size; j++)
                        {
                            if ((i == 0 && j == 0) || (i == 1 && j == 0) || (i == 2 && j == 0) || (i == 2 && j == 2) || (i == 2 && j == 1) || (i == 1 && j == 3) || (i == 1 && j == 4) ||
                                (i == 2 && j == 4) || (i == 3 && j == 4) || (i == 4 && j == 4) || (i == 4 && j == 2))
                                map[i][j] = new Position() { Contents = 1, IslandNum = 0, X = i, Y = j };
                            else
                                map[i][j] = new Position() { Contents = 0, IslandNum = 0, X = i, Y = j };
                        }
                    }
                    seed = tick.ToString();
                }
                else
                {
                    Console.WriteLine("How many islands do you want?");
                    islandNums = int.Parse(Console.ReadLine());
                    Console.WriteLine("How large do you want the array?");
                    size = int.Parse(Console.ReadLine());
                    Console.WriteLine("Seed? (enter \"?\" for random seed)");
                    seed = Console.ReadLine();
                    if (seed == "?")
                    {
                        rnd = new Random();
                        seed = Environment.TickCount.ToString();
                    }
                    else if (int.TryParse(seed, out int iSeed))
                    {
                        rnd = new Random(iSeed);
                    }
                    else
                        continue;
                    map = GenerateIsland(islandNums, size, rnd);
                }


                List<List<Position>> islands = FindIslands(map);
                AddBridges(map, size, islands, rnd);
                int bridges = PrintMap(map);
                Console.WriteLine("Map size: " + size);
                Console.WriteLine("Number of islands (connected blocks of '██'): " + islands.Count);
                Console.WriteLine("Number of land blocks ('██'): " + CountLandSpots(map));
                Console.WriteLine("Number of bridges ('¦¦' or '=='): " + bridges);
                Console.WriteLine("Seed: " + seed);
                Console.WriteLine("\r\nWould you like to create a new map? (Y/N)");
            } while (Console.ReadLine().ToUpper() == "Y");
            
        }
        /// <summary>
        /// Prints out the map.
        /// Each piece of land is represented as "██", each vertical bridge is represented as "¦¦", each horizontal bridge is represented as "==", and each empy block "  " represents water.
        /// </summary>
        /// <param name="map"></param>
        /// <returns></returns>
        static int PrintMap(Position[][] map)
        {
            int bridges = 0;
            for (int i = 0; i < map.Length; i++)
            {
                for (int j = 0; j < map[i].Length; j++)
                {
                    if (map[i][j].Contents == 1)
                        Console.Write("██");
                    else if (map[i][j].Contents == 3)
                    { Console.Write("¦¦"); bridges++; }
                    else if (map[i][j].Contents == 4)
                    { Console.Write("=="); bridges++; }
                    else
                        Console.Write("  ");
                }
                Console.WriteLine();
            }
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
        /// <param name="map"></param>
        /// <returns></returns>
        static List<List<Position>> FindIslands(Position[][] map)
        {
            List<List<Position>> islands = new List<List<Position>>();
            for (int i = 0; i < map.Length; i++)
                for (int j = 0; j < map.Length; j++)
                    map[i][j].IslandNum = 0;

            for (int i = 0; i < map.Length; i++)
            {
                for (int j = 0; j < map[i].Length; j++)
                {
                    if (map[i][j].Contents == 1 && map[i][j].IslandNum == 0)
                    {
                        map[i][j].IslandNum = islands.Count + 1;
                        List<Position> island = new List<Position> { map[i][j] };
                        islands.Add(island);                        
                        for (int k = 0; k < island.Count; k++)
                        {
                            if (island[k].X > 0)
                                if (map[island[k].X - 1][island[k].Y].Contents == 1 && map[island[k].X - 1][island[k].Y].IslandNum != map[i][j].IslandNum)
                                {
                                    map[island[k].X - 1][island[k].Y].IslandNum = map[i][j].IslandNum;
                                    island.Add(map[island[k].X - 1][island[k].Y]);
                                }
                            if (island[k].X < map.Length-1)
                                if (map[island[k].X + 1][island[k].Y].Contents == 1 && map[island[k].X + 1][island[k].Y].IslandNum != map[i][j].IslandNum)
                                {
                                    map[island[k].X + 1][island[k].Y].IslandNum = map[i][j].IslandNum;
                                    island.Add(map[island[k].X + 1][island[k].Y]);
                                }
                            if (island[k].Y > 0)
                                if (map[island[k].X][island[k].Y - 1].Contents == 1 && map[island[k].X][island[k].Y - 1].IslandNum != map[i][j].IslandNum)
                                {
                                    map[island[k].X][island[k].Y - 1].IslandNum = map[i][j].IslandNum;
                                    island.Add(map[island[k].X][island[k].Y - 1]);
                                }
                            if (island[k].Y < map[island[k].X].Length - 1)
                                if (map[island[k].X][island[k].Y + 1].Contents == 1 && map[island[k].X][island[k].Y + 1].IslandNum != map[i][j].IslandNum)
                                {
                                    map[island[k].X][island[k].Y + 1].IslandNum = map[i][j].IslandNum;
                                    island.Add(map[island[k].X][island[k].Y + 1]);
                                }
                        }
                    }
                }
            }
            return islands;
        }
        static Position[][] GenerateIsland(int numOfIslands, int size, Random rnd)
        {
            List<List<Position>> islands = new List<List<Position>>();
            Position[][] map = new Position[size][];
            for (int i = 0; i < size; i++)
            {
                map[i] = new Position[size];
                for (int j = 0; j < size; j++)
                {
                    map[i][j] = new Position() { Contents = 1, IslandNum = 0, X = i, Y = j };
                }
            }
            while (islands.Count < numOfIslands && CountLandSpots(map) > numOfIslands)
            {
                map[rnd.Next(0,size)][rnd.Next(0, size)].Contents = 0;
                islands = FindIslands(map);
            }
            AddBridges(map, size, islands, rnd);
            return map;
        }
        static void AddBridges(Position[][] map, int size, List<List<Position>> islands, Random rnd)
        {
            for (int i = 0; i < size; i++)
            {
                for (int j = 0; j < size; j++)
                {
                    if (map[i][j].Contents == 1)
                    {
                        if (i > 1)
                            if (map[i - 1][j].Contents == 0 && map[i - 2][j].Contents == 1 && map[i - 2][j].IslandNum != map[i][j].IslandNum && rnd.Next(0,5) == 1)
                                map[i - 1][j].Contents = 3;
                        if (i < size - 2)
                            if (map[i + 1][j].Contents == 0 && map[i + 2][j].Contents == 1 && map[i + 2][j].IslandNum != map[i][j].IslandNum && rnd.Next(0, 5) == 1)
                                map[i + 1][j].Contents = 3;
                        if (j > 1)
                            if (map[i][j - 1].Contents == 0 && map[i][j - 2].Contents == 1 && map[i][j - 2].IslandNum != map[i][j].IslandNum && rnd.Next(0, 5) == 1)
                                map[i][j - 1].Contents = 4;
                        if (j < size - 2)
                            if (map[i][j + 1].Contents == 0 && map[i][j + 2].Contents == 1 && map[i][j + 2].IslandNum != map[i][j].IslandNum && rnd.Next(0, 5) == 1)
                                map[i][j + 1].Contents = 4;
                    }           
                }
            }
        }
        static int CountLandSpots(Position[][] map)
        {
            int landSpots = 0;
            for (int i = 0; i < map.Length; i++)
            {
                for (int j = 0; j < map[i].Length; j++)
                {
                    if (map[i][j].Contents == 1)
                    {
                        landSpots++;
                    }
                }
            }

            return landSpots;
        }
    }
}
