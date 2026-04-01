using UnityEngine;

public class Billboard : MonoBehaviour
{
    [SerializeField]
    private Camera mainCamera;

    void Start()
    {
    }

    void LateUpdate()
    {
        if (mainCamera == null) return;
        transform.LookAt(transform.position + mainCamera.transform.rotation * Vector3.forward,
                     mainCamera.transform.rotation * Vector3.up);
        //transform.rotation = mainCamera.transform.rotation;
    }
}

