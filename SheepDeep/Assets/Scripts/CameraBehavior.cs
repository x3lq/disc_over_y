using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraBehavior : MonoBehaviour {

    private Camera camera;

    public static List<Player> playerObjects;

    private Vector3 centerOfPlayers;

    private float biggestDistance;

    public float cameraMinSize;
    public float cameraSize;
    public float cameraSpeed;

	// Use this for initialization
	void Start () {
        camera = GetComponent<Camera>();
	}
	
	// Update is called once per frame
	void Update () {
        CalculateCenterOfPlayers();

        FocusOnCenter();
	}

    void FocusOnCenter()
    {
        transform.position = Vector3.Lerp(transform.position, centerOfPlayers + Vector3.back, Time.deltaTime * cameraSpeed);

        camera.orthographicSize = Mathf.Lerp(camera.orthographicSize, Mathf.Max(biggestDistance * cameraSize, cameraMinSize), Time.deltaTime * cameraSpeed);
    }

    void CalculateCenterOfPlayers()
    {
        centerOfPlayers = Vector3.zero;

        foreach(Player player in playerObjects)
        {
            centerOfPlayers += player.transform.position;
        }

        centerOfPlayers /= playerObjects.Count;

        CalculateBiggestDistance();
    }

    void CalculateBiggestDistance()
    {
        biggestDistance = 0;

        foreach (Player player in playerObjects)
        {
            Vector2 centerAdjusted = new Vector2(centerOfPlayers.x * 9 / 16, centerOfPlayers.y);
            Vector2 positionAdjusted = new Vector2(player.transform.position.x * 9 / 16, player.transform.position.y);

            float distance = (centerAdjusted - positionAdjusted).magnitude;
            if (distance > biggestDistance)
            {
                biggestDistance = distance;
            }
        }
    }
}
