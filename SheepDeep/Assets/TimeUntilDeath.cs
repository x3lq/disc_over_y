using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class TimeUntilDeath : MonoBehaviour {

    public float startTime;
    public Text text;

    void Start()
    {
        startTime = SheepManager.deathInChildBirth;
    }

    void FixedUpdate()
    {
        int time = (int)(startTime - Time.deltaTime);
        string timeString = "" + time;
        GetComponent<TextMesh>().text = timeString;

        if (time <= 0)
        {
            Destroy(gameObject);
        }
    }


}
