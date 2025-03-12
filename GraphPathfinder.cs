using UnityEngine;
using System.Collections.Generic;

public class SortedPathNodeQueue {
    private List<(PathNode item,float priority)> _queue = new();
    private int[] indices;

    public SortedPathNodeQueue(int capacity) {
        indices = new int[capacity];
        for (int i = 0; i < capacity; i++) {
            indices[i] = -1;
        }
    }
    
    public int Count => _queue.Count;
    public bool Contains(PathNode item) => indices[item.GraphIndex] != -1;
    public void Clear() => _queue.Clear();
    
    public void ModifyPriority(PathNode item, float priority) { 
        int index = indices[item.GraphIndex];
        float currentPriotiy = _queue[index].priority;
        if (currentPriotiy <= priority) return;
        _queue.RemoveAt(index);
        Enqueue(item, priority);
    }
    
    public void Enqueue(PathNode item, float priority) {
        int index = GetInsertIndex(priority);
        _queue.Insert(index, (item, priority));
        indices[item.GraphIndex] = index;
        for (int i = index + 1; i < _queue.Count; i++) {
            indices[_queue[i].item.GraphIndex]++;
        }
    }

    public PathNode Dequeue() {
        PathNode result = _queue[0].item;
        _queue.RemoveAt(0);
        for (int i = 0; i < _queue.Count; i++) {
            indices[_queue[i].item.GraphIndex]++;
        }
        return result;
    }

    private int GetInsertIndex(float priority) {
        int left = 0; 
        int right = _queue.Count; 
        while (left < right) {
            int mid = (left + right) /2;
            if (_queue[mid].priority < priority) {
                left = mid + 1;
            } else {
                right = mid;
            }
        }
        return left;
    }
}

public class GraphPathfinder : MonoBehaviour {
    private Dictionary<(PathNode, PathNode), PathNode[]> _pathCache = new();
    public float Heuristic(PathNode a, PathNode b) => (b.Position - a.Position).sqrMagnitude;

    public PathNode[] FindShortestPath(PathNode[] all, PathNode from, PathNode to) {
        // Step One: Cache check
        if (_pathCache.ContainsKey((from,to))) return _pathCache[(from,to)];

        // Step Two: Intialization
        bool[] visited = new bool[all.Length];
        PathNode[] traceback = new PathNode[all.Length];
        float[] distanceFromStart = new float[all.Length];
        SortedPathNodeQueue toVisit = new(all.Length);

        // Step Three: Initial Values
        toVisit.Enqueue(from, Heuristic(from, to));
        for (int i = 0; i < distanceFromStart.Length; i++) {
            distanceFromStart[i] = float.MaxValue;
        }
        distanceFromStart[from.GraphIndex] = 0;

        // Step Four: Find the path
        while (toVisit.Count > 0 && !visited[to.GraphIndex]) {
            PathNode visiting = toVisit.Dequeue();
            visited[visiting.GraphIndex] = true;
            foreach (Neighbor n in visiting.Neighbors) {
                if (visited[n.Node.GraphIndex]) continue;
                float distance = distanceFromStart[visiting.GraphIndex] + n.Weight;

                if (distance >= distanceFromStart[n.Node.GraphIndex]) continue;
                traceback[n.Node.GraphIndex] = visiting;
                distanceFromStart[n.Node.GraphIndex] = distance;

                if (toVisit.Contains(n.Node)) {
                    toVisit.ModifyPriority(n.Node, Heuristic(n.Node, to));
                } else {
                    toVisit.Enqueue(n.Node, Heuristic(n.Node, to));
                }
            }
        }

        // Step Five: Traceback
        if (!visited[to.GraphIndex]) return new PathNode[0];
        int pathLength = 0;
        for (PathNode p = to; p != null; p = traceback[p.GraphIndex]) {
            pathLength++;
        }

        PathNode[] path = new PathNode[pathLength];
        for (PathNode p = to; p != null; p = traceback[p.GraphIndex]) {
            path[--pathLength] = p;
        }
        _pathCache.Add((from,to), path);
        return path;
    }
}
