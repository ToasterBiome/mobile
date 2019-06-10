using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
using System;
using Random = UnityEngine.Random;

public class BattleManager : MonoBehaviour {

    public static BattleManager instance;
    [Header("Prefabs")]
    public GameObject damageText;
    public GameObject criticalExplosion;
    public GameObject regularExplosion;
    public GameObject failExplosion;
    [Header("Links")]
    public GameObject canvas;
    public GameObject slash;
    public GameObject menuButtons;
    public GameObject lootMenu;
    public ParticleSystem healthRestore;
    public GameObject shield;
    public GameObject blackFade;
    [Header("Battle Settings")]
    public GameObject enemy;
    public float enemyOffset;
    public Monster monster;
    public PlayerData player;
    public float timer = 2.0f;
    public float maxTimer = 2.0f;
    public bool hasShield = false;
    public bool shieldActive = false;
    public float shieldHp;
    public float enemyHp = 100f;
    public float playerHP = 100f;
    [Header("UI Stuff")]
    public TextMeshProUGUI enemyNameLevel;
    public Slider enemyHpSlider;
    public TextMeshProUGUI enemyHpText;
    public Slider playerHpSlider;
    public TextMeshProUGUI playerHpText;
    public Image playerHpColor;
    public Slider energySlider;
    public Text timerText;
    public Slider shieldSlider;
    public Text shieldText;
    public Image LightningFlash;

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
    // Use this for initialization
    void Start () {
        Application.targetFrameRate = 60;
        setEnemy(monster);
        player.hp = 100;
        PlayerPrefs.DeleteAll();
        refreshUI();
        StartCoroutine(BattleFade());
    }



    IEnumerator BattleFade()
    {
        float waitTime = 1f; //seconds
        float counter = 0f;
        while (counter < waitTime)
        {
            blackFade.GetComponent<Image>().material.SetFloat("_DissolvePower", 1 - counter / waitTime);
            counter += Time.deltaTime;
            yield return null;
        }
    }

    private void OnValidate()
    {
        if(monster != null)
        {
            setEnemy(monster);
        } else
        {
            Debug.LogWarning("Monster is not set in BattleManager");
        }
        
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
            Lightning();
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
            //check if lightning

            int rand = Random.Range(1, 360);
            if(rand == 1)
            {
                StartCoroutine(LightningStrike());
            }

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

    IEnumerator LightningStrike()
    {
        Lightning();

        for (int i = 60; i > 0; i--)
        {
            Color lightningColor = LightningFlash.GetComponent<Image>().color;
            lightningColor.a = (i / 60f);
            LightningFlash.GetComponent<Image>().color = lightningColor;
            yield return null;
        }
    }

    public void recieveHit(GameObject slash)
    {
        int hitType = slash.GetComponent<Slash>().hitType;

        if (hitType >= 1) //if normal hit or above
        {
            float damage = 5f;
            if (hitType == 2) //if critical
            {
                damage *= 1.5f;
            }

            if (slash.GetComponent<Slash>().distance > 5)
            {
                damage += (slash.GetComponent<Slash>().distance / 5f);
            }

            damage = Mathf.Floor(damage);

            if (shieldActive) //reroute damage to shield first
            {

                damage -= monster.baseShieldDEF;
                if (damage <= 0)
                {
                    damage = 0;
                    spawnDamageText(Vector3.zero, damage, 0);
                    slash.GetComponent<Slash>().color = new Color(1, 0.862f, 0.180f);
                    StartCoroutine(ShakeEnemy(0.125f, .25f));
                    return;
                }
                shieldHp -= damage;
                shieldSlider.value = (shieldHp / monster.baseShieldHP);
                shieldText.text = Mathf.Ceil(shieldHp).ToString() + "/" + monster.baseShieldHP;
                spawnDamageText(Vector3.zero, damage, hitType);
                StartCoroutine(ShakeEnemy(0.125f, .25f));
                if (shieldHp <= 0) //shield dead
                {
                    shieldSlider.value = (0 / monster.baseShieldHP);
                    shieldText.text = "0/" + monster.baseShieldHP;
                    shieldHp = 0f;
                    shieldActive = false;
                    StartCoroutine(DestroyShield());
                    return;
                }
                else
                {
                    return;
                }
            }

            TakeDamage(hitType, damage);
            return;
        }
        else
        {
            spawnDamageText(Vector3.zero, 0, hitType);
            StartCoroutine(ShakeEnemy(0.125f, .25f));
            return;
        }
    }

    private void TakeDamage(int hitType,float damage)
    {
        damage -= monster.baseDEF;
        if (damage <= 0)
        {
            damage = 0;

            spawnDamageText(Vector3.zero, damage, hitType);
            StartCoroutine(ShakeEnemy(0.125f, .25f));
            return;
        }
        enemyHp -= damage;
        if (enemyHp <= 0)
        {
            enemyHp = 0;

            StartCoroutine("DissolveEnemy");
            currentBattlePhase = BattlePhase.Ending;
        }
        enemyHpSlider.value = (enemyHp / monster.baseHP);
        enemyHpText.text = Mathf.Ceil(enemyHp).ToString() + "/" + monster.baseHP;

        spawnDamageText(Vector3.zero, damage, hitType);

        StartCoroutine(ShakeEnemy(0.25f, .5f));

        return;
    }

    public void Lightning()
    {
        TakeDamage(3, 16);
    }

    public GameObject spawnDamageText(Vector2 location, float number, int hitType)
    {
        //spawn the actual text
        GameObject damageTextObject = Instantiate(damageText, slash.GetComponent<SlashContainer>().slashMidPos, Quaternion.identity);
        damageTextObject.transform.SetParent(canvas.transform);
        damageTextObject.GetComponent<DamageText>().setValues(location, number, hitType);
        //critical or not
        if(hitType == 0)
        {
            GameObject critObject = Instantiate(failExplosion, slash.GetComponent<SlashContainer>().slashMidPos, Quaternion.identity); //create fail explosion gameObject

            critObject.transform.SetParent(canvas.transform);
            critObject.GetComponent<RectTransform>().localScale = new Vector3(1, 1, 1);
            return damageTextObject;
        } else if (hitType == 2)
        {
            GameObject critObject = Instantiate(criticalExplosion, slash.GetComponent<SlashContainer>().slashMidPos, Quaternion.identity); //create critical explosion gameObject

            critObject.transform.SetParent(canvas.transform);
            critObject.GetComponent<RectTransform>().localScale = new Vector3(1, 1, 1);
        } else if (hitType == 3)
        {
            GameObject critObject = Instantiate(criticalExplosion, slash.GetComponent<SlashContainer>().slashMidPos, Quaternion.identity); //create critical explosion gameObject

            ParticleSystem.MainModule settings = critObject.GetComponent<ParticleSystem>().main;
            settings.startColor = new ParticleSystem.MinMaxGradient(Globals.Instance.lightningHitColor);
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
        player.hp -= 10;
        refreshUI();
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

    public void refreshUI()
    {
        playerHpSlider.value = ((float)player.hp / (float)player.maxhp);
        playerHpText.text = player.hp.ToString() + "/" + player.maxhp.ToString() ;
        playerHpColor.color = Globals.Instance.healthGradient.Evaluate(playerHpSlider.value);
    }
}
