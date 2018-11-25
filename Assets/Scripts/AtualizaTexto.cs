using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AtualizaTexto : MonoBehaviour {

    // Use this for initialization
    float tempo = 2.0f;
    public Text texto;
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

		if(texto.text != string.Empty)
        {
            tempo -= Time.deltaTime;
            if(tempo <= 0)
            {
                texto.text = string.Empty;
                tempo = 2.0f;
            }
        }
	}
}
