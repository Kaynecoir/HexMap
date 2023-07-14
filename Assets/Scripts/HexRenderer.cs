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

	public float innerSize, outerSize, height;

	public Material material;
	private void Awake()
	{
        m_meshFilter = GetComponent<MeshFilter>();
        m_meshRenderer = GetComponent<MeshRenderer>();

        m_mesh = new Mesh();
        m_mesh.name = "Hex";
		//DrawMesh();

        m_meshFilter.mesh = m_mesh;
        m_meshRenderer.material = material;
		Debug.Log(m_mesh);
	}
	private void OnEnable()
	{
        DrawMesh();
	}
	public void OnValidate()
	{
		if(Application.isPlaying && m_mesh != null)
		{
			DrawMesh();		//Debug.Log("DrawMesh()");
		}
	}

	public void DrawMesh()
	{
		DrawFaces();
		CombineFaces();
	}
	private void DrawFaces()
	{
		m_faces = new List<Face>();

		for (int point = 0; point < 2; point++)
		{
			m_faces.AddRange(CreateFace(innerSize, outerSize, height / 2f, point));     //Debug.Log("CreateFace()" + point.ToString());
		}
		//Debug.Log("m_face: " + m_faces.Count);
	}
	private void CombineFaces()
	{
		List<Vector3> vertices = new List<Vector3>();
		List<int> tris = new List<int>();
		List<Vector2> uvs = new List<Vector2>();

		for(int i = 0; i < m_faces.Count; i++)
		{
			vertices.AddRange(m_faces[i].vertices.ToArray());		//Debug.Log("m_face[" + i + "].vertices: " + m_faces[i].vertices.Count);
			uvs.AddRange(m_faces[i].uvs);				//Debug.Log("m_face[" + i + "].uvs: " + m_faces[i].uvs.Count);

			int offset = (3 * i);
			foreach(int triangle in m_faces[i].triangles)
			{
				tris.Add(triangle + offset);
			}
		}

		//foreach()
		m_mesh.vertices = vertices.ToArray();	
		m_mesh.uv = uvs.ToArray();				
		m_mesh.triangles = tris.ToArray();		
		m_mesh.RecalculateNormals();
	}
	public List<Face> CreateFace(float innerRadius, float outerRadius, float height, int point, bool reverse = false)
	{
		List<Face> temp = new List<Face>();
		Vector3 pointA = GetPosition(innerRadius, height, point);
		Vector3 pointB = GetPosition(innerRadius, height, (point < 5) ? point + 1 : 0);
		Vector3 pointC = GetPosition(outerRadius, height, (point < 5) ? point + 1 : 0);
		Vector3 pointD = GetPosition(outerRadius, height, point);

		temp.Add(new Face(pointA, pointB, pointC));
		temp.Add(new Face(pointA, pointC, pointD));

		if (reverse)
		{
			//vertices.Reverse();
		}

		return temp;
	}
	protected Vector3 GetPosition(float size, float height, int index)
	{
		float angle_deg = 60 * index;
		float angle_rad = Mathf.PI / 180f * angle_deg;

		return new Vector3((size * Mathf.Cos(angle_rad)), height, (size * Mathf.Sin(angle_rad)));
	}
}
