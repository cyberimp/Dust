using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class FPS : MonoBehaviour {

    private float FPSvalue;
    private Text FPStext;
    private float deltaTime;

    private float CD = 1.0f;
    private float frames = 0.0f;

    // Use this for initialization
    void Start () {
        FPStext = GetComponent<Text>();
	}
	
	// Update is called once per frame
	void Update () {
        deltaTime += Time.deltaTime;
        frames++;
        if (deltaTime > CD)
        {
            FPSvalue = Mathf.Floor(frames / deltaTime);
            frames = 0.0f;
            deltaTime = 0.0f;
            FPStext.text = "FPS:" + FPSvalue.ToString();
        }
	}
}
