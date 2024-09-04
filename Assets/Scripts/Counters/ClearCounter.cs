using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;

public class ClearCounter : BaseCounter
{
    [SerializeField] private KitchenObjectSO kitchenObjectSO;

    public override void Interact(Player player)
    {
        if (!HasKitchenObject()) // empty
        {
            if (player.HasKitchenObject())
            {
                player.GetKitchenObject().SetKitchenObjectParent(this);
            } 
        } 
        else // not empty
        {
            if (player.HasKitchenObject())
            {
                if (player.GetKitchenObject().TryGetPlate(out PlateKitchenObject plateKitchenObject))
                {
                    if(plateKitchenObject.TryAddIngridient(GetKitchenObject().GetKitchenObjectSO()))
                    {
                    GetKitchenObject().DestroySelf();
                    }
                }
                else
                {
                    // Player not carrying Plate but something else
                    if(GetKitchenObject().TryGetPlate(out plateKitchenObject)){
                       if(plateKitchenObject.TryAddIngridient(player.GetKitchenObject().GetKitchenObjectSO()))
                       {
                        player.GetKitchenObject().DestroySelf();
                       }
                    }
                }
            }
            else
            {
                GetKitchenObject().SetKitchenObjectParent(player);
            }
        }
    }
}
