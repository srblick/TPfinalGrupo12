using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWeaponManager : MonoBehaviour
{
    public List<WeaponController> startingWeapons = new List<WeaponController>();//Se define una lista de WeaponControllers

    public List<GrabableWeapon> grabableWeapons= new List<GrabableWeapon>();//Se define una lista de GrabableWeapon

    [SerializeField] private Transform weaponParentSocket;// transform donde se guarda la posicion por defecto de las armas
    [SerializeField] private Transform defaultParentSocket;// transform donde se almacenan las armas
    [SerializeField] private Transform aimingParentSocket;// transform donde se posiciona el arma al apuntar
    [SerializeField] private Transform dropParentSocket;// tranform donde se posiona el arma al tirarla

    [SerializeField] private LayerMask hittableLayers;// layer de las cuales va a dectectar el raycast

    [Header ("Cameras")]
    [SerializeField] private Camera cameraPlayer;
    [SerializeField] private float defaultFovPlayer;
    [SerializeField] private Camera cameraWeapon;
    [SerializeField] private float defaultFovWeapon;

    public int activeWeaponIndex {get; private set;}
    public bool isAiming {get; private set;}
    private WeaponController[] weaponSlots = new WeaponController[3];// arreglo de armas

    private bool isGrappingPrimaryWeapon=false;
    private bool isGrappingSecondaryWeapon=false;
    private bool isUsingFuturistWeapon=false;
    private float currentRotationSensibility;
    // Start is called before the first frame update
    void Start()
    {
        currentRotationSensibility=GetComponent<PlayerController>().rotationSensibility;
        isAiming=false;
        activeWeaponIndex=-1;
        foreach (WeaponController startingWeapon in startingWeapons){// se agregan las armas al arreglo de armas
            addWeapon(startingWeapon);
        }
    }

    // Update is called once per frame
    void Update()
    {
        //si se presiona el 1 y tiene activa el arma principal cambia de arma
        if (Input.GetKeyDown(KeyCode.Alpha1) && isGrappingPrimaryWeapon){
            if(isUsingFuturistWeapon){
                StartCoroutine(switchWeapon(0));
            }else{
                StartCoroutine(switchWeapon(1));
            }
            
        }
        //si se presiona el 2 y tiene activa el arma secundaria cambia de arma
        if (Input.GetKeyDown(KeyCode.Alpha2) && isGrappingSecondaryWeapon){           
            StartCoroutine(switchWeapon(2));
        }
        //si presiona el boton apuntar y tiene alguna arma agarrada, activa o desactiva el apuntado
        if (Input.GetButtonDown("Aim")&&(isGrappingPrimaryWeapon|| isGrappingSecondaryWeapon)){
            isAiming=!isAiming; 
        }
        //si presiona G suelta el arma
        if (Input.GetKeyDown(KeyCode.G)){
            dropWeapon(activeWeaponIndex);
        }
        //si está apuntado:
        if (isAiming){       
            //se posiona el arma en la posicion de apuntado, se acerca el fov y se reduce la sensibilidad      
            weaponParentSocket.position= Vector3.Lerp(weaponParentSocket.position,aimingParentSocket.position,12f*Time.deltaTime); 
            cameraPlayer.fieldOfView = Mathf.Lerp(cameraPlayer.fieldOfView, defaultFovPlayer/4, 12f * Time.deltaTime);
            cameraWeapon.fieldOfView = Mathf.Lerp(cameraWeapon.fieldOfView, defaultFovWeapon/2, 12f * Time.deltaTime);
            GetComponent<PlayerController>().rotationSensibility=currentRotationSensibility/2;
        }else{// sino se vuelve a los valores por defecto             
            weaponParentSocket.position= Vector3.Lerp(weaponParentSocket.position,defaultParentSocket.position,12f*Time.deltaTime);
            cameraPlayer.fieldOfView = Mathf.Lerp(cameraPlayer.fieldOfView, defaultFovPlayer, 12f * Time.deltaTime);
            cameraWeapon.fieldOfView = Mathf.Lerp(cameraWeapon.fieldOfView, defaultFovWeapon, 12f * Time.deltaTime);
            GetComponent<PlayerController>().rotationSensibility=currentRotationSensibility;
        }

        interact();
    }

    //corrutina que cambia el arma
    private IEnumerator switchWeapon(int p_weaponIndex){
        isAiming=false;
        int activeWeaponIndexBefore;// variable que alcena el index anterior
        if(p_weaponIndex != activeWeaponIndex && p_weaponIndex >=0){//si el index que recibe es distinto al indice actual y el index es mayor igual a 0
            if(activeWeaponIndex>=0){//esconde el arma anterior
                activeWeaponIndexBefore=activeWeaponIndex;
                weaponSlots[activeWeaponIndexBefore].hide();
                yield return new WaitForSeconds(0.2f);//espera el tiempo de la animacion
                EventManager.current.NewInstaceGunEvent.Invoke(false);//oculta hud de balas
                weaponSlots[activeWeaponIndexBefore].gameObject.SetActive(false);//desactiva el arma
            }
            weaponSlots[p_weaponIndex].gameObject.SetActive(true);//activa el arma
            activeWeaponIndex = p_weaponIndex;//el index pasa a ser el index activo
            EventManager.current.NewInstaceGunEvent.Invoke(true);// se activa el evento para que muestre la informacion de las balas
            EventManager.current.updateBulletsEvent.Invoke(weaponSlots[p_weaponIndex].currentAmmo,weaponSlots[p_weaponIndex].totalAmmo);// actualiza el el contador de balas en el hud
            EventManager.current.activeWeaponHUD.Invoke(isGrappingPrimaryWeapon,isGrappingSecondaryWeapon,activeWeaponIndex);// se muestra el icono del arma activa en el hud
        }else if(p_weaponIndex == activeWeaponIndex && p_weaponIndex >=0){// en caso de apretar el mismo boton del arma activa, esta se desactiva
            
            weaponSlots[p_weaponIndex].hide();
            yield return new WaitForSeconds(0.2f);
            weaponSlots[p_weaponIndex].gameObject.SetActive(false);
            activeWeaponIndex = -1; 
            EventManager.current.NewInstaceGunEvent.Invoke(false);
            EventManager.current.activeWeaponHUD.Invoke(isGrappingPrimaryWeapon,isGrappingSecondaryWeapon,activeWeaponIndex);        
        }

    }

    private void addWeapon(WeaponController p_weaponPrefab){
            weaponParentSocket.position = defaultParentSocket.position;//se posiciona las armas a la posicion por defecto
            //añade las armas al arreglo de armas y las desactiva
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

    //Metodo para interactuar con los objetos/armas
    private void interact(){
        RaycastHit hit;//se declara un raycast hit
        //detecta colision en un rango de 2.5
        if(Physics.Raycast(cameraPlayer.transform.position,cameraPlayer.transform.forward, out hit, 2.5f, hittableLayers)){
            
            if(hit.transform.tag == "ArmaAgarrable"){//si colisiona con un arma agarrable
                EventManager.current.crossHairChange.Invoke(true);//Invoca al evento que cambia de color a la mira

                //si presiona E agarra el arma dependiedo las condiones
                if(hit.transform.GetComponent<GrabableWeapon>().typeWeapon == 1 && Input.GetKeyDown(KeyCode.E)){
                    if(hit.transform.GetComponent<GrabableWeapon>().futuristWeapon){
                        if(isGrappingPrimaryWeapon){// si ya tenia un arma agarrada la suelta
                            dropWeapon(1);
                        }
                        isUsingFuturistWeapon=true;
                        isGrappingPrimaryWeapon=true;
                        StartCoroutine(switchWeapon(0));
                        Destroy(hit.transform.gameObject);// destruye el objeto que agarro
                    }else{
                        if(isGrappingPrimaryWeapon){
                            dropWeapon(0);
                        }
                        isUsingFuturistWeapon=false;
                        isGrappingPrimaryWeapon=true;
                        StartCoroutine(switchWeapon(1));
                        Destroy(hit.transform.gameObject);
                    }
                    
                }
            
                if(hit.transform.GetComponent<GrabableWeapon>().typeWeapon == 2 && Input.GetKeyDown(KeyCode.E)){
                    isGrappingSecondaryWeapon=true;
                    StartCoroutine(switchWeapon(2));
                    Destroy(hit.transform.gameObject);
                }
            }else{
                EventManager.current.crossHairChange.Invoke(false);
            }
            if(hit.transform.tag=="Premio"){
                EventManager.current.crossHairChange.Invoke(true);
            }
            // if(hit.transform.tag=="Enemy"){
            //     if(activeWeaponIndex<=-1 && Input.GetButtonDown("Fire1")){
            //         //Codigo que hace daño al enemigo a melee
            //         // hit.transform.GetComponent<EnemyBehavior>().sufferDamage(1);
            //     }
            // }
        }else {
            EventManager.current.crossHairChange.Invoke(false);
        }
    }

    //método que tira el arma
    private void dropWeapon(int activeWeaponIndex){//recibe un indice para saber que arma tirar

        if(activeWeaponIndex>=-1){// si el indice una esta fuera del rango del arraglo:
            switch (activeWeaponIndex)
            {
                case 0:
                    isGrappingPrimaryWeapon=false;
                    StartCoroutine(switchWeapon(activeWeaponIndex));//desactiva el arma
                    Instantiate(grabableWeapons[activeWeaponIndex],dropParentSocket.position,Quaternion.identity);// crea un arma agarrable
                    break;
                case 1:
                    isGrappingPrimaryWeapon=false;
                    StartCoroutine(switchWeapon(activeWeaponIndex));
                    Instantiate(grabableWeapons[activeWeaponIndex],dropParentSocket.position,Quaternion.identity);
                    break;
                case 2:
                    isGrappingSecondaryWeapon=false;
                    StartCoroutine(switchWeapon(activeWeaponIndex));
                    Instantiate(grabableWeapons[activeWeaponIndex],dropParentSocket.position,Quaternion.identity);
                    break;
            }      
        }
    }
    //metodo accesor
    public void setIsAiming(bool active){
        isAiming=active;
    }
}
