using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System;

enum Direction
{
    Left,Right,Up,Down
}

public class Character : MonoBehaviour {

    private Vector2 oldPosition;
    [SerializeField]
    private Text hpText;
    private int _hp;
    [SerializeField]
    private Text damText;

    [SerializeField]
    Vector2 position;

    public LevelController level; 

    private Animator animus;
    private int _attack;
    private bool isIdle = true;

    public int hp
    {
        get { return _hp; }
        set
        {
            _hp = value;
            hpText.text = value.ToString();
        }
    }
    public int attack
    {
        get { return _attack; }
        set
        {
            _attack = value;
            damText.text = value.ToString();
        }
    }

    // Use this for initialization
    void Start () {
        animus = GetComponent<Animator>();
        level = GameObject.Find("LevelManager").GetComponent<LevelController>();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void BeginDrag(BaseEventData eventData)
    {
            PointerEventData ed = (PointerEventData)eventData;
            oldPosition = ed.position;
    }

    public void EndDrag(BaseEventData eventData)
    {
        if (isIdle)
        {
            PointerEventData ed = (PointerEventData)eventData;
            Vector2 delta = ed.position - oldPosition;
            if (Mathf.Abs(delta.x) > Mathf.Abs(delta.y))
                if (delta.x < 0)
                    Move(Vector2.left);
                else
                    Move(Vector2.right);
            else
                if (delta.y > 0)
                    Move(Vector2.down);
                else
                    Move(Vector2.up);

        }

    }

    void Move(Vector2 where)
    {
        Vector2 new_pos = position + where;
        if (new_pos.x >= 0 && new_pos.x <= 8 &&
            new_pos.y >= 0 && new_pos.y <= 8 &&
            (level.CheckMap(new_pos) == 1 || level.CheckMap(new_pos) == 6
            || (level.CheckMap(new_pos) == 3 && gameObject.CompareTag("Player"))))
        {
            
            if (level.CheckAttack(new_pos,gameObject))
            {
                level.Attack(new_pos, attack);
                animus.SetTrigger("isAttack");
                return;
            }
            else if (level.CheckMap(new_pos) == 6 && gameObject.CompareTag("Player"))
            {
                hp += 1;
                level.SetMap(new_pos, 1);
                GameObject[] hearts = GameObject.FindGameObjectsWithTag("Heart");
                foreach (GameObject heart in hearts)
                {
                    if (heart.GetComponent<ObstaclePosition>().checkPosition(new_pos))
                        Destroy(heart);
                }
            }
            LeanTween.moveX(gameObject, gameObject.transform.position.x + where.x * 0.97f, 0.5f);
            LeanTween.moveY(gameObject, gameObject.transform.position.y - where.y * 0.99f, 0.5f);
            position = new_pos;
//            level.SendMessage("UpdatePos", gameObject);
        }
        animus.SetTrigger("isMove");
    }

    void ApplyDamage(int value)
    {
        animus.SetTrigger("isHurt");
        hp -= value;
        if (hp <= 0)
            animus.SetTrigger("isDead");
    }

    public void SetIdle(bool state)
    {
        isIdle = state;
    }

    void SetPosition(Vector2 pos)
    {
//        Vector2 inverse = pos;
//        inverse.y = -inverse.y;
        position = pos;
//        gameObject.transform.position = new Vector2(-3.81f, 4.11f) +inverse * 0.93f;
    }

    internal Vector2 GetPosition()
    {
        return position;
    }
}
