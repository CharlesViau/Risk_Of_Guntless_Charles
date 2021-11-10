using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProgrammedDestruction : MonoBehaviour
{
    public float aliveTime;
    private Timer timer;
    // Start is called before the first frame update

    void Awake()
    {
        timer = new Timer(aliveTime);
        timer.StartCount();
    }

    // Update is called once per frame
    void Update()
    {
        if(timer.IsOver)
        { 
            GameObject.Destroy(gameObject); 
        }
    }
}
