using UnityEngine;
using System.Collections;

public class failController : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void Fail()
    {
        LevelController.instance.Fail();

    }
}
