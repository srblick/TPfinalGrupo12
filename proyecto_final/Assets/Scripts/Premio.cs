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
                puntos++;
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