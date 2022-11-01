using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IconMission : MonoBehaviour
{
    public float speedY = 0.01f;
    public int condicion = 1;
    public float contador = 0f;

    void Start()
    {
        
    }

    void Update()
    {
        //Subida y Bajada
        switch (condicion)
        {
            case 1:
             transform.Translate(0,speedY,0);
             contador = contador + 0.1f;
             if(contador >= 5f){
                condicion = 2;
             }
            break;

            case 2:
             transform.Translate(0,-speedY,0);
             contador = contador - 0.1f;
             if(contador <= 0f){
                condicion = 1;
             }
            break;
        }
    }
}
