using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IslandProblem
{
    public class Unit
    {
        public Unit() { }
        public Unit(byte contents, Island island, int x, int y, double f, int g, List<Unit> neighbours)
        {
            Contents = contents;
            Island = island;
            X = x;
            Y = y;
            F = f;
            G = g;
            Neighbours = neighbours;
        }

        public byte Contents { get; set; }
        public Island Island { get; set; }
        public int X { get; set; }
        public int Y { get; set; }
        public double F { get; set; }
        public int G { get; set; }
        public List<Unit> Neighbours { get; set; }
    }
}
