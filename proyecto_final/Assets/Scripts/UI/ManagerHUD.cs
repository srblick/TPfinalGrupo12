using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class ManagerHUD : MonoBehaviour
{
    public GameObject weaponInfoPrefab;
    public GameObject weaponInfoPrefabClone {get; private set;}
    public TMP_Text dotCrossHair;
    public TMP_Text mensajeAgarrar;
    [Header ("Inventary")]
    [SerializeField] private GameObject[] weaponsHUD = new GameObject[2];
    [SerializeField] private TMP_Text[] keyNumberHUD = new TMP_Text[2];
    [SerializeField] private Image[] weaponsActivesHUD  = new Image[2];
    [SerializeField] private Sprite[] spriteWeapons = new Sprite[2];

    private Color color= Color.white;
    private int activeIndex;
    // private bool state;
    private void Start() {
        Cursor.visible=false;
        Application.targetFrameRate=60;
    }
    private void OnEnable() {//al activarse este Objeto añade escuchadores
        EventManager.current.NewInstaceGunEvent.AddListener(createWeaponInfo);
        EventManager.current.crossHairChange.AddListener(changeColorCrosshairSelect);
        EventManager.current.activeWeaponHUD.AddListener(addWeaponHUD);
    }
    private void OnDisable() {
        EventManager.current.NewInstaceGunEvent.RemoveListener(createWeaponInfo);
        EventManager.current.crossHairChange.RemoveListener(changeColorCrosshairSelect);
        EventManager.current.activeWeaponHUD.RemoveListener(addWeaponHUD);
    }

    //Metodo que crea o destruye el canvas donde se ven las balas
    public void createWeaponInfo(bool isActive){
        if (isActive){
            weaponInfoPrefabClone= Instantiate(weaponInfoPrefab,transform);
        }else if(weaponInfoPrefab != null){
            Destroy(weaponInfoPrefabClone);
        }
        
    }

    //Metodo que cambia el color de la mira y muestra un mensaje
    private void changeColorCrosshairSelect(bool isActive){
        if(isActive){
            dotCrossHair.color= Color.white;
            mensajeAgarrar.gameObject.SetActive(true);
        }else{
            dotCrossHair.color= Color.green;
            mensajeAgarrar.gameObject.SetActive(false);
        }
    }

    //Metodo que añade al hud las armas que el player tiene el en inventario
    private void addWeaponHUD(bool primaryWeapon, bool secondaryweapon, int activeWeaponIndex){
        switch (activeWeaponIndex)// switch que se utiliza para cambiar los valores de los indice de 0,1,2 a 0,1
        {
            case 0:
                activeIndex=0;
                weaponsActivesHUD[0].sprite=spriteWeapons[0];
                break;
            case 1:
                activeIndex=0;
                weaponsActivesHUD[0].sprite=spriteWeapons[1];
                break;
            case 2:
                activeIndex=1;
                break;
            default:
                activeIndex=-1;
                break;

        }    

        if(primaryWeapon){
            activeWeaponHUD(activeIndex);
                weaponsHUD[0].gameObject.SetActive(true);
                keyNumberHUD[0].gameObject.SetActive(true);

            
        }else{
                weaponsHUD[0].gameObject.SetActive(false);
                keyNumberHUD[0].gameObject.SetActive(false);

        }

        if(secondaryweapon){   
            activeWeaponHUD(activeIndex);
            weaponsHUD[1].gameObject.SetActive(true);
            
        }else{
            weaponsHUD[1].gameObject.SetActive(false);
        }
    }

    //Metodo que activa visualmente el hud del arma activa   
    private void activeWeaponHUD(int index){
        for(int i = 0; i<2 ;i++){
            if(i == activeIndex){
                weaponsActivesHUD[i].color= Color.white;
                keyNumberHUD[i].color= Color.white;
            }else{
                weaponsActivesHUD[i].color= Color.gray;
                keyNumberHUD[i].color= Color.gray;
            }
            
        }
    }
}
