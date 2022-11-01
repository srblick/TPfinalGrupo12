using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ManagerHUD : MonoBehaviour
{
    public GameObject weaponInfoPrefab;
    public GameObject weaponInfoPrefabClone {get; private set;}
    public TMP_Text dotCrossHair;
    public TMP_Text mensajeAgarrar;

    private void Start() {
        Cursor.visible=false;
    }
    private void OnEnable() {
        EventManager.current.NewInstaceGunEvent.AddListener(createWeaponInfo);
        EventManager.current.crossHairChange.AddListener(changeColorCrosshairSelect);
    }
    private void OnDisable() {
        EventManager.current.NewInstaceGunEvent.RemoveListener(createWeaponInfo);
        EventManager.current.crossHairChange.RemoveListener(changeColorCrosshairSelect);
    }
    public void createWeaponInfo(bool isActive){

        if (isActive){
            weaponInfoPrefabClone= Instantiate(weaponInfoPrefab,transform);
        }else if(weaponInfoPrefab != null){
            Destroy(weaponInfoPrefabClone);
        }
        
    }
    private void Update() {
        dotCrossHair.color= Color.green;
        mensajeAgarrar.gameObject.SetActive(false);
    }
    private void changeColorCrosshairSelect(bool isActive){
        if(isActive){
            dotCrossHair.color= Color.white;
            mensajeAgarrar.gameObject.SetActive(true);
        }
        // else{
        //     dotCrossHair.color= Color.green;
        // }
    }
}
