using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class PolyTerrain : MonoBehaviour
{
	public Material terrainmat;
	public int polyscale = 10;
	public int polysize = 8;
	public float heightscale = 2;
	public float sizescale = 1;
	public float perlinscale = .75f;
	public float perlinoffset = 100;
	public bool deform = false;
	public float deformamount = .1f;
	public int deformseed = 15;
	private Vector3[,] areaMap;
	private Vector3[] terrainVerticesSet;
	private Vector3[,] terrainNormalsSet;
	private GameObject[,] polyTerrainMeshSet;
	private int meshArrayWidth;
	public int textureRes = 10;

	void Start()
	{
		meshArrayWidth = Mathf.CeilToInt(polyscale / (float)(polysize + 1));
		polyTerrainMeshSet = new GameObject[meshArrayWidth, meshArrayWidth];
		terrainVerticesSet = new Vector3[polyscale * polyscale + (polyscale - 1) * (polyscale - 1)];
		terrainNormalsSet = new Vector3[polyscale, polyscale];
		areaMap = new Vector3[polyscale, polyscale];
		for (int z = 0; z < polyscale; z++)
		{
			for (int x = 0; x < polyscale; x++)
			{
				areaMap[z, x] = new Vector3(x * sizescale, Mathf.PerlinNoise(x * perlinscale + perlinoffset, z * perlinscale + perlinoffset) * heightscale, z * sizescale);
			}
		}
		for (int z = 0; z < meshArrayWidth; z++)
		{
			for (int x = 0; x < meshArrayWidth; x++)
			{
				GameObject polyTerrainMesh = new GameObject("Plane");
				polyTerrainMesh.layer = 9;
				polyTerrainMesh.AddComponent<MeshFilter>();
				polyTerrainMesh.AddComponent<MeshRenderer>();
				polyTerrainMesh.GetComponent<MeshRenderer>().material = Instantiate<Material>(terrainmat);
				polyTerrainMeshSet[z, x] = polyTerrain(x * polysize, z * polysize, polyTerrainMesh);
				polyTerrainMeshSet[z, x].GetComponent<MeshRenderer>().material.mainTexture.wrapMode = TextureWrapMode.Clamp;
			}
		}

		calcNormals();
	}

	void Update()
	{

	}

	public GameObject polyTerrain(int xStart, int zStart, GameObject polyTerrainMesh)
	{
		int xEnd = xStart + polysize + 1;
		int zEnd = zStart + polysize + 1;
		if (xEnd > polyscale)
			xEnd = polyscale;
		if (zEnd > polyscale)
			zEnd = polyscale;
		int xPoly = xEnd - xStart;
		int zPoly = zEnd - zStart;
		Vector3[] terrainVertices = new Vector3[xPoly * zPoly + (xPoly - 1) * (zPoly - 1)];    //areamap in the first polyscale square, the inbetweens in the second
		int[] terrainTriangles = new int[(xPoly - 1) * (zPoly - 1) * 12];

		for (int z = 0; z < zPoly; z++)
		{
			for (int x = 0; x < xPoly; x++)
			{
				terrainVertices[z * xPoly + x] = areaMap[x + xStart, z + zStart];
				terrainVerticesSet[(z + zStart) * polyscale + x + xStart] = terrainVertices[z * xPoly + x];
			}
		}

		for (int z = 0; z < zPoly - 1; z++)
		{
			for (int x = 0; x < xPoly - 1; x++)
			{
				float y = (areaMap[x + xStart, z + zStart].y + areaMap[x + 1 + xStart, z + zStart].y + areaMap[x + xStart, z + 1 + zStart].y + areaMap[x + 1 + xStart, z + 1 + zStart].y) / 4;
				terrainVertices[xPoly * zPoly + ((xPoly - 1) * z) + x] = new Vector3(areaMap[x + xStart, z + zStart].x + .5f * sizescale, y, areaMap[x + xStart, z + zStart].z + .5f * sizescale);
				terrainVerticesSet[polyscale * polyscale + ((polyscale - 1) * (z + zStart)) + x + xStart] = terrainVertices[xPoly * zPoly + ((xPoly - 1) * z) + x];
			}
		}

		int cur = 0;
		for (int z = 0; z < zPoly - 1; z++)
		{
			for (int x = 0; x < xPoly - 1; x++)
			{
				//setting triangles
				terrainTriangles[cur + 2] = z * xPoly + x;
				terrainTriangles[cur + 1] = xPoly * zPoly + ((xPoly - 1) * z) + x;
				terrainTriangles[cur + 0] = z * xPoly + x + 1;
				terrainTriangles[cur + 3] = xPoly * zPoly + ((xPoly - 1) * z) + x;
				terrainTriangles[cur + 4] = z * xPoly + x + 1;
				terrainTriangles[cur + 5] = (z + 1) * xPoly + x + 1;
				terrainTriangles[cur + 8] = z * xPoly + x;
				terrainTriangles[cur + 7] = (z + 1) * xPoly + x;
				terrainTriangles[cur + 6] = xPoly * zPoly + ((xPoly - 1) * z) + x;
				terrainTriangles[cur + 9] = (z + 1) * xPoly + x;
				terrainTriangles[cur + 10] = xPoly * zPoly + ((xPoly - 1) * z) + x;
				terrainTriangles[cur + 11] = (z + 1) * xPoly + x + 1;
				//increment
				cur += 12;
			}
		}

		polyTerrainMesh.GetComponent<MeshFilter>().mesh.vertices = terrainVertices;
		polyTerrainMesh.GetComponent<MeshFilter>().mesh.triangles = terrainTriangles;
		polyTerrainMesh.GetComponent<MeshFilter>().mesh.RecalculateNormals();

		Vector2[] terrainUV = new Vector2[terrainVertices.Length];

		for (int z = 0; z < zPoly; z++)
		{
			for (int x = 0; x < xPoly; x++)
			{
				terrainUV[z * xPoly + x] = new Vector2((x + 2) / (float)(xPoly + 4), (z + 2) / (float)(zPoly + 4));
			}
		}

		for (int z = 0; z < zPoly - 1; z++)
		{
			for (int x = 0; x < xPoly - 1; x++)
			{
				terrainUV[xPoly * zPoly + ((xPoly - 1) * z) + x] = new Vector2((x + 2.5f) / (float)(xPoly + 4), (z + 2.5f) / (float)(zPoly + 4));
			}
		}

		polyTerrainMesh.GetComponent<MeshFilter>().mesh.uv = terrainUV;

		Texture2D terrainTexture = new Texture2D((xPoly + 4) * textureRes, (zPoly + 4) * textureRes, TextureFormat.ARGB32, false);

		Color gr = Color.green;
		gr = new Color(gr.r / 1.3f, gr.g / 1.3f, gr.b / 1.3f);

		for (int z = 0; z < (zPoly + 4) * textureRes; z++)
		{
			for (int x = 0; x < (xPoly + 4) * textureRes; x++)
			{
				float y = Mathf.PerlinNoise(((z) / (float)textureRes + zStart - 2) * perlinscale + perlinoffset, ((x) / (float)textureRes + xStart - 2) * perlinscale + perlinoffset);
				terrainTexture.SetPixel(x, z, new Color(gr.r * (1f * y) + .1f, gr.g * (1f * y) + .1f, gr.b * (1f * y) + .1f));
			}
		}
		terrainTexture.Apply();
		terrainTexture.filterMode = FilterMode.Point;

		polyTerrainMesh.GetComponent<MeshRenderer>().material.mainTexture = terrainTexture;

		return polyTerrainMesh;
	}

	void calcNormals()
	{
		//this loop pair can be optimized
		//normalize all normals in terrainNormalsSet so they're not recalculated every call to getSlope
		for (int z = 0; z < polyscale; z++)
		{
			for (int x = 0; x < polyscale; x++)
			{
				terrainNormalsSet[z, x] = Vector3.zero;
			}
		}

		for (int z = 0; z < meshArrayWidth; z++)
		{
			for (int x = 0; x < meshArrayWidth; x++)
			{
				Vector3[] normals = polyTerrainMeshSet[z, x].GetComponent<MeshFilter>().mesh.normals;
				int xStart = x * polysize;
				int zStart = z * polysize;
				int xEnd = xStart + polysize + 1;
				int zEnd = zStart + polysize + 1;
				if (xEnd >= polyscale)
					xEnd = polyscale;
				if (zEnd >= polyscale)
					zEnd = polyscale;
				int xPoly = xEnd - xStart;
				int zPoly = zEnd - zStart;
				for (int meshz = 0; meshz < zPoly; meshz++)
				{
					for (int meshx = 0; meshx < xPoly; meshx++)
					{
						terrainNormalsSet[zStart + meshz, xStart + meshx] += normals[meshz * xPoly + meshx];
					}
				}
			}
		}

		for (int z = 0; z < meshArrayWidth; z++)
		{
			for (int x = 0; x < meshArrayWidth; x++)
			{
				Vector3[] normals = polyTerrainMeshSet[z, x].GetComponent<MeshFilter>().mesh.normals;
				int xStart = x * polysize;
				int zStart = z * polysize;
				int xEnd = xStart + polysize + 1;
				int zEnd = zStart + polysize + 1;
				if (xEnd >= polyscale)
					xEnd = polyscale;
				if (zEnd >= polyscale)
					zEnd = polyscale;
				int xPoly = xEnd - xStart;
				int zPoly = zEnd - zStart;
				for (int meshz = 0; meshz < zPoly; meshz++)
				{
					normals[meshz * xPoly + 0] = terrainNormalsSet[zStart + meshz, xStart + 0].normalized;
					normals[meshz * xPoly + xPoly - 1] = terrainNormalsSet[zStart + meshz, xStart + xPoly - 1].normalized;
				}
				for (int meshx = 0; meshx < xPoly; meshx++)
				{
					normals[0 * xPoly + meshx] = terrainNormalsSet[zStart + 0, xStart + meshx].normalized;
					normals[(zPoly - 1) * xPoly + meshx] = terrainNormalsSet[zStart + zPoly - 1, xStart + meshx].normalized;
				}
				polyTerrainMeshSet[z, x].GetComponent<MeshFilter>().mesh.normals = normals;
			}
		}
	}

	public int[] triPoints(float xcoord, float zcoord)		//returns the three points of the triangle x and z sit within
	{														//the order unfortunately does matter, the fourth index is 0 for x long, 1 for z long
		float x = xcoord % 1;
		float z = zcoord % 1;
		int[] rPoints = new int[4];
		if (x + z < 1 && x > z)
		{
			rPoints[0] = Mathf.FloorToInt(zcoord) * polyscale + Mathf.FloorToInt(xcoord);
			rPoints[1] = Mathf.FloorToInt(zcoord) * polyscale + Mathf.FloorToInt(xcoord) + 1;
			rPoints[2] = polyscale * polyscale + Mathf.FloorToInt(zcoord) * (polyscale - 1) + Mathf.FloorToInt(xcoord);
			rPoints[3] = 0;
		}
		else if (x + z > 1 && x > z)
		{
			rPoints[0] = Mathf.FloorToInt(zcoord) * polyscale + Mathf.FloorToInt(xcoord) + 1;
			rPoints[1] = (Mathf.FloorToInt(zcoord) + 1) * polyscale + Mathf.FloorToInt(xcoord) + 1;
			rPoints[2] = polyscale * polyscale + Mathf.FloorToInt(zcoord) * (polyscale - 1) + Mathf.FloorToInt(xcoord);
			rPoints[3] = 1;
		}
		else if (x + z > 1 && x < z)
		{
			rPoints[0] = (Mathf.FloorToInt(zcoord) + 1) * polyscale + Mathf.FloorToInt(xcoord) + 1;
			rPoints[1] = (Mathf.FloorToInt(zcoord) + 1) * polyscale + Mathf.FloorToInt(xcoord);
			rPoints[2] = polyscale * polyscale + Mathf.FloorToInt(zcoord) * (polyscale - 1) + Mathf.FloorToInt(xcoord);
			rPoints[3] = 0;
		}
		else if (x + z < 1 && x < z)
		{
			rPoints[0] = (Mathf.FloorToInt(zcoord) + 1) * polyscale + Mathf.FloorToInt(xcoord);
			rPoints[1] = Mathf.FloorToInt(zcoord) * polyscale + Mathf.FloorToInt(xcoord);
			rPoints[2] = polyscale * polyscale + Mathf.FloorToInt(zcoord) * (polyscale - 1) + Mathf.FloorToInt(xcoord);
			rPoints[3] = 1;
		}
		else
		{
			rPoints[0] = polyscale * polyscale + Mathf.FloorToInt(zcoord) * (polyscale - 1) + Mathf.FloorToInt(xcoord);
			rPoints[1] = rPoints[0];
			rPoints[2] = rPoints[0];
			rPoints[3] = 2;
		}
		return rPoints;
	}

	public float getHeight(float x, float z)	//gets the terrain height at x and z
	{
		return Mathf.PerlinNoise(x * perlinscale / sizescale + perlinoffset, z * perlinscale / sizescale + perlinoffset) * heightscale;
	}

	public Vector3 getNormal(float x, float z)
	{
		if (x >= 0 && z >= 0 && x <= (polyscale - 1) * sizescale && z <= (polyscale - 1) * sizescale)
		{
			x /= sizescale;
			z /= sizescale;
			int[] tri = triPoints(x, z);
			Vector3 u = terrainVerticesSet[tri[1]] - terrainVerticesSet[tri[0]];
			Vector3 v = terrainVerticesSet[tri[2]] - terrainVerticesSet[tri[0]];
			Vector3 n = new Vector3();
			n.x = u.y * v.z - u.z * v.y;
			n.y = u.z * v.x - u.x * v.z;
			n.z = u.x * v.y - u.y * v.x;
			return n.normalized;
		}
		return Vector3.up;
	}
}
