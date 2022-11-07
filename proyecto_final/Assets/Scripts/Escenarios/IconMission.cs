using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IconMission : MonoBehaviour
{
    public float speedY = 0.01f;
    public int condicion = 1;
    public float contador = 0f;

    void Update()
    {
        //Subida y Bajada
        switch (condicion)
        {
            case 1:
            //Cambia la posicion del objeto
             transform.Translate(0,speedY,0);

             //Suma 0,1 al contador
             contador = contador + 0.1f;
             if(contador >= 5f){
                condicion = 2;
             }
            break;

            case 2:
            //Cambia la posicion del objeto
             transform.Translate(0,-speedY,0);

             //Resta 0,1 al contador
             contador = contador - 0.1f;
             if(contador <= 0f){
                condicion = 1;
             }
            break;
        }
    }
}