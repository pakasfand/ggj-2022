using System.Collections;
using System.Collections.Generic;
using Misc;
using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    // Start is called before the first frame update
    private void OnCollisionEnter2D(Collision2D other)
    {
        SerializationManager.instance.CreateSnapshot();
        gameObject.SetActive(false);
    }
}
