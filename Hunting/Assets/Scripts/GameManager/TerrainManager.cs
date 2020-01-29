using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrainManager : MonoBehaviour
{
    public int boundaryWidth;

    private Vector3 terrainSize;
    private Vector3 terrainPosition;

    public Vector3 terrainMinPosition;
    public Vector3 terrainMaxPosition;

    void Awake()
    {
        terrainSize = GetComponent<Terrain>().terrainData.size;
        terrainPosition = GetComponent<Terrain>().transform.position;

        terrainMinPosition = terrainPosition + new Vector3(boundaryWidth, 0, boundaryWidth);
        terrainMinPosition.y = 0;

        terrainMaxPosition = terrainPosition + terrainSize - new Vector3(boundaryWidth, 0, boundaryWidth);
        terrainMaxPosition.y = 0;

    }


}
