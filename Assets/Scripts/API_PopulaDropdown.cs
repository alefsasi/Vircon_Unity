using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class API_PopulaDropdown : MonoBehaviour {

    
    public Dropdown m_Dropdown;
    //public Text URI;

    public class DropDownPOP
    {
        public DropDownPOP()
        {
            ListaDropDown = new List<string>();
        }


        public List<string> ListaDropDown; 
    }


    public void SetDropDown(string url)
    {
   
           // StartCoroutine(OnResponse(url));
      
       
            m_Dropdown = GetComponent<Dropdown>();
            m_Dropdown.ClearOptions();
            List<string> lst = new List<string>();
            lst.Add("Node MCU");

            m_Dropdown.AddOptions(lst);
    }


    private IEnumerator OnResponse(string url)
    {
        if(url == string.Empty || url == null)
        {
            yield return null;
        }
        else
        {
            using (WWW www = new WWW("http://" + url + "/api/values"))
            {
                yield return www;
                m_Dropdown = GetComponent<Dropdown>();
                m_Dropdown.ClearOptions();
                DropDownPOP pop = new DropDownPOP();

                var json = www.text;

                www.Dispose();

                pop = JsonUtility.FromJson<DropDownPOP>(json);

            

                m_Dropdown.AddOptions(pop.ListaDropDown);

            }
        }

    }


}
