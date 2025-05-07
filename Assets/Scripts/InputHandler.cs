using System;
using UnityEngine;
using UnityEngine.InputSystem;
namespace TheStack
{
    public class InputHandler : MonoBehaviour
    {
        public Action onClick;

        void OnClick(InputValue inputValue)
        {
            onClick?.Invoke();
        }
    }

}
