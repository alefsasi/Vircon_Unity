using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.UI;

public class AcenderLampada : MonoBehaviour
{
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

    void Start()
    {
       
        myAudioSource = GetComponent<AudioSource>();
      
        RenderComponent = lamp.GetComponent<Renderer>();
    }

    // Update is called once per frame
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
                    Acender(); //Receber status
                    AcenderSpot(_estado);
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
                         Acender(); //Receber status
                        AcenderSpot(_estado);
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
            MostraMessage("API não configurada!");
        }
        else
        {
            //int led = 0;

            string url = "http://" + DATA.url + "/" + API_LED;

            
            StartCoroutine(OnResponse(url));
        }
    }
    private void MostraMessage(string ms)
    {
        
        //Thread.Sleep(2000);
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
                    lamp.GetComponent<Animator>().SetInteger(Cond_, 1);
                    myAudioSource.clip = aClip[2];
                    myAudioSource.Play();
                    MostraMessage(status.status);
                    _estado = true;


                }
                else
                {
                    lamp.GetComponent<Animator>().SetInteger(Cond_, 2);
                    myAudioSource.clip = aClip[3];
                    myAudioSource.Play();
                    MostraMessage(status.status);
                    _estado = false;



                }
            }
        }

    }
}
