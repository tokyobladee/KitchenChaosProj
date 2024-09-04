using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlateKitchenObject : KitchenObject
{
    [SerializeField] private List<KitchenObjectSO> validKitchenObjectSOList;

    private List<KitchenObjectSO> KitchenObjectSOList;

    private void Awake()
    {
        KitchenObjectSOList = new List<KitchenObjectSO>();
    }

    public bool TryAddIngridient(KitchenObjectSO kitchenObjectSO)
    {
        if(!validKitchenObjectSOList.Contains(kitchenObjectSO))
        {
            return false;
        }
        
        if(KitchenObjectSOList.Contains(kitchenObjectSO))
        {
            // Alredy has this
            return false;
        }

        KitchenObjectSOList.Add(kitchenObjectSO);
        return true;
        
    }
}
