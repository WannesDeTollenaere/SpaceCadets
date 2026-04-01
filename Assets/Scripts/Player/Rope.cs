using UnityEngine;

public class Rope : MonoBehaviour
{
    [SerializeField] private ConfigurableJoint _startJoint;
    [SerializeField] private ConfigurableJoint _endJoint;

    [SerializeField] private PlayerJoin _playerManager;

    void Start()
    {
        if (_playerManager == null) return;

        Rigidbody bigPlayerRb = _playerManager.BigPlayer.GetComponent<Rigidbody>();
        Rigidbody smallPlayerRb = _playerManager.SmallPlayer.GetComponent<Rigidbody>();


        _startJoint.connectedBody = bigPlayerRb;
        _startJoint.autoConfigureConnectedAnchor = false;
        //_startJoint.anchor = Vector3.zero;
        //_startJoint.connectedAnchor = bigPlayerRb.transform.InverseTransformPoint(bigPlayerRb.transform.position);

        _endJoint.connectedBody = smallPlayerRb;
        _endJoint.autoConfigureConnectedAnchor = false;
        //_endJoint.anchor = Vector3.zero;
        //_endJoint.connectedAnchor = smallPlayerRb.transform.InverseTransformPoint(bigPlayerRb.transform.position);

    }
}
