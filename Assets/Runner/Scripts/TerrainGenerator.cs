using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HyperCasual.Runner
{
    public class TerrainBuilder : MonoBehaviour
    {
        public struct TerrainDimensions
        {
            public float Width;
            public float Length;
            public float StartBuffer;
            public float EndBuffer;
            public float Thickness;
        }

        public static void BuildTerrain(TerrainDimensions dimensions, Material terrainMaterial, ref GameObject terrainObject)
        {
            float width = dimensions.Width;
            float length = dimensions.Length;
            float startBuffer = dimensions.StartBuffer;
            float endBuffer = dimensions.EndBuffer;
            float thickness = dimensions.Thickness;

            Mesh mesh = new Mesh();
            mesh.name = "Terrain";
            Vector3[] vertices = new Vector3[24];
            Vector3[] normals = new Vector3[24];
            Vector2[] uvs = new Vector2[24];
            int[] triangles = new int[36];

            float halfWidth = width * 0.5f;
            float startEdge = -startBuffer;
            float endEdge = length + endBuffer;

            Vector3 upperStartLeft = new Vector3(-halfWidth, 0.0f, startEdge);
            Vector3 upperStartRight = new Vector3(halfWidth, 0.0f, startEdge);
            Vector3 lowerStartLeft = new Vector3(-halfWidth, -thickness, startEdge);
            Vector3 lowerStartRight = new Vector3(halfWidth, -thickness, startEdge);

            Vector3 upperEndLeft = new Vector3(-halfWidth, 0.0f, endEdge);
            Vector3 upperEndRight = new Vector3(halfWidth, 0.0f, endEdge);
            Vector3 lowerEndLeft = new Vector3(-halfWidth, -thickness, endEdge);
            Vector3 lowerEndRight = new Vector3(halfWidth, -thickness, endEdge);

            Vector3 upNormal = Vector3.up;
            Vector3 rightNormal = Vector3.right;
            Vector3 forwardNormal = Vector3.forward;
            Vector3 downNormal = -upNormal;
            Vector3 leftNormal = -rightNormal;
            Vector3 backwardNormal = -forwardNormal;

            int vertexIndex = 0;
            int triangleIndex = 0;

            vertices[vertexIndex + 0] = upperStartLeft;
            vertices[vertexIndex + 1] = upperEndLeft;
            vertices[vertexIndex + 2] = upperEndRight;
            vertices[vertexIndex + 3] = upperStartRight;

            normals[vertexIndex + 0] = upNormal;
            normals[vertexIndex + 1] = upNormal;
            normals[vertexIndex + 2] = upNormal;
            normals[vertexIndex + 3] = upNormal;

            uvs[vertexIndex + 0] = new Vector2(0.0f, startEdge);
            uvs[vertexIndex + 1] = new Vector2(0.0f, endEdge);
            uvs[vertexIndex + 2] = new Vector2(1.0f, endEdge);
            uvs[vertexIndex + 3] = new Vector2(1.0f, startEdge);

            triangles[triangleIndex + 0] = vertexIndex;
            triangles[triangleIndex + 1] = vertexIndex + 1;
            triangles[triangleIndex + 2] = vertexIndex + 2;

            triangles[triangleIndex + 3] = vertexIndex;
            triangles[triangleIndex + 4] = vertexIndex + 2;
            triangles[triangleIndex + 5] = vertexIndex + 3;

            vertexIndex += 4;
            triangleIndex += 6;

            vertices[vertexIndex + 0] = lowerStartLeft;
            vertices[vertexIndex + 1] = lowerEndLeft;
            vertices[vertexIndex + 2] = lowerEndRight;
            vertices[vertexIndex + 3] = lowerStartRight;

            normals[vertexIndex + 0] = downNormal;
            normals[vertexIndex + 1] = downNormal;
            normals[vertexIndex + 2] = downNormal;
            normals[vertexIndex + 3] = downNormal;

            uvs[vertexIndex + 0] = new Vector2(0.0f, startEdge);
            uvs[vertexIndex + 1] = new Vector2(0.0f, endEdge);
            uvs[vertexIndex + 2] = new Vector2(1.0f, endEdge);
            uvs[vertexIndex + 3] = new Vector2(1.0f, startEdge);

            triangles[triangleIndex + 0] = vertexIndex;
            triangles[triangleIndex + 1] = vertexIndex + 2;
            triangles[triangleIndex + 2] = vertexIndex + 1;

            triangles[triangleIndex + 3] = vertexIndex;
            triangles[triangleIndex + 4] = vertexIndex + 3;
            triangles[triangleIndex + 5] = vertexIndex + 2;

            vertexIndex += 4;
            triangleIndex += 6;

            vertices[vertexIndex + 0] = upperStartRight;
            vertices[vertexIndex + 1] = upperEndRight;
            vertices[vertexIndex + 2] = lowerEndRight;
            vertices[vertexIndex + 3] = lowerStartRight;

            normals[vertexIndex + 0] = rightNormal;
            normals[vertexIndex + 1] = rightNormal;
            normals[vertexIndex + 2] = rightNormal;
            normals[vertexIndex + 3] = rightNormal;

            uvs[vertexIndex + 0] = new Vector2(1.0f, startEdge);
            uvs[vertexIndex + 1] = new Vector2(1.0f, endEdge);
            uvs[vertexIndex + 2] = new Vector2(1.0f - thickness, endEdge);
            uvs[vertexIndex + 3] = new Vector2(1.0f - thickness, startEdge);

            triangles[triangleIndex + 0] = vertexIndex;
            triangles[triangleIndex + 1] = vertexIndex + 1;
            triangles[triangleIndex + 2] = vertexIndex + 2;

            triangles[triangleIndex + 3] = vertexIndex;
            triangles[triangleIndex + 4] = vertexIndex + 2;
            triangles[triangleIndex + 5] = vertexIndex + 3;

            vertexIndex += 4;
            triangleIndex += 6;

            vertices[vertexIndex + 0] = lowerStartLeft;
            vertices[vertexIndex + 1] = lowerEndLeft;
            vertices[vertexIndex + 2] = upperEndLeft;
            vertices[vertexIndex + 3] = upperStartLeft;

            normals[vertexIndex + 0] = leftNormal;
            normals[vertexIndex + 1] = leftNormal;
            normals[vertexIndex + 2] = leftNormal;
            normals[vertexIndex + 3] = leftNormal;

            uvs[vertexIndex + 0] = new Vector2(-thickness, startEdge);
            uvs[vertexIndex + 1] = new Vector2(-thickness, endEdge);
            uvs[vertexIndex + 2] = new Vector2(0.0f, endEdge);
            uvs[vertexIndex + 3] = new Vector2(0.0f, startEdge);

            triangles[triangleIndex + 0] = vertexIndex;
            triangles[triangleIndex + 1] = vertexIndex + 1;
            triangles[triangleIndex + 2] = vertexIndex + 2;

            triangles[triangleIndex + 3] = vertexIndex;
            triangles[triangleIndex + 4] = vertexIndex + 2;
            triangles[triangleIndex + 5] = vertexIndex + 3;

            vertexIndex += 4;
            triangleIndex += 6;

            vertices[vertexIndex + 0] = lowerStartLeft;
            vertices[vertexIndex + 1] = upperStartLeft;
            vertices[vertexIndex + 2] = upperStartRight;
            vertices[vertexIndex + 3] = lowerStartRight;

            normals[vertexIndex + 0] = backwardNormal;
            normals[vertexIndex + 1] = backwardNormal;
            normals[vertexIndex + 2] = backwardNormal;
            normals[vertexIndex + 3] = backwardNormal;

            uvs[vertexIndex + 0] = new Vector2(0.0f, 0.0f);
            uvs[vertexIndex + 1] = new Vector2(0.0f, thickness);
            uvs[vertexIndex + 2] = new Vector2(1.0f, thickness);
            uvs[vertexIndex + 3] = new Vector2(1.0f, 0.0f);

            triangles[triangleIndex + 0] = vertexIndex;
            triangles[triangleIndex + 1] = vertexIndex + 1;
            triangles[triangleIndex + 2] = vertexIndex + 2;

            triangles[triangleIndex + 3] = vertexIndex;
            triangles[triangleIndex + 4] = vertexIndex + 2;
            triangles[triangleIndex + 5] = vertexIndex + 3;

            vertexIndex += 4;
            triangleIndex += 6;

            vertices[vertexIndex + 0] = lowerEndRight;
            vertices[vertexIndex + 1] = upperEndRight;
            vertices[vertexIndex + 2] = upperEndLeft;
            vertices[vertexIndex + 3] = lowerEndLeft;

            normals[vertexIndex + 0] = forwardNormal;
            normals[vertexIndex + 1] = forwardNormal;
            normals[vertexIndex + 2] = forwardNormal;
            normals[vertexIndex + 3] = forwardNormal;

            uvs[vertexIndex + 0] = new Vector2(0.0f, 0.0f);
            uvs[vertexIndex + 1] = new Vector2(0.0f, thickness);
            uvs[vertexIndex + 2] = new Vector2(1.0f, thickness);
            uvs[vertexIndex + 3] = new Vector2(1.0f, 0.0f);

            triangles[triangleIndex + 0] = vertexIndex;
            triangles[triangleIndex + 1] = vertexIndex + 1;
            triangles[triangleIndex + 2] = vertexIndex + 2;

            triangles[triangleIndex + 3] = vertexIndex;
            triangles[triangleIndex + 4] = vertexIndex + 2;
            triangles[triangleIndex + 5] = vertexIndex + 3;

            mesh.vertices = vertices;
            mesh.normals = normals;
            mesh.triangles = triangles;
            mesh.uv = uvs;

            if (terrainObject != null)
            {
                if (Application.isPlaying)
                {
                    Object.Destroy(terrainObject);
                }
                else
                {
                    Object.DestroyImmediate(terrainObject);
                }
            }

            terrainObject = new GameObject("Terrain");
            MeshFilter meshFilter = terrainObject.AddComponent<MeshFilter>();
            meshFilter.sharedMesh = mesh;
            MeshRenderer meshRenderer = terrainObject.AddComponent<MeshRenderer>();
            meshRenderer.sharedMaterial = terrainMaterial;
        }
    }
}