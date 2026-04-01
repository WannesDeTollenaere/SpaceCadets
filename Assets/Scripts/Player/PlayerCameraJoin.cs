using Unity.Cinemachine;
using UnityEngine;

public class PlayerCameraJoin : MonoBehaviour
{
    private void Awake()
    {
        GameObject MainCamera = GameObject.FindGameObjectWithTag("MainCamera");
        if (MainCamera == null)
        {
            Debug.Log("Did not find a camera");
            return;
        }
        CinemachineTargetGroup targetGroup = Object.FindAnyObjectByType<CinemachineTargetGroup>();
        if (targetGroup == null)
        {
            Debug.Log("Did not find a targetGroup");
            return;
        }
        targetGroup.AddMember(transform, 1f, 2f);
    }
}
