using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Wunderwunsch.HexGridSimplified
{
    public class Edge
    {        
        public int EdgeType { get; private set; }

        public Edge(int type)
        {
            //CellA = cellA;
            //CellB = cellB;
            EdgeType = type;
        }
    }
}
