using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Camera : MonoBehaviour {

    public float minXLimmit;
    public float maxXLimmit;
    public float minYLimmit;
    public float maxYLimmit;
    private float cameraDistance = -10;

    GameObject player;

	// Use this for initialization
	void Start () {
        player = GameObject.FindGameObjectWithTag("Player");
	}
	
	// Update is called once per frame
	void Update () {
        gameObject.transform.position = new Vector3(player.transform.position.x, player.transform.position.y, cameraDistance);
        if (gameObject.transform.position.x < minXLimmit)
        {
            gameObject.transform.position = new Vector3(minXLimmit, gameObject.transform.position.y, cameraDistance);
        }
        if (gameObject.transform.position.x > maxXLimmit)
        {
            gameObject.transform.position = new Vector3(maxXLimmit, gameObject.transform.position.y, cameraDistance);
        }
        if (gameObject.transform.position.y < minYLimmit)
        {
            gameObject.transform.position = new Vector3(gameObject.transform.position.x, minYLimmit, cameraDistance);
        }
        if (gameObject.transform.position.y > maxYLimmit)
        {
            gameObject.transform.position = new Vector3(gameObject.transform.position.x, maxYLimmit, cameraDistance);
        }
    }
}
