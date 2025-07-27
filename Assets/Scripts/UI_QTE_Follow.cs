using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_QTE_Follow : MonoBehaviour
{
    public Transform playerTransform;
    public Vector3 offset = new Vector3(0f,2f,0f);
    public Canvas canvas;

    private RectTransform rt;
    private Camera mainCamera;

    private void Start()
    {
        rt = GetComponent<RectTransform>();
        mainCamera = Camera.main    ;
    }

    private void Update()
    {
        if (playerTransform != null)
        {
            Vector3 worldPosition = playerTransform.position + offset;
            Vector3 screenPositon = mainCamera.WorldToScreenPoint(worldPosition);
            rt.position = screenPositon;
        }
    }
}
