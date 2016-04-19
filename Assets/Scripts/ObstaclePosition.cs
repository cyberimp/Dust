using UnityEngine;
using System.Collections;
using System;

public class ObstaclePosition : MonoBehaviour {

    private Vector2 position;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
    void SetPosition(Vector2 pos)
    {
        Vector2 inverse = pos;
        inverse.y = -inverse.y*0.99f;
        inverse.x *= 0.97f;
        gameObject.transform.position = new Vector2(-0.97f*4f, 0.99f*4f) + inverse;
        position = pos;
    }

    internal bool checkPosition(Vector2 pos)
    {
        return pos == position;
    }
}
