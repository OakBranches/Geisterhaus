using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    protected GameObject player;//precisa ser inicializado no start do filho
    protected bool attackMode;

    virtual public void AttackMove(float a)
    {

    }

    public void SetAttackMode(bool i)
    {
        attackMode = i;
    }

    public bool getAttackMode() { return attackMode; }
}
