using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json.Linq;
using Unity.VisualScripting;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.Assertions;

public class BuildManager : MonoBehaviour
{
    public Transform sceneParent;

    [Serializable]
    public struct LevelObjectPrefabPair
    {
        public LevelObject levelObject;
        public GameObject prefab; 
    };

    public LevelObjectPrefabPair[] pairMap;
    private Dictionary<LevelObject, GameObject> LevelObjectToPrefab;
    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("Start");
        LevelObjectToPrefab = new Dictionary<LevelObject, GameObject>();
        // set up the dictionary
        foreach (LevelObjectPrefabPair pair in pairMap)
        {
            Debug.Log(pair.levelObject);
            Debug.Log(pair.prefab);
            LevelObjectToPrefab[pair.levelObject] = pair.prefab;
        }
    }

    public void SaveScene()
    {
        SerializeObject[] scene;
        List<GameObject> gs = new List<GameObject>();
        // set scene equal to the children of sceneParent
        scene= sceneParent.GetComponentsInChildren<SerializeObject>();
        string result = "[\n";
        for(int i = 0; i < scene.Length; i++)
        {
            string json = JsonUtility.ToJson(scene[i], true);
            result += json;
            if (i != scene.Length - 1)
            {
                result += ",\n";
            }
        }

        result += "]";
        Debug.Log(result);
    }

    public void LoadScene(string sceneString)
    {
        SerializeObject[] scene = JsonUtility.FromJson<SerializeObject[]>(sceneString);
        foreach (var sceneObject in scene)
        {
            switch (sceneObject.myLevelObject)
            {
                case LevelObject.Sphere:
                    Instantiate(LevelObjectToPrefab[sceneObject.myLevelObject], 
                                sceneParent);
                    break;
                case LevelObject.Cube:
                    break;
                default:
                    Debug.LogError("Unhandled Serializable Object in loadScene");
                    break;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.S))
        {
            Debug.Log("Saving");
            SaveScene();
        }
        
    }
}
