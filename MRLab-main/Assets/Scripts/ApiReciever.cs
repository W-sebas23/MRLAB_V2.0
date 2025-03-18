using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using SimpleJSON;
using System;
using TMPro;
using UnityEngine.UI;


public class ApiReciever : MonoBehaviour
{
    [SerializeField]
    string url;

    [SerializeField]
    TMP_Text initialTimeText;
    [SerializeField]
    TMP_Text totalTimeText;
    [SerializeField]
    TMP_Text bottlesText;
    [SerializeField]
    TMP_Text energyCostText;
    [SerializeField]
    Image fillingBottle;
    [SerializeField]
    Color[] colors;

    private bool start = true;

    private void Start()
    {
        if (start)
        {
            StartCoroutine(ReadAPI());
            start = false;
        }
    }


    private void OnEnable()
    {
        if (!start)
        StartCoroutine(ReadAPI());
    }

    public class ForceAcceptAll : CertificateHandler
    {
        protected override bool ValidateCertificate(byte[] certificateData)
        {
            return true;
        }
    }

    IEnumerator ReadAPI()
    {
        while(true)
        {
            UnityWebRequest web = UnityWebRequest.Get(url);
            var cert = new ForceAcceptAll();
            web.certificateHandler = cert;


            yield return web.SendWebRequest();

            if (web.result == UnityWebRequest.Result.ConnectionError || web.result == UnityWebRequest.Result.ProtocolError)
            {
                Debug.LogError(web.error);
                initialTimeText.text = web.error.ToString();
                yield break;
            }

            JSONNode response = JSON.Parse(web.downloadHandler.text);
            var data = response["data"];

            // data["..."]


            DateTime initialTime = new(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
            int apiInitialTime = int.Parse(int.Parse(data["tiempooperacionh"]).ToString("D4") + int.Parse(data["tiempooperacionm"]).ToString("D4") + int.Parse(data["tiempooperacionl"]).ToString(""));
            Debug.Log(apiInitialTime);
            initialTime = initialTime.AddSeconds(apiInitialTime).ToLocalTime();

            TimeSpan difference = DateTime.Now - initialTime;

            float fillAmount = (float) data["botellasllenas"] / (float) data["totalbotellas"];

            double energyCost = difference.TotalHours * data["consumo"];


            initialTimeText.text = initialTime.ToString(@"HH:mm:ss");
            totalTimeText.text = difference.ToString(@"hh\:mm\:ss");
            bottlesText.text = data["botellasllenas"].ToString()+"/"+ data["totalbotellas"].ToString();
            fillingBottle.fillAmount = fillAmount;
            fillingBottle.color = fillAmount < 0.33 ? colors[0] : (fillAmount >= 0.33 && fillAmount < 0.66) ? colors[1] : colors[2];
            energyCostText.text = energyCost.ToString("N2")+"KW";

            yield return new WaitForSeconds(0.50f);
        }
   
    }
}
