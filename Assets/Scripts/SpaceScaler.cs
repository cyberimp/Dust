using UnityEngine;
using System.Collections;

public class SpaceScaler : MonoBehaviour {

	// Use this for initialization
	void Start () {
        Vector3[] corners = new Vector3[4];
        gameObject.GetComponent<RectTransform>().GetWorldCorners(corners);
        foreach (Vector3 corner in corners)
        {
            Debug.Log(corner);
        }
        Debug.Log(RectTransformUtility.PixelAdjustRect(gameObject.GetComponent<RectTransform>(), transform.parent.GetComponent<Canvas>()));
        Debug.Log(gameObject.GetComponent<RectTransform>().sizeDelta);
        Debug.Log(gameObject.GetComponent<RectTransform>().rect);

    }

    // Update is called once per frame
    void Update () {
	
	}
}
