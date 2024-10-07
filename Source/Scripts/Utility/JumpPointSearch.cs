using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

// Pathinding logic for NPS's to ask for new paths to follow, using altered A* algorithm

public class JumpPointSearch : MonoBehaviour
{
    private HashSet<Vector2Int> map;

    public void SetMap(HashSet<Vector2Int> map) {
        this.map = map;
    }

    public Vector2Int RandomPoint() {
        return map.ElementAt(Random.Range(0, map.Count));
    }

    public bool CanCall() {
        return map.Count > 0;
    }

    public List<Vector2Int> FindPath(Vector2Int start, Vector2Int goal)
    {
        HashSet<Vector2Int> closedSet = new();
        PriorityQueue<Vector2Int> openSet = new();
        Dictionary<Vector2Int, Vector2Int> cameFrom = new();
        Dictionary<Vector2Int, float> distanceScore = new();

        openSet.Enqueue(start, 0);
        distanceScore[start] = 0;

        while (openSet.Count > 0) {
            Vector2Int current = openSet.Dequeue();

            if (current == goal)
                return AStar.ReconstructPath(cameFrom, current);

            closedSet.Add(current);

            List<Vector2Int> jumpPoints = findJumpPoints(current, goal);

            foreach (Vector2Int neighbor in jumpPoints) {
                if (closedSet.Contains(neighbor))
                    continue;

                float tentativeGScore = distanceScore[current] + Vector2Int.Distance(current, neighbor);

                if (!distanceScore.ContainsKey(neighbor) || tentativeGScore < distanceScore[neighbor]) {
                    cameFrom[neighbor] = current;
                    distanceScore[neighbor] = tentativeGScore;
                    float heuristicScore = tentativeGScore + AStar.CalculateHeuristic(neighbor, goal);
                    openSet.Enqueue(neighbor, heuristicScore);
                }
            }
        }

        return new List<Vector2Int>();
    }

    public List<Vector2Int> findJumpPoints(Vector2Int start, Vector2Int goal) {
        List<Vector2Int> jumpPoints = new();

        if (isValid(start) == false || isValid(goal) == false) {
            return jumpPoints;
        }

        foreach (Vector2Int dir in Directions2D.EightDir) {
            List<Vector2Int> points = jump(start, goal, dir);
            
            foreach (Vector2Int pos in points)
            {
                if (pos.x > -10000 && pos.y > -10000)
                    jumpPoints.Add(pos);
            }
        }

        return jumpPoints;
    }

    private List<Vector2Int> forcedNeighbor(Vector2Int pos, Vector2Int dir) {
        List<Vector2Int> forcedNeighbors = new();

        Vector2Int nextPos = pos + dir;
        Vector2Int left = nextPos + Directions2D.EightLeftOf(dir);
        Vector2Int right = nextPos + Directions2D.EightRightOf(dir);

        if (isValid(left) == true)
            forcedNeighbors.Add(left);

        if (isValid(right) == true)
            forcedNeighbors.Add(right);
        
        return forcedNeighbors;
    }

    private List<Vector2Int> jump(Vector2Int pos, Vector2Int goal, Vector2Int dir) {
        List<Vector2Int> jumpPoints = new();
        Vector2Int nextPos = pos + dir;

        if (isValid(nextPos) == false)
            return forcedNeighbor(pos, dir);

        if (nextPos == goal){
            jumpPoints.Add(nextPos);
            return jumpPoints;
        }

        return jump(nextPos, goal, dir);
    }

    private bool isValid(Vector2Int pos) {
        if (map.Contains(pos))
            return true;
        else
            return false;
    }
}
