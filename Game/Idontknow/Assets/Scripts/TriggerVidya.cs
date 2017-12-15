using UnityEngine;
using System.Collections;

public class TriggerVidya : MonoBehaviour
{

    public static bool triggered;

    void Start()
    {
        triggered = false;
    }

    void OnTriggerEnter(Collider other)
    {
        Debug.Log("Yaaay");

        if (triggered == false)
            {
                triggered = true;
                StreamVideo streamVideo = GetComponent<StreamVideo>();
                streamVideo.execute();
            }

    }

}
