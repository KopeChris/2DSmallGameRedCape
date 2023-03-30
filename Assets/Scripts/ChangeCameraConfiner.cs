using UnityEngine;
using Cinemachine;

public class ChangeCameraConfiner : MonoBehaviour
{
    public CinemachineConfiner2D confiner;
    public CinemachineVirtualCamera virtualCamera;

    public PolygonCollider2D boundingShape;
    public float cameraDistance;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        confiner.m_BoundingShape2D = boundingShape;
        //change camera distance
    }
}
