using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.ExceptionServices;
using UnityEngine;

public class PerlinNoise : AbstractGen
{
    [Range(0.1f, 1f)]
    public float Persistence = 0.5f;
    [Range(1f, 3f)]
    public float Lacunarity = 1.5f;
    public int Octaves = 1;
    public float Threshold;
    public bool ShowValid = true;

    public override void Generate()
    {
        // Init
        random = new(Seed);
        HashSet<Vector2Int> map = new();

        Vector2Int mapSize = new(MapSize, MapSize);
        Vector2Int[] offset = new Vector2Int[Octaves];
        
        for (int i = 0; i < Octaves; i++) {
            offset[i].x = random.Next(-10000, 10000);
            offset[i].y = random.Next(-10000, 10000);
        }

        //Generate map
        for (int y = 0; y < mapSize.y; y++) {
            for (int x = 0; x < mapSize.x; x++) {
                float value = 0;
                float amplitude = 1;
                float frequency = 1;

                for (int i = 0; i < Octaves; i++) { // MapSize is our scale, seed is our offset
                    float xPos = (float)x / MapSize * 5f * frequency + offset[i].x;
                    float yPos = (float)y / MapSize * 5f * frequency + offset[i].y;

                    value += Mathf.PerlinNoise(xPos, yPos) * amplitude;
                    amplitude *= Persistence;
                    frequency *= Lacunarity;
                }
                
                value /= Octaves;

                if (value >= Threshold)
                    map.Add(new Vector2Int(x, y));
            }
        }

        // Generate start pos and end pos on map
        Vector2Int startPos = LevelGen.GenObjPositions(random, MapSize, map);

        // Check map
        if(ShowValid == true){
            HashSet<Vector2Int> cleanMap = FloodFill.CheckReachableTiles(startPos, map);
            // Generate tiles
            LevelGen.GenTiles(cleanMap);
            // Cleanup
            // cleanMap.Clear();
        }
        else {
            // Generate tiles
            LevelGen.GenTiles(map);
        }
        
        // Cleanup
        // map.Clear(); // Cleanup Issue explained in CA
    }
}
