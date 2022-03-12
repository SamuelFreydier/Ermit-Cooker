using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour
{
    public TMPro.TextMeshProUGUI timerText ;
    public float timeLeft = 0.0f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
        timeLeft += Time.deltaTime;

        timerText.SetText(timeLeft.ToString("0"));
    }
}
