using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IslandProblem
{
    public class Map
    {
        public Map() { }
        public Map(int id, Unit[][] gameMap, List<Island> islands, string seed, int size, int initIslandCount)
        {
            Id = id;
            GameMap = gameMap;
            Islands = islands;
            Seed = seed;
            Size = size;
            InitIslandCount = initIslandCount;
        }

        public int Id { get; set; }
        public Unit[][] GameMap { get; set; }
        public List<Island> Islands { get; set; }
        public string Seed { get; set; }
        public int Size { get; set; }
        public int InitIslandCount { get; set; }
    }
}
