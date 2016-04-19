using UnityEngine;
using System.Collections;
using LitJson;
using System;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Purchasing;

public class LevelController : MonoBehaviour, IStoreListener {
    private string gamerId;
    private int sid = 1;
    private long[,] level;
    public long level_no;

    private Vector2 heroPos;
    private Vector2 foe1Pos;
    private Vector2 foe2Pos;
    [SerializeField]
    private Sprite[] obstacles;
    [SerializeField]
    private GameObject obsPrefab;
    [SerializeField]
    private GameObject portalPrefab;
    [SerializeField]
    private GameObject heartPrefab;
    private Character hero;
    private Character foe1;
    private Character foe2;

    private ConfigurationBuilder cb;
    public static LevelController instance;

    private IStoreController controller;
    private IExtensionProvider extensions;


    private string replayID;
    private long replayPrice;

    public long price { get { return replayPrice; } }

    private JsonData testData;

    public void GameOver()
    {
        SceneManager.LoadScene(2);
    }

    public void Fail()
    {
        StartCoroutine(LoadWWW("http://test.playmonstertoons.com/level_failed?id=" + gamerId));
    }

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else if (instance != this)
            Destroy(gameObject);
    }

    public void NextLevel()
    {
        StartCoroutine(LoadWWW("http://test.playmonstertoons.com/level_complete?id=" + gamerId + "&hp=" + hero.hp.ToString()));
    }


    private IEnumerator LoadWWW(string url, WWWForm purchase = null)
    {
        WWW query;
        if (purchase == null)
            query = new WWW(url);
        else
            query = new WWW(url, purchase);
        yield return query;
        testData = JsonMapper.ToObject(query.text);
        AsyncOperation ao = SceneManager.LoadSceneAsync(0);
        ao.allowSceneActivation = true;
        yield return ao;
        StartCoroutine(OnIntro());

    }


    // Use this for initialization
    IEnumerator Start() {
        cb = ConfigurationBuilder.Instance(StandardPurchasingModule.Instance());
        WWW query;
        //yield return query;
        if (PlayerPrefs.HasKey("myId"))
        {
            gamerId = PlayerPrefs.GetString("myId");
            query = new WWW("http://test.playmonstertoons.com/init?sid=" + sid.ToString() + "&id=" + gamerId);
        }
        else {
            gamerId = "";
            query = new WWW("http://test.playmonstertoons.com/init?sid=" + sid.ToString());
        }
        yield return query;
        //        System.IO.File.AppendAllText("test.txt", query.text);
        Debug.Log(query.text);
        testData = JsonMapper.ToObject(query.text);

        //Dust.JSONInit test = JsonMapper.ToObject<Dust.JSONInit>(query.text);
        PlayerPrefs.SetString("myId", testData["id"].GetString());
        PlayerPrefs.Save();
        for  (int i = 0;i < testData["purchases"].Count;i++)
        {

            string id = testData["purchases"][i]["id"].GetString();
            cb.AddProduct(id, ProductType.Consumable);
            if (id.Contains("replay"))
            {
                replayID = id;
                replayPrice = testData["purchases"][i]["price"].GetNatural();
            }

        }
        UnityPurchasing.Initialize(this, cb);
        StartCoroutine(OnIntro());
    }

    public IEnumerator OnIntro()
    {
        level_no = testData["level_num"].GetNatural() + 1;
        GameObject levelNo = GameObject.Find("LevelNo");
        if (levelNo != null)
            levelNo.GetComponent<Text>().text = "УРОВЕНЬ " + level_no.ToString();
        AsyncOperation ao = SceneManager.LoadSceneAsync(1);
        ao.allowSceneActivation = true;
        yield return ao;
        OnLevelLoaded();

    }

    public void OnLevelLoaded(){
        level = new long[testData["level"].Count,testData["level"][0].Count];
        hero = GameObject.Find("Hero").GetComponent<Character>();
        foe1 = GameObject.Find("Foe 1").GetComponent<Character>();
        foe2 = GameObject.Find("Foe 2").GetComponent<Character>();
        foe1Pos = Vector2.zero;
        foe2Pos = Vector2.zero;
        for (int i = 0; i < level.GetLength(0); i++)
        {
            for (int j = 0; j < level.GetLength(1); j++)
            {
                level[j, i] = testData["level"][i][j].GetNatural();
                if((i != 0 || j != 0) && (i != 8 || j!= 8) &&
                    (i != 8 || j != 0) && (i != 0 || j != 8))
                switch (level [j,i])
                {
                    case 0:
                        GameObject obs = Instantiate(obsPrefab);
                        obs.SendMessage("SetPosition", new Vector2(j,i));
                        obs.GetComponent<SpriteRenderer>().sprite = obstacles[UnityEngine.Random.Range(0,5)];
                        break;

                    case 2:
                        heroPos.x = j;
                        heroPos.y = i;
                        hero.SendMessage("SetPosition", heroPos);
                        level[j, i] = 1;
                        break;

                    case 3:
                        GameObject portal = Instantiate(portalPrefab);
                        portal.SendMessage("SetPosition", new Vector2(j, i));

                        break;
                    case 4:
                        foe1Pos.x = j;
                        foe1Pos.y = i;
                        foe1.SendMessage("SetPosition", foe1Pos);
                        level[j, i] = 1;
                        break;
                    case 5:
                        foe2Pos.x = j;
                        foe2Pos.y = i;
                        foe2.SendMessage("SetPosition", foe2Pos);
                        level[j, i] = 1;
                        break;
                    case 6:
                        GameObject heart = Instantiate(heartPrefab);
                        heart.SendMessage("SetPosition", new Vector2(j, i));

                        break;
                    default:
                        break;
                }
            }
        }

        if (foe2Pos == Vector2.zero)
            Destroy(foe2.gameObject);
        if (foe1Pos == Vector2.zero)
            Destroy(foe1.gameObject);


        hero.hp = (int)testData["hp"].GetNatural();
        hero.attack = (int)testData["attack"].GetNatural();

        foe1.hp = 2;
        foe1.attack = 1;
        foe2.hp = 4;
        foe2.attack = 2;


        //SyncPosition();
        //Debug.Log(test.level_num);
        //Debug.Log(test.level);
    }

    public void BuyReplay()
    {
        controller.InitiatePurchase(replayID);
    }

    public void SetMap(Vector2 new_pos, int v)
    {
        level[Mathf.FloorToInt(new_pos.x), Mathf.FloorToInt(new_pos.y)] = v;
    }

    public void Attack(Vector2 new_pos, int attack)
    {
        if (new_pos == hero.GetPosition())
            hero.SendMessage("ApplyDamage", attack);
        if (foe1 != null && new_pos == foe1.GetPosition())
            foe1.SendMessage("ApplyDamage", attack);
        if (foe2 != null && new_pos == foe2.GetPosition())
            foe2.SendMessage("ApplyDamage", attack);

    }

    public bool CheckAttack(Vector2 new_pos, GameObject who)
    {
        if (who.CompareTag("Player"))
            return ((foe1!=null && new_pos == foe1.GetPosition() ) ||
                ( foe2 != null && new_pos == foe2.GetPosition()));
        else
            return (new_pos == hero.GetPosition());

    }

    public int CheckMap(Vector2 where)
    {
        int x = Mathf.FloorToInt(where.x);
        int y = Mathf.FloorToInt(where.y);
        return (int)level[x, y];
    }

    void MoveFoes()
    {
        if (foe1 != null)
            foe1.SendMessage("MoveTo", hero.GetPosition(),SendMessageOptions.DontRequireReceiver);
        if (foe2 != null)
            foe2.SendMessage("MoveTo", hero.GetPosition(), SendMessageOptions.DontRequireReceiver);
    }

    void SyncPosition()
    {
    }

    // Update is called once per frame
    void Update () {

    }

    public void OnInitializeFailed(InitializationFailureReason error)
    {
        Debug.LogWarning(error);
    }

    public PurchaseProcessingResult ProcessPurchase(PurchaseEventArgs e)
    {
        string reciept = e.purchasedProduct.receipt;
        Debug.Log(reciept);
        WWWForm purchase = new WWWForm();
        purchase.AddField("id",gamerId);
        purchase.AddField("pid", replayID);
        purchase.AddField("purchaseData", reciept);
        StartCoroutine(LoadWWW("http://test.playmonstertoons.com/cb/mobile", purchase));
        return PurchaseProcessingResult.Complete;
    }

    public void OnPurchaseFailed(Product i, PurchaseFailureReason p)
    {
        Debug.LogWarning(p);
    }

    public void OnInitialized(IStoreController controller, IExtensionProvider extensions)
    {
        this.controller = controller;
        this.extensions = extensions;
    }
}
