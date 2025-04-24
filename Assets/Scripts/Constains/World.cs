using UnityEngine;

public class World : MonoBehaviour
{
    internal float GRAVITY_FORCE = -21f;

    public static World _instance;

    private void Awake()
    {
        _instance = this;
    }
}