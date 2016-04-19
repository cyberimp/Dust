using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ProgressController : MonoBehaviour {

	// Use this for initialization
	void Start () {
        GetComponent<Text>().text = "ТЫ ДОШЁЛ ДО " + LevelController.instance.level_no.ToString() + " УРОВНЯ";
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
