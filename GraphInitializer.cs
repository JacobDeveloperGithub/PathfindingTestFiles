using UnityEngine;

[RequireComponent(typeof(Graph))]
public class GraphInitializer : MonoBehaviour {
    [SerializeField] private PathNodeBehaviour[] _initialPathNodes;

    private Graph _self;

    private void Start() {
        _self = GetComponent<Graph>();
        _self.InitializeArray(_initialPathNodes.Length);
        for (int i = 0; i < _initialPathNodes.Length; i++) {
            _self.AddNode(_initialPathNodes[i].BehaviourToNode(), i);
            Destroy(_initialPathNodes[i].gameObject);
        }
        _self.CalculateNeighbors();
        Destroy(this);
    }    
}
