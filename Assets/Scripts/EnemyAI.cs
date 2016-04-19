using UnityEngine;
using System.Collections;

public class EnemyAI : MonoBehaviour {

    void MoveTo(Vector2 pos)
    {
        Vector2 new_pos = gameObject.GetComponent<Character>().GetPosition() - pos;
        if (Mathf.Abs(new_pos.x) > Mathf.Abs(new_pos.y))
        {
            if (new_pos.x > 0)
                gameObject.SendMessage("Move", Vector2.left);
            else
                gameObject.SendMessage("Move", Vector2.right);
        }
        else 
        if (new_pos.y > 0)
            gameObject.SendMessage("Move", Vector2.down);
        else
            gameObject.SendMessage("Move", Vector2.up);


    }

    // Use this for initialization
    void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
