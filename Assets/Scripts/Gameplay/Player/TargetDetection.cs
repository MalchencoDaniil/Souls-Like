using UnityEngine;
using Cinemachine;

public class TargetDetection : MonoBehaviour
{
    [SerializeField]
    private float _detectionRadius = 10f;

    [SerializeField]
    private CinemachineFreeLook _freeLookCamera;

    [SerializeField]
    private LayerMask _obstacleLayer;

    public Transform FindClosestTarget()
    {
        Vector3 _cameraPosition = _freeLookCamera.Follow.position;

        Vector3 _cameraForward = _freeLookCamera.Follow.forward;

        Collider[] _colliders = Physics.OverlapSphere(_cameraPosition, _detectionRadius, ~_obstacleLayer);

        Transform _closestTarget = null;
        float _minDistance = Mathf.Infinity;

        foreach (Collider _collider in _colliders)
        {
            Transform _targetTransform = _collider.transform;
            Vector3 _directionToTarget = (_targetTransform.position - _cameraPosition).normalized;

            if (Physics.Raycast(_cameraPosition, _directionToTarget, out RaycastHit _hit, Vector3.Distance(_cameraPosition, _targetTransform.position), _obstacleLayer))
            {
                continue; 
            }

            float _angle = Vector3.Angle(_cameraForward, _directionToTarget);
            float _fov = _freeLookCamera.m_Lens.FieldOfView;

            float _fovThreshold = _fov / 2f + 5f;

            if (_angle <= _fovThreshold)
            {
                float _distance = Vector3.Distance(_cameraPosition, _targetTransform.position);

                if (_distance < _minDistance)
                {
                    _minDistance = _distance;
                    _closestTarget = _targetTransform;
                }
            }
        }

        return _closestTarget;
    }
}