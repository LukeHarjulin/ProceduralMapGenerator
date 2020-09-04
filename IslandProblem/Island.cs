using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IslandProblem
{
    public class Island
    {
        public Island() { }
        public Island(int id, List<Unit> landUnits, List<Island> connections)
        {
            Id = id;
            LandUnits = landUnits;
            Connections = connections;
        }

        public int Id { get; set; }
        public List<Unit> LandUnits { get; set; }
        public List<Island> Connections { get; set; }
    }
}
