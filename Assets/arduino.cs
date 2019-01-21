using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Threading;
//using static System.IO.Ports.SerialPort;

public class arduino : MonoBehaviour
{
    public Text mytext = null;
    System.IO.Ports.SerialPort port;


    bool threadstarted = false;

    int score = 0; //here you can take the value from the thread
    // Use this for initialization
    void Start()
    {
        if ((port = new System.IO.Ports.SerialPort("COM5", 9600)) != null)
        {
            port.Close();
            port.Open();
        }
        Debug.Log("created serial");

        float scalevalue;
        if (Screen.width / 729f < Screen.height / 423f)
        {
            scalevalue = Screen.width / 729f;
        }
        else
        {
            scalevalue = Screen.height / 423f;
        }
        mytext.rectTransform.localScale = new Vector3(scalevalue, scalevalue, 1);


        float offset = 0.041f * Screen.width;

        mytext.rectTransform.anchoredPosition = new Vector2((Screen.width / (-2f)) + offset + (mytext.rectTransform.sizeDelta.x * scalevalue / 2f), (Screen.height / 2f) - offset - (mytext.rectTransform.sizeDelta.y * scalevalue / 2f));
    }

    // Update is called once per frame
    void Update()
    {
        if (!threadstarted)
        {
            ParameterizedThreadStart pts = new ParameterizedThreadStart(ThreadedColorCopy);
            Thread workerForOneRow = new System.Threading.Thread(pts);
            workerForOneRow.Start();
            threadstarted = true;
        }
        mytext.text = "Arduino Value " + score;
    }

    private void ThreadedColorCopy(object threadParamsVar)
    {
        if (port != null)
        {
            string tmp = port.ReadLine();
            int.TryParse(tmp, out score);
        }
    }
}
