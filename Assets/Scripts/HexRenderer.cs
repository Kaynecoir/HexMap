using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshRenderer))]
public class HexRenderer : MonoBehaviour
{
    private Mesh m_mesh;
    private MeshFilter m_meshFilter;
    private MeshRenderer m_meshRenderer;

	public List<Face> m_faces; 

    public Material material;
	private void Awake()
	{
        m_meshFilter = GetComponent<MeshFilter>();
        m_meshRenderer = GetComponent<MeshRenderer>();

        m_mesh = new Mesh();
        m_mesh.name = "Hex";

        m_meshFilter.mesh = m_mesh;
        m_meshRenderer.material = material;
	}
	private void OnEnable()
	{
        DrawMesh();
	}
	public void OnValidate()
	{
		if(Application.isPlaying)
		{
			DrawMesh();
		}
	}

	public void DrawMesh()
	{
		DrawFaces();
		//CombineFaces();
	}
	private void DrawFaces()
	{
		List<Vector3> vertices = new List<Vector3>();
		List<int> tris = new List<int>();
		List<Vector2> uvs = new List<Vector2>();

		for(int i = 0; i < m_faces.Count; i++)
		{
			vertices.AddRange(m_faces[i].vertices);
			uvs.AddRange(m_faces[i].uvs);

			int offset = (4 * i);
			foreach(int triangle in m_faces[i].triangles)
			{
				tris.Add(triangle + offset);
			}
		}

		m_mesh.vertices = vertices.ToArray();
		m_mesh.triangles = tris.ToArray();
		m_mesh.uv = uvs.ToArray();
		m_mesh.RecalculateNormals(); 
	}
}
