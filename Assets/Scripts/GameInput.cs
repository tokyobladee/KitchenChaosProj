using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameInput : MonoBehaviour
{

    private const string PLAYER_PREFS_BINDINGS = "InputBindings";

    public static GameInput Instance {get; private set;}
    public event EventHandler OnInteractAction;
    public event EventHandler OnInteractAlternateAction;
    public event EventHandler OnPauseAction;
    public event EventHandler OnBindingRebind;

    public enum Binding{
        Move_Up,
        Move_Down,
        Move_Right,
        Move_Left,
        Interact,
        InteractAlternative,
        Pause
        
    }
    
    private PlayerInputActions playerInputActions;
    private void Awake()
    {
        Instance = this;

        playerInputActions = new PlayerInputActions();
        playerInputActions.Player.Enable();

        playerInputActions.Player.Interact.performed += Interact_performed;
        playerInputActions.Player.InteractAlternative.performed += InteractAlternate_perfomed;
        playerInputActions.Player.Pause.performed += Pause_Performed;

        if(PlayerPrefs.HasKey(PLAYER_PREFS_BINDINGS))
        {
            playerInputActions.LoadBindingOverridesFromJson(PlayerPrefs.GetString(PLAYER_PREFS_BINDINGS));
        }
    }

    private void OnDestroy(){
        playerInputActions.Player.Interact.performed -= Interact_performed;
        playerInputActions.Player.InteractAlternative.performed -= InteractAlternate_perfomed;
        playerInputActions.Player.Pause.performed -= Pause_Performed;

        playerInputActions.Dispose();
    }

    private void Pause_Performed(InputAction.CallbackContext context)
    {
        OnPauseAction?.Invoke(this, EventArgs.Empty);
    }

    private void InteractAlternate_perfomed(InputAction.CallbackContext obj)
    {
        OnInteractAlternateAction?.Invoke(this, EventArgs.Empty);
    }

    private void Interact_performed(InputAction.CallbackContext obj)
    {
        OnInteractAction?.Invoke(this, EventArgs.Empty);
    }


    public Vector2 GetMovementVectorNormalized()
    {
        Vector2 inputVector = playerInputActions.Player.Move.ReadValue<Vector2>();
        
        inputVector = inputVector.normalized;
        
        return inputVector;
    }

    public string GetBindengText(Binding binding)
    {
        switch(binding)
        {
            default:
            case Binding.Move_Up:
            return playerInputActions.Player.Move.bindings[1].ToDisplayString();

            case Binding.Move_Down:
            return playerInputActions.Player.Move.bindings[2].ToDisplayString();

            case Binding.Move_Right:
            return playerInputActions.Player.Move.bindings[3].ToDisplayString();

            case Binding.Move_Left:
            return playerInputActions.Player.Move.bindings[4].ToDisplayString();

            case Binding.Interact:
            return playerInputActions.Player.Interact.bindings[0].ToDisplayString();


            case Binding.InteractAlternative:
            return playerInputActions.Player.InteractAlternative.bindings[0].ToDisplayString();
            
            case Binding.Pause:
            return playerInputActions.Player.Pause.bindings[0].ToDisplayString();
        }
    }

    public void RebindBinding(Binding binding, Action onActionRebound)
    {
        playerInputActions.Player.Disable();

        InputAction inputAction;
        int bindingIndex;

        switch(binding)
        {
            default:
            case Binding.Move_Up:
            inputAction = playerInputActions.Player.Move;
            bindingIndex = 1;
            break;

            case Binding.Move_Down:
            inputAction = playerInputActions.Player.Move;
            bindingIndex = 2;
            break;

            case Binding.Move_Right:
            inputAction = playerInputActions.Player.Move;
            bindingIndex = 3;
            break;

            case Binding.Move_Left:
            inputAction = playerInputActions.Player.Move;
            bindingIndex = 4;
            break;

            case Binding.Interact:
            inputAction = playerInputActions.Player.Interact;
            bindingIndex = 0;
            break;


            case Binding.InteractAlternative:
            inputAction = playerInputActions.Player.InteractAlternative;
            bindingIndex = 0;
            break;
            
            case Binding.Pause:
            inputAction = playerInputActions.Player.Pause;
            bindingIndex = 0;
            break;
        }

        inputAction.PerformInteractiveRebinding(bindingIndex).OnComplete(callback => {
            callback.Dispose();
            playerInputActions.Player.Enable();
            onActionRebound();

            PlayerPrefs.SetString(PLAYER_PREFS_BINDINGS, playerInputActions.SaveBindingOverridesAsJson());
            PlayerPrefs.Save();

            OnBindingRebind?.Invoke(this, EventArgs.Empty);
        }).Start();
    }
}
