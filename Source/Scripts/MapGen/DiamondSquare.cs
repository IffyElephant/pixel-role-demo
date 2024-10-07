using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UIElements;

public class DiamondSquare : AbstractGen
{
    [Range(0.001f, 2.000f)]
    public float Roughness = 0.5f;
    [Range(0.00f, 1.00f)]
    public float Threshold = 0.03f;
    public bool ShowValid = true;
    private float[,] heightData;
    
    public override void Generate()
    {
        // Initialize
        random = new System.Random(Seed);
        UnityEngine.Random.InitState(Seed);
        heightData = new float[MapSize, MapSize];
        HashSet<Vector2Int> map = new();

        // Set corner heights
        SetCornerHeights();

        // Diamond-Square algorithm
        if(MapSize == 35) {
            Roughness = 0.5f;
            Threshold = 0.03f;
        } else {
            // Roughness = 2f;
            Threshold = 0.0065f;
        }
        float scale = Roughness;

        for (int sideLength = MapSize; sideLength > 1; sideLength /= 2) {
            DiamondStep(sideLength, scale);
            SquareStep(sideLength, scale);

            scale -= scale * 0.5f * Roughness;
        }

        // Generate map based on height data
        for (int y = 0; y < MapSize; y++) {
            for (int x = 0; x < MapSize; x++) {
                Debug.Log(heightData[x, y]);
                if (heightData[x, y] <= Threshold)
                    map.Add(new Vector2Int(x, y));
            }
        }

        // Generate start pos and end pos on map
        Vector2Int startPos = LevelGen.GenObjPositions(random, MapSize, map);

        // Check map
        if(ShowValid == true) {
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

    private void SetCornerHeights() {
        // Set initial corner heights with random values
        heightData[0, 0] = UnityEngine.Random.Range(0f, 1f);
        heightData[0, MapSize - 1] = UnityEngine.Random.Range(0f, 1f);
        heightData[MapSize - 1, 0] = UnityEngine.Random.Range(0f, 1f);
        heightData[MapSize - 1, MapSize - 1] = UnityEngine.Random.Range(0f, 1f);
    }


    // Inspired by https://github.com/RStivanson/unity-diamond-square
    private void DiamondStep(int sideLength, float scale) {
        int halfSide = sideLength / 2;

        for (int x = 0; x < MapSize - sideLength; x += sideLength) {
            for (int y = 0; y < MapSize - sideLength; y += sideLength) {
                // Average of corners
                float avg = heightData[x, y];
                avg += heightData[x + sideLength, y];
                avg += heightData[x, y + sideLength];
                avg += heightData[x + sideLength, y + sideLength];
                avg /= 4.0f;

                // Random offset
                avg += UnityEngine.Random.Range(0, scale * 2) - scale;
                heightData[x + halfSide, y + halfSide] = avg;
            }
        }
    }

    // Inspired by https://github.com/RStivanson/unity-diamond-square
    private void SquareStep(int sideLength, float scale) {
        int halfSide = sideLength / 2;

        for (int x = 0; x < MapSize; x += halfSide) {
            for (int y = (x + halfSide) % sideLength; y < MapSize; y += sideLength) {
                // Average of corners
                float avg = heightData[(x - halfSide + MapSize - 1) % (MapSize - 1), y];
                avg += heightData[(x + halfSide) % (MapSize - 1), y];
                avg += heightData[x, (y + halfSide) % (MapSize - 1)];
                avg += heightData[x, (y - halfSide + MapSize - 1) % (MapSize - 1)];
                avg /= 4.0f;

                // Random offset
                avg += UnityEngine.Random.Range(0, scale * 2) - scale;
                heightData[x, y] = avg;

                // Set opposite edge if its on the edge
                if (x == 0)
                    heightData[MapSize - 1, y] = avg;

                if (y == 0)
                    heightData[x, MapSize - 1] = avg;
            }
        }
    }
}
