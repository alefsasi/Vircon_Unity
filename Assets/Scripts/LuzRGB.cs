using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.UI;

public class LuzRGB : MonoBehaviour {

    public string lampName;
    public AudioSource myAudioSource;
    public AudioClip[] aClip;
    public Text message;
    public GameObject lamp;
    private Renderer RenderComponent;
    private Light luz;
    private bool _estado = true;
    public string API_LED;
    public string Cond_;
    public string resposta;

    public class LedStatus
    {
        public string status;
    }
    private int RandomNumber(int date)
    {
        System.Random random = new System.Random(date);
        return random.Next(255);
    }
    void Start()
    {

        myAudioSource = GetComponent<AudioSource>();

        RenderComponent = lamp.GetComponent<Renderer>();


    }

    void Update()
    {
        if (Input.touchCount > 0 && Input.touches[0].phase == TouchPhase.Began)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.GetTouch(0).position);
            RaycastHit Hit;

            if (Physics.Raycast(ray, out Hit))
            {
                string Name = Hit.transform.name;
                if (Name == lampName)
                {
                    try
                    {

                        int r, g, b;
                        r = RandomNumber((int)DateTime.Now.Ticks);
                        Thread.Sleep(10);
                        g = RandomNumber((int)DateTime.Now.Ticks);
                        Thread.Sleep(10);
                        b = RandomNumber((int)DateTime.Now.Ticks);

                        Acender(r, g, b); //Receber status


                        MostraMessage(r.ToString() + " - " + g.ToString() + " - " + b.ToString());
                        Color32 cor = new Color32((byte)r, (byte)g, (byte)b, 255);

                        AcenderSpot(_estado, cor);
                    }
                    catch
                    {
                        lamp.GetComponent<Animator>().SetInteger(Cond_, 0);
                        myAudioSource.clip = aClip[1];
                        myAudioSource.Play();
                        MostraMessage("Não Conectado Com API!");
                    }
                }
            }
        }
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit Hit;

            if (Physics.Raycast(ray, out Hit))
            {
                string Name = Hit.transform.name;
                if (Name == lampName)
                {
                    try
                    {
                        int r, g, b;
                        r = RandomNumber((int)DateTime.Now.Ticks);
                        Thread.Sleep(10);
                        g = RandomNumber((int)DateTime.Now.Ticks);
                        Thread.Sleep(10);
                        b = RandomNumber((int)DateTime.Now.Ticks);

                        Acender(r, g, b); //Receber status


                        MostraMessage(r.ToString() + " - " + g.ToString() + " - " + b.ToString());
                        Color32 cor = new Color32((byte)r, (byte)g, (byte)b, 255);

                        AcenderSpot(_estado, cor);

                    }
                    catch(Exception)
                    {
                    lamp.GetComponent<Animator>().SetInteger(Cond_, 0);
                    myAudioSource.clip = aClip[1];
                    myAudioSource.Play();
                    MostraMessage("Não Conectado Com API!");
                    }

            }
            }
        }

    }
    void AcenderSpot(bool _estado, Color32 cor)
    {
        RenderComponent.material.EnableKeyword("_EMISSION");
        Material _mt = RenderComponent.material;
        if (_estado)
        {
            _mt.SetColor("_EmissionColor", (Color)cor * 1);
            luz.enabled = true;
        }
        else
        {
            _mt.SetColor("_EmissionColor", (Color)cor * 0);
            luz.enabled = false;
        }
    }
    public void Acender(int R, int G, int B)
    {
        if ((DATA.url == string.Empty || DATA.url == null))
        {
            myAudioSource.clip = aClip[0];
            myAudioSource.Play();
            MostraMessage("API não configurada!");
        }
        else
        {

            string url = "http://" + DATA.url + "/" + API_LED + "?Red=" +R+ "&Green=" +G+ "&Blue=" +B;


            StartCoroutine(OnResponse(url));
        }
    }
    private void MostraMessage(string ms)
    {
        message.text = ms;
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

                if (status.status == API_LED + resposta)
                {
                    lamp.GetComponent<Animator>().SetBool(Cond_, _estado);
                    myAudioSource.clip = aClip[2];
                    myAudioSource.Play();
                    MostraMessage(status.status);
                    _estado = false;


                }
                else
                {
                    //lamp.GetComponent<Animator>().SetInteger(Cond_, 2);
                    lamp.GetComponent<Animator>().SetBool(Cond_, _estado);
                    myAudioSource.clip = aClip[3];
                    myAudioSource.Play();
                    MostraMessage(status.status);
                    _estado = true;
                }
            }
        }

    }
}
