using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshRenderer))]
public class SquareRenderer : MonoBehaviour
{
	private Mesh m_mesh;
	private MeshFilter m_meshFilter;
	private MeshRenderer m_meshRenderer;

	public List<Face> m_faces;

	public float innerSize, outerSize, height;

	public Material material;
	private void Awake()
	{
		m_meshFilter = GetComponent<MeshFilter>();
		m_meshRenderer = GetComponent<MeshRenderer>();

		m_mesh = new Mesh();
		m_mesh.name = "USquare";

		List<Face> m_face = new List<Face>();
		m_face.Add(new Face(new Vector3(1, 0, 0), new Vector3(0.8f, 0, 0), new Vector3(0, 0, 0.8f)));
		m_face.Add(new Face(new Vector3(0, 0, 1), new Vector3(1f, 0, 0), new Vector3(0, 0, 0.8f)));


		List<Vector3> vertices = new List<Vector3>();
		List<int> trian = new List<int>();
		List<Vector2> uvs = new List<Vector2>();
		for(int i = 0; i < m_face.Count; i++)
		{
			Face f = m_face[i];
			vertices.AddRange(f.vertices);
			uvs.AddRange(f.uvs);
			for(int j = 0; j < f.triangles.Count; j++)
			{
				trian.Add(f.triangles[j] + i*3);
			}
		}

		m_mesh.vertices = vertices.ToArray();
		m_mesh.uv = uvs.ToArray();
		m_mesh.triangles = trian.ToArray();

		m_meshFilter.mesh = m_mesh;
		m_meshRenderer.material = material;
	}
	public void OnValidate()
	{
		if (Application.isPlaying)
		{
			DrawMesh();     //Debug.Log("DrawMesh()");
		}
	}
	public void DrawMesh()
	{

	}
}
