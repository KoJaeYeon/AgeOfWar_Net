using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraPanel : MonoBehaviour
{
    [SerializeField] float cameraSpeed;

    Transform cameraTrans;
    float m_minPosX_Camera = 4.25f;
    bool canCameraMove;

    Vector2 m_look = Vector2.zero;
    private void Awake()
    {
        cameraTrans = FindObjectOfType<Camera>().transform;
    }

    private void LateUpdate()
    {
        if(!canCameraMove) { return; }

        cameraTrans.Translate(m_look * Time.deltaTime * cameraSpeed);
        if(cameraTrans.position.x < m_minPosX_Camera * -1)
        {
            Vector3 pos = cameraTrans.position;
            pos.x = m_minPosX_Camera * -1;
            cameraTrans.position = pos;
            canCameraMove = false;
        }
        else if(cameraTrans.position.x > m_minPosX_Camera)
        {
            Vector3 pos = cameraTrans.position;
            pos.x = m_minPosX_Camera;
            cameraTrans.position = pos;
            canCameraMove = false;
        }
    }
    public void OnPoniterEnter_CameraMove(bool right)
    {
        canCameraMove = true;
        m_look = right ? Vector2.right : Vector2.left;
    }

    public void OnPonitereExit_CameraMove()
    {
        canCameraMove = false;
        m_look = Vector2.zero;
    }
}
