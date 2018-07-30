using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBarController : MonoBehaviour {
	public Slider healthBar;
	public Slider healthBarInstantiated;

	void Awake()
	{
		healthBarInstantiated = Instantiate(healthBar,  GameObject.Find("Canvas").transform) as Slider;
	}

	public void SetMaxHP(float HP){
		healthBarInstantiated.maxValue = HP;
	}

	public void SetHP(float newHP){
		healthBarInstantiated.value = newHP;
	}

	public void SetPos(Vector3 pos){
		healthBarInstantiated.transform.position = pos;
	}
}
