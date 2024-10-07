using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CellularAutomata : AbstractGen
{
    [Range(0.00f, 1.00f)]
    public float PercentCoverage = 0.5f;
    public int Iterations;
    public bool ShowValid = true;
    
    public override void Generate() 
    {
        // Init
        random = new(Seed);
        HashSet<Vector2Int> map = new();
        int[,] mapArray = GenerateNoiseMap();

        // Run CLA on 2D map array
        for(int i = 0; i < Iterations; i++){
            mapArray = RunCLA(mapArray);
        }

        // Convert 2D map array to map
        ConvertToMap(mapArray, map);

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
        // map.Clear(); 
        // It would be a generally good idea to cleanup data we dont need, sadly we need it for pathfinding so as long as we want to use pathfinding we cant clean our maps
    }

    private void ConvertToMap(int[,] mapArray, HashSet<Vector2Int> map)
    {
        for (int x = 0; x < mapArray.GetUpperBound(0); x++)
        {
            for (int y = 0; y < mapArray.GetUpperBound(1); y++)
            {
                if (mapArray[x, y] == 1)
                    map.Add(new Vector2Int(x, y));
            }
        }
    }

    public int[,] RunCLA(int[,] mapArray)
    {
        int[,] newMapArray = new int[MapSize, MapSize];

        // We check every single tile in a map
        for (int x = 0; x < mapArray.GetUpperBound(0); x++)
        {
            for (int y = 0; y < mapArray.GetUpperBound(1); y++)
            {
                // Each tile is checked for its neighbours
                //   we change current tile if theres more or exactly four neighbouring tiles
                //   with value of 1 (meaning floor tile) to another floor tile
                //   on the other saide if there are four 0 values (meaning wall tile)
                //   we change current tile to wall aswell
                int neighbourCount = 0;
                foreach (var dir in Directions2D.EightDir)
                {
                    int xPos = x + dir.x;
                    int yPos = y + dir.y;

                    if (xPos < 0 || xPos > mapArray.GetUpperBound(0) || yPos < 0 || yPos > mapArray.GetUpperBound(1))
                        continue;

                    else if (mapArray[xPos, yPos] == 1)
                        neighbourCount++;
                }

                if (neighbourCount > 4)
                    newMapArray[x, y] = 1;
                else
                    newMapArray[x, y] = 0;
            }
        }

        return newMapArray;
    }

    private int[,] GenerateNoiseMap()
    {
        // We generate our own simple noise based on percent coverage
        int[,] map = new int[MapSize, MapSize]; // zmenit na bool
        int cellsCount = Mathf.RoundToInt(MapSize * MapSize * PercentCoverage);

        for (int x = 0; x < MapSize; x++)
        {
            for (int y = 0; y < map.GetUpperBound(1); y++)
            {
                map[x, y] = 0;
            }
        }

        for (int i = 0; i < cellsCount; i++)
        {
            int x = random.Next(0, MapSize);
            int y = random.Next(0, MapSize);

            if (map[x, y] == 1)
                i--;
            else
                map[x, y] = 1;
        }

        return map;
    }
}