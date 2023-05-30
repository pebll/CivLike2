using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Wunderwunsch.HexGridSimplified
{
    /// <summary>
    /// Contains methods which you usually won't need to use directly but which are used by the Hex and HexInstance class
    /// </summary>
    internal static class HexUtility
    {
        internal static Vector3 LerpCube(Vector3Int a, Vector3Int b, float offsetFromCenter, float t)
        {
            float x = Mathf.Lerp(a.x + offsetFromCenter, b.x, t);
            float y = Mathf.Lerp(a.y, b.y, t);
            float z = Mathf.Lerp(a.z - offsetFromCenter, b.z, t);
            return new Vector3(x, y, z);
        }

        internal static Vector3Int RoundCube(Vector3 cubeFloatCoords)
        {
            int rx = Mathf.RoundToInt(cubeFloatCoords.x);
            int ry = Mathf.RoundToInt(cubeFloatCoords.y);
            int rz = Mathf.RoundToInt(cubeFloatCoords.z);

            float diffX = Mathf.Abs(rx - cubeFloatCoords.x);
            float diffY = Mathf.Abs(ry - cubeFloatCoords.y);
            float diffZ = Mathf.Abs(rz - cubeFloatCoords.z);

            if (diffX > diffY && diffX > diffZ)
            {
                rx = -ry - rz;
            }
            else if (diffY > diffZ)
            {
                ry = -rx - rz;
            }
            else
            {
                rz = -rx - ry;
            }

            return new Vector3Int(rx, ry, rz);
        }

        internal static Vector3Int GetCloserTargetPositionIncludingHorizontalWrap(Vector3Int origin, Vector3Int target, int mapSizeX)
        {
            //if horizontal distance in offset coordinates is < than half mapSize then the original targetPosition is closer
            Vector2Int originOffsetCoord = HexConverter.CubeCoordToOffsetCoord(origin);
            Vector2Int targetOffsetCoord = HexConverter.CubeCoordToOffsetCoord(target);
            int distance = Mathf.Abs(originOffsetCoord.x - targetOffsetCoord.x);
            if (distance * 2 <= mapSizeX) return target;

            //now we check if the target is "right" or "left of the origin and shift its imaginary position to the opposite
            if (originOffsetCoord.x < targetOffsetCoord.x) //target is right of the origin so we shift it left!
            {
                targetOffsetCoord.x -= mapSizeX;
            }
            else targetOffsetCoord.x += mapSizeX; //target is left of the origin  so we shift it right
            return HexConverter.OffsetCoordToCubeCoord(targetOffsetCoord);
        }
    }
}
 