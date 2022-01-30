using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Misc;
using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    private bool hasBeenReached;

    private void Awake()
    {
        hasBeenReached = false;
    }

    // Start is called before the first frame update
    private void OnCollisionEnter2D(Collision2D other)
    {
        if (hasBeenReached) return;
        hasBeenReached = true;
        SerializationManager.instance.CreateSnapshot();
        transform.GetChild(0).gameObject.SetActive(true);
    }
}
