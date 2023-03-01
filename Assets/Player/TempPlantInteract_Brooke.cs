using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class TempPlantInteract_Brooke : MonoBehaviour
{
    private GameObject detected_object;

    public GameObject plantPrefab;
    
    [SerializeField] private Sprite[] seedSprites;
    private SpriteRenderer spriteRenderer;
    private int seedNum = 0;

    [SerializeField] private Plant[] plantPrefabs;

    private Vector3 mousePos;

    void Awake()
    {
        spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
        spriteRenderer.sprite = seedSprites[0];
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.RightBracket))
        {
            seedNum++;
            if (seedNum >= seedSprites.Length) seedNum = 0;
            
            spriteRenderer.sprite = seedSprites[seedNum];
        }

        if (Input.GetKeyDown(KeyCode.LeftBracket))
        {
            seedNum--;
            if (seedNum < 0) seedNum = seedSprites.Length-1;

            spriteRenderer.sprite = seedSprites[seedNum];
        }

        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            //Debug.Log("SEED");
            Instantiate(plantPrefabs[seedNum], new Vector3((Input.mousePosition.x/120)-4, 0, 0), Quaternion.identity);
        }

        if (detected_object && Input.GetKeyDown(KeyCode.H))
        {
            //Debug.Log("HARVEST");
            detected_object.GetComponent<Plant>().Harvest();
        }

        if (detected_object && Input.GetKeyDown(KeyCode.E))
        {
            //Debug.Log("WATER");
            detected_object.GetComponent<Plant>().Water();
        }

        if (detected_object && Input.GetKeyDown(KeyCode.X))
        {
            //Debug.Log("DESTROY");
            detected_object.GetComponent<Plant>().Dig();
        }

        if (detected_object && Input.GetKeyDown(KeyCode.K))
        {
            //Debug.Log("DESTROY");
            detected_object.GetComponent<Plant>().Die();
        }
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<Collider2D>().tag == "Plant")
        {
            //Debug.Log("Plant!");
            detected_object = collision.gameObject;
        }
        else
        {
            detected_object = null;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (detected_object == collision.gameObject)
        {
            detected_object = null;
        }
    }


}