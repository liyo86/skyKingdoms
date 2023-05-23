using System;
using UnityEngine;

public class PlusOneRotation : MonoBehaviour
{
    private RectTransform rectTransform;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
    }

    void Start()
    {
       // rectTransform.rotation = Quaternion.Euler(0f, -90f, 0f);
    }
}
