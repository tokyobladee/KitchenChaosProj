using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DeliveryResultUI : MonoBehaviour
{
    private const string POPUP = "Popup";

    [SerializeField] private Image backgroundImage;
    [SerializeField] private Image iconImage;
    [SerializeField] private TextMeshProUGUI messageText;
    [SerializeField] Color succesColor;
    [SerializeField] Color failedColor;
    [SerializeField] Sprite succesSprite;
    [SerializeField] Sprite failedSprite;

    private Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    private void Start()
    {
        DeliveryManager.Instance.OnRecipeSuccess += DeliveryManager_OnRecipeSucces;
        DeliveryManager.Instance.OnRecipeFailed += DeliveryManager_OnRecipeFailed;
        Hide();
    }

    private void DeliveryManager_OnRecipeFailed(object sender, EventArgs e)
    {
        Show();
        animator.SetTrigger(POPUP);
        backgroundImage.color = failedColor;
        iconImage.sprite = failedSprite;
        messageText.text = "DELIVERY\nFAILED"; 
    }

    private void DeliveryManager_OnRecipeSucces(object sender, EventArgs e)
    {
        Show();
        animator.SetTrigger(POPUP);
        backgroundImage.color = succesColor;
        iconImage.sprite = succesSprite;
        messageText.text = "DELIVERY\nSUCCES"; 
    }

    private void Show()
    {
        gameObject.SetActive(true);
    }

    private void Hide()
    {
        gameObject.SetActive(false);
    }
}
