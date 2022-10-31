using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum LevelObject
{
    Sphere,
    Cube
}

/*
 * ObjectInfo is the class used for serialization of game objects
 * unity seems to have issues with serialization of MonoBehaviours,
 * so this is our workaround. Think of ObjectInfo as all information
 * needed to specify an object
 *
 * levelObject is an enum of all objects that can exist in our scene
 * this allows us to quickly lookup which object a given game object is
 * supposed to be
 */
[Serializable]
public class ObjectInfo
{
    [SerializeField] public LevelObject levelObject;
    [SerializeField] public Vector3 position;
    [SerializeField] public Quaternion rotation;
    [SerializeField] public Vector3 scale;
    
}


/*
 * SerializeObject is the script that handles the serialization
 * of a given game object. it contains and updates an ObjectInfo class
 * which is used to serialize the information.
 */
public class SerializeObject : MonoBehaviour
{
    /*
     * objectName is a public field to let us set the LevelObject
     * from unity
     */
    [SerializeField] public LevelObject objectName;
    public ObjectInfo info;

    /*
     * GetObjectInfo()
     *
     * Called by the BuildManager to get all of the ObjectInfo's
     * in the scene for serialization. Also updates the ObjectInfo,
     * to ensure no stale information gets stored
     */
    public ObjectInfo GetObjectInfo()
    {
        info.levelObject = objectName;
        info.position = transform.position;
        info.rotation = transform.rotation;
        info.scale = transform.localScale;
        return info;
    }

    private void Start()
    {
        info = new ObjectInfo
        {
            levelObject = objectName,
            position = transform.position,
            rotation = transform.rotation,
            scale = transform.localScale
        };
    }
}
