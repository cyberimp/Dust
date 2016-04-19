using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Donate : MonoBehaviour {

	// Use this for initialization
	void Start () {
        transform.GetChild(0).GetComponent<Text>().text = "ПРОДОЛЖИТЬ $" + LevelController.instance.price;
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void Click()
    {
        LevelController.instance.BuyReplay();
    }
}
