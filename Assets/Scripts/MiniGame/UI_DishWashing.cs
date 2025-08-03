using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_DishWashing : MonoBehaviour
{
    public Transform target; // Sink's world position
    public Vector3 offset = new Vector3(0f, 1.5f, 0f); // UI offset above the sink
    private RectTransform rectTransform;
    private Camera mainCamera;

    public void Initialize(Transform followTarget)
    {
        target = followTarget;
    }

    void Start()
    {
        rectTransform = GetComponent<RectTransform>();
        mainCamera = Camera.main;
    }

    void LateUpdate()
    {
        if (target == null || mainCamera == null) return;

        Vector3 worldPosition = target.position + offset;
        Vector3 screenPosition = mainCamera.WorldToScreenPoint(worldPosition);

        // Optional: check if behind the camera
        if (screenPosition.z < 0)
        {
            gameObject.SetActive(false); // hide UI if behind camera
            return;
        }

        gameObject.SetActive(true);
        rectTransform.position = screenPosition;
    }
}
