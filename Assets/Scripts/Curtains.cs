using UnityEngine;
using System.Collections;

public class Curtains : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void OnClick ()
    {
        LeanTween.moveY(GetComponent<RectTransform>(), 1000.0f, 1.0f);
        Destroy(transform.parent.gameObject, 1.0f);
    }
}
