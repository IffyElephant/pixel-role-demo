using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Simple yet effective script to check every point on map, points not found by this script will be eliminated

public static class FloodFill
{
    public static HashSet<Vector2Int> CheckReachableTiles(Vector2Int start, HashSet<Vector2Int> map)
    {
        HashSet<Vector2Int> reachable = new HashSet<Vector2Int>();
        Queue<Vector2Int> queue = new Queue<Vector2Int>();
        HashSet<Vector2Int> visited = new HashSet<Vector2Int>();

        queue.Enqueue(start);
        visited.Add(start);

        while (queue.Count > 0) {
            Vector2Int current = queue.Dequeue();
            reachable.Add(current);

            foreach (Vector2Int neighbor in Directions2D.GetCardinalNeighbors(current)) {
                if (visited.Contains(neighbor) == false && map.Contains(neighbor) == true) {
                    queue.Enqueue(neighbor);
                    visited.Add(neighbor);
                }
            }
        }

        return reachable;
    }
}
