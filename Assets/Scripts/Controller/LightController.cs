using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

[RequireComponent(typeof(Light2D))]
public class LightController : MonoBehaviour
{
    new Light2D light;

    // Start is called before the first frame update
    void Start()
    {
        light = GetComponent<Light2D>();
        light.enabled = false;    
    }

    void OnTriggerStay2D(Collider2D other)
    {
        OnTriggerEnter2D(other); 
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (light.enabled && other.transform.tag == "Player")
        {
            light.enabled = false;
        }
        else if (!light.enabled && other.gameObject.layer == LayerMask.NameToLayer("Enemies"))
        {
            light.enabled = true;
        }
    }
}
