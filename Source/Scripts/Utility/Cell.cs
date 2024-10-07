using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cell
{
    public Vector2Int position;
    public List<List<Color>> patterns;
    public int entropy;
    public List<Cell> neighbors;

    public Cell(Vector2Int position, List<List<Color>> patterns)
    {
        this.position = position;
        this.patterns = patterns;
        this.entropy = patterns.Count;
        this.neighbors = new List<Cell>();
    }

    public bool IsDefinite()
    {
        return entropy == 1;
    }

    public void Collapse()
    {
        int e = Random.Range(0, entropy);
        int i;
        for (i = 0; i < patterns.Count; i++)
        {
            e -= patterns[i].Count;
            if (e <= 0)
            {
                break;
            }
        }
        patterns.RemoveRange(0, i);
        entropy = 1;
    }

    public bool UpdateConstraints(Cell observedCell)
    {
        bool dirty = false;
        for (int i = 0; i < patterns.Count; i++)
        {
            bool legit = true;
            for (int j = 0; j < patterns[i].Count; j++)
            {
                if (observedCell.patterns[i][j] != patterns[i][j])
                {
                    legit = false;
                    break;
                }
            }
            if (!legit)
            {
                patterns.RemoveAt(i);
                dirty = true;
                i--;
            }
        }

        return dirty;
    }
}