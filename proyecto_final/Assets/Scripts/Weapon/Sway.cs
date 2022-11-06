using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sway : MonoBehaviour
{
    private Quaternion originLocalRotation;
    void Start()
    {
        originLocalRotation = transform.localRotation;
    }

    void Update()
    {
        updateSway();
    }

    private void updateSway(){
        float lookInputX = Input.GetAxis("Mouse X");
        float lookInputY = Input.GetAxis("Mouse Y");
        //clacula la rotacion del arma
        Quaternion angleAdjustmentX = Quaternion.AngleAxis(-lookInputX * 1.5f, Vector3.up);
        Quaternion angleAdjustmentY = Quaternion.AngleAxis(lookInputY * 1.5f, Vector3.right);
        Quaternion targetRotation = originLocalRotation * angleAdjustmentX * angleAdjustmentY;
        //rota hacia el target rotation
        transform.localRotation = Quaternion.Lerp(transform.localRotation,targetRotation, Time.deltaTime *15f);
    }
}
