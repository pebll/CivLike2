using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Wunderwunsch.HexGridSimplified
{
    /// Helperclass to Create Meshdata of Regular Convex Polygons, st
    public class Polygon
    {
        public Vector3[] Vertices { get; private set; }
        public int[] Triangles { get; private set; }
        public Vector2[] UVs { get; private set; }

        private Polygon(Vector3[] vertices, int[] tris)
        {
            this.Vertices = vertices;
            this.Triangles = tris;
            this.UVs = new Vector2[vertices.Length];
        }

        private Polygon(Vector3[] vertices, int[] tris, Vector2[] uvs)
        {
            this.Vertices = vertices;
            this.Triangles = tris;
            this.UVs = uvs;
        }

        public static Polygon CreateNGon(int sides, Vector3 PositionFirstVertex, Vector3 rotationalAxis, bool addCentroid)
        {
            Vector3[] verts = CreateVertices(PositionFirstVertex, rotationalAxis, sides, addCentroid);
            int[] triangles = CreateTriangles(sides, addCentroid, true);
            Polygon polygon = new Polygon(verts, triangles);
            return polygon;
        }

        /// <summary>
        /// Returns the vertices of a regular convex polygon with the coordinate origin as center 
        /// </summary>
        /// <param name="PositionFirstVertex"></param>
        /// <param name="rotationAxis"></param>
        /// <param name="sides"></param>
        /// <param name="addCentroid">adds a vertex in the middle</param>
        private static Vector3[] CreateVertices(Vector3 PositionFirstVertex, Vector3 rotationAxis, int sides, bool addCentroid)
        {
            if (sides < 3) throw new System.ArgumentOutOfRangeException("Polygons need to have at least 3 Sides to not be degeneratee");

            int vertexCount = addCentroid ? sides + 1 : sides;
            float anglePerStep = 360f / sides;
            Vector3[] verts = new Vector3[vertexCount];

            for (int i = 0; i < sides; i++)
            {
                verts[i] = Quaternion.AngleAxis(anglePerStep * i, rotationAxis) * PositionFirstVertex;
            }
            return verts;
            //Centroid is 0,0,0 which is default value of Vector3 so we don't need to actually do anything for that point
        }

        private static int[] CreateTriangles(int sides, bool lastIsCentroid, bool clockwise)
        {
            int triangleAmount = lastIsCentroid ? sides : sides - 2;
            int[] triangles = new int[triangleAmount * 3]; //every 3 entries = 1 Triangle. (0,1,2 = first trianlge; 3,4,5 = second Triangle)

            for (int i = 0; i < triangleAmount; i++)
            {
                if (lastIsCentroid)
                {
                    int a = sides;
                    int b = i;
                    int c = (i + 1) % sides;

                    if (clockwise)
                    {
                        triangles[3 * i] = a;
                        triangles[3 * i + 1] = b;
                        triangles[3 * i + 2] = c;

                    }
                    else
                    {
                        triangles[3 * i] = a;
                        triangles[3 * i + 1] = c;
                        triangles[3 * i + 2] = b;
                    }
                }

                else
                {
                    int a = 0;
                    int b = i + 1;
                    int c = i + 2;

                    if (clockwise)
                    {
                        triangles[3 * i] = a;
                        triangles[3 * i + 1] = b;
                        triangles[3 * i + 2] = c;
                    }
                    else
                    {
                        triangles[3 * i] = a;
                        triangles[3 * i + 1] = c;
                        triangles[3 * i + 2] = b;

                    }
                }
            }
            return triangles;
        }

        /// <summary>
        /// combines multiple Polygons into one
        /// </summary>
        public static Polygon Combine(List<Polygon> polygons)
        {
            int offset = 0;
            List<Vector3> points = new List<Vector3>();
            List<int> triangles = new List<int>();
            List<Vector2> uvs = new List<Vector2>();
            foreach (Polygon data in polygons)
            {
                Polygon adjusted = data.OffsetTriangleIndices(offset);
                offset += adjusted.Vertices.Length;
                points.AddRange(adjusted.Vertices);
                uvs.AddRange(adjusted.UVs);
                triangles.AddRange(adjusted.Triangles);
            }
            Polygon combined = new Polygon(points.ToArray(), triangles.ToArray());
            return combined;
        }

        /// <summary>
        /// returns new Polygon with offset position of all vertices
        /// </summary>
        public Polygon OffsetPosition(Vector3 offset)
        {
            Vector3[] newVerts = new Vector3[Vertices.Length];
            for (int i = 0; i < Vertices.Length; i++)
            {
                newVerts[i] = Vertices[i] + offset;
            }
            return new Polygon(newVerts, Triangles);
        }

        /// <summary>
        /// returns new Polygon with offset Triangle Indices - used for merging multiple Polygons into one
        /// </summary>
        private Polygon OffsetTriangleIndices(int offset)
        {
            int[] newTris = new int[Triangles.Length];
            for (int i = 0; i < newTris.Length; i++)
            {
                newTris[i] = Triangles[i] + offset;
            }
            return new Polygon(Vertices, newTris);
        }

        public Mesh ToMesh()
        {
            Mesh mesh = new Mesh
            {
                vertices = Vertices,
                triangles = Triangles
            };
            mesh.RecalculateNormals();
            mesh.RecalculateBounds();
            //blabla UVs, blabla tangents.
            ;
            return mesh;
        }
    }
}
