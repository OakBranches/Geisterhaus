using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class LifeManager : MonoBehaviour
{
	public float vida;

    public void addLife(float valor)
	{
		vida += valor;
	}

	public bool subLife(float valor)
	{
		vida -= valor;
		if (vida <= 0)
			return true;
		return false;
	}
}
