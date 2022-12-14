using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class WeaponInfo_UI : MonoBehaviour
{
    
    public TMP_Text currentBullets;
    public TMP_Text totalBullets;

    private void OnEnable() {
        EventManager.current.updateBulletsEvent.AddListener(updateBullets);  
    }

    private void OnDisable() {
        EventManager.current.updateBulletsEvent.RemoveListener(updateBullets);
    }

    //Método que actualiza la informacion de las balas y cambia el color a rojo si llega a 0
    public void updateBullets(int newCurrentBullets, int newTotalBullets){

        if(newCurrentBullets<=0){
            currentBullets.color = Color.red;
        }else{
            currentBullets.color = Color.white;
        }
        if(newTotalBullets<=0){
            totalBullets.color = Color.red;
        }else{
            totalBullets.color = Color.white;
        }
        currentBullets.text = newCurrentBullets.ToString();
        totalBullets.text = newTotalBullets.ToString();
    }

    
}
