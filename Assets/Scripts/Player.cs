using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class Player : MonoBehaviour, IKitchenObjectParent
{ 
    public static Player Instance { get; private set; }

    public event EventHandler <OnSelectedCounterChangedEventArgs> OnSelectedCounterChanged;

    public class OnSelectedCounterChangedEventArgs : EventArgs
    {
        public BaseCounter selectedCounter;
    }
    
    [SerializeField] private float moveSpeed = 7f; 
    [SerializeField] private float rotateSpeed = 10f;
    [SerializeField] private GameInput gameInput;
    [SerializeField] private LayerMask counterLayerMask;
    [SerializeField] private Transform kitchenOjectHoldPoint;
    private float playerRadius = 0.7f;
    private float playerHight = 2f;
    private KitchenObject kitchenObject;

    private BaseCounter selectedCounter;
    private bool isWalking;
    private Vector3 lastInteractDir;

    public void Awake()
    {
        if (Instance != null)
        {
            Debug.LogError("More than one player");
        }
        Instance = this;
    }
    public void Start()
    {
        gameInput.OnInteractAction += GameInput_OnInteractAction;
    }

    private void GameInput_OnInteractAction(object sender, EventArgs e)
    {
        if (selectedCounter != null)
        {
            selectedCounter.Interact(this);
        }
    }

    private void Update()
    {
        HandleMovement();
        HandleInteractions();
    }

    public bool IsWalking()
    {
        return isWalking;
    }

    private void HandleInteractions()
    {
        Vector2 inputVector = gameInput.GetMovementVectorNormalized();
        
        Vector3 moveDir = new Vector3(inputVector.x,0f,inputVector.y);
        
        if (moveDir != Vector3.zero)
        {
            lastInteractDir = moveDir;
        }
        
        float interactDistance = 2f;
        if (Physics.Raycast(transform.position, lastInteractDir, out RaycastHit raycastHit, interactDistance,
                counterLayerMask))
        {
            if (raycastHit.transform.TryGetComponent(out BaseCounter baseCounter))
            {
                if (baseCounter != selectedCounter)
                {
                    SetSelectedCounter(baseCounter);
                }
            }
            else { SetSelectedCounter(null); }
        }
        else { SetSelectedCounter(null); }
        
        Debug.Log(selectedCounter);
    }
    
    private void HandleMovement()
    {
        Vector2 inputVector = gameInput.GetMovementVectorNormalized();
        Vector3 moveDir = new Vector3(inputVector.x,0f,inputVector.y);
        
        float moveDistance = moveSpeed * Time.deltaTime;
        bool canMove = !Physics.CapsuleCast(transform.position, transform.position + Vector3.up *
            playerHight, playerRadius, moveDir, moveDistance);

        if (!canMove)
        {
            Vector3 moveDirX = new Vector3(moveDir.x, 0f, 0f).normalized;
            canMove = !Physics.CapsuleCast(transform.position, transform.position + Vector3.up *
                playerHight, playerRadius, moveDirX, moveDistance);
            
            if (canMove)
            {
                moveDir = moveDirX;
            }
            else {
                Vector3 moveDirY = new Vector3(0f, 0f, moveDir.y).normalized;
                canMove = !Physics.CapsuleCast(transform.position, transform.position + Vector3.up *
                    playerHight, playerRadius, moveDirY, moveDistance);

                if (canMove)
                {
                    moveDir = moveDirY;
                }
                else
                {
                    // Cannot move
                }
            }
        }
        
        
        if(canMove){
            transform.position += moveDir * moveDistance;
        }
        
        isWalking = moveDir != Vector3.zero;
        transform.forward = Vector3.Slerp(transform.forward, moveDir, rotateSpeed * Time.deltaTime);
    }

    private void SetSelectedCounter(BaseCounter selectedCounter)
    {
        this.selectedCounter = selectedCounter;
        OnSelectedCounterChanged?.Invoke(this, new OnSelectedCounterChangedEventArgs
        {
            selectedCounter = selectedCounter
        });
    }

    public Transform GetKitchenObjectFollowTransform()
    {
        return kitchenOjectHoldPoint;
    }

    public void SetKitchenObject(KitchenObject kitchenObject)
    {
        this.kitchenObject = kitchenObject;
    }

    public KitchenObject GetKitchenObject()
    {
        return kitchenObject;
    }

    public void ClearKitchenObject()
    {
        kitchenObject = null;
    }

    public bool HasKitchenObject()
    {
        return kitchenObject != null;
    }
}
