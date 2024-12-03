using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollowerComponent : MonoBehaviour
{
    #region Public Variables
    public GameObject playerPosition;
    public GameObject camera;

    public float cameraMaxOffsetX = 5f;
    public float cameraMaxOffsetY = 7.5f;
    public float cameraMinOffsetX = 2.5f;
    public float cameraMinOffsetY = 2.5f;
    public float followSpeed = 10f;
    #endregion
    #region Private Variables
    private float offsetX = 0f;
    private float offsetY = 0f;
    #endregion
    void Start()
    {
        
    }
    void Update()
    {
        offsetX = this.transform.position.x - playerPosition.transform.position.x;
        offsetY = this.transform.position.y - playerPosition.transform.position.y;

        if (offsetX < -cameraMinOffsetX)
        {
            offsetX += followSpeed * Time.deltaTime;
        }
        else if (offsetX > cameraMinOffsetX)
        {
            offsetX -= followSpeed * Time.deltaTime;
        }

        if (offsetY < -cameraMinOffsetY)
        {
            offsetY += followSpeed * Time.deltaTime;
        }
        else if (offsetY > cameraMinOffsetY)
        {
            offsetY -= followSpeed * Time.deltaTime;
        }

        if (offsetX < -cameraMaxOffsetX)
        {
            offsetX = -cameraMaxOffsetX;
        }
        else if (offsetX > cameraMaxOffsetX)
        {
            offsetX = cameraMaxOffsetX;
        }

        if (offsetY < -cameraMaxOffsetY)
        {
            offsetY = -cameraMaxOffsetY;
        }
        else if (offsetY > cameraMaxOffsetY)
        {
            offsetY = cameraMaxOffsetY;
        }

        this.transform.position = new Vector2(playerPosition.transform.position.x + offsetX, playerPosition.transform.position.y + offsetY);

        camera.transform.position = new Vector3(this.transform.position.x,this.transform.position.y,-10);
    }
}
