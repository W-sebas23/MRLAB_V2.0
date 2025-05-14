using System;
using System.Collections;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class ApiReciever : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] TMP_Text initialTimeText;
    [SerializeField] TMP_Text totalTimeText;
    [SerializeField] TMP_Text bottlesText;
    [SerializeField] TMP_Text energyCostText;
    [SerializeField] Image fillingBottle;
    [SerializeField] Color[] colors;           // 0: bajo, 1: medio, 2: alto

    [Header("Simulation Settings")]
    [SerializeField] int totalBotellas = 100;
    [SerializeField] float updateInterval = 0.5f;   // cada cuánto actualiza la UI
    [SerializeField] float bottleIncrementPeriod = 8f;     // cada cuántos segundos +1 botella

    private DateTime horaInicio;
    private int botellasLlenas;
    private float tiempoDesdeUltimoIncremento;

    private void Start()
    {
        horaInicio = DateTime.Now;
        botellasLlenas = 0;
        tiempoDesdeUltimoIncremento = 0f;
        StartCoroutine(SimulateData());
    }

    IEnumerator SimulateData()
    {
        while (true)
        {
            // 1) Tiempo de operación
            TimeSpan diff = DateTime.Now - horaInicio;

            // 2) Incremento de botellas cada X segundos
            tiempoDesdeUltimoIncremento += updateInterval;
            if (tiempoDesdeUltimoIncremento >= bottleIncrementPeriod)
            {
                if (botellasLlenas < totalBotellas)
                    botellasLlenas++;
                tiempoDesdeUltimoIncremento = 0f;
            }

            // 3) Consumo energético aleatorio
            float consumoAleatorio = UnityEngine.Random.Range(50000f, 90000f);
            double energyCost = diff.TotalHours * consumoAleatorio;

            // 4) Actualizar UI
            initialTimeText.text = horaInicio.ToString("HH:mm:ss");
            totalTimeText.text = diff.ToString(@"hh\:mm\:ss");
            bottlesText.text = $"{botellasLlenas}/{totalBotellas}";
            energyCostText.text = $"{energyCost:N2} kW";

            // 5) Barra de llenado
            float fillAmount = (float)botellasLlenas / totalBotellas;
            fillingBottle.fillAmount = fillAmount;
            fillingBottle.color = fillAmount < 0.33f
                ? colors[0]
                : (fillAmount < 0.66f
                    ? colors[1]
                    : colors[2]);

            yield return new WaitForSeconds(updateInterval);   
        }
    }
}
