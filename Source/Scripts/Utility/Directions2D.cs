using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Helping class for determining, rotating in cardinal or eight direction way in 2D

public static class Directions2D
{
    public static List<Vector2Int> CardinalDir = new()
    {
        Vector2Int.up,
        Vector2Int.right,
        Vector2Int.down,
        Vector2Int.left
    };

    public static Vector2Int CardinalLeftOf(Vector2Int dir) {
        if (dir == Vector2Int.right)
            return Vector2Int.up;
        else if (dir == Vector2Int.up)
            return Vector2Int.left;
        else if (dir == Vector2Int.left)
            return Vector2Int.down;
        else if (dir == Vector2Int.down)
            return Vector2Int.right;
        else
            return Vector2Int.zero; 
    }

    public static Vector2Int EightLeftOf(Vector2Int dir) {
        if (dir == Vector2Int.right)
            return Vector2Int.up;
        else if (dir == Vector2Int.up)
            return Vector2Int.left;
        else if (dir == Vector2Int.left)
            return Vector2Int.down;
        else if (dir == Vector2Int.down)
            return Vector2Int.right;
        else if (dir == new Vector2Int(1, 1)) // up - right
            return new Vector2Int(-1, 1); // up - left
        else if (dir == new Vector2Int(-1, 1)) // up - left
            return new Vector2Int(-1, -1); // down - left
        else if (dir == new Vector2Int(-1, -1))  // down - left
            return new Vector2Int(1, -1); // down - right
        else if (dir == new Vector2Int(1, -1)) // up - right
            return new Vector2Int(1, 1); // up - right
        else
            return Vector2Int.zero; 
    }

    public static Vector2Int CardinalRightOf(Vector2Int dir) {
        if (dir == Vector2Int.right)
            return Vector2Int.down;
        else if (dir == Vector2Int.down)
            return Vector2Int.left;
        else if (dir == Vector2Int.left)
            return Vector2Int.up;
        else if (dir == Vector2Int.up)
            return Vector2Int.right;
        else
            return Vector2Int.zero; 
    }

    public static Vector2Int EightRightOf(Vector2Int dir) {
        if (dir == Vector2Int.right)
            return Vector2Int.down;
        else if (dir == Vector2Int.down)
            return Vector2Int.left;
        else if (dir == Vector2Int.left)
            return Vector2Int.up;
        else if (dir == Vector2Int.up)
            return Vector2Int.right;
        else if (dir == new Vector2Int(1, 1)) // up - right
            return new Vector2Int(1, -1); // down - right
        else if (dir == new Vector2Int(1, -1)) // down - right
            return new Vector2Int(-1, -1); // down - left
        else if (dir == new Vector2Int(-1, -1))  // down - left
            return new Vector2Int(-1, 1); // up - left
        else if (dir == new Vector2Int(1, -1)) // up - left
            return new Vector2Int(1, 1); // up - right
        else
            return Vector2Int.zero; 
    }

    public static List<Vector2Int> EightDir = new List<Vector2Int> {
        Vector2Int.up,
        new(1,1),       // up - right
        Vector2Int.right,
        new(1,-1),      // right - down
        Vector2Int.down,
        new(-1,-1),     // down - left
        Vector2Int.left,
        new(-1,1),      // left - up
    };

    public static List<Vector2> FloatEightDir = new List<Vector2> {
        Vector2.up,
        new(0.5f,0.5f),       // up - right
        Vector2.right,
        new(0.5f,-0.5f),      // right - down
        Vector2.down,
        new(-0.5f,-0.5f),     // down - left
        Vector2.left,
        new(-0.5f,0.5f),      // left - up
    };

    public static Vector2Int GetRandomCardinalDir(System.Random random)
    {
        return CardinalDir[random.Next(0, CardinalDir.Count)];
    }

    public static List<Vector2Int> GetCardinalNeighbors(Vector2Int current)
    {
        List<Vector2Int> neighbors = new()
        {
            current + Vector2Int.up,
            current + Vector2Int.right,
            current + Vector2Int.down,
            current + Vector2Int.left
        };

        return neighbors;
    }
}
