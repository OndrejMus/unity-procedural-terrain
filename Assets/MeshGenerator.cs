using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter))]
public class MeshGenerator : MonoBehaviour
{
    Mesh mesh;
    Vector3[] vertices;
    int[] triangles;
    
    // uvs -> mapping 2d image to mesh surface
    //Vector2[] uvs;

    Color[] colors;

    public int xSize = 256;
    public int zSize = 256;
    public int ySize = 20;
    public float scale = 20f;
    public float offsetX = 100f;
    public float offsetZ = 100f;

    public Gradient gradient;
    float minTerrainHeight;
    float maxTerrainHeight;

    void Start()
    {
        offsetX = Random.Range(0f,9999f);
        offsetZ = Random.Range(0f,9999f);
    }

    // Start is called before the first frame update
    void Update()
    {
        mesh = new Mesh();
        GetComponent<MeshFilter>().mesh = mesh;
        CreateShape();
        UpdateMesh();
    }

    void CreateShape() 
    {
        minTerrainHeight = 9999f;
        maxTerrainHeight = -9999f;
        vertices = new Vector3[(xSize+1)*(zSize+1)];

        for (int i=0, z = 0; z <= zSize; z++)
        {
            for (int x = 0; x <= xSize; x++)
            {
                float y = Mathf.PerlinNoise((float)x/xSize*scale + offsetX,(float)z/zSize*scale+offsetZ)*ySize;
                vertices[i] = new Vector3(x,y,z);

                if (y>maxTerrainHeight)
                {
                    maxTerrainHeight = y;
                }
                if (y<minTerrainHeight)
                {
                    minTerrainHeight = y;
                }

                i++;
            }
        }

        triangles = new int[xSize * zSize * 6];
        int vert = 0;
        int tris = 0;

        for (int z = 0; z < zSize; z++)
        {
            for (int x = 0; x < xSize; x++)
            {
                
                triangles[tris + 0] = vert + 0;
                triangles[tris + 1] = vert + xSize+1;
                triangles[tris + 2] = vert + 1;
                triangles[tris + 3] = vert + 1;
                triangles[tris + 4] = vert + xSize+1;
                triangles[tris + 5] = vert + xSize+2;    

                vert++;
                tris +=6;

                // delay for visualization
                //yield return new WaitForSeconds(0.01f);
            }    
            vert++;
        }

        // uvs = new Vector2[vertices.Length];
        // for (int i=0, z = 0; z <= zSize; z++)
        // {
        //     for (int x = 0; x <= xSize; x++)
        //     {
        //         uvs[i] = new Vector2((float)x/xSize,(float)z/zSize);
        //         i++;
        //     }
        // }


        colors = new Color[vertices.Length];
        for (int i=0, z = 0; z <= zSize; z++)
        {
            for (int x = 0; x <= xSize; x++)
            {
                float height = Mathf.InverseLerp(minTerrainHeight, maxTerrainHeight, vertices[i].y);
                colors[i] = gradient.Evaluate(height);
                i++;
            }
        }
        
        

    }

    void UpdateMesh()
    {
        mesh.Clear();

        mesh.vertices = vertices;
        mesh.triangles = triangles;
        //mesh.uv = uvs;

        mesh.colors = colors;

        mesh.RecalculateNormals();

        // optionally, add a mesh collider (As suggested by Franku Kek via Youtube comments).
        // To use this, your MeshGenerator GameObject needs to have a mesh collider
        // component added to it.  Then, just re-enable the code below.
        /*
        mesh.RecalculateBounds();
        MeshCollider meshCollider = gameObject.GetComponent<MeshCollider>();
        meshCollider.sharedMesh = mesh;
        //*/
    }

    // draw gismos for visualization
    // private void OnDrawGizmos()
    // {
    //     if (vertices == null)
    //     {
    //         return;
    //     }
    //     for (int i = 0; i < vertices.Length; i++)
    //     {
    //         Gizmos.DrawSphere(vertices[i], .1f);
    //     }
    // }

}
