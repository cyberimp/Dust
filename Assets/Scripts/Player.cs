using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour {


    void Awake()
    {
        if (gameObject.name.Contains("Clone"))
            Destroy(gameObject);
    }
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
