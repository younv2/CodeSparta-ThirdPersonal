using UnityEngine.InputSystem;
using UnityEngine;
using System;

public class InputHandler : MonoBehaviour
{
    private static InputHandler instance;
    public static InputHandler Instance
    {
        get
        {
            if (instance == null)
            {
                instance = GameObject.FindObjectOfType<InputHandler>();

                if (instance == null)
                {
                    GameObject go = new GameObject("InputHandler");
                    instance = go.AddComponent<InputHandler>();
                }

                DontDestroyOnLoad(instance.gameObject);
            }

            return instance;
        }
    }

    public Vector2 MoveInput { get; private set; }

    public event Action OnInteractInput;
    public event Action OnProgressDialogInput;

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
    }

    public void OnMove(InputValue value)
    {
        MoveInput = value.Get<Vector2>();
    }

    public void OnInteract(InputValue value)
    {
        if (value.isPressed)
            OnInteractInput?.Invoke();
    }

    public void OnProgressDialog(InputValue value)
    {
        if (value.isPressed)
            OnProgressDialogInput?.Invoke();
    }
}