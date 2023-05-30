using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Wunderwunsch.HexGridSimplified
{
    public struct Tile
    {
        public int BaseTerrain { get; private set; }
        public int Topography { get; private set; }
        public int Vegetation { get; private set; }

        public Tile(int baseTerrain, int topography, int vegetation)
        {
            this.BaseTerrain = baseTerrain; //Plains, Grassland, Desert etc.
            this.Topography = topography; //Flat, Hills, Mountains etc.
            this.Vegetation = vegetation; //Open, Forest, Jungle etc.
        }
    }
}
