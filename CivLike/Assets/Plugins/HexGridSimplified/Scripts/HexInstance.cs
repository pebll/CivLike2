using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System.Runtime.CompilerServices;

namespace Wunderwunsch.HexGridSimplified
{
    /// <summary>
    /// Use this if you need to have more than one Map or want to take a more pure OOP approach rather than using the Static Hex Class 
    /// </summary>
    public class HexInstance
    {
        private Vector2Int mapSize = new Vector2Int(int.MaxValue, int.MaxValue);
        private bool wrapsHorizontal = false;
        public Vector2Int MapSize { get { return mapSize; } }
        public bool WrapsHorizontal { get { return wrapsHorizontal; } }


        public void SetMapAttributes(Vector2Int mapSize, bool wrapsHorizontal)
        {
            this.wrapsHorizontal = wrapsHorizontal;
            this.mapSize = mapSize;
        }

        public delegate T GetValidPositions<T>(T rawPositions) where T : ICollection<Vector3Int>, new(); //TO BE USED LATER WHEN WE ALSO have hexagonal shaped maps

        /// <summary>
        /// Given a Collection of Positions and the mapSize, it will return a Collection which only includes positions which are within the mapBounds 
        /// and wraps the coordinate horizontally if allowed
        /// WARNING : for certain collection types like for example Stacks this will reverse and/or change the order.
        /// It is used for Lists and Hashets here, but Queues and other similar Collections where the order does not change if you loop through them 
        /// with foreach and create a new Collection with some or all of its items are all safe to use.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="rawPositions"></param>
        /// <param name="mapSize"></param>
        /// <returns></returns>
        public T GetValidPositionsSquareMap<T>(T rawPositions) where T : ICollection<Vector3Int>, new()
        {
            T cleanedupCollection = new T();
            foreach (Vector3Int position in rawPositions)
            {
                if (!WrapsHorizontal)
                {
                    Vector2Int offsetCoord = HexConverter.CubeCoordToOffsetCoord(position);
                    if (offsetCoord.x < MapSize.x && offsetCoord.x >= 0 && offsetCoord.y < MapSize.y && offsetCoord.y >= 0)
                    {
                        cleanedupCollection.Add(position);
                    }
                }
                else
                {
                    Vector3Int wrappedPos = Hex.WrapTileHorizontal(position);
                    if (wrappedPos.y < MapSize.y && wrappedPos.y >= 0)
                    {
                        cleanedupCollection.Add(wrappedPos);
                    }

                }
            }
            return cleanedupCollection;
        }

        /// <summary>
        /// Returns the hex-grid manhattan distance between 2 points, accounts for mapwrap if possible
        /// </summary>
        /// <param name="posA"></param>
        /// <param name="posB"></param>
        /// <returns></returns>
        public int Distance(Vector3Int posA, Vector3Int posB)
        {
            if (WrapsHorizontal)
            {
                posB = HexUtility.GetCloserTargetPositionIncludingHorizontalWrap(posA, posB, MapSize.x);
            }

            int DeltaX = Mathf.Abs(posA.x - posB.x);
            int DeltaY = Mathf.Abs(posA.y - posB.y);
            int DeltaZ = Mathf.Abs(posA.z - posB.z);

            return Mathf.Max(DeltaX, DeltaY, DeltaZ);
        }

