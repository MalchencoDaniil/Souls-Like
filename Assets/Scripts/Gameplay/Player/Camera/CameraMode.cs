using Cinemachine;
using UnityEngine;
using Zenject;

public class CameraMode : MonoBehaviour
{
    private bool _canTPS = true;

    [SerializeField]
    private CinemachineFreeLook _tpsCamera;

    [SerializeField]
    internal CinemachineVirtualCamera _focusCamera;

    public bool CanTPS() => _canTPS;

    private void Start()
    {
        _tpsCamera.gameObject.SetActive(_canTPS);
        _focusCamera.gameObject.SetActive(!_canTPS);
    }

    public void SwitchCamera(Animator _playerAnimator)
    {
        _canTPS = !_canTPS;

        _tpsCamera.gameObject.SetActive(_canTPS);
        _focusCamera.gameObject.SetActive(!_canTPS);

        if (_canTPS) _playerAnimator.SetBool(PlayerAnimationNames._focusName, false);

        if (!_canTPS)
        {
            _playerAnimator.SetBool(PlayerAnimationNames._focusName, true);

            _tpsCamera.Follow = _focusCamera.Follow;
            _tpsCamera.LookAt = _focusCamera.LookAt;
        }
    }
}