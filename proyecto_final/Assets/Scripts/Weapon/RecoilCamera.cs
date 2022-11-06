using UnityEngine;

public class RecoilCamera : MonoBehaviour
{
    [Header ("References")]

    [SerializeField] private PlayerWeaponManager player_script;

    private Vector3 currentRotation;
    private Vector3 targetRotation;
    private bool isAiming;
    private int index;

    void Update()
    {
        isAiming = player_script.isAiming;
        index = (player_script.activeWeaponIndex>=0)?player_script.activeWeaponIndex : 0;

        // Codigo que hace que la camara vuelva a posicion inicial
        targetRotation = Vector3.Lerp(targetRotation, Vector3.zero, player_script.startingWeapons[index].returnTime * Time.deltaTime);
        currentRotation = Vector3.Slerp(currentRotation, targetRotation, player_script.startingWeapons[index].snappiness * Time.fixedDeltaTime);
        transform.localRotation = Quaternion.Euler(currentRotation);
    }

    public void recoilFire(){
        //Condicion que cambia el recoil dependiendo si esta apuntando o no.
        if (isAiming){
            targetRotation += new Vector3(
                player_script.startingWeapons[index].aimRecoilX, 
                Random.Range(-player_script.startingWeapons[index].aimRecoilY, 
                player_script.startingWeapons[index].aimRecoilY), 
                Random.Range(-player_script.startingWeapons[index].aimRecoilZ, 
                player_script.startingWeapons[index].aimRecoilZ));
        }else{
            targetRotation += new Vector3(
                player_script.startingWeapons[index].recoilX, 
                Random.Range(-player_script.startingWeapons[index].recoilY, 
                player_script.startingWeapons[index].recoilY), 
                Random.Range(-player_script.startingWeapons[index].recoilZ, 
                player_script.startingWeapons[index].recoilZ));
        }
        
    }
}