        /// <summary>
        /// Returns all tiles withing a certain hex-grid manhattan distance of the origin
        /// </summary>
        /// <param name="origin"></param>
        /// <param name="range"></param>
        /// <param name="includeSelf"></param>
        /// <param name="removeInvalid">wraps coordinates (if map wraps around) and removes out all results which are out of bound</param>
        /// <returns></returns>
        public HashSet<Vector3Int> GetAllWithinManhattanRange(Vector3Int origin, int range, bool includeSelf, bool removeInvalid)
        {
            HashSet<Vector3Int> positions = new HashSet<Vector3Int>();

            int minX = origin.x - range;
            int maxX = origin.x + range;
            int minY = origin.y - range;
            int maxY = origin.y + range;

            for (int x = minX; x <= maxX; x++)
            {
                for (int y = minY; y <= maxY; y++)
                {
                    int z = -x - y;
                    if (Mathf.Abs(z - origin.z) > range) continue;
                    positions.Add(new Vector3Int(x, y, z));
                }
            }
            if (!includeSelf) positions.Remove(origin);

            if (removeInvalid)
            {
                positions = GetValidPositionsSquareMap(positions);
            }
            return positions;
        }

        /// <summary>
        /// Returns all tiles which have a specific distance to the origin. Thickness goes inwards
        /// </summary>
        /// <param name="origin"></param>
        /// <param name="radius"></param>
        /// <param name="thickness"></param>
        /// <param name="removeInvalid">wraps coordinates (if map wraps around) and removes out all results which are out of bound</param>
        /// <returns></returns>
        public HashSet<Vector3Int> GetRing(Vector3Int origin, int radius, int thickness, bool removeInvalid)
        {
            //This is also not the most performant way to do it but again for almost all use cases it should be absolutely no issue.
            //If you happen to need a faster solution, again just precalculate the offets for each range once, store it in a list or dictionary and used that stored data 
            HashSet<Vector3Int> ring = new HashSet<Vector3Int>();
            HashSet<Vector3Int> allInManhattanrange = GetAllWithinManhattanRange(origin, radius, false, false);
            foreach (var v in allInManhattanrange)
            {
                if (Distance(origin, v) > radius - thickness) ring.Add(v);
            }

            if (removeInvalid)
            {
                ring = GetValidPositionsSquareMap(ring);
            }
            return ring;
        }

        /// <summary>
        /// Returns an List forming a line between 2 points.
        /// </summary>
        /// <param name="origin"></param>
        /// <param name="target"></param>
        /// <param name="horizontalNudgeFromOriginCenter"></param>
        /// <param name="trimStart"></param>
        /// <param name="trimEnd"></param>
        /// <param name="removeInvalid">wraps coordinates (if map wraps around) and removes out all results which are out of bound</param>
        /// <returns></returns>
        public List<Vector3Int> GetLine(Vector3Int origin, Vector3Int target, float horizontalNudgeFromOriginCenter, int trimStart, int trimEnd, bool removeInvalid) //maybe add option to only return those in mapBounds
        {
            if (WrapsHorizontal)
            {
                target = HexUtility.GetCloserTargetPositionIncludingHorizontalWrap(origin, target, MapSize.x);
            }
            List<Vector3Int> lineCells = new List<Vector3Int>();

            var dist = Distance(origin, target);
            for (int i = trimStart; i <= dist - trimEnd; i++)
            {
                Vector3 lerped = HexUtility.LerpCube(origin, target, horizontalNudgeFromOriginCenter, (1f / dist) * i);
                Vector3Int cell = HexUtility.RoundCube(lerped);
                lineCells.Add(cell);
            }

            if (removeInvalid)
            {
                lineCells = GetValidPositionsSquareMap(lineCells);
            }
            return lineCells;
        }

        /// <summary>
        /// Returns all adjacent tiles of a tile. Uses hardcoded offsets for speed
        /// </summary>
        /// <param name="origin"></param>
        /// <param name="removeInvalid">wraps coordinates (if map wraps around) and removes out all results which are out of bound</param>
        /// <returns></returns>
        public List<Vector3Int> GetNeighbours(Vector3Int origin, bool removeInvalid)
        {
            List<Vector3Int> neighbours = new List<Vector3Int>
            {
                new Vector3Int(origin.x + 1, origin.y, origin.z - 1),
                new Vector3Int(origin.x + 1, origin.y - 1, origin.z),
                new Vector3Int(origin.x, origin.y - 1, origin.z + 1),
                new Vector3Int(origin.x - 1, origin.y, origin.z + 1),
                new Vector3Int(origin.x - 1, origin.y + 1, origin.z),
                new Vector3Int(origin.x, origin.y + 1, origin.z - 1)
            };

            if (removeInvalid)
            {
                neighbours = GetValidPositionsSquareMap(neighbours);
            }
            return neighbours;
        }

