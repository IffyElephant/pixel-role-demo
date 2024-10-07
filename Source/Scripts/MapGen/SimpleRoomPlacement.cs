using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleRoomPlacement : AbstractGen
{
    public Vector2Int minRoomSize, maxRoomSize;
    public int RoomCount, Offset, CorridorWidth;

    public override void Generate()
    {
        // Init
        random = new(Seed);
        HashSet<Vector2Int> map = new();
        List<Vector2Int> roomCenters = new();

        // Generate rooms
        GenerateRooms(roomCenters, map);

        // Generate corridors
        GenerateCorridors(roomCenters, map, CorridorWidth);

        // Generate start pos and end pos on map
        LevelGen.GenObjPositions(random, MapSize, map);
        
        // Generate tiles
        LevelGen.GenTiles(map);

        // Cleanup
        // map.Clear(); // Cleanup Issue explained in CA
    }

    private void GenerateRooms(List<Vector2Int> roomCenters, HashSet<Vector2Int> map)
    {
        Vector2Int roomCenter, roomSize;

        int roomCount = RoomCount + MapSize / 15;

        for (int i = 0; i < roomCount;)
        {
            roomCenter = new Vector2Int(random.Next(0, MapSize), random.Next(0, MapSize));
            roomSize = new Vector2Int(random.Next(minRoomSize.x, maxRoomSize.x), random.Next(minRoomSize.y, maxRoomSize.y));

            // Check if map contains future room
            if (CheckIfMapContainsRoom(roomCenter, roomSize, map, Offset))
                continue;

            HashSet<Vector2Int> room = GenerateRoom(roomCenter, roomSize, map);

            roomCenters.Add(roomCenter);
            map.UnionWith(room);

            i++;
        }
    }

    public static bool CheckIfMapContainsRoom(Vector2Int roomCenter, Vector2Int roomSize, HashSet<Vector2Int> map, int offset)
    {
        Vector2Int startPoint = roomCenter - (roomSize / 2);

        for (int x = -offset; x < roomSize.x + offset; x++)
        {
            for (int y = -offset; y < roomSize.y + offset; y++)
            {
                bool contains = map.Contains(startPoint + new Vector2Int(x, y));
                if (contains)
                    return true;
            }
        }

        return false;
    }

    private HashSet<Vector2Int> GenerateRoom(Vector2Int roomCenter, Vector2Int roomSize, HashSet<Vector2Int> map)
    {
        HashSet<Vector2Int> room = new();
        Vector2Int startPoint = roomCenter - (roomSize / 2);

        for (int x = 0; x < roomSize.x; x++)
        {
            for (int y = 0; y < roomSize.y; y++)
            {
                room.Add(startPoint + new Vector2Int(x, y));
            }
        }

        return room;
    }

    public static void GenerateCorridors(List<Vector2Int> roomCenters, HashSet<Vector2Int> map, int corridorWidth)
    {
        Vector2Int curPos = roomCenters[0];
        int roomCount = roomCenters.Count;

        for (int i = 1; i < roomCount; i++)
        {
            Vector2Int nextRoom = FindClosestCenter(curPos, roomCenters);
            HashSet<Vector2Int> corridor = new();

            Vector2Int dir = Vector2Int.right;

            while (curPos.x != nextRoom.x)
            {
                if (curPos.x < nextRoom.x)
                {
                    curPos += Vector2Int.right;
                    dir = Vector2Int.left;
                }
                else
                {
                    curPos += Vector2Int.left;
                    dir = Vector2Int.right;
                }

                corridor.Add(curPos);
                if (corridorWidth > 1)
                    for (int width = 0; width < corridorWidth; width++)
                    {
                        corridor.Add(curPos + Vector2Int.up * width);
                    }
            }

            while (curPos.y != nextRoom.y)
            {
                if (curPos.y < nextRoom.y)
                {
                    curPos += Vector2Int.up;
                }
                else
                {
                    curPos += Vector2Int.down;
                }

                corridor.Add(curPos);
                if (corridorWidth > 1)
                    for (int width = 0; width < corridorWidth; width++)
                    {
                        corridor.Add(curPos + dir * width);
                    }
            }

            map.UnionWith(corridor);
        }
    }

    public static Vector2Int FindClosestCenter(Vector2Int currentPos, List<Vector2Int> roomCenters)
    {
        int closestId = 0;
        float distance = 0;

        roomCenters.Remove(currentPos);

        for (int i = 0; i < roomCenters.Count; i++)
        {
            float newDistance = Vector2.Distance(currentPos, roomCenters[i]);

            if (newDistance < distance && newDistance > 0)
            {
                distance = newDistance;
                closestId = i;
            }

            if (i == 0)
                distance = newDistance;
        }

        return roomCenters[closestId];
    }
}
