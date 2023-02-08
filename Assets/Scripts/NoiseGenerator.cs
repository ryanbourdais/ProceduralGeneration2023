using UnityEngine;

public class NoiseGenerator : MonoBehaviour
{

    public Terrain[] terrains;
    [Range(513,2048)]
    public int width = 513; 
    [Range(513,2048)]
    public int height = 513;
    [Range(0,100)]
    public int depth = 20;
    
    public int seed = 0;
    [Range(0, 8)]
    public int octaves =  4;
    public float worldOffset = 0f;
    public float scale = 8;
    public AnimationCurve heightCurve;
    [Range(0,1)]
    public float persistance = .5f;
    [Range(0,2)]
    public float lacunarity = 2f;
    public float _offsetX = 0f;
    public float _offsetZ = 0f;
    public bool autoUpdate;
    public bool generateTexture;
    public TextureModifier textureGenerator;

    void Awake()
    {
       GenerateTerrain(); 
    }

    public void GenerateTerrain()
    {
        for(int i = 0; i <= terrains.Length - 1; i++)
        {
            terrains[i].terrainData.heightmapResolution = width + 1;
            terrains[i].terrainData.size = new Vector3(width, depth, height);
            try
            {
                terrains[i].terrainData.SetHeights(0, 0, GenerateHeights());
            }
            catch
            {
                Debug.Log("X or Y base is out of bounds");
            }
            if(generateTexture)
            {
                textureGenerator.generateTextures(terrains[i]);
            }
        }
    }
        
    float [,] GenerateHeights()
    {
        float[,] heights = new float[width, height];

        System.Random prng = new System.Random(seed);
        Vector2[] octaveOffsets = new Vector2[octaves];

        for(int i = 0; i < octaves; i++)
        {
            float offsetX = prng.Next(-100000, 100000) + worldOffset;
            float offsetY = prng.Next(-100000, 100000) + worldOffset;
            octaveOffsets[i] = new Vector2(offsetX, offsetY);
        }

        float maxNoiseHeight = float.MinValue;
        float minNoiseHeight = float.MaxValue;

        float halfWidth = width / 2f;
        float halfHeight = height / 2f;

        for(int x = 0; x < width; x++)
        {
            for(int y = 0; y < height; y++)
            {
                float amplitude = 1;
                float frequency = 1;
                float noiseHeight = 0;

                for(int i = 0; i < octaves; i++)
                {
                    float xCoord = ((float)x - halfWidth) / scale * frequency + octaveOffsets[i].x + _offsetX;
                    float yCoord = ((float)y - halfHeight)/ scale * frequency + octaveOffsets[i].y + _offsetZ;

                    float perlinValue = Mathf.PerlinNoise(xCoord, yCoord) * 2 - 1;
                    noiseHeight += perlinValue * amplitude * heightCurve.Evaluate(perlinValue);
                    
                    amplitude *= persistance;
                    frequency *= lacunarity;
                }

                if(noiseHeight > maxNoiseHeight)
                {
                    maxNoiseHeight = noiseHeight;
                }
                else if (noiseHeight < minNoiseHeight)
                {
                    minNoiseHeight = noiseHeight;
                }
                heights[x,y] = noiseHeight;
            }
        }

        for(int y = 0; y < height; y++)
        {
            for(int x = 0; x < width; x++)
            {
                heights[x,y] = Mathf.InverseLerp(minNoiseHeight, maxNoiseHeight, heights[x,y]);
            }
        }

        return heights;
    }
}
