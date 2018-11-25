using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TrocarCena : MonoBehaviour {

    bool act;

    private void Start()
    {
        act = true;
    }
    public void CarregarCena(string cena)
    {
        SceneManager.LoadScene(cena);
    }
    public void ActivePainel(GameObject ob)
    {
        ob.SetActive(act);
        act = !act;
    }
}
