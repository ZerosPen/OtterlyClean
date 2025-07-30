using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cameraFollow : MonoBehaviour
{
    [SerializeField] private float speedFollow = 2f;
    [SerializeField] private Transform target;

    public static cameraFollow Instance;

    void Awake()
    {
        // Singleton pattern
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }

        AudioListener listener = GetComponent<AudioListener>();
        if (listener != null && FindObjectsOfType<AudioListener>().Length > 1)
        {
            Debug.LogWarning("Multiple AudioListeners detected. Removing extra from: " + gameObject.name);
            Destroy(listener);
        }
    }

    void Update()
    {
        if (target == null) return;

        Vector3 newPos = new Vector3(target.position.x, target.position.y, -10f);
        transform.position = Vector3.Slerp(transform.position, newPos, speedFollow * Time.deltaTime);
    }
}
