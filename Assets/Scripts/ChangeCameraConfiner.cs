using UnityEngine;
using Cinemachine;

public class ChangeCameraConfiner : MonoBehaviour
{
    // Reference to the Cinemachine Virtual Camera
    public CinemachineVirtualCamera virtualCamera;

    // Reference to the new camera bounds GameObject
    public GameObject newCameraBounds;

    // Call this method when you want to change the confiner
    public void ChangeConfiner()
    {
        // Get the CinemachineConfiner extension
        CinemachineConfiner confiner = virtualCamera.GetComponent<CinemachineConfiner>();

        if (confiner != null)
        {
            // Assign the new PolygonCollider2D component to the confiner
            confiner.m_BoundingShape2D = newCameraBounds.GetComponent<PolygonCollider2D>();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        ChangeConfiner();
        Debug.Log("Change");
    }
}
