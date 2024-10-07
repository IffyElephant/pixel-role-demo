using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BinarySpacePartitioning : AbstractGen
{
    public Vector2Int MinRoomSize;
    public int RoomCount, Offset, CorridorWidth;

    public override void Generate()
    {
        // Init
        random = new(Seed);
        HashSet<Vector2Int> map = new();
        BoundsInt mapBounds = new(Vector3Int.zero, new (MapSize, MapSize, 0));
        List<BoundsInt> roomList = new();
        Queue<BoundsInt> roomQueue = new();

        // Generate room bounds by splits
        GenerateRooms(roomList, roomQueue, mapBounds);

        // Generate map by bounds
        GenerateMap(map, roomList);

        // Generate start pos and end pos on map
        LevelGen.GenObjPositions(random, MapSize, map);

        // Generate tiles
        LevelGen.GenTiles(map);
        
        // Cleanup
        // map.Clear();
    }

    private void GenerateRooms(List<BoundsInt> roomList, Queue<BoundsInt> roomQueue, BoundsInt mapBounds)
    {
        roomQueue.Clear();
        roomList.Clear();

        int rooms = RoomCount + PlayerPrefs.GetInt("Size", 35) / 15;

        roomQueue.Enqueue(mapBounds);
        while (roomQueue.Count > 0)
        {
            BoundsInt room = roomQueue.Dequeue();
            if(roomList.Count >= rooms)
                break;

            int roomX = room.size.x,
                roomY = room.size.y,
                twiceMinRoomX = MinRoomSize.x * 2,
                twiceMinRoomY = MinRoomSize.y * 2;

            if (roomX >= MinRoomSize.x && roomY >= MinRoomSize.y)
            {
                if (random.Next(0, 2) < 1)
                {
                    // After dequing we take room size and split it into new rooms
                    //   to make thigns more even we have 50% chance between spliting horizontaly/verticaly first
                    //   if we cant split anymore but room size si big enough we simply add it and continue
                    if (roomX >= twiceMinRoomX)
                        VerticalSplit(roomQueue, room);
                    else if (roomY >= twiceMinRoomY)
                        HorizontalSplit(roomQueue, room);
                    else if (roomX >= MinRoomSize.x && roomY >= MinRoomSize.y)
                        roomList.Add(room);
                }
                else
                {
                    if (roomY >= twiceMinRoomY)
                        HorizontalSplit(roomQueue, room);
                    else if (roomX >= twiceMinRoomX)
                        VerticalSplit(roomQueue, room);
                    else if (roomX >= MinRoomSize.x && roomY >= MinRoomSize.y)
                        roomList.Add(room);
                }
            }
        }
    }

    private void HorizontalSplit(Queue<BoundsInt> roomQueue, BoundsInt currentRoom)
    {
        int ySplit = random.Next(1, currentRoom.size.y);

        BoundsInt newRoom1 = new(currentRoom.min, new Vector3Int(currentRoom.size.x, ySplit, currentRoom.size.z));
        BoundsInt newRoom2 = new(new Vector3Int(currentRoom.min.x, currentRoom.min.y + ySplit, currentRoom.min.z),
                                           new Vector3Int(currentRoom.size.x, currentRoom.size.y - ySplit, currentRoom.size.z));

        roomQueue.Enqueue(newRoom1);
        roomQueue.Enqueue(newRoom2);
    }

    private void VerticalSplit(Queue<BoundsInt> roomQueue, BoundsInt currentRoom)
    {
        int xSplit = random.Next(1, currentRoom.size.x);

        BoundsInt newRoom1 = new(currentRoom.min, new Vector3Int(xSplit, currentRoom.size.y, currentRoom.size.z));
        BoundsInt newRoom2 = new(new Vector3Int(currentRoom.min.x + xSplit, currentRoom.min.y, currentRoom.min.z),
                                           new Vector3Int(currentRoom.size.x - xSplit, currentRoom.size.y, currentRoom.size.z));

        roomQueue.Enqueue(newRoom1);
        roomQueue.Enqueue(newRoom2);
    }

    private void GenerateMap(HashSet<Vector2Int> map, List<BoundsInt> roomList)
    {
        map.UnionWith(BoundToHashRoom(roomList));
        List<Vector2Int> roomCenters = new();

        foreach (var room in roomList)
        {
            roomCenters.Add(new Vector2Int((int)room.center.x, (int)room.center.y));
        }

        SimpleRoomPlacement.GenerateCorridors(roomCenters, map, CorridorWidth);
    }

    private HashSet<Vector2Int> BoundToHashRoom(List<BoundsInt> roomList)
    {
        HashSet<Vector2Int> rooms = new();

        foreach (var room in roomList)
        {
            for (int row = Offset; row < room.size.x - Offset; row++)
            {
                for (int col = Offset; col < room.size.y - Offset; col++)
                {
                    Vector2Int newPos = (Vector2Int)room.min + new Vector2Int(row, col);
                    rooms.Add(newPos);
                }
            }
        }

        return rooms;
    }
}
