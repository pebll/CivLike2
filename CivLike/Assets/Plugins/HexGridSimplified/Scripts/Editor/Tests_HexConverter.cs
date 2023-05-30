using UnityEngine;
using UnityEditor;
using UnityEngine.TestTools;
using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;


namespace Wunderwunsch.HexGridSimplified
{ 
    public class Tests_HexConverter
    {
        [Test]
        public void WorldToOffset()
        {
            //TODO: add a few more positions
            List<Vector3> knownWorldPositions = new List<Vector3>();
            List<Vector2Int> knownOffsetCoords = new List<Vector2Int>();
            knownWorldPositions.Add(new Vector3(0, 0, -0.6f));
            knownWorldPositions.Add(new Vector3(-1.2f, 0, -1.6f));
            knownWorldPositions.Add(new Vector3(4.8f, 0, 0.7f));
            knownWorldPositions.Add(new Vector3(9.5f, 0, 4.8f));
    
            knownOffsetCoords.Add(new Vector2Int(0, 0));
            knownOffsetCoords.Add(new Vector2Int(-1, -1));
            knownOffsetCoords.Add(new Vector2Int(3, 0));
            knownOffsetCoords.Add(new Vector2Int(5, 3));
    
            for (int i = 0; i < knownOffsetCoords.Count; i++)
            {

                Vector2Int o = HexConverter.WorldPositionToOffsetCoord(knownWorldPositions[i]);
                Assert.AreEqual(o, knownOffsetCoords[i]);
            }
        }

        [Test]
        public void WorldToCube()
        {
            List<Vector3> knownWorldPositions = new List<Vector3>();
            List<Vector3Int> knownCubeCoords = new List<Vector3Int>();
            knownWorldPositions.Add(new Vector3(8.6f, 0, 5.7f));
            knownWorldPositions.Add(new Vector3(0.3f, 0, 4.5f));
            knownWorldPositions.Add(new Vector3(7.4f, 0, -1.5f));
            knownWorldPositions.Add(new Vector3(-1.8f, 0, -2.9f));
            knownWorldPositions.Add(new Vector3(-5.7f, 0, 4.5f));


            knownCubeCoords.Add(new Vector3Int(3, 4, -7));
            knownCubeCoords.Add(new Vector3Int(-1, 3, -2));
            knownCubeCoords.Add(new Vector3Int(5, -1, -4));
            knownCubeCoords.Add(new Vector3Int(0, -2, 2));
            knownCubeCoords.Add(new Vector3Int(-5, 3, 2));

            for (int i = 0; i < knownCubeCoords.Count; i++)
            {
                Vector3Int c = HexConverter.WorldPositionToCubeCoord(knownWorldPositions[i]);
                Assert.AreEqual(c.x + c.y + c.z, 0);
                Assert.AreEqual(c, knownCubeCoords[i]);
            }
        }

        [Test]
        public void CubeToWorld()
        {
            List<Vector3Int> knownCubeCoords = new List<Vector3Int>();
            List<Vector3> knownCenterOfTile = new List<Vector3>();

            knownCubeCoords.Add(new Vector3Int(0, 0, 0));
            knownCubeCoords.Add(new Vector3Int(1, 5, -6));
            knownCubeCoords.Add(new Vector3Int(3, 4, -7));
            knownCubeCoords.Add(new Vector3Int(-1, 3, -2));
            knownCubeCoords.Add(new Vector3Int(2, -3, 1));
            knownCubeCoords.Add(new Vector3Int(-4, -2, 6));

            knownCenterOfTile.Add(new Vector3(0f, 0, 0f));
            knownCenterOfTile.Add(new Vector3(Constants.sqrt3 * (1 + (5 / 2f)), 0, 7.5f));
            knownCenterOfTile.Add(new Vector3(Constants.sqrt3 * (3 + (4 / 2f)), 0, 6f));
            knownCenterOfTile.Add(new Vector3(Constants.sqrt3 * (-1 + (3 / 2f)), 0, 4.5f));
            knownCenterOfTile.Add(new Vector3(Constants.sqrt3 * (2 + (-3 / 2f)), 0, -4.5f));
            knownCenterOfTile.Add(new Vector3(Constants.sqrt3 * (-4 + (-2 / 2f)), 0, -3f));

            for (int i = 0; i < knownCubeCoords.Count; i++)
            {
                Vector3 center = HexConverter.CubeCoordToWorldPosition(knownCubeCoords[i]);
                Assert.AreEqual(center, knownCenterOfTile[i]);
            }
        }

        [Test]
        public void CubeToOffset()
        {
            for (int i = 0; i < 1000; i++)
            {
                TestCoords coords = GenerateTestValues();
                Vector2Int o = HexConverter.CubeCoordToOffsetCoord(coords.cube);
                Assert.AreEqual(o, coords.offset);
            }
        }

        [Test]
        public void OffsetToWorld()
        {
            List<Vector2Int> knownOffsetCoords = new List<Vector2Int>();
            List<Vector3> knownCenterOfTile = new List<Vector3>();

            float offset = 0.5f * Constants.sqrt3;

            knownOffsetCoords.Add(new Vector2Int(0, 0));
            knownOffsetCoords.Add(new Vector2Int(1, 3));
            knownOffsetCoords.Add(new Vector2Int(3, 4));
            knownOffsetCoords.Add(new Vector2Int(-1, 3));
            knownOffsetCoords.Add(new Vector2Int(2, -3));
            knownOffsetCoords.Add(new Vector2Int(-4, -2));

            knownCenterOfTile.Add(new Vector3(0f, 0, 0f));
            knownCenterOfTile.Add(new Vector3(1.5f * Constants.sqrt3, 0, 4.5f));
            knownCenterOfTile.Add(new Vector3(3f * Constants.sqrt3, 0, 6f));
            knownCenterOfTile.Add(new Vector3(-0.5f * Constants.sqrt3, 0, 4.5f));
            knownCenterOfTile.Add(new Vector3(2.5f * Constants.sqrt3, 0, -4.5f));
            knownCenterOfTile.Add(new Vector3(-4f * Constants.sqrt3, 0, -3f));

            for (int i = 0; i < knownOffsetCoords.Count; i++)
            {
                Vector3 center = HexConverter.OffsetCoordToWorldPosition(knownOffsetCoords[i]);
                Assert.AreEqual(center, knownCenterOfTile[i]);
            }
        }

        [Test]
        public void OffsetToCube()
        {
            for (int i = 0; i < 1000; i++)
            {
                TestCoords coords = GenerateTestValues();
                Vector3Int c = HexConverter.OffsetCoordToCubeCoord(coords.offset);
                Assert.AreEqual(c, coords.cube);
                Assert.AreEqual(c.x + c.y + c.z, 0);
            }
        }

        public TestCoords GenerateTestValues()
        {
            Vector3 worldPos = new Vector3(Random.Range(-100, 100), 0, Random.Range(-100, 100));
            Vector2Int offset = HexConverter.WorldPositionToOffsetCoord(worldPos);
            Vector3Int cube = HexConverter.WorldPositionToCubeCoord(worldPos);
            return new TestCoords(worldPos, offset,  cube);
        }


        public struct TestCoords
        {
            public readonly Vector3 worldPos;
            public readonly Vector2Int offset;
            public readonly Vector3Int cube;

            public TestCoords(Vector3 wp, Vector2Int o, Vector3Int c)
            {
                worldPos = wp;
                offset = o;
                cube = c;
            }
        }
    }
}
