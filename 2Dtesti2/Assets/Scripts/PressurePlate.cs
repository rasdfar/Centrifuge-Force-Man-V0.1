using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PressurePlate : MonoBehaviour
{
    public GameObject PressurePlateDoor;
    int objectsOnPlate = 0;

    void OnTriggerExit2D(Collider2D other)
    {
        //if player pushes object outside of the pressureplate collider
        //->door closes again
        if (other.gameObject.CompareTag("Player") || other.gameObject.CompareTag("MovableObject"))
        {
            objectsOnPlate--;
            if (objectsOnPlate <= 0)
            {
                Debug.Log("Pressure plate is deactivated");
                PressurePlateDoor.SetActive(true);
            }
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player") || other.gameObject.CompareTag("MovableObject"))
        {
            objectsOnPlate++;
            Debug.Log("Pressure plate is activated");
            PressurePlateDoor.SetActive(false);
        }
    }
}
