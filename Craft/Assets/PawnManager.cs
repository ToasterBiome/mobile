using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PawnManager : MonoBehaviour
{
    public bool isWalking = false;
    public bool startedBattle = false;

    public Image selectedTileSprite;
    public GameObject selectButtonGameObject;

    public GameObject selectedTile;

    public Text pawnTileName;
    public Text pawnTileDescription;

    public CanvasGroup blackFade;

    public Monster mon;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);

            if (hit.collider != null)
            {
                Debug.Log("Target Position: " + hit.collider.gameObject.transform.position);
                selectedTile = hit.collider.gameObject;
                if (!isWalking)
                {
                    selectButtonGameObject.SetActive(false);
                    isWalking = true;
                    StartCoroutine(Walk(hit.collider.gameObject.transform.position));
                }
            }
        }
    }

    void ReachedDestination()
    {
        PawnData pawnData = selectedTile.GetComponent<PawnData>();

        if(pawnData.PawnType == PawnData.PawnTypes.Battle)
        {
            selectButtonGameObject.SetActive(true);
        }

        if (pawnData.PawnType == PawnData.PawnTypes.Chest)
        {
            selectButtonGameObject.SetActive(true);
        }
        //enable image
        //set image to tile
        selectedTileSprite.sprite = selectedTile.GetComponent<SpriteRenderer>().sprite;


        pawnTileDescription.text = pawnData.Description;
        pawnTileName.text = pawnData.PawnName;

    }

    public void attemptStartBattle()
    {
        Debug.Log("attempting to start");
        if (!startedBattle)
        {
            StartCoroutine(StartBattle());
        }
    }

    IEnumerator StartBattle()
    {
        while(Camera.main.orthographicSize >= 2f)
        {
            blackFade.alpha += 1.5f * Time.deltaTime;
            Camera.main.orthographicSize = Camera.main.orthographicSize - 1f * Time.deltaTime * 2f;
            yield return new WaitForEndOfFrame();
        }

        BigGameManager.instance.StartBattle(mon);

        yield return null;
        

        
    }

    IEnumerator Walk(Vector3 target)
    {
        while(transform.position != target)
        {
            transform.position = transform.position + (target - transform.position).normalized * Time.deltaTime * 8f;

            if(Vector3.Distance(transform.position,target) <= 0.16f)
            {
                transform.position = target;
                isWalking = false;
                ReachedDestination();
                yield return null;
            }

            yield return new WaitForEndOfFrame();
        }
        isWalking = false;
        ReachedDestination();
        yield return null;
    }
}
