using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BigGameManager : MonoBehaviour
{
    public static BigGameManager instance;

    public Monster nextMonster;


    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        DontDestroyOnLoad(this.gameObject);
    }

        // Start is called before the first frame update
        void Start()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void StartBattle(Monster monster)
    {
        SceneManager.LoadScene("Battle Scene");

        nextMonster = monster;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        Debug.Log("OnSceneLoaded: " + scene.name);
        Debug.Log(mode);

        BattleManager.instance.monster = nextMonster;
    }


}
