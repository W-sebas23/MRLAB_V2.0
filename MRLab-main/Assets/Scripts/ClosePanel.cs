using UnityEngine;
using UnityEngine.UI;

public class ClosePanel : MonoBehaviour
{
    public GameObject panel;
    public Text buttonText;

    public void SetActive()
    {
        if (panel.activeSelf)
        {
            panel.SetActive(false);
            buttonText.text = "Open";
        }
        else
        {
            panel.SetActive(true);
            buttonText.text = "Close";
        }
    }
}
