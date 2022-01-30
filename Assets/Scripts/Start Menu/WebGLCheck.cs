using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WebGLCheck : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        if (Application.platform == RuntimePlatform.WebGLPlayer)
        {
            
            
            try
            {
                var credO = GameObject.Find("ExitButton");
                Destroy(credO);
            }
            catch (Exception ignored)
            {
                
            }
            // GetComponent<Canvas>().transform.GetChild(1).GetChild(3).gameObject.SetActive(false);
            
        }
    }

}
