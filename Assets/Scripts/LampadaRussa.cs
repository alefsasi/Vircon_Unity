using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Vuforia;

public class LampadaRussa : MonoBehaviour, IVirtualButtonEventHandler {

    public class LedStatus
    {
        public string status;
    }

    public GameObject BTN;
    public GameObject Lamp;
    public Text message;
    private Renderer RenderComponent;
    private Light luz;
    private bool _estado = true;
    public string API_LED;
    public string Cond_;
    public AudioSource myAudioSource;
    public AudioClip[] aClip;

    public void OnButtonPressed(VirtualButtonBehaviour vb)
    {

        try
        {
        Acender(); //Receber status
        AcenderSpot(_estado);
        }
        catch(Exception)
        {
            myAudioSource.clip = aClip[0];
            myAudioSource.Play();
            message.text = "Não foi Possível Conectar com API";
        }
        
;


    }

    public void OnButtonReleased(VirtualButtonBehaviour vb)
    {
       
    }

    // Use this for initialization
    void Start () {
        BTN = GameObject.Find("Interruptor");
        BTN.GetComponent<VirtualButtonBehaviour>().RegisterEventHandler(this);
       
        RenderComponent = Lamp.GetComponent<Renderer>();

        myAudioSource = GetComponent<AudioSource>();
    }
    void AcenderSpot(bool _estado)
    {
        RenderComponent.material.EnableKeyword("_EMISSION");
        Material _mt = RenderComponent.material;
        if (_estado)
        {
            _mt.SetColor("_EmissionColor", new Color(177f, 116f, 203f, 1) * 1);
            luz.enabled = true;
        }
        else
        {
            _mt.SetColor("_EmissionColor", new Color(177f, 116f, 203f, 1) * 0);
            luz.enabled = false;
        }
    }
    public void Acender()
    {

        if ((DATA.url == string.Empty || DATA.url == null))
        {
            myAudioSource.clip = aClip[0];
            myAudioSource.Play();
            message.text = "API não configurada!";
        }
        else
        {
            string url = "http://" + DATA.url + "/" + API_LED;
            StartCoroutine(OnResponse(url));
        }
    }
    private IEnumerator OnResponse(string url)
    {

        using (WWW www = new WWW(url))
        {
            yield return www;

            var json = www.text;

            LedStatus status = new LedStatus();

            status = JsonUtility.FromJson<LedStatus>(json);


            www.Dispose();

            if (json != string.Empty)
            {
                    myAudioSource.clip = aClip[1];
                    myAudioSource.Play();
                    message.text = "Click!";
                    _estado = !_estado;
            }
        }

    }
}
