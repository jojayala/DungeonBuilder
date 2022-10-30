using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum LevelObject
{
    Sphere,
    Cube
}

[Serializable]
public class SerializeObject : MonoBehaviour
{
    // [SerializeField] public Transform myTransform;

    [SerializeField] public LevelObject myLevelObject;
    [SerializeField] public Vector3 myPosition;
    [SerializeField] public Quaternion myRotation;
    [SerializeField] public Vector3 myScale;

    private void Start()
    {
        myPosition = transform.position;
        myRotation = transform.rotation;
        myScale = transform.localScale;
    }
}