        public HashSet<Vector2Int> GetMirroredTileCoordinates(Vector2Int originalCoord, bool mirrorHorizontal, bool mirrorVertical, bool includeOriginal, bool removeInvalid)
        {
            HashSet<Vector2Int> result = new HashSet<Vector2Int>();
            if (includeOriginal) result.Add(originalCoord);

            int Hmirrored = 0;
            int Vmirrored = 0;
            if (mirrorHorizontal)
            {
                int offset = 0;
                if (originalCoord.y % 2 == 1) offset = 1;
                Hmirrored = (mapSize.x - 1) - (originalCoord.x + offset);
                Vector2Int mirroredH = new Vector2Int(Hmirrored, originalCoord.y);
                result.Add(mirroredH);
            }

            if (mirrorVertical)
            {
                Vmirrored = (mapSize.y - 1) - originalCoord.y;
                Vector2Int mirroredV = new Vector2Int(originalCoord.x, Vmirrored);
                result.Add(mirroredV);
            }

            if (mirrorHorizontal && mirrorVertical)
            {
                Vector2Int mirroredHV = new Vector2Int(Hmirrored, Vmirrored);
                result.Add(mirroredHV);
            }

            if (removeInvalid)
            {
                HashSet<Vector2Int> cleanedUpResults = new HashSet<Vector2Int>();
                HashSet<Vector3Int> cubeCoords = new HashSet<Vector3Int>();
                foreach (var r in result)
                {
                    cubeCoords.Add(HexConverter.OffsetCoordToCubeCoord(r));
                }

                cubeCoords = GetValidPositionsSquareMap(cubeCoords);

                foreach (var c in cubeCoords)
                {
                    cleanedUpResults.Add(HexConverter.CubeCoordToOffsetCoord(c));
                }

                result = cleanedUpResults;
            }

            return result;
        }

        public HashSet<Vector3Int> GetMirroredTileCoordinates(Vector3Int originalCoord, bool mirrorHorizontal, bool mirrorVertical, bool includeOriginal, bool removeInvalid)
        {
            Vector2Int originalOffsetCoord = HexConverter.CubeCoordToOffsetCoord(originalCoord);
            HashSet<Vector2Int> offsetResults = GetMirroredTileCoordinates(originalOffsetCoord, mirrorHorizontal, mirrorVertical, includeOriginal, removeInvalid);
            HashSet<Vector3Int> cubeResults = new HashSet<Vector3Int>();
            foreach (var o in offsetResults)
            {
                cubeResults.Add(HexConverter.OffsetCoordToCubeCoord(o));
            }
            return cubeResults;
        }

        /// <summary>
        /// Clamps positions which are vertically out of bounds to the closest allowed Value
        /// </summary>
        /// <param name="position"></param>
        /// <returns></returns>
        public Vector3Int ClampToVerticalBounds(Vector3Int position)
        {
            Vector2Int offsetCoord = HexConverter.CubeCoordToOffsetCoord(position);
            if (offsetCoord.y < 0) offsetCoord.y = 0;
            if (offsetCoord.y >= MapSize.y) offsetCoord.y = MapSize.y - 1;
            return HexConverter.OffsetCoordToCubeCoord(offsetCoord);
        }

        /// <summary>
        /// Rotates the input position 60° in clockwise direction
        /// </summary>
        /// <param name="center"></param>
        /// <param name="point"></param>
        /// <returns></returns>
        public Vector3Int Rotate60DegreeClockwise(Vector3Int center, Vector3Int point)
        {
            Vector3Int direction = point - center;
            int rotatedX = -direction.z;
            int rotatedY = -direction.x;
            int rotatedZ = -direction.y;
            Vector3Int rotated = new Vector3Int(rotatedX, rotatedY, rotatedZ) + center;
            return rotated;
        }

