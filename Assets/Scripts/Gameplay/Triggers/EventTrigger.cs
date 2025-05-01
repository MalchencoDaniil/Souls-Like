using UnityEngine;
using UnityEngine.Events;

public class EventTrigger : MonoBehaviour
{
    private Transform _target;

    [SerializeField]
    private UnityEvent _triggerEvent;

    [SerializeField]
    private bool _canDestroy;

    private void Awake()
    {
        _target = FindObjectOfType<PlayerController>().transform;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.transform == _target)
        {
            _triggerEvent.Invoke();

            if (_canDestroy)
                Destroy(gameObject);
        }
    }
}