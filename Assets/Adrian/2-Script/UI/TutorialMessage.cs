using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class TutorialMessage : MonoBehaviour
{
    [SerializeField] private InputActionReference _anyButtonAction;

    private void Start()
    {
        _anyButtonAction.action.performed += Deactivate;
    }

    private void Deactivate(InputAction.CallbackContext context)
    {
        gameObject.SetActive(false);
    }

    private void OnDisable()
    {
        _anyButtonAction.action.performed -= Deactivate;
    }
}
