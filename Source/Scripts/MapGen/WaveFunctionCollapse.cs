using System.Collections.Generic;
using UnityEngine;

public class WaveFunctionCollapse : AbstractGen
{
    public Texture2D image; // Input image
    public int patternSize = 3; // Size of the patterns
    public bool ShowValid = true;
    private HashSet<Vector2Int> map = new();

    public override void Generate()
    {
        Seed = UnityEngine.Random.Range(0, 10000000);
        if (image != null)
        {
            random = new(Seed);
            
            List<List<Color>> patterns = ProcessInputImage();

            InitializeOutputMap();

            map = WaveFunctionCollapseAlgorithm(patterns);

            // Generate start pos and end pos on map
            Vector2Int startPos = LevelGen.GenObjPositions(random, MapSize, map);

            // Check map
            if(ShowValid == true){
                HashSet<Vector2Int> cleanMap = FloodFill.CheckReachableTiles(startPos, map);
                // Generate tiles
                LevelGen.GenTiles(cleanMap);
                // Cleanup
                cleanMap.Clear();
            }
            else {
                // Generate tiles
                LevelGen.GenTiles(map);
            }
            
            // Cleanup
            map.Clear();
        } else {
            Debug.LogError("Input image is not assigned.");
        }
    }

    private List<List<Color>> ProcessInputImage()
    {
        List<List<Color>> patterns = new();

        for (int y = 0; y <= image.height - patternSize; y++) {
            for (int x = 0; x <= image.width - patternSize; x++) {
                List<Color> pattern = new();

                for (int dy = 0; dy < patternSize; dy++)
                    for (int dx = 0; dx < patternSize; dx++)
                        pattern.Add(image.GetPixel(x + dx, y + dy));

                patterns.Add(pattern);
            }
        }

        return patterns;
    }

    private void InitializeOutputMap()
    {
        for (int y = 0; y < image.height; y++)
        {
            for (int x = 0; x < image.width; x++)
            {
                map.Add(new Vector2Int(x, y));
            }
        }
    }

    private HashSet<Vector2Int> WaveFunctionCollapseAlgorithm(List<List<Color>> patterns)
    {
        // Initialize wave
        List<Cell> wave = new();
        foreach (Vector2Int position in map)
        {
            wave.Add(new Cell(position, patterns));
        }

        // Main loop
        while (wave.Count > 0)
        {
            List<Cell> lowestEntropyCells = FindLowestEntropyCells(wave);
            
            if (lowestEntropyCells.Count == 0)
            {
                break;
            }

            // Choose a cell to collapse
            Cell cellToCollapse = lowestEntropyCells[random.Next(0, lowestEntropyCells.Count)];
            cellToCollapse.Collapse();

            // Propagate changes
            Propagate(cellToCollapse, wave);
        }

        return GetFinishedMap(wave);
    }

    private HashSet<Vector2Int> GetFinishedMap(List<Cell> wave)
    {
        HashSet<Vector2Int> finishedMap = new();

        foreach (Cell cell in wave)
        {
            if (cell.IsDefinite())
            {
                finishedMap.Add(cell.position);
            }
        }

        return finishedMap;
    }

    private List<Cell> FindLowestEntropyCells(List<Cell> wave)
    {
        List<Cell> lowestEntropyCells = new();
        int currentLowestEntropy = -1;

        foreach (Cell cell in wave)
        {
            if (!cell.IsDefinite())
            {
                if (currentLowestEntropy == -1 || cell.entropy < currentLowestEntropy)
                {
                    lowestEntropyCells.Clear();
                    currentLowestEntropy = cell.entropy;
                }
                
                if (cell.entropy == currentLowestEntropy)
                {
                    lowestEntropyCells.Add(cell);
                }
            }
        }

        return lowestEntropyCells;
    }

    private void Propagate(Cell cell, List<Cell> wave)
    {
        foreach (Cell neighbor in cell.neighbors)
        {
            if (neighbor.IsDefinite())
            {
                continue;
            }

            if (neighbor.UpdateConstraints(cell))
            {
                Propagate(neighbor, wave);
            }
        }
    }
}