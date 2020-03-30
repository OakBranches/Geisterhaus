using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

public class AllLamps : MonoBehaviour
{
    Light2D[] lamps;
    LifeManager lifeManager;
    public float damage = 1f;

    // Start is called before the first frame update
    void Start()
    {
        lamps = new Light2D[6];
        int i = 0;
        foreach (Transform child in transform)
        {
            lamps[i++] = child.gameObject.GetComponent<Light2D>();
        }
        lifeManager = FindObjectOfType<LifeManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if (AllTurnedOn())
        {
            if (lifeManager.subLife(damage * Time.fixedDeltaTime))
            {
                GameController.GameOver();
            }
        }
    }

    bool AllTurnedOn()
    {
        foreach (Light2D light in lamps)
        {
            if (!light.enabled)
            {
                return false;
            }
        }
        return true;
    }
}
