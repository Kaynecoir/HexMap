using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Kars.Debug;

namespace Kars
{
	namespace Object
	{
		class Hex
		{
			public HexGrid hexGrid;
			private int gridHeight, gridWidth;
			public Vector3 worldPosition;
			public Vector3[] corner { get; private set; }
			public List<Hex> neigbourHex;
			public int positionX { get; private set; }
			public int positionY { get; private set; }
			public float radius { get; private set; }
			public float littleRadius { get; protected set; }
			public float height { get; private set; }   // for 3D
			public bool isVertical { get; private set; }

			public Hex(float radius, bool isVertical = false)
			{
				this.radius = radius;
				this.littleRadius = radius * Mathf.Sin(Mathf.PI / 3);
				this.worldPosition = Vector3.zero;
				this.isVertical = isVertical;
				corner = new Vector3[6];

				//SetCorner();
			}
			public Hex(float radius, Vector3 worldPosition, bool isVertical = false)
			{
				this.radius = radius;
				this.littleRadius = radius * Mathf.Sin(Mathf.PI / 3);
				this.worldPosition = worldPosition;
				this.isVertical = isVertical;
				corner = new Vector3[6];

				SetCorner();
			}

			public void SetCorner()
			{
				for (int i = 0; i < 6; i++)
				{
					float angel_del = 60 * i + (isVertical ? 90 : 0);
					float angel_rad = angel_del / 180 * Mathf.PI;
					corner[i] = new Vector3(Mathf.Cos(angel_rad), Mathf.Sin(angel_rad)) * radius + worldPosition;
				}
			}
			public bool inTrianArea(float x, float y)
			{
				if (y < 0) return false;
				if (x < radius / 2 && x >= 0) return Mathf.Tan(Mathf.PI / 3) >= y / x;
				if (x >= radius / 2 && x <= radius) return Mathf.Tan(Mathf.PI / 3) >= y / (radius - x);
				return false;
			}
			public bool inHexArea(Vector3 pos)
			{
				return inHexArea(pos.x, pos.y);
			}
			public bool inHexArea(float x, float y)
			{
				float angel_del = isVertical ? 90 : 0;

				for (int i = 0; i < 6; i++)
				{
					DebugUtilites.RotateSysCoord(angel_del, ref x, ref y);
					if (inTrianArea(x, y)) return true;
					angel_del += 60;
				}
				return false;
			}


			public Mesh CreateHexMesh()
			{
				Mesh mesh = new Mesh();
				mesh.name = "Hex";
				Vector3[] vertices = new Vector3[7];
				Vector3[] normals = new Vector3[7];
				Vector2[] uv = new Vector2[7];
				for (int i = 0; i < 6; i++)
				{
					vertices[i] = corner[i];
					normals[i] = Vector3.back;
					uv[i] = corner[i] / radius / 2 + new Vector3(0.5f, 0.5f);
				}
				vertices[6] = worldPosition;
				normals[6] = Vector3.back;
				uv[6] = new Vector2(0.5f, 0.5f);

				int[] triangles = new int[18];
				for (int i = 0; i < 6; i++)
				{
					triangles[i * 3 + 0] = 6;
					triangles[i * 3 + 1] = i == 5 ? 0 : i + 1;
					triangles[i * 3 + 2] = i;
				}

				mesh.vertices = vertices;
				mesh.normals = normals;
				mesh.uv = uv;
				mesh.triangles = triangles;

				return mesh;
			}
		}

	}
}
