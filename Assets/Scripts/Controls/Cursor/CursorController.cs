using UnityEngine;

public class CursorController : MonoBehaviour
{
    public static CursorController _instance;

    public CursorState _cursorState;

    private void Awake()
    {
        _instance = this;
    }

    private void Start()
    {
        ChangeState(_cursorState);
    }

    public void ChangeState(CursorState _newCursorState)
    {
        _cursorState = _newCursorState;

        switch (_cursorState)
        {
            case CursorState.Locked:
                Cursor.lockState = CursorLockMode.Locked;
                break;
            case CursorState.UnLocked:
                Cursor.lockState = CursorLockMode.None;
                break;
            case CursorState.Unvisible:
                Cursor.visible = false;
                break;
            case CursorState.Visible:
                Cursor.visible = true;
                break;
        }
    }
}