using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class TempPlantInteract_Brooke : MonoBehaviour
{
    private GameObject detected_object;

    void Update()
    {
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