using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Graph), typeof(GraphPathfinder))]
public class GraphPathTester : MonoBehaviour {
	private Graph _self;

    private PathNode to;
    private PathNode from;
    private PathNode[] path;
    private bool setTo = true;


	private void Awake() {
		_self = GetComponent<Graph>();
	}

    private void Update() {
        if (Mouse.current.leftButton.wasPressedThisFrame) {
            if (setTo) {
                to = _self.ClosestNodeToPoint(Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue()));
            } else {
                from = _self.ClosestNodeToPoint(Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue()));
                if (to != null && from != null) {
                    path = _self.GetPath(to, from);
                }
            }
            setTo = !setTo;
        }
    }
	
    private void OnDrawGizmos() {
        if (path != null) {
            Gizmos.color = Color.cyan;
            if (to != null)
                Gizmos.DrawSphere(to.Position, 0.05f);
            if (from != null)
                Gizmos.DrawSphere(from.Position, 0.05f);
            if (path.Length <= 1) return;
            for (int i = 1; i < path.Length; i++) {
                Vector2 thicknessOffset = Vector2.one * 0.02f;
                Vector2 thicknessOffset2 = Vector2.one * -0.02f;
                Gizmos.DrawLine(path[i].Position, path[i-1].Position);
                Gizmos.DrawLine(path[i].Position + thicknessOffset, path[i-1].Position + thicknessOffset);
                Gizmos.DrawLine(path[i].Position + thicknessOffset2, path[i-1].Position + thicknessOffset2);
            }
        }
    }
}