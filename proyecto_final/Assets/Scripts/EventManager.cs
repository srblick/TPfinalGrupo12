using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System;

[Serializable]
public class Int2Event : UnityEvent<int,int>{}
public class BoolEvent : UnityEvent<bool>{}
public class EventManager : MonoBehaviour
{
    #region Singlenton
    public static EventManager current;
    private void Awake() {
        if (current == null){
            current=this;
        }else if (current != null){
            Destroy(this);
        }
    }
    #endregion
    
    public Int2Event updateBulletsEvent = new Int2Event();

    public BoolEvent NewInstaceGunEvent = new BoolEvent();
    public BoolEvent crossHairChange = new BoolEvent();
}
