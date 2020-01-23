using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class GameManager : MonoBehaviour
{

    public static GameManager instance;

    //class random moster
    
    public RegionData curRegions;

    public string nextSpawnPoint;
    //Hero
    public GameObject heroCharacher;

    //position
    public Vector3 nextHeroPosition;
    public Vector3 lastHeroPosition;//battle

    //scenes
    public string sceneToLoad;
    public string lastScene;//battle

    //bools
    public bool isWalking = false;
    public bool isRunning = false;
    public bool canGetEncounter = false;
    public bool gotAttacked = false;

    //ENUM
    public enum GameStates
    {
        WOLRD_STATE,
        TOWN_STATE,
        BATTLE_STATE,
        IDLE
    }

    //battle
    public int enemyAmount;
    public List<GameObject> enemyToBattle = new List<GameObject>();

    public GameStates gameState;

    void Awake()
    {
        //check if instace exist
        if(instance == null)
        {
            //if not set the instace
            instance = this;
        }
        //if it exist but is not this instace
        else if(instance != this)
        {
            //destroy it
            Destroy(gameObject);
        }
        //set this to be not destroyable
        DontDestroyOnLoad(gameObject);

        if (!GameObject.Find("HeroCharacher"))
        {
            GameObject Hero = Instantiate(heroCharacher,Vector3.zero, Quaternion.identity) as GameObject;
            Hero.name = "HeroCharacher";
        }
    }

    void Update()
    {
        switch (gameState)
        {
            case (GameStates.WOLRD_STATE):
                if (isWalking)
                {
                    RandomEncounter();
                }
                if (gotAttacked)
                {
                    gameState = GameStates.BATTLE_STATE;
                }
                break;
            case (GameStates.TOWN_STATE):

                break;
            case (GameStates.BATTLE_STATE):
                //Load Battle Scene
                StartBattle();
                //Go to Idle
                gameState = GameStates.IDLE;
                break;
            case (GameStates.IDLE):
                break;
        }
    }
    public void LoadNextScene()
    {
        SceneManager.LoadScene(sceneToLoad);
    }

    public void LoadSceneAfterBattle()
    {
        SceneManager.LoadScene(lastScene);
    }

    void RandomEncounter()
    {
        if (isWalking && canGetEncounter)
        {
            if(Random.Range(0,1000) < 10)
            {
                Debug.Log("I got attacked");
                gotAttacked = true;
            }
        }
    }

    void StartBattle()
    {
        //Amount of Enemys
        enemyAmount = Random.Range(1, curRegions.maxAmountEnemys + 1);
        //which enemys
        for (int i = 0; i < enemyAmount; i++)
        {
            enemyToBattle.Add(curRegions.possibleEnemys[Random.Range(0, curRegions.possibleEnemys.Count)]);
        }
        //Hero
        lastHeroPosition = GameObject.Find("HeroCharacher").gameObject.transform.position;
        nextHeroPosition = lastHeroPosition;
        lastScene = SceneManager.GetActiveScene().name;
        //Load Level
        SceneManager.LoadScene(curRegions.battleScene);
        //reset hero
        isWalking = false;
        gotAttacked = false;
        canGetEncounter = false;
    }
}
