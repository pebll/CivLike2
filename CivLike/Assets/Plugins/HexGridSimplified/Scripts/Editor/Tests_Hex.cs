using UnityEngine;
using UnityEditor;
using UnityEngine.TestTools;
using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Wunderwunsch.HexGridSimplified
{
    public class Tests_Hex
    {
        [Test]
        public void Distance_WithoutWrap()
        {
            Hex.SetMapAttributes(new Vector2Int(30, 20), false);
            Vector3Int A1 = new Vector3Int(0, 0, 0);
            Vector3Int B1 = new Vector3Int(2, 3, -5);
            Vector3Int A2 = new Vector3Int(3, 4, -7);
            Vector3Int B2 = new Vector3Int(12, 15, -27);
            Vector3Int A3 = new Vector3Int(0, 0, 0);
            Vector3Int B3 = new Vector3Int(29, 19, -48);
            Vector3Int A4 = new Vector3Int(23, 3, -26);
            Vector3Int B4 = new Vector3Int(1, 1, -2);
            int ResultT1 = 5;
            int ResultT2 = 20;
            int ResultT3 = 48;
            int ResultT4 = 24;
            Assert.AreEqual(ResultT1, Hex.Distance(A1, B1));
            Assert.AreEqual(ResultT2, Hex.Distance(A2, B2));
            Assert.AreEqual(ResultT3, Hex.Distance(A3, B3));
            Assert.AreEqual(ResultT4, Hex.Distance(A4, B4));
        }

        [Test]
        public void Distance_WithWrap()
        {
            Hex.SetMapAttributes(new Vector2Int(30, 20), true);
            Vector3Int A1 = new Vector3Int(0, 0, 0);
            Vector3Int B1 = new Vector3Int(2, 3, -5);
            Vector3Int A2 = new Vector3Int(3, 4, -7);
            Vector3Int B2 = new Vector3Int(12, 15, -27);
            Vector3Int A3 = new Vector3Int(0, 0, 0);
            Vector3Int B3 = new Vector3Int(29, 19, -48);
            Vector3Int A4 = new Vector3Int(23, 3, -26);
            Vector3Int B4 = new Vector3Int(1, 1, -2);
            int ResultT1 = 5;
            int ResultT2 = 20;
            int ResultT3 = 19;
            int ResultT4 = 8;
            Assert.AreEqual(ResultT1, Hex.Distance(A1, B1));
            Assert.AreEqual(ResultT2, Hex.Distance(A2, B2));
            Assert.AreEqual(ResultT3, Hex.Distance(A3, B3));
            Assert.AreEqual(ResultT4, Hex.Distance(A4, B4));
        }

        [Test]
        public void GetNeighbours_WithoutWrap()
        {
            Hex.SetMapAttributes(new Vector2Int(30, 20), false);

            Vector3Int P1 = new Vector3Int(0, 0, 0);
            Vector3Int P2 = new Vector3Int(3, 5, -8);
            Vector3Int P3 = new Vector3Int(20, 19, -39);

            Vector3Int P4 = new Vector3Int(10, 0, -10);
            List<Vector3Int> N1 = new List<Vector3Int> { new Vector3Int(1, 0, -1), new Vector3Int(0, 1, -1) };
            List<Vector3Int> N2 = new List<Vector3Int> { new Vector3Int(2, 5, -7), new Vector3Int(2, 6, -8), new Vector3Int(3, 6, -9), new Vector3Int(4, 5, -9) , new Vector3Int(4, 4, -8), new Vector3Int(3, 4, -7) };
            List<Vector3Int> N3 = new List<Vector3Int> { new Vector3Int(19, 19, -38), new Vector3Int(20, 18, -38) };
            List<Vector3Int> N4 = new List<Vector3Int> { new Vector3Int(9, 0, -9), new Vector3Int(11, 0, -11), new Vector3Int(9, 1, -10), new Vector3Int(10, 1, -11) };
            //TODO: N3: CornerTopRight

            CollectionAssert.AreEquivalent(N1, Hex.GetNeighbours(P1, true));
            CollectionAssert.AreEquivalent(N2, Hex.GetNeighbours(P2, true));
        }

        [Test]
        public void GetNeighbours_WithWrap()
        {
            Hex.SetMapAttributes(new Vector2Int(30, 20), true);

            Vector3Int P1 = new Vector3Int(0, 0, 0);
            Vector3Int P2 = new Vector3Int(3, 5, -8);
            Vector3Int P3 = new Vector3Int(20, 19, -39);

            Vector3Int P4 = new Vector3Int(10, 0, -10);
            List<Vector3Int> N1 = new List<Vector3Int> { new Vector3Int(1, 0, -1), new Vector3Int(0, 1, -1), new Vector3Int(29, 1, -30), new Vector3Int(29,0,-29) };
            List<Vector3Int> N2 = new List<Vector3Int> { new Vector3Int(2, 5, -7), new Vector3Int(2, 6, -8), new Vector3Int(3, 6, -9), new Vector3Int(4, 5, -9), new Vector3Int(4, 4, -8), new Vector3Int(3, 4, -7) };
            List<Vector3Int> N3 = new List<Vector3Int> { new Vector3Int(19, 19, -38), new Vector3Int(20, 18, -38), new Vector3Int(-9, 19, -10), new Vector3Int(-9, 18, -9) };
            List<Vector3Int> N4 = new List<Vector3Int> { new Vector3Int(9, 0, -9), new Vector3Int(11, 0, -11), new Vector3Int(9, 1, -10), new Vector3Int(10, 1, -11) };
            //TODO: N3: CornerTopRight

            CollectionAssert.AreEquivalent(N1, Hex.GetNeighbours(P1, true));
            CollectionAssert.AreEquivalent(N2, Hex.GetNeighbours(P2, true));

            CollectionAssert.AreEquivalent(N4, Hex.GetNeighbours(P4, true));

        }

        [Test]
        public void GetAllWithinRange_WithoutWrap()
        {
            Hex.SetMapAttributes(new Vector2Int(30, 20), false);
            throw new InconclusiveException("test not implemented yet");
        }

        [Test]
        public void GetAllWithinRange_WithWrap()
        {
            Hex.SetMapAttributes(new Vector2Int(30, 20), true);
            throw new InconclusiveException("test not implemented yet");
        }

        [Test]
        public void GetRing_WithoutWrap()
        {
            Hex.SetMapAttributes(new Vector2Int(30, 20), false);
            throw new InconclusiveException("test not implemented yet");
        }

        [Test]
        public void GetRing_WithWrap()
        {
            Hex.SetMapAttributes(new Vector2Int(30, 20), true);
            throw new InconclusiveException("test not implemented yet");
        }

        [Test]
        public void GetLine_WithoutWrap()
        {
            Hex.SetMapAttributes(new Vector2Int(30, 20), true);
            throw new InconclusiveException("test not implemented yet");
        }

        [Test]
        public void GetLine_WithWrap()
        {
            Hex.SetMapAttributes(new Vector2Int(30, 20), true);
            throw new InconclusiveException("test not implemented yet");
        }

        [Test]
        public void GetMirrorCoordinates_Horizontal()
        {
            throw new InconclusiveException("test not implemented yet");
        }

        [Test]
        public void GetMirrorCoordinates_Vertical()
        {
            throw new InconclusiveException("test not implemented yet");
        }

        [Test]
        public void GetMirrorCoordinates_HorizontalAndVertical()
        {
            throw new InconclusiveException("test not implemented yet");
        }


        [Test]
        public void GetEdgeCoordBetweenTiles_WithoutWrap()
        {
            Hex.SetMapAttributes(new Vector2Int(30, 20), false);
            Vector3Int A1 = new Vector3Int(0, 0, 0);
            Vector3Int B1 = new Vector3Int(1, 0, -1);
            Vector3Int I1 = new Vector3Int(2, 5, -7);

            Vector3Int A2 = new Vector3Int(0, 0, 0);
            Vector3Int B2 = new Vector3Int(29, 0, -29);

            Vector3Int A3 = new Vector3Int(5, 3, -8);
            Vector3Int B3 = new Vector3Int(4, 4, -8);

            Vector3Int E1 = new Vector3Int(1, 0, -1);
            Vector3Int E3 = new Vector3Int(9, 7, -16);

            Vector3Int A4 = new Vector3Int(29, 0, -29);
            Vector3Int B4 = new Vector3Int(0, 0, 0);

            Assert.AreEqual(E1,Hex.GetEdgeCoordBetween(A1, B1));
            Assert.AreEqual(E3, Hex.GetEdgeCoordBetween(A3, B3));
            Assert.Throws<System.ArgumentException>(() => Hex.GetEdgeCoordBetween(A1,I1));
            Assert.Throws<System.ArgumentException>(() => Hex.GetEdgeCoordBetween(A2,B2));
            Assert.Throws<System.ArgumentException>(() => Hex.GetEdgeCoordBetween(A4, B4));
        }

        [Test]
        public void GetEdgeCoordBetweenTiles_WithWrap()
        {
            Hex.SetMapAttributes(new Vector2Int(30, 20), true);
            Vector3Int A1 = new Vector3Int(0, 0, 0);
            Vector3Int B1 = new Vector3Int(1, 0, -1);
            Vector3Int I1 = new Vector3Int(2, 5, -7);
            Vector3Int E1 = new Vector3Int(1, 0, -1);

            Vector3Int A2 = new Vector3Int(0, 0, 0);
            Vector3Int B2 = new Vector3Int(29, 0, -29);
            Vector3Int E2 = new Vector3Int(-1, 0, 1);

            Vector3Int A3 = new Vector3Int(5, 3, -8);
            Vector3Int B3 = new Vector3Int(4, 4, -8);
            Vector3Int E3 = new Vector3Int(9, 7, -16);

            Vector3Int A4 = new Vector3Int(29, 0, -29);
            Vector3Int B4 = new Vector3Int(0, 0, 0);
            Vector3Int E4 = new Vector3Int(-1, 0, 1);

            Assert.AreEqual(E1, Hex.GetEdgeCoordBetween(A1, B1));
            Assert.AreEqual(E2, Hex.GetEdgeCoordBetween(A2, B2));

            Assert.AreEqual(E3, Hex.GetEdgeCoordBetween(A3, B3));
            Assert.AreEqual(E4, Hex.GetEdgeCoordBetween(A4, B4));
            Assert.Throws<System.ArgumentException>(() => Hex.GetEdgeCoordBetween(A1, I1));
            
        }

        [Test]
        public void GetEdgeCoordsOfTile_WithoutWrap()
        {
            Debug.Log("still needs a for more test cases to be really solid");
            Hex.SetMapAttributes(new Vector2Int(30, 20), false);
            Vector3Int A1 = new Vector3Int(0, 0, 0);
            Vector3Int A2 = new Vector3Int(29, 0, -29);
            HashSet<Vector3Int> E1 = new HashSet<Vector3Int>() { new Vector3Int(0, 1, -1), new Vector3Int(1, 0, -1), new Vector3Int(1, -1, 0), new Vector3Int(0, -1, 1), new Vector3Int(-1, 0, 1), new Vector3Int(-1, 1, 0) };
            HashSet<Vector3Int> E2 = new HashSet<Vector3Int>() { new Vector3Int(59, 0, -59) , new Vector3Int(59, -1, -58), new Vector3Int(58, -1, -57), new Vector3Int(57, 0, -57) , new Vector3Int(57, 1, -58), new Vector3Int(58, 1, -59) };

            CollectionAssert.AreEquivalent(E1, Hex.GetEdgeCoordinatesOfTile(A1));
            CollectionAssert.AreEquivalent(E2, Hex.GetEdgeCoordinatesOfTile(A2));
        }

        [Test]
        public void GetEdgeCoordsOfTile_WithWrap()
        {
            Debug.Log("still needs a for more test cases to be really solid");
            Hex.SetMapAttributes(new Vector2Int(30, 20), true);
            Vector3Int A1 = new Vector3Int(0, 0, 0);
            Vector3Int A2 = new Vector3Int(29, 0, -29);
            HashSet<Vector3Int> E1 = new HashSet<Vector3Int>() { new Vector3Int(0, 1, -1), new Vector3Int(1, 0, -1), new Vector3Int(1, -1, 0), new Vector3Int(0, -1, 1), new Vector3Int(-1, 0, 1), new Vector3Int(-1, 1, 0) };
            HashSet<Vector3Int> E2 = new HashSet<Vector3Int>() { new Vector3Int(57, 0, -57) , new Vector3Int(57,1,-58) , new Vector3Int(58,1,-59), new Vector3Int(-1,0,1), new Vector3Int(59,-1,-58), new Vector3Int(58,-1,-57)};

            CollectionAssert.AreEquivalent(E1, Hex.GetEdgeCoordinatesOfTile(A1));
            CollectionAssert.AreEquivalent(E2, Hex.GetEdgeCoordinatesOfTile(A2));
        }

        [Test]
        public void GetAdjacentTilesOfEdge_WithoutWrap()
        {
            Hex.SetMapAttributes(new Vector2Int(30, 20), true);
            throw new InconclusiveException("test not implemented yet");
        }

        [Test]
        public void GetAdjacentTilesOfEdge_WithWrap()
        {
            Hex.SetMapAttributes(new Vector2Int(30, 20), true);
            throw new InconclusiveException("test not implemented yet");
        }
    }
}
