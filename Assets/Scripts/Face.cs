using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct Face
{
	public List<Vector3> vertices { get; private set; }
	public List<int> triangles { get; private set; }
	public List<Vector2> uvs { get; private set; }

	public Face(List<Vector3> vertices, List<int> triangles, List<Vector2> uvs)
	{
		this.vertices = vertices;
		this.triangles = triangles;
		this.uvs = uvs;
	}
	public Face(Vector3 pointA, Vector3 pointB, Vector3 pointC)
	{
		this.vertices = new List<Vector3>();
		this.vertices.Add(pointA);
		this.vertices.Add(pointB);
		this.vertices.Add(pointC);

		this.uvs = new List<Vector2>();
		this.uvs.Add(new Vector2(1, 0));
		this.uvs.Add(new Vector2(0, 0));
		this.uvs.Add(new Vector2(0, 1));

		this.triangles = new List<int>();
		for (int i = 0; i < 3; i++) this.triangles.Add(i);
		for (int i = 2; i >= 0; i--) this.triangles.Add(i);


	}
	public Face(Vector3 pointA, Vector3 pointB, Vector3 pointC, Vector3 pointD)
	{
		this.vertices = new List<Vector3>();
		this.vertices.Add(pointA);
		this.vertices.Add(pointB);
		this.vertices.Add(pointC);
		this.vertices.Add(pointD);

		this.uvs = new List<Vector2>();
		this.uvs.Add(new Vector2(1, 0));
		this.uvs.Add(new Vector2(0, 0));
		this.uvs.Add(new Vector2(0, 1));
		this.uvs.Add(new Vector2(1, 1));

		this.triangles = new List<int>();
		for (int i = 0; i < 3; i++) this.triangles.Add(i);
		for (int i = 2; i >= 0; i--) this.triangles.Add(i);
		for (int i = 1; i < 4; i++) this.triangles.Add(i);
		for (int i = 3; i >= 1; i--) this.triangles.Add(i);
	}
}
