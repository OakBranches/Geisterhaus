﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{

    protected bool AttackMode;
    public void SetAttackMode(bool i){
        AttackMode=i;
    }
    public bool getAttackMode(){return AttackMode;}
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
