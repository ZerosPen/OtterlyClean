using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_QTE_Follow : MonoBehaviour
{
    private Transform playerTransform;
    private Canvas canvas;
    public Vector3 offset = new Vector3(0f, 2f, 0f);

    private RectTransform rt;
    private Camera mainCamera;

    public void Initialize(Transform player, Canvas canvas)
    {
        this.playerTransform = player;
        this.canvas = canvas;
    }

    private void Start()
    {
        rt = GetComponent<RectTransform>();
        mainCamera = Camera.main;
    }

    private void Update()
    {
        if (playerTransform == null || mainCamera == null) return;

        Vector3 worldPosition = playerTransform.position + offset;
        Vector3 screenPosition = mainCamera.WorldToScreenPoint(worldPosition);
        rt.position = screenPosition;
    }
}
