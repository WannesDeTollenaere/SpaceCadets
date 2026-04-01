using UnityEngine;

public class PiggyBackInputHint : MonoBehaviour
{
    [SerializeField]
    private GameObject _inputImage;

    private void Start()
    {
        Hide();
    }

    public void Show()
    {
        if (!_inputImage) return;

        _inputImage.SetActive(true);
    }

    public void Hide()
    {
        if (!_inputImage) return;

        _inputImage.SetActive(false);
    }

    public void SetPosition(Vector3 position)
    {
        if (!_inputImage) return;

        _inputImage.transform.position = position;
    }
}
