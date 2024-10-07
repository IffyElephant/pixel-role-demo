using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// A* algorithm implemented using our own priority que for pathfinding and checking if map is correctly generated using Euclidian heuristic

public static class AStar
{
    public static List<Vector2Int> FindPath(Vector2Int start, Vector2Int goal, HashSet<Vector2Int> map)
    {
        HashSet<Vector2Int> closedSet = new();
        PriorityQueue<Vector2Int> openSet = new();
        Dictionary<Vector2Int, Vector2Int> cameFrom = new();
        Dictionary<Vector2Int, float> heauristicScore = new();
        Dictionary<Vector2Int, float> distanceScore = new();

        openSet.Enqueue(start, 0);
        heauristicScore[start] = CalculateHeuristic(start, goal);
        distanceScore[start] = 0;

        while (openSet.Count > 0) {
            Vector2Int current = openSet.Dequeue();

            if (current == goal)
                return ReconstructPath(cameFrom, current);

            closedSet.Add(current);

            foreach (Vector2Int neighbor in Directions2D.GetCardinalNeighbors(current)) {
                if (!map.Contains(neighbor) || closedSet.Contains(neighbor))
                    continue;

                float tentativeGScore = distanceScore[current] + Vector2Int.Distance(current, neighbor);

                if (!distanceScore.ContainsKey(neighbor) || tentativeGScore < distanceScore[neighbor]) {
                    cameFrom[neighbor] = current;
                    distanceScore[neighbor] = tentativeGScore;
                    heauristicScore[neighbor] = tentativeGScore + CalculateHeuristic(neighbor, goal);

                    if (!openSet.Contains(neighbor))
                        openSet.Enqueue(neighbor, heauristicScore[neighbor]);
                }
            }
        }

        return new List<Vector2Int>();
    }

    public static float CalculateHeuristic(Vector2Int start, Vector2Int finish)
    {
        return Mathf.Abs(start.x - finish.x) + Mathf.Abs(start.y - finish.y);
    }

    public static List<Vector2Int> ReconstructPath(Dictionary<Vector2Int, Vector2Int> cameFrom, Vector2Int current)
    {
        List<Vector2Int> path = new();
        while (cameFrom.ContainsKey(current)) {
            path.Add(current);
            current = cameFrom[current];
        }
        path.Reverse();
        return path;
    }
}