        /// <summary>
        /// Rotates the input position 60° in counterclockwise direction
        /// </summary>
        /// <param name="center"></param>
        /// <param name="point"></param>
        /// <returns></returns>
        public Vector3Int Rotate60DegreeCounterClockwise(Vector3Int center, Vector3Int point)
        {
            Vector3Int direction = point - center;
            int rotatedX = -direction.y;
            int rotatedY = -direction.z;
            int rotatedZ = -direction.x;
            Vector3Int rotated = new Vector3Int(rotatedX, rotatedY, rotatedZ) + center;
            return rotated;
        }

        /// <summary>
        /// Returns the edge coordinate between 2 tiles. Edge coordinate is the sum of both tile coordinates.
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public Vector3Int GetEdgeCoordBetween(Vector3Int a, Vector3Int b)
        {
            if (Distance(a, b) != 1) throw new System.ArgumentException("Tiles don't have a distance of 1, therefore and not neighbours and share no Edge");

            if (!wrapsHorizontal)
            {
                Vector3Int EdgeCoordinate = a + b;
                return EdgeCoordinate;
            }

            else
            {
                if (a.x > b.x)
                {
                    Vector3Int swap = a;
                    a = b;
                    b = swap;
                }

                b = HexUtility.GetCloserTargetPositionIncludingHorizontalWrap(a, b, MapSize.x);
                Vector3Int EdgeCoordinate = a + b;
                return EdgeCoordinate;
            }



        }

        /// <summary>
        /// Returns the coordinates of all edges of a tile
        /// </summary>
        /// <param name="origin"></param>
        /// <returns></returns>
        public HashSet<Vector3Int> GetEdgeCoordinatesOfTile(Vector3Int origin)
        {
            HashSet<Vector3Int> edgeCoords = new HashSet<Vector3Int>
            {
                //TODO : find out if we can get from which elements are even/uneven which direction they face.
                new Vector3Int(2*origin.x+1,2*origin.y,2*(origin.z - 1)+1),
                new Vector3Int(2*origin.x+1,2*(origin.y-1)+1,2*origin.z),
                new Vector3Int(2*origin.x,2*(origin.y-1)+1,2*origin.z+1),
                new Vector3Int(2*(origin.x-1)+1,2*origin.y,2*origin.z+1),
                new Vector3Int(2*(origin.x-1)+1,2*origin.y+1,2*origin.z),
                new Vector3Int(2*origin.x,2*origin.y+1,2*(origin.z-1)+1),
            };

            HashSet<Vector3Int> edgeCoordsWrapped;
            if (WrapsHorizontal)
            {
                edgeCoordsWrapped = new HashSet<Vector3Int>();
                foreach (var edge in edgeCoords)
                {
                    var wrapped = Hex.WrapEdgeHorizontal(edge);
                    edgeCoordsWrapped.Add(wrapped);
                }
                edgeCoords = edgeCoordsWrapped;
            }

            return edgeCoords;
        }

        public HashSet<Vector3Int> GetBorderEdgesOfSetOfTiles(IEnumerable<Vector3Int> tiles)
        {
            HashSet<Vector3Int> edges = new HashSet<Vector3Int>();

            foreach (var tile in tiles)
            {
                HashSet<Vector3Int> edgeCoordsOfCell = Hex.GetEdgeCoordinatesOfTile(tile);
                foreach (var c in edgeCoordsOfCell)
                {
                    if (edges.Contains(c)) edges.Remove(c);
                    else edges.Add(c);
                }
            }
            return edges;
        }

