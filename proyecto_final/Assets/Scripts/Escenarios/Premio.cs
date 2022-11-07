using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Premio : MonoBehaviour
{
    public int puntos = 0;
    public bool toca = false;

    void Update()
    {
        if(toca == true)
        {
            if(Input.GetKeyDown(KeyCode.E))
            {
                //Cuando el jugador toque E se sumara un punto
                puntos++;
                //Y se desactivara el objeto Premio
                gameObject.SetActive(false);
            }
        }
    }

    public void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            toca = true;
        }
    }
}