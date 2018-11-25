using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SaveLoad : MonoBehaviour {
    public InputField url;
    public Dropdown drop;
	void Start ()
    {


        url.text = DATA.url;
        drop.captionText.text = DATA.porta;
        
           
    }
	
	// Update is called once per frame
	void Update () {
		
	}
    public void Save()
    {
        DATA.url = url.text;
        DATA.porta = drop.captionText.text;

        PlayerPrefs.SetString("url",DATA.url);
        PlayerPrefs.Save();
    }
}
