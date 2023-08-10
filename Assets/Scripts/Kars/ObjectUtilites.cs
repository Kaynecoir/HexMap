using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Karsss.Debug;

namespace Karsss.Object
{
	static class KarsMesh
	{
		public static void CreateEmptyMeshArray(int quadCount, out Vector3[] vertices, out Vector2[] uv, out int[] triangles)
		{
			vertices = new Vector3[4 * quadCount];
			uv = new Vector2[4 * quadCount];
			triangles = new int[6 * quadCount];
		}
		public static void CreateEmptyMeshArray(int quadCount, out Vector3[] vertices, out Vector3[] normals, out Vector2[] uv, out int[] triangles)
		{
			normals = new Vector3[4 * quadCount];
			CreateEmptyMeshArray(quadCount, out vertices, out uv, out triangles);
		}
		public static void AddToMeshArray(Vector3[] vertices, Vector3[] normals, Vector2[] uv, int[] triangles, int index, Vector3 worldPosition, float f, Vector3 tileSize, Vector3 v3, Vector2 imagePoint)
		{
			normals[index * 4 + 0] = Vector3.back;
			normals[index * 4 + 1] = Vector3.back;
			normals[index * 4 + 2] = Vector3.back;
			normals[index * 4 + 3] = Vector3.back;

			AddToMeshArray(vertices, uv, triangles, index, worldPosition, f, tileSize, v3, imagePoint);
		}
		public static void AddToMeshArray(Vector3[] vertices, Vector2[] uv, int[] triangles, int index, Vector3 worldPosition, float f, Vector3 tileSize, Vector3 v3, Vector2 imagePoint)
		{
			tileSize *= .5f;
			vertices[index * 4 + 0] = new Vector3(-tileSize.x, -tileSize.y) + worldPosition;
			vertices[index * 4 + 1] = new Vector3(-tileSize.x, tileSize.y) + worldPosition;
			vertices[index * 4 + 2] = new Vector3(tileSize.x, tileSize.y) + worldPosition;
			vertices[index * 4 + 3] = new Vector3(tileSize.x, -tileSize.y) + worldPosition;

			uv[index * 4 + 0] = imagePoint + new Vector2(0.0f, 0.0f);
			uv[index * 4 + 1] = imagePoint + new Vector2(0.0f, 1.0f);
			uv[index * 4 + 2] = imagePoint + new Vector2(0.0f, 1.0f);
			uv[index * 4 + 3] = imagePoint + new Vector2(0.0f, 0.0f);

			triangles[index * 6 + 0] = index * 4 + 0;
			triangles[index * 6 + 1] = index * 4 + 1;
			triangles[index * 6 + 2] = index * 4 + 2;

			triangles[index * 6 + 3] = index * 4 + 0;
			triangles[index * 6 + 4] = index * 4 + 2;
			triangles[index * 6 + 5] = index * 4 + 3;
		}
	}
}

