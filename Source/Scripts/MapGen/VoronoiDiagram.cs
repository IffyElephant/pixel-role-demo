using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Distance{
    Manhattan,
    Euclidian
}

public class VoronoiDiagram : AbstractGen
{
    public Vector2Int MinRoomSize, MaxRoomSize;
    public int VoronoiSeeds, RoomCount, Offset;
    public Distance DistanceMethod = Distance.Manhattan;
    public bool ShowValid = true;

    public override void Generate()
    {
        // Init
        random = new(Seed);
        int[,] mapArray;
        HashSet<Vector2Int> map = new();
        List<Vector2Int> roomCenters = new();

        // Generate map
        while (roomCenters.Count < RoomCount)
        {
            map.Clear();
            roomCenters.Clear();

            mapArray = GenerateArray();
            map.UnionWith(ConvertVoronoiEdges(mapArray));

            SimpleRandomWalk.FindDeadEnds(map, roomCenters);
        }

        // Generate rooms
        HashSet<Vector2Int> rooms = GenerateRooms(roomCenters);
        map.UnionWith(rooms);

        HashSet<Vector2Int> cleanMap = new();
        // Check map
        if(ShowValid == true) {
            // Cleanup dead corridors
            for (int i = 0; i < roomCenters.Count - 1; i++) {
                List<Vector2Int> path = AStar.FindPath(roomCenters[i], roomCenters[i + 1], map);
                cleanMap.UnionWith(path);
            }
            cleanMap.UnionWith(rooms);
        } else 
            cleanMap = map;

        // Generate start pos and end pos on map
        LevelGen.GenObjPositions(random, MapSize, cleanMap);
        
        // Generate tiles
        LevelGen.GenTiles(cleanMap);
            
        // Cleanup
        cleanMap.Clear();
        // Cleanup
        // map.Clear(); // Cleanup Issue explained in CA
    }

    private int[,] GenerateArray()
    {
        int[,] mapArray = new int[MapSize, MapSize];

        List<Vector2Int> seedsPos = new();
        for (int i = 0; i < VoronoiSeeds; i++) {
            Vector2Int seedPos = new(random.Next(0, MapSize), random.Next(0, MapSize));
            if(mapArray[seedPos.x, seedPos.y] != 0) {
                i--;
                continue;
            }

            seedsPos.Add(seedPos);
        }

        for (int x = 0; x < MapSize; x++) {
            for (int y = 0; y < MapSize; y++) {
                int minDistance = MapSize; // Set to map width so its far 
                for (int vs = 0; vs < VoronoiSeeds; vs++) {
                    int distance;
                    if (DistanceMethod == Distance.Manhattan)
                        distance = Math.Abs(x - seedsPos[vs].x) + Math.Abs(y - seedsPos[vs].y); // Manhattan distance
                    else
                        distance = (int)Math.Sqrt(Math.Pow(seedsPos[vs].x - x, 2) + Math.Pow(seedsPos[vs].y - y, 2)); // Euclidian distance
                    
                    if (distance < minDistance) {
                        minDistance = distance;
                        mapArray[x,y] = vs;
                    }
                }
            }
        }

        return mapArray;
    }

    private HashSet<Vector2Int> ConvertVoronoiEdges(int[,] mapArray)
    {
        HashSet<Vector2Int> edges = new();
        List<Vector2Int> directions = new() {
            Vector2Int.right,
            Vector2Int.up,
            Vector2Int.right + Vector2Int.up
        };

        for (int x = 0; x < mapArray.GetUpperBound(0); x++) {
            for (int y = 0; y < mapArray.GetUpperBound(1); y++) {
                foreach (var dir in directions) {
                    Vector2Int newPos = new Vector2Int(x, y) + dir;

                    if (newPos.x < 0 || newPos.x > mapArray.GetUpperBound(0) || newPos.y < 0 || newPos.y > mapArray.GetUpperBound(1))
                        continue;

                    if (mapArray[x, y] != mapArray[newPos.x, newPos.y] && edges.Contains(newPos) == false)
                        edges.Add(newPos);
                }
            }
        }

        return edges;
    }

    private HashSet<Vector2Int> GenerateRooms(List<Vector2Int> roomCenters)
    {
        Vector2Int roomCenter, roomSize;
        List<Vector2Int> usedCenters = new();
        HashSet<Vector2Int> rooms = new();

        for (int i = 0; i < RoomCount;) {
            roomCenter = roomCenters[random.Next(0, roomCenters.Count)];
            roomSize = new Vector2Int(random.Next(MinRoomSize.x, MaxRoomSize.x), random.Next(MinRoomSize.y, MaxRoomSize.y));

            HashSet<Vector2Int> room = GenerateRoom(roomCenter, roomSize);
            if(room.Count == 0) // Generated room is invalid
                continue;

            roomCenters.Remove(roomCenter);
            usedCenters.Add(roomCenter);
            rooms.UnionWith(room);

            if(roomCenters.Count == 0)
                break;

            i++;
        }

        roomCenters.Clear();
        roomCenters.AddRange(usedCenters);

        return rooms;
    }

    private HashSet<Vector2Int> GenerateRoom(Vector2Int roomCenter, Vector2Int roomSize)
    {
        HashSet<Vector2Int> room = new();
        Vector2Int startPoint = roomCenter - (roomSize / 2);

        for (int x = Offset; x < roomSize.x - Offset; x++)
            for (int y = Offset; y < roomSize.y - Offset; y++)
                room.Add(startPoint + new Vector2Int(x, y));
        
        return room;
    }
}
