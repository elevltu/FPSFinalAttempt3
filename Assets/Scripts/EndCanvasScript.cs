using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class EndCanvasScript : MonoBehaviour
{
    public TextMeshProUGUI timer;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void onPlayerDie()
    {
        if (UniversalTimer.timer % 60 < 10)
        {
            timer.text = "You Survived " + ((int)(UniversalTimer.timer / 60) + ":0" + (int)(UniversalTimer.timer % 60));
        }
        else
        {
            timer.text = "You Survived " + ((int)(UniversalTimer.timer / 60) + ":" + (int)(UniversalTimer.timer % 60));
        }
    }
}
