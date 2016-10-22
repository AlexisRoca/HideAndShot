using UnityEngine;
using System.Collections;

public class Zone : MonoBehaviour {

    // Dimension of the active crossing
    protected Vector2 _min = Vector2.zero;
    protected Vector2 _max = Vector2.zero;

    
    // Use this for initialization
    void Start() {
        // Compute dimensions
        Transform dimension = GetComponent<Transform>();
        _min = new Vector2(dimension.position.x, dimension.position.z) - new Vector2(dimension.localScale.x, dimension.localScale.z) * 5;
        _max = new Vector2(dimension.position.x, dimension.position.z) + new Vector2(dimension.localScale.x, dimension.localScale.z) * 5;
    }


    // Indicate if the agent is in the zone
    public bool isInZone (Vector2 position) {
        return position.x > _min.x
            && position.y > _min.y
            && position.x < _max.x
            && position.y < _max.y;
    }
}
