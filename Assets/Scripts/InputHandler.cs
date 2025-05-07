using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
namespace TheStack
{
    public class InputHandler : MonoBehaviour
    {
        public Action onClick;

        void OnClick(InputValue inputValue)
        {
            onClick?.Invoke();
        }
        void OnExit(InputValue inputValue)
        {
            SceneManager.LoadScene("MainScene");
        }
    }

}
