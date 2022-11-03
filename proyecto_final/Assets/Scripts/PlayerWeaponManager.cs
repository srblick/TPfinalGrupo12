﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWeaponManager : MonoBehaviour
{
    public List<WeaponController> startingWeapons = new List<WeaponController>();

    public List<GrabableWeapon> grabableWeapons= new List<GrabableWeapon>();

    [SerializeField] private Transform weaponParentSocket;
    [SerializeField] private Transform defaultParentSocket;
    [SerializeField] private Transform aimingParentSocket;
    [SerializeField] private Transform dropParentSocket;

    [SerializeField] private LayerMask hittableLayers;

    [Header ("Cameras")]
    [SerializeField] private Camera cameraPlayer;
    [SerializeField] private float defaultFovPlayer;
    [SerializeField] private Camera cameraWeapon;
    [SerializeField] private float defaultFovWeapon;

    public int activeWeaponIndex {get; private set;}
    public bool isAiming {get; private set;}
    private WeaponController[] weaponSlots = new WeaponController[3];

    private bool isGrappingPrimaryWeapon=false;
    private bool isGrappingSecondaryWeapon=false;
    private bool isUsingFuturistWeapon=false;
    // Start is called before the first frame update
    void Start()
    {
        isAiming=false;
        activeWeaponIndex=-1;

        foreach (WeaponController startingWeapon in startingWeapons){
            addWeapon(startingWeapon);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1) && isGrappingPrimaryWeapon){
            if(isUsingFuturistWeapon){
                switchWeapon(0);
            }else{
                switchWeapon(1);
            }
            
        }
        if (Input.GetKeyDown(KeyCode.Alpha2) && isGrappingSecondaryWeapon){           
            switchWeapon(2);
        }

        if (Input.GetButtonDown("Aim")&&(isGrappingPrimaryWeapon|| isGrappingSecondaryWeapon)){
            isAiming=!isAiming; 
        }
        if (Input.GetKeyDown(KeyCode.G)){
            dropWeapon(activeWeaponIndex);
        }
        if (isAiming){             
            weaponParentSocket.position= Vector3.Lerp(weaponParentSocket.position,aimingParentSocket.position,12f*Time.deltaTime); 
            cameraPlayer.fieldOfView = Mathf.Lerp(cameraPlayer.fieldOfView, defaultFovPlayer/4, 12f * Time.deltaTime);
            cameraWeapon.fieldOfView = Mathf.Lerp(cameraWeapon.fieldOfView, defaultFovWeapon/2, 12f * Time.deltaTime);
            GetComponent<PlayerController>().rotationSensibility=60;
        }else{             
            weaponParentSocket.position= Vector3.Lerp(weaponParentSocket.position,defaultParentSocket.position,12f*Time.deltaTime);
            cameraPlayer.fieldOfView = Mathf.Lerp(cameraPlayer.fieldOfView, defaultFovPlayer, 12f * Time.deltaTime);
            cameraWeapon.fieldOfView = Mathf.Lerp(cameraWeapon.fieldOfView, defaultFovWeapon, 12f * Time.deltaTime);
            //cambiar tambien la sensibilidad del player
            GetComponent<PlayerController>().rotationSensibility=150;
        }

        agarrar();
    }

    private void switchWeapon(int p_weaponIndex){
        isAiming=false;
        
        if(p_weaponIndex != activeWeaponIndex && p_weaponIndex >=0){
            weaponSlots[p_weaponIndex].gameObject.SetActive(true);
            activeWeaponIndex = p_weaponIndex;
            EventManager.current.NewInstaceGunEvent.Invoke(true);
            EventManager.current.updateBulletsEvent.Invoke(weaponSlots[p_weaponIndex].currentAmmo,weaponSlots[p_weaponIndex].totalAmmo);
            EventManager.current.activeWeaponHUD.Invoke(isGrappingPrimaryWeapon,isGrappingSecondaryWeapon,activeWeaponIndex);
        }else if(p_weaponIndex == activeWeaponIndex && p_weaponIndex >=0){
            weaponSlots[p_weaponIndex].gameObject.SetActive(false);
            activeWeaponIndex = -1; 
            EventManager.current.NewInstaceGunEvent.Invoke(false);
            EventManager.current.activeWeaponHUD.Invoke(isGrappingPrimaryWeapon,isGrappingSecondaryWeapon,activeWeaponIndex);        
        }
        for(int i=0;i<weaponSlots.Length;i++){
            if (i != p_weaponIndex){
                weaponSlots[i].gameObject.SetActive(false);
            }
        }
         
    }

    private void addWeapon(WeaponController p_weaponPrefab){
            weaponParentSocket.position = defaultParentSocket.position;
            //añade arma
            for (int i = 0; i < weaponSlots.Length; i++)
            {
                if (weaponSlots[i] == null)
                {
                    WeaponController weaponClone = Instantiate(p_weaponPrefab, weaponParentSocket);
                    weaponClone.gameObject.SetActive(false);
                    weaponSlots[i] = weaponClone;
                    return;
                }
            }   
    }

    private void agarrar(){
        RaycastHit hit;
        if(Physics.Raycast(cameraPlayer.transform.position,cameraPlayer.transform.forward, out hit, 3.5f, hittableLayers)){
        //    Debug.Log(hit.transform.tag);
            if(hit.transform.tag == "ArmaAgarrable"){
                EventManager.current.crossHairChange.Invoke(true);

                if(hit.transform.GetComponent<GrabableWeapon>().typeWeapon == 1 && Input.GetKeyDown(KeyCode.E)){
                    if(hit.transform.GetComponent<GrabableWeapon>().futuristWeapon){
                        if(isGrappingPrimaryWeapon){
                            dropWeapon(1);
                        }
                        isUsingFuturistWeapon=true;
                        isGrappingPrimaryWeapon=true;
                        switchWeapon(0);
                        Destroy(hit.transform.gameObject);
                    }else{
                        if(isGrappingPrimaryWeapon){
                            dropWeapon(0);
                        }
                        isUsingFuturistWeapon=false;
                        isGrappingPrimaryWeapon=true;
                        switchWeapon(1);
                        Destroy(hit.transform.gameObject);
                    }
                    
                }
            
                if(hit.transform.GetComponent<GrabableWeapon>().typeWeapon == 2 && Input.GetKeyDown(KeyCode.E)){
                    isGrappingSecondaryWeapon=true;
                    switchWeapon(2);
                    Destroy(hit.transform.gameObject);
                }
            }
        }
    }

    private void dropWeapon(int activeWeaponIndex){

        if(activeWeaponIndex>=-1){
            switch (activeWeaponIndex)
            {
                case 0:
                    isGrappingPrimaryWeapon=false;
                    switchWeapon(activeWeaponIndex);
                    Instantiate(grabableWeapons[activeWeaponIndex],dropParentSocket.position,Quaternion.identity);
                    break;
                case 1:
                    isGrappingPrimaryWeapon=false;
                    switchWeapon(activeWeaponIndex);
                    Instantiate(grabableWeapons[activeWeaponIndex],dropParentSocket.position,Quaternion.identity);
                    break;
                case 2:
                    isGrappingSecondaryWeapon=false;
                    switchWeapon(activeWeaponIndex);
                    Instantiate(grabableWeapons[activeWeaponIndex],dropParentSocket.position,Quaternion.identity);
                    break;
            }      
        }
    }
}
