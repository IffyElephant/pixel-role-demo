using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Helping class for A* pathfinding sice unity doesnt have priority que that we could use for it

public class PriorityQueue<T>
{
    private readonly SortedDictionary<float, Queue<T>> elements = new();

    public void Enqueue(T item, float priority) {
        if (!elements.ContainsKey(priority))
            elements[priority] = new Queue<T>();

        elements[priority].Enqueue(item);
    }

    public T Dequeue() {
        var first = elements.Keys.GetEnumerator();
        first.MoveNext();
        var item = elements[first.Current].Dequeue();

        if (elements[first.Current].Count == 0)
            elements.Remove(first.Current);

        return item;
    }

    public bool Contains(T item) {
        foreach (var queue in elements.Values)
        {
            if (queue.Contains(item))
                return true;
        }
        return false;
    }

    public int Count {
        get {
            int count = 0;
            foreach (var queue in elements.Values) {
                count += queue.Count;
            }
            return count;
        }
    }
}