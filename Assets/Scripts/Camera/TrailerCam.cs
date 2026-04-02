using UnityEngine;

public class TrailerCam : MonoBehaviour
{
    [SerializeField] private float _speed;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        transform.position += Vector3.forward * (_speed * Time.fixedDeltaTime);
    }
}
