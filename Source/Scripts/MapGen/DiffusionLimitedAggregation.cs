using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiffusionLimitedAggregation : AbstractGen
{
    [Range(0.00f, 1.00f)]
    public float PercentCoverage = 0.5f;

    public bool Central = true;
    public int CentralChance = 50;
    public bool Circle = true;

    public override void Generate()
    {
        // Init
        random = new(Seed);
        HashSet<Vector2Int> map = new();
        
        // Find radius starting points for walkers
        List<Vector2Int> startPositions = FindRadiusPoints(map);

        // Run DFA walkers
        RunWalker(map, startPositions);

        // Generate start pos and end pos on map
        Vector2Int startPos = LevelGen.GenObjPositions(random, MapSize, map);

        // Generate tiles
        LevelGen.GenTiles(map);
        
        // Cleanup
        // map.Clear(); // Cleanup Issue explained in CA
    }

    private List<Vector2Int> FindRadiusPoints(HashSet<Vector2Int> mapArray)
    {
        Vector2Int centerPoint = new(MapSize / 2, MapSize / 2);
        mapArray.Add(centerPoint);
        List<Vector2Int> radiusPoints = new();

        for (int x = 0; x < MapSize + 1; x++)
        {
            for (int y = 0; y < MapSize + 1; y++)
            {
                if ((int)Vector2.Distance(centerPoint, new Vector2(x, y)) == MapSize / 2)
                {
                    radiusPoints.Add(new Vector2Int(x, y));
                }
            }
        }

        return radiusPoints;
    }

    private void RunWalker(HashSet<Vector2Int> mapArray, List<Vector2Int> startPositions)
    {
        Vector2Int centerPoint = new(MapSize / 2, MapSize / 2);
        int tilesAmount = Mathf.RoundToInt(Mathf.Pow(MapSize, 2) * PercentCoverage);

        while(tilesAmount > 0) {
            Vector2Int pos;

            if(Circle == true)
                pos = startPositions[random.Next(0, startPositions.Count)];
            else
                pos = new(random.Next(0, MapSize), random.Next(0, MapSize));

            while (true) {
                Vector2Int dir = Directions2D.GetRandomCardinalDir(random);

                if(Central == true && random.Next(0, 100) > CentralChance) {
                    if (random.Next(0, 2) > 0.5f) // 0 or 1 so 0.5f = 50%
                    { // starts with X pos
                        if (pos.x > centerPoint.x)
                            dir = Vector2Int.left;
                        else if (pos.x < centerPoint.x)
                            dir = Vector2Int.right;
                        else if (pos.y > centerPoint.y)
                            dir = Vector2Int.down;
                        else if (pos.y < centerPoint.y)
                            dir = Vector2Int.up;
                    }
                    else
                    { // starts with Y pos
                        if (pos.y > centerPoint.y)
                            dir = Vector2Int.down;
                        else if (pos.y < centerPoint.y)
                            dir = Vector2Int.up;
                        else if (pos.x > centerPoint.x)
                            dir = Vector2Int.left;
                        else if (pos.x < centerPoint.x)
                            dir = Vector2Int.right;
                    }
                }

                Vector2Int newPos = pos + dir;

                if (mapArray.Contains(newPos)) {
                    mapArray.Add(pos);
                    tilesAmount--;
                    break;
                }

                if (Vector2Int.Distance(newPos, centerPoint) > MapSize)
                    break;

                pos = newPos;
            }
        }
    }
}
