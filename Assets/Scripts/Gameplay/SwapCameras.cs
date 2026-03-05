using Unity.Cinemachine;
using UnityEngine;

public class SwapCameras : MonoBehaviour
{
    [SerializeField] private GameObject[] _cameras;
    private int _currentCameraIndex = 0;

    private void Awake()
    {
        InputManager.Instance.OnCameraForward += SwapCam;

        for (int i = 1; i < _cameras.Length; i++)
        {
            bool isActive = i == _currentCameraIndex;
            _cameras[i].SetActive(isActive);
        }
    }

    private void SwapCam()
    {
        _cameras[_currentCameraIndex].SetActive(false);
        _currentCameraIndex = (_currentCameraIndex + 1) % _cameras.Length;
        _cameras[_currentCameraIndex].SetActive(true);
    }
}