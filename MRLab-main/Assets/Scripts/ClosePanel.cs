using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ClosePanel : MonoBehaviour
{
    public GameObject panel;
    public GameObject apiReciever;
    public Text buttonText;

    public void SetActive()
    {
        if (panel.activeSelf){
            panel.SetActive(false);
            apiReciever.SetActive(false);
            buttonText.text = "Open";
        }
        else { 
            panel.SetActive(true);
            apiReciever.SetActive(true);
            buttonText.text = "Close";
        }
    }


}
