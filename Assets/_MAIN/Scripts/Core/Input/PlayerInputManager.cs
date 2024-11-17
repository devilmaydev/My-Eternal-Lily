using System;
using System.Collections.Generic;
using _MAIN.Scripts.Core.Dialogue;
using UnityEngine;
using UnityEngine.InputSystem;

namespace _MAIN.Scripts.Core.Input
{
    public class PlayerInputManager : MonoBehaviour
    {
        private PlayerInput _input;
        private List<(InputAction action, Action<InputAction.CallbackContext> command)> _actions = new();

        private void Awake()
        {
            _input = GetComponent<PlayerInput>();
            
            InitializeActions();
        }

        private void InitializeActions()
        {
            _actions.Add((_input.actions["Next"], OnNext));
        }

        private void OnEnable()
        {
            foreach (var inputAction in _actions)
                inputAction.action.performed += inputAction.command;
        }

        private void OnDisable()
        {
            foreach (var inputAction in _actions)
                inputAction.action.performed -= inputAction.command;
        }

        public void OnNext(InputAction.CallbackContext c)
        {
            DialogueSystem.Instance.OnUserPromptNext();
        }
    }
}