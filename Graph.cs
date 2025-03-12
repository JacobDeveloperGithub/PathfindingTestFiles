using UnityEngine;
using System.Linq;

[RequireComponent(typeof(GraphPathfinder))]
public class Graph : MonoBehaviour {
    private PathNode[] _pathNodes = new PathNode[0];
    private GraphPathfinder _pathing;

    private void Start() => _pathing = GetComponent<GraphPathfinder>();

    public void CalculateNeighbors() {
        for (int i = 0; i < _pathNodes.Length; i++) {
            _pathNodes[i].Neighbors = GetNeighborsOnGraph(_pathNodes[i]);
        }
    }

    public void InitializeArray(int capacity) => _pathNodes = new PathNode[capacity];
    public void AddNode(PathNode data, int atIndex) {
        _pathNodes[atIndex] = data;
        _pathNodes[atIndex].GraphIndex = atIndex;
    }
    
    public PathNode[] GetPath(PathNode from, PathNode to) {
        return _pathing.FindShortestPath(_pathNodes, from, to);
    }

    public PathNode ClosestNodeToPoint(Vector2 point) => _pathNodes.OrderBy(node => Vector2.Distance(point, node.Position)).First();
    public PathNode GetRandomNode() => _pathNodes[Random.Range(0,_pathNodes.Length)];    

    public Neighbor[] GetNeighborsOnGraph(PathNode p) {
        PathNode[] nearbyNodes = _pathNodes.Where(node => Vector2.Distance(p.Position, node.Position) <= CONSTANTS.MAX_DISTANCE_BETWEEN_NODES && node != p).ToArray();
        Neighbor[] neighbors = new Neighbor[nearbyNodes.Length];
        for (int i = 0; i < nearbyNodes.Length; i++) {
            neighbors[i] = new Neighbor{
                Node = nearbyNodes[i],
                Weight = Vector2.Distance(nearbyNodes[i].Position, p.Position) / CONSTANTS.MAX_DISTANCE_BETWEEN_NODES
            };
        }
        return neighbors;
    }
    
    private void OnDrawGizmos() {
        if (_pathNodes == null || _pathNodes.Length == 0) return;
        foreach (PathNode p in _pathNodes) {
            Gizmos.color = Color.black;
            Gizmos.DrawCube(p.Position, Vector3.one * 0.1f);
            
            if (p.Neighbors == null) continue;

            Gizmos.color = Color.white;
            foreach (Neighbor neighbor in p.Neighbors) {
                Gizmos.DrawLine(p.Position, neighbor.Node.Position);
            }
        }
    }
}
