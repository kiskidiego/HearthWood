using UnityEngine;

public class TerrainSampler : MonoBehaviour
{
    Terrain _terrain;

    void Awake()
    {
        _terrain = GetComponent<Terrain>();
    }
    public TerrainLayer GetTerrainLayerAtPosition(Vector3 position)
    {
        TerrainData terrainData = _terrain.terrainData;
        float[,,] alphamaps = terrainData.GetAlphamaps((int)((position.x / terrainData.size.x) * terrainData.alphamapWidth), (int)((position.z / terrainData.size.z) * terrainData.alphamapHeight), 1, 1);
        int dominantTextureIndex = 0;
        float maxAlpha = 0f;
        for (int j = 0; j < alphamaps.GetLength(2); j++)
        {
            if (alphamaps[0, 0, j] > maxAlpha)
            {
                maxAlpha = alphamaps[0, 0, j];
                dominantTextureIndex = j;
            }
        }

        return terrainData.terrainLayers[dominantTextureIndex];
    }
}