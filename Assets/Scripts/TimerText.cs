using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TimerText : MonoBehaviour
{

    TMP_Text text;
    bool start = false;
    private float runTime = 0;
    // Start is called before the first frame update
    void Start()
    {
        text = GetComponent<TMP_Text>();
    }

    // Update is called once per frame
    void Update()
    {
        if (start)
        {
            runTime +=Time.deltaTime;
            text.text = "Time: " + System.TimeSpan.FromSeconds(runTime).ToString("mm\\:ss\\.ff");
        }
    }
    public void startTimer()
    {
        start = true;

    }
}
