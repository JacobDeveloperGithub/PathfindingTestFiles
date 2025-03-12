using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public struct Neighbor {
    public PathNode Node;
    public float Weight;
}

public class PathNode {
    public Vector2 Position;
    public Neighbor[] Neighbors;
    public int GraphIndex;

    public static bool operator== (PathNode a, PathNode b) {
        if (ReferenceEquals(a, b)) return true;
        if (a is null || b is null) return false;
        return a.Position == b.Position;
    }
    public static bool operator!= (PathNode a, PathNode b) => !(a == b);
    public override bool Equals(object obj) {
        if (obj is PathNode other) {
            return Position == other.Position;
        }
        return false;
    }
    public override int GetHashCode() => Position.GetHashCode();
}

[ExecuteInEditMode]
public class PathNodeBehaviour : MonoBehaviour {
    private List<PathNodeBehaviour> _neighbors = new(); // Only used for rendering a gizmo lol

    public PathNode BehaviourToNode() {
        return new PathNode
        {
            Position = transform.position
        };
    }

    private void Awake()
    {
        ResetAndPopulateNeighbors();
    }

    
    // Everything below here is to be ran during editor for ease of building and has no use ingame
    private void Update() {
        if (!transform.hasChanged) return;
        ResetAndPopulateNeighbors();
    }

    private void ResetAndPopulateNeighbors() {
        _neighbors = new();
        foreach (PathNodeBehaviour obj in FindObjectsByType<PathNodeBehaviour>(FindObjectsSortMode.None)) {
            if (Vector2.Distance(obj.transform.position, transform.position) < CONSTANTS.MAX_DISTANCE_BETWEEN_NODES) {
                _neighbors.Add(obj);
            }
        }
    }

    private void OnDrawGizmos()
    {
        // If the editor is in test mode, do not render a gizmo
        if (EditorApplication.isPlaying) return;
        Gizmos.color = Color.black;
        Gizmos.DrawWireSphere(transform.position, 0.1f); // Small gizmo dot to help place transforms in editor
        Gizmos.color = Color.white;
        foreach (PathNodeBehaviour obj in _neighbors) {
            Gizmos.DrawLine(transform.position, obj.transform.position);
            Gizmos.DrawLine(transform.position, obj.transform.position);
        }
    }
}