        /// <summary>
        /// Returns the tiles which share this edge.
        /// </summary>
        /// <param name="edge"></param>
        /// <param name="removeInvalid">wraps coordinates (if map wraps around) and removes out all results which are out of bound</param>
        /// <returns></returns>
        public List<Vector3Int> GetAdjacentTilesOfEdge(Vector3Int edge, bool removeInvalid)
        {
            //TODO: Test if that works correctly
            int tileAx = 0;
            int tileAy = 0;
            int tileAz = 0;
            int tileBx = 0;
            int tileBy = 0;
            int tileBz = 0;

            if (edge.x % 2 == 0)
            {
                tileAx = edge.x / 2;
                tileBx = edge.x / 2;
                tileAy = (edge.y - 1) / 2;
                tileAz = (edge.z + 1) / 2;
                tileBy = (edge.y + 1) / 2;
                tileBz = (edge.z - 1) / 2;
            }

            else if (edge.y % 2 == 0)
            {
                tileAy = edge.y / 2;
                tileBy = edge.y / 2;
                tileAx = (edge.x + 1) / 2;
                tileAz = (edge.z - 1) / 2;
                tileBx = (edge.x - 1) / 2;
                tileBz = (edge.z + 1) / 2;
            }
            else
            {
                tileAz = edge.z / 2;
                tileBz = edge.z / 2;
                tileAx = (edge.x - 1) / 2;
                tileAy = (edge.y + 1) / 2;
                tileBx = (edge.x + 1) / 2;
                tileBy = (edge.y - 1) / 2;
            }

            List<Vector3Int> tiles = new List<Vector3Int>
            {
                new Vector3Int(tileAx, tileAy, tileAz),
                new Vector3Int(tileBx, tileBy, tileBz)
            };
            if (removeInvalid)
            {
                return GetValidPositionsSquareMap(tiles);
            }
            else
            {
                for (int i = 0; i < tiles.Count; i++)
                {
                    tiles[i] = Hex.WrapTileHorizontal(tiles[i]);
                }
                return tiles.ToList();
            }


        }

        /// <summary>
        /// returns the angle of the edge.
        /// </summary>
        /// <param name="edge"></param>
        /// <returns></returns>
        public float GetEdgeDirectionAngle(Vector3Int edge)
        {

            if (edge.y % 2 == 0)
            {
                return 0;
            }
            else if (edge.x % 2 == 0)
            {
                return 120;
            }
            else return 240;
        }

        /// <summary>
        /// Wraps offsetX coordinate (horizontal) between 0 and upperBound(excluding)
        /// By assuming 0 for the lower bound it is faster and simpler than the more general solution
        /// </summary>
        /// <param name="position"></param>
        /// <param name="upperBound"></param>
        /// <returns></returns>
        public Vector3Int WrapTileHorizontal(Vector3Int position)
        {
            if (!WrapsHorizontal) return position; //if Map isn't a wraparound we return original.

            Vector2Int offsetPosition = HexConverter.CubeCoordToOffsetCoord(position); //We need to use the upperBound parameter instead of using the mapsize Property because for edges we need mapsize.y*2
            offsetPosition.x = ((offsetPosition.x) % MapSize.x);
            if (offsetPosition.x < 0)
            {
                offsetPosition.x = MapSize.x + offsetPosition.x;
            }

            return HexConverter.OffsetCoordToCubeCoord(offsetPosition);
        }

        public Vector3Int WrapEdgeHorizontal(Vector3Int position)
        {
            if (!WrapsHorizontal) return position;
            Vector2Int offsetPosition = HexConverter.CubeCoordToOffsetCoord(position); //We need to use the upperBound parameter instead of using the mapsize Property because for edges we need mapsize.y*2
            if (offsetPosition.x > ((MapSize.x - 1) * 2))
            {
                offsetPosition.x -= (MapSize.x * 2);
            }

            return HexConverter.OffsetCoordToCubeCoord(offsetPosition);
        }

    }
}
