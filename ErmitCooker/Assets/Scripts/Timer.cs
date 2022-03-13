using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour
{
    public float timeSpent = 0.0f;
    private bool finished = false;

    // Update is called once per frame
    void Update()
    {
        if(finished)
        {
            return;
        }
        timeSpent += Time.deltaTime;

        ScoreManager.Instance.UpdateTime(timeSpent);
    }

    public void Finish()
    {
        finished = true;
        ScoreManager.Instance.UpdateBestTime(timeSpent);
    }
}
