using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class WayPointEditor : EditorWindow
{
    // Prefab của GameObject muốn thêm
    public GameObject objectToSpawn;

    // Tạo menu để mở cửa sổ
    [MenuItem("Window/Spawn Object In Editor")]
    public static void ShowWindow()
    {
        GetWindow<WayPointEditor>("Spawn Object");
    }

    void OnGUI()
    {
        GUILayout.Label("Spawn Object Tool", EditorStyles.boldLabel);
        objectToSpawn = (GameObject)EditorGUILayout.ObjectField("Object to Spawn", objectToSpawn, typeof(GameObject), false);

        if (objectToSpawn == null)
        {
            EditorGUILayout.HelpBox("Please assign a prefab to spawn.", MessageType.Warning);
        }
    }

    // Hàm này cho phép vẽ và tương tác trong SceneView
    void OnEnable()
    {
        SceneView.duringSceneGui += OnSceneGUI;
    }

    void OnDisable()
    {
        SceneView.duringSceneGui -= OnSceneGUI;
    }

    void OnSceneGUI(SceneView sceneView)
    {
        Event e = Event.current;

        // Kiểm tra nếu người dùng nhấn phím "P" và nhấn chuột trái
        
            if (e.type == EventType.MouseDown && e.button == 0 && objectToSpawn != null)
            {
                Ray ray = HandleUtility.GUIPointToWorldRay(e.mousePosition);
                if (Physics.Raycast(ray, out RaycastHit hit))
                {
                    // Spawn đối tượng tại vị trí hit.point
                    GameObject spawnedObject = PrefabUtility.InstantiatePrefab(objectToSpawn) as GameObject;
                    spawnedObject.transform.position = hit.point;
                    Undo.RegisterCreatedObjectUndo(spawnedObject, "Spawn Object");
                }

                // Đảm bảo Unity không nhận sự kiện tiếp theo
                e.Use();
            }
     
    }

}

