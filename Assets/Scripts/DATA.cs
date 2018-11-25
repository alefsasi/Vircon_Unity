using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DATA : MonoBehaviour {

    public static string url;
    public static string porta = "ASP8266";
    private GameObject[] Datas;



    void Awake()
    {
        Datas = GameObject.FindGameObjectsWithTag("data");
        if(Datas.Length >= 2)
        {
            Destroy(Datas[0]);
        }
        DontDestroyOnLoad(transform.gameObject);

        if(PlayerPrefs.GetString("url") != null || PlayerPrefs.GetString("url") != string.Empty)
        {
            url = PlayerPrefs.GetString("url");
        }
    }
}
