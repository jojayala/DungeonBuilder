using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json.Linq;
using Unity.VisualScripting;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.Assertions;

[Serializable]
public class BuildManager : MonoBehaviour
{
    private class ObjectInfoWrapper
    {
        public ObjectInfo[] values;
    };
    
    public Transform sceneParent;

    [Serializable]
    public struct LevelObjectPrefabPair
    {
        public LevelObject levelObject;
        public GameObject prefab; 
    };

    public LevelObjectPrefabPair[] pairMap;
    private Dictionary<LevelObject, GameObject> _levelObjectToPrefab;
    private void Start()
    {
        // Set up the dictionary
        _levelObjectToPrefab = new Dictionary<LevelObject, GameObject>();
        foreach (LevelObjectPrefabPair pair in pairMap)
        {
            _levelObjectToPrefab[pair.levelObject] = pair.prefab;
        }
    }


    /*
     * LoadScene()
     *
     * Finds every SerializeObject component that is a child of sceneParent, then serializes
     * each ObjectInfo to level.json
     */
    public void SaveScene()
    {
        ObjectInfoWrapper scene = new ObjectInfoWrapper();
        SerializeObject[] serializeObjects = sceneParent.GetComponentsInChildren<SerializeObject>();
        scene.values = (from s in serializeObjects 
            select s.GetObjectInfo()).ToArray();
        string result = JsonUtility.ToJson(scene, true);
        Debug.Log(result);
        System.IO.File.WriteAllTextAsync(Application.persistentDataPath + "level.json", result);
    }
    
    public void DeleteScene()
    {
        Transform[] transforms = sceneParent.GetComponentsInChildren<Transform>();
        foreach (var t in transforms)
        {
            if (t.gameObject.CompareTag("level objects"))
            {
                Destroy(t.gameObject);
            }
        }
    }

    /*
     * LoadScene()
     *
     * Checks level.json for level information, then adds each prefab to the scene as a child of scene parent
     */
    public void LoadScene()
    {
        // TODO: may be preferable for this to be async, but I'm not sure how to do that
        string level = System.IO.File.ReadAllText(Application.persistentDataPath + "level.json");
        ObjectInfoWrapper scene = JsonUtility.FromJson<ObjectInfoWrapper>(level);
        Debug.Log(scene);
        foreach (var sceneObject in scene.values)
        {
            if (_levelObjectToPrefab.ContainsKey(sceneObject.levelObject))
            {
                Instantiate(_levelObjectToPrefab[sceneObject.levelObject], 
                            sceneObject.position,
                            sceneObject.rotation,
                            sceneParent);
            }
            else
            {
                Debug.LogError($"Could not find scene object {sceneObject.levelObject} in map");
            }
        }
    }

    // Update is called once per frame
    private void Update()
    {
        // TODO: this is not VR input, will be triggered by UI
        if (Input.GetKeyDown(KeyCode.S))
        {
            Debug.Log("Saving");
            SaveScene();
        }
        if (Input.GetKeyDown(KeyCode.L))
        {
            Debug.Log("Loading");
            LoadScene();
        }
        if (Input.GetKeyDown(KeyCode.D))
        {
            Debug.Log("Deleting");
            DeleteScene();
        }
        
        
    }
}
