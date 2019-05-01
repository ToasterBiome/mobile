using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class BattleManager : MonoBehaviour {


    public static BattleManager instance;

    public bool isFighting = false;

    public GameObject enemy;
    public float enemyOffset;

    public GameObject shield;

    public float enemyHp = 100f;

    public float playerHP = 100f;

    public bool hasShield = false;
    public bool shieldActive = false;
    public float shieldHp;

    public Slider enemyHpSlider;
    public Text enemyHpText;

    public Slider playerHpSlider;
    public Text playerHpText;

    public Slider energySlider;
    public Text timerText;

    public Slider shieldSlider;
    public Text shieldText;

    public Text enemyNameLevel;

    public bool yourTurn = true;

    public GameObject slash;

    public GameObject damageText;

    public GameObject canvas;

    public GameObject criticalExplosion;

    public GameObject regularExplosion;

    public GameObject failExplosion;

    public CursorController cursorController;

    public GameObject menuButtons;

    public GameObject lootMenu;

    public Monster monster;

    public PlayerData player;

    public enum BattlePhase
    {
        Start,
        Idle,
        StartAttack,
        WaitForAttack,
        Attacking,
        EndAttack,
        Switch,
        DefendStart,
        Defend,
        DefendEnd,
        Ending,
        End,
        Ended
    }

    public BattlePhase currentBattlePhase = BattlePhase.Start;

    public float timer = 2.0f;

    public float maxTimer = 2.0f;



	// Use this for initialization
	void Start () {
        cursorController = slash.GetComponent<CursorController>();
        Application.targetFrameRate = 60;
        setEnemy(monster);
    }

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
    }

    // Update is called once per frame
    void Update () {




        if (Input.GetKeyDown(KeyCode.Space))
        {
            StartCoroutine(EnemyAttack());
        }
      
        if(currentBattlePhase == BattlePhase.Start)
        {
            currentBattlePhase = BattlePhase.Idle;
            menuButtons.SetActive(true);
            StartCoroutine(FadeInUI(menuButtons,15f));
            return;
        }   
        if(currentBattlePhase == BattlePhase.Idle)
        {
            return;
        }
        if(currentBattlePhase == BattlePhase.StartAttack)
        {
            currentBattlePhase = BattlePhase.WaitForAttack;
            timer = maxTimer;
            timerText.text = "2:00";
            energySlider.value = 1f;
            return;
           
        }
        if (currentBattlePhase == BattlePhase.WaitForAttack)
        {
            if(Input.GetMouseButtonDown(0))
            {
                currentBattlePhase = BattlePhase.Attacking;
            }
            return;

        }
        if (currentBattlePhase == BattlePhase.Attacking)
        {
            timer -= Time.deltaTime;
            if (timer > 0)
            {
                timerText.text = (timer % 60).ToString("N2");
                energySlider.value = timer / maxTimer;
            }
            else
            {
                timerText.text = "0:00";
                energySlider.value = 0f;
            }
            
            if(timer < 0)
            {
                if(slash.GetComponent<SlashContainer>().inUse)
                {
                    return;
                }
                currentBattlePhase = BattlePhase.EndAttack;
            }
            
            return;
        }
        if(currentBattlePhase == BattlePhase.EndAttack)
        {
            if(!slash.GetComponent<SlashContainer>().inUse)
            {
                slash.SetActive(false);
                currentBattlePhase = BattlePhase.Switch;
            }
            return;
        }
        if (currentBattlePhase == BattlePhase.Switch)
        {
            currentBattlePhase = BattlePhase.DefendStart;
            return;
        }
        if (currentBattlePhase == BattlePhase.DefendStart)
        {
            StartCoroutine(EnemyAttack());
            currentBattlePhase = BattlePhase.Defend;
            return;
        }
        if (currentBattlePhase == BattlePhase.Defend)
        {
            //do nothing
            return;
        }
        if (currentBattlePhase == BattlePhase.DefendEnd)
        {
            //do nothing
            currentBattlePhase = BattlePhase.Start;
            return;
        }
        if (currentBattlePhase == BattlePhase.Ending)
        {
            if (!slash.GetComponent<SlashContainer>().inUse)
            {
                slash.SetActive(false);
                currentBattlePhase = BattlePhase.End;
                
            }
            Debug.Log("Battle ending");
            //currentBattlePhase = BattlePhase.Start;
            return;
        }
        if (currentBattlePhase == BattlePhase.End)
        {
            Debug.Log("Battle ended");
            lootMenu.SetActive(true);
            lootMenu.GetComponent<CanvasGroup>().alpha = 0f;
            List<ItemStack> loot = new List<ItemStack>();
            loot = monster.loot.getRandomItem(5);
            lootMenu.GetComponent<LootManager>().GenerateItems(loot);
            StartCoroutine(FadeInLoot());
            currentBattlePhase = BattlePhase.Ended;
            return;
        }
        if (currentBattlePhase == BattlePhase.Ended)
        {
            return;
        }









    }

    public void startAttack()
    {
        currentBattlePhase = BattlePhase.StartAttack;
        
        StartCoroutine(FadeOutUI(menuButtons,15f));
        slash.SetActive(true);
    }

    IEnumerator ShakeEnemy(float intensity, float seconds)
    {
        float waitTime = seconds;
        float counter = 0f;
        while (counter < waitTime)
        {
            enemy.transform.position = Random.insideUnitCircle * intensity * (1 - counter/waitTime);
            counter += Time.deltaTime;
            yield return null;
        }
        enemy.transform.position = new Vector2(0, 0);
    }

    IEnumerator DissolveEnemy()
    {
        float waitTime = 2f; //seconds
        float counter = 0f;
        while (counter < waitTime)
        {
            enemy.GetComponent<Renderer>().material.SetFloat("_DissolvePower", 1 - counter/waitTime);
            enemy.transform.GetChild(0).GetComponent<Renderer>().material.SetFloat("_DissolvePower", 1 - counter / waitTime);
            counter += Time.deltaTime;
            yield return null;
        }
        Destroy(enemy);
    }

    IEnumerator DestroyShield()
    {
        float waitTime = 1.5f; //seconds
        float counter = 0f;
        while (counter < waitTime)
        {
            shield.GetComponent<Renderer>().material.SetFloat("_DissolvePower", 1 - counter / waitTime);
            counter += Time.deltaTime;
            yield return null;
        }
        //shield = null;
        //Destroy(shield);
    }

    IEnumerator FadeInUI(GameObject UI, float numberOfFrames)
    {
        UI.SetActive(true);
        for (int i = 0; i < (int)numberOfFrames; i++)
        {
            UI.GetComponent<CanvasGroup>().alpha = (i / numberOfFrames);
            yield return null;
        }
        UI.GetComponent<CanvasGroup>().alpha = 1f;
    }

    IEnumerator FadeOutUI(GameObject UI, float numberOfFrames)
    {
        for (int i = (int)numberOfFrames; i > 0; i--)
        {
            UI.GetComponent<CanvasGroup>().alpha = (i / numberOfFrames);
            yield return null;
        }
        UI.GetComponent<CanvasGroup>().alpha = 0f;
        UI.SetActive(false);
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    IEnumerator FadeInLoot()
    {
        yield return new WaitForSeconds(1f);
        for (int i = 0; i < 60; i++)
        {
            lootMenu.GetComponent<CanvasGroup>().alpha = (i / 60f);
            yield return null;
        }
    }

    public bool checkForSuccessfulHit()
    {
        if (slash.GetComponent<SlashContainer>().successfulAttack)
        {
            bool killed = false;
            float damage = 5f;
            if (slash.GetComponent<SlashContainer>().criticalAttack)
            {
                damage *= 1.5f;
            }
            damage = Mathf.Floor(damage);

            if(shieldActive) //reroute damage to shield first
            {
                damage -= monster.baseShieldDEF;
                shieldHp -= damage;
                shieldSlider.value = (shieldHp / monster.baseShieldHP);
                shieldText.text = Mathf.Ceil(shieldHp).ToString() + "/" + monster.baseShieldHP;
                spawnDamageText(Vector3.zero, damage, slash.GetComponent<SlashContainer>().criticalAttack);
                StartCoroutine(ShakeEnemy(0.125f, .25f));
                if (shieldHp <= 0) //shield dead
                {
                    shieldSlider.value = (0 / monster.baseShieldHP);
                    shieldText.text = "0/" + monster.baseShieldHP;
                    shieldHp = 0f;
                    shieldActive = false;
                    StartCoroutine(DestroyShield());
                    return false;
                } else
                {
                    return false;
                }
            }



            damage -= monster.baseDEF;
            if(damage <= 0)
            {
                damage = 0;

                spawnDamageText(Vector3.zero, damage, slash.GetComponent<SlashContainer>().criticalAttack);
                StartCoroutine(ShakeEnemy(0.125f, .25f));
                return false;
            }
            enemyHp -= damage;
            if (enemyHp <= 0)
            {
                enemyHp = 0;
                
                killed = true;
            }
            enemyHpSlider.value = (enemyHp / monster.baseHP);
            enemyHpText.text = Mathf.Ceil(enemyHp).ToString() + "/" + monster.baseHP;

            spawnDamageText(Vector3.zero, damage, slash.GetComponent<SlashContainer>().criticalAttack);

            //Debug.Log(slash.GetComponent<CursorController>().midPoint);
            StartCoroutine(ShakeEnemy(0.25f,.5f));
            if(killed)
            {
                return true;
            } else
            {
                return false;
            }
        } else
        {
            return false;
        }
       
    }

    public bool recieveHit(GameObject slash)
    {
        if (slash.GetComponent<Slash>().hitType >= 1) //if normal hit or above
        {
            bool killed = false;
            float damage = 5f;
            if (slash.GetComponent<Slash>().hitType == 2) //if critical
            {
                damage *= 1.5f;
            }
            damage = Mathf.Floor(damage);

            if (shieldActive) //reroute damage to shield first
            {
                damage -= monster.baseShieldDEF;
                shieldHp -= damage;
                shieldSlider.value = (shieldHp / monster.baseShieldHP);
                shieldText.text = Mathf.Ceil(shieldHp).ToString() + "/" + monster.baseShieldHP;
                spawnDamageText(Vector3.zero, damage, slash.GetComponent<SlashContainer>().criticalAttack);
                StartCoroutine(ShakeEnemy(0.125f, .25f));
                if (shieldHp <= 0) //shield dead
                {
                    shieldSlider.value = (0 / monster.baseShieldHP);
                    shieldText.text = "0/" + monster.baseShieldHP;
                    shieldHp = 0f;
                    shieldActive = false;
                    StartCoroutine(DestroyShield());
                    return false;
                }
                else
                {
                    return false;
                }
            }



            damage -= monster.baseDEF;
            if (damage <= 0)
            {
                damage = 0;

                spawnDamageText(Vector3.zero, damage, slash.GetComponent<Slash>().hitType);
                StartCoroutine(ShakeEnemy(0.125f, .25f));
                return false;
            }
            enemyHp -= damage;
            if (enemyHp <= 0)
            {
                enemyHp = 0;

                killed = true;
            }
            enemyHpSlider.value = (enemyHp / monster.baseHP);
            enemyHpText.text = Mathf.Ceil(enemyHp).ToString() + "/" + monster.baseHP;

            spawnDamageText(Vector3.zero, damage, slash.GetComponent<Slash>().hitType);

            //Debug.Log(slash.GetComponent<CursorController>().midPoint);
            StartCoroutine(ShakeEnemy(0.25f, .5f));
            if (killed)
            {
                StartCoroutine("DissolveEnemy");
                currentBattlePhase = BattlePhase.Ending;
                return true;
            }
            else
            {
                return false;
            }
        }
        else
        {
            spawnDamageText(Vector3.zero, 0, slash.GetComponent<Slash>().hitType);
            StartCoroutine(ShakeEnemy(0.125f, .25f));
            return false;
        }
    }

    public GameObject spawnDamageText(Vector2 location, float number, int hitType)
    {
        //spawn the actual text
        GameObject damageTextObject = Instantiate(damageText, slash.GetComponent<SlashContainer>().slashMidPos, Quaternion.identity);
        damageTextObject.transform.SetParent(canvas.transform);
        damageTextObject.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, 0);
        damageTextObject.GetComponent<Text>().text = number.ToString();
        damageTextObject.GetComponent<Text>();
        damageTextObject.GetComponent<RectTransform>().localScale = new Vector3(1, 1, 1);
        //critical or not
        if(hitType == 0)
        {
            damageTextObject.GetComponent<Text>().color = new Color(.5f, 0.5f, 0.5f);
            damageTextObject.GetComponent<Text>().text = "miss";
            damageTextObject.GetComponent<Text>().fontSize = 48;
            damageTextObject.GetComponent<Outline>().effectDistance = new Vector2(1, 1);

            GameObject critObject = Instantiate(failExplosion, slash.GetComponent<SlashContainer>().slashMidPos, Quaternion.identity); //create fail explosion gameObject

            critObject.transform.SetParent(canvas.transform);
            critObject.GetComponent<RectTransform>().localScale = new Vector3(1, 1, 1);
            return damageTextObject;
        }

        if (hitType == 2)
        {
            damageTextObject.GetComponent<Text>().color = new Color(1, 0.862f, 0.180f); //set color to gold

            GameObject critObject = Instantiate(criticalExplosion, slash.GetComponent<SlashContainer>().slashMidPos, Quaternion.identity); //create critical explosion gameObject

            critObject.transform.SetParent(canvas.transform);
            critObject.GetComponent<RectTransform>().localScale = new Vector3(1, 1, 1);
        }
        else
        {
            GameObject normObject = Instantiate(regularExplosion, slash.GetComponent<SlashContainer>().slashMidPos, Quaternion.identity); //create normal explosion gameObject

            normObject.transform.SetParent(canvas.transform);
            normObject.GetComponent<RectTransform>().localScale = new Vector3(1, 1, 1);
        }

        return damageTextObject;
    }

    public GameObject spawnDamageText(Vector2 location, float number, bool critical)
    {
        //spawn the actual text
        GameObject damageTextObject = Instantiate(damageText, slash.GetComponent<SlashContainer>().slashMidPos, Quaternion.identity);
        damageTextObject.transform.SetParent(canvas.transform);
        damageTextObject.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, 0);
        damageTextObject.GetComponent<Text>().text = number.ToString();
        damageTextObject.GetComponent<Text>();
        damageTextObject.GetComponent<RectTransform>().localScale = new Vector3(1, 1, 1);
        //critical or not
        if (number == 0)
        {
            damageTextObject.GetComponent<Text>().color = new Color(.5f, 0.5f, 0.5f);

            GameObject critObject = Instantiate(failExplosion, slash.GetComponent<SlashContainer>().slashMidPos, Quaternion.identity); //create fail explosion gameObject

            critObject.transform.SetParent(canvas.transform);
            critObject.GetComponent<RectTransform>().localScale = new Vector3(1, 1, 1);
            return damageTextObject;
        }

        if (critical)
        {
            damageTextObject.GetComponent<Text>().color = new Color(1, 0.862f, 0.180f); //set color to gold

            GameObject critObject = Instantiate(criticalExplosion, slash.GetComponent<SlashContainer>().slashMidPos, Quaternion.identity); //create critical explosion gameObject

            critObject.transform.SetParent(canvas.transform);
            critObject.GetComponent<RectTransform>().localScale = new Vector3(1, 1, 1);
        }
        else
        {
            GameObject normObject = Instantiate(regularExplosion, slash.GetComponent<SlashContainer>().slashMidPos, Quaternion.identity); //create normal explosion gameObject

            normObject.transform.SetParent(canvas.transform);
            normObject.GetComponent<RectTransform>().localScale = new Vector3(1, 1, 1);
        }

        return damageTextObject;
    }

    IEnumerator EnemyAttack()
    {
        float timeToWait = .5f;
        float counter = 0f;
        while(counter < timeToWait)
        {
            counter += Time.deltaTime;
            yield return null;
        }
        int phaseOfAttack = 0;
        while(enemy.transform.position.y < 0.5f && phaseOfAttack == 0)
        {
            enemy.transform.position = new Vector2(0, enemy.transform.position.y + Time.deltaTime * 4);
            yield return null;
        }
        enemy.transform.position = new Vector2(0, 0.5f);
        phaseOfAttack = 1;
        while (enemy.transform.position.y > -1 && phaseOfAttack == 1)
        {
            enemy.transform.position = new Vector2(0, enemy.transform.position.y - Time.deltaTime * 16);
            yield return null;
        }
        enemy.transform.position = new Vector2(0, -1f);
        playerHit();
        phaseOfAttack = 2;
        while (enemy.transform.position.y < 0 && phaseOfAttack == 2)
        {
            enemy.transform.position = new Vector2(0, enemy.transform.position.y + Time.deltaTime * 8);
            yield return null;
        }
        enemy.transform.position = new Vector2(0, 0);
        phaseOfAttack = 3;
        enemyAttackProcess();
    }

    public void playerHit()
    {
        StartCoroutine(ShakePlayer(0.25f, .5f));
        playerHP -= 10;
        playerHpSlider.value = (playerHP / 100f);
        playerHpText.text = playerHP.ToString() + "/100";
    }

    public void enemyAttackProcess()
    {
        
        currentBattlePhase = BattlePhase.DefendEnd;
    }

    IEnumerator ShakePlayer(float intensity, float seconds)
    {
        Vector2 originalPos = playerHpSlider.gameObject.transform.position;
        float waitTime = seconds;
        float counter = 0f;
        while (counter < waitTime)
        {
            playerHpSlider.gameObject.transform.position = originalPos + Random.insideUnitCircle * intensity * (1 - counter / waitTime);
            counter += Time.deltaTime;
            yield return null;
        }
        playerHpSlider.gameObject.transform.position = originalPos;
    }


    public void setEnemy(Monster monster)
    {
        enemy.GetComponent<SpriteRenderer>().sprite = monster.sprite;
        enemyHp = monster.baseHP;
        enemyNameLevel.text = monster.name.ToUpper() + " LV. " + monster.level;
        enemyHpText.text = monster.baseHP + "/" + monster.baseHP;
        if(monster.hasShield)
        {
            shieldSlider.gameObject.SetActive(true);
            shieldText.text = monster.baseShieldHP + "/" + monster.baseShieldHP;
            shieldHp = monster.baseShieldHP;
            hasShield = true;
            shieldActive = true;
            shield.GetComponent<SpriteRenderer>().sprite = monster.sheildSprite;
        } else
        {
            shield.GetComponent<SpriteRenderer>().sprite = null;
            shieldSlider.gameObject.SetActive(false);
            hasShield = false;
            shieldActive = false;
        }
    }

    public void toggleMenu(GameObject menu)
    {
        Debug.Log("Toggling");
        if(menu.activeInHierarchy)
        {
            StartCoroutine(FadeOutUI(menu, 8f));
        } else
        {
            StartCoroutine(FadeInUI(menu, 8f));
        }
    }
}
