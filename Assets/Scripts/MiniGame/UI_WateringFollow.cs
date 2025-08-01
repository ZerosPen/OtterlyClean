using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_WateringFollow : MonoBehaviour
{
    private Transform plantTransform;
    private Camera mainCamera;
    private RectTransform rt;
    public Vector3 offset = new Vector3(0f, 2f, 0f);

    public void Initialize(Transform plant)
    {
        plantTransform = plant;
    }

    private void Start()
    {
        rt = GetComponent<RectTransform>();
        mainCamera = Camera.main;
    }

    private void Update()
    {
        if (plantTransform == null || mainCamera == null) return;

        Vector3 worldPosition = plantTransform.position + offset;
        Vector3 screenPosition = mainCamera.WorldToScreenPoint(worldPosition);
        rt.position = screenPosition;
    }
}
