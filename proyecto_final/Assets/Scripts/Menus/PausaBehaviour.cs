using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PausaBehaviour : MonoBehaviour
{
    [SerializeField]
    private GameObject panel;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKey(KeyCode.P)){
            Debug.Log("ok");
            Cursor.visible = true;
            panel.SetActive(true);
        }
        if(Input.GetKey(KeyCode.Escape)){
            panel.SetActive(false);
            Cursor.visible = false;
        }
       
    }
}
