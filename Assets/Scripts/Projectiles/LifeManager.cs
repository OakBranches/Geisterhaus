using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LifeManager : MonoBehaviour
{
	public float vida;
	float vidaInicial;
	public Slider slider;

	void Awake()
	{
		vidaInicial = vida > 0 ? vida : 1f;
	}

    public void addLife(float valor)
	{
		vida += valor;
	}

	void Update()
	{
		if (transform.tag == "Player")
		{
			SetSliderValue((float) (vida / vidaInicial));
		}
	}

	public bool subLife(float valor)
	{
		vida -= valor;
		if (vida <= 0)
			return true;
		return false;
	}

	public void SetSliderValue(float value)
	{
		slider.value = value;
	}
}
