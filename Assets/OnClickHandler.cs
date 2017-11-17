﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnClickHandler : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () 
	{
		if (Input.GetMouseButtonDown(0))
		{
			var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
			RaycastHit hit;
			if (Physics.Raycast(ray, out hit)){
				Debug.Log("Mouse Down hit: "+ hit.collider.name);
			}
		}
	}
}
