using System.Collections.Generic;
using System.IO;
using UnityEngine;

[System.Serializable]
public class SceneState
{
    public List<ObjectState> objects;
    public float selectionTransparency;
}

[System.Serializable]
public class ObjectState
{
    public string name;
    public Vector3 position;
    public Quaternion rotation;
    public Vector3 scale;
    public bool isActive;
    public bool isSelected;
}

public class SceneDataManager : MonoBehaviour
{
    private static SceneDataManager _instance;

    private const string SAVE_FILE_NAME = "sceneState.json";
    private string saveFilePath;

    public static SceneDataManager GetInstance() => _instance;

    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            _instance = this;
        }

        saveFilePath = Path.Combine(Application.persistentDataPath, SAVE_FILE_NAME);
    }

    public void SaveScene()
    {
        GameObject[] interactableObjects = GameObject.FindGameObjectsWithTag("Interactable");

        SceneState sceneState = new SceneState
        {
            objects = new List<ObjectState>(),
            selectionTransparency = SelectionHandler.GetInstance().selectedMat.color.a
        };

        foreach (var obj in interactableObjects)
        {
            ObjectState objectState = new ObjectState
            {
                name = obj.name,
                position = obj.transform.position,
                rotation = obj.transform.rotation,
                scale = obj.transform.localScale,
                isActive = obj.activeSelf,
                isSelected = SelectionHandler.GetInstance().IsSelected(obj)
            };

            sceneState.objects.Add(objectState);
        }

        string json = JsonUtility.ToJson(sceneState, true);
        File.WriteAllText(saveFilePath, json);
        Debug.Log("Scene was saved successfully");
    }

    public void LoadScene()
    {
        if (!File.Exists(saveFilePath))
        {
            return;
        }

        string json = File.ReadAllText(saveFilePath);
        SceneState loadedState = JsonUtility.FromJson<SceneState>(json);

        if (loadedState == null || loadedState.objects == null) return;

        SelectionHandler.GetInstance().SetSelectionTransparency(loadedState.selectionTransparency);

        List<GameObject> objectsToSelect = new List<GameObject>();

        foreach (var objectState in loadedState.objects)
        {
            GameObject sceneObject = GameObject.Find(objectState.name);
            if (sceneObject != null)
            {
                sceneObject.transform.position = objectState.position;
                sceneObject.transform.rotation = objectState.rotation;
                sceneObject.transform.localScale = objectState.scale;
                sceneObject.SetActive(objectState.isActive);

                if (objectState.isSelected)
                {
                    objectsToSelect.Add(sceneObject);
                }
            }
            else
            {
                Debug.LogWarning($"Some objects are either hidden or otherwise modified");
            }
        }

        SelectionHandler.GetInstance().ReplaceSelection(objectsToSelect);

        Debug.Log("Scene has been loaded successfully");
    }

}
