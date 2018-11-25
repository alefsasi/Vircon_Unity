using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Vuforia;

public class CasaLiga : MonoBehaviour, IVirtualButtonEventHandler
{

    private GameObject SunMon;
    public Text message;
    public string[] API_LED;
    public AudioSource myAudioSource;
    public AudioClip[] aClip;
    public Material[] m_SunMoonMaterials;

    public class LedStatus
    {
        public string status;
    }
    void Start()
    {
        VirtualButtonBehaviour[] vbs = GetComponentsInChildren<VirtualButtonBehaviour>();
        for (int i = 0; i < vbs.Length; ++i)
        {
            vbs[i].RegisterEventHandler(this);
        }

        myAudioSource = GetComponent<AudioSource>();
        SunMon = transform.Find("SunMoon").gameObject;
    }
    public void OnButtonPressed(VirtualButtonBehaviour vb)
    {
        if (!IsValid())
        {
            return;
        }
        try
        {
            switch (vb.VirtualButtonName)
            {
                case "Dia":
                    ApagarAcender(0);
                    break;

                case "Noite":
                    ApagarAcender(1);
                    break;
            }

        }
        catch
        {
            myAudioSource.clip = aClip[2];
            myAudioSource.Play();
            message.text = "Não foi Possível Conectar com API";
        }
    }

    public void OnButtonReleased(VirtualButtonBehaviour vb)
    {

    }
    public void ApagarAcender(int indice)
    {

        if ((DATA.url == string.Empty || DATA.url == null))
        {
            myAudioSource.clip = aClip[3];
            myAudioSource.Play();
            message.text = "API não configurada!";
        }
        else
        {
            string url = "http://" + DATA.url + "/" + API_LED[indice];
            StartCoroutine(OnResponse(url, indice));
        }
    }
    private IEnumerator OnResponse(string url, int indice)
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
                myAudioSource.clip = aClip[indice];
                SunMon.GetComponent<Renderer>().material = m_SunMoonMaterials[indice];
                message.text = status.status;
                myAudioSource.Play();
            }
        }

    }
    private bool IsValid()
    {
        // Check the materials and teapot have been set:
        return m_SunMoonMaterials != null &&
                m_SunMoonMaterials.Length == 2 &&
                SunMon != null;
    }
}
