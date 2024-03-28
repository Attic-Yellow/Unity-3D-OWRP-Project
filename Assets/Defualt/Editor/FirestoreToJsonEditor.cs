#if UNITY_EDITOR
using UnityEditor;
using Firebase.Firestore;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using Firebase.Extensions;
using UnityEngine;
using System.Drawing.Printing;

public class FirestoreToJsonEditor : EditorWindow
{
    // ���� ID ����� �����մϴ�.
    private static readonly string[] collections = { "createCharacter", "createCharacterJob" };
    private static readonly string[] tribes = { "Human", "Elf", "Dwarf" };
    private static readonly string[] jobs = { "Warrior", "Dragoon", "Bard", "WhiteMage", "BlackMage" };

    [MenuItem("Firestore/Export Data to JSON")]
    public static void ShowWindow()
    {
        EditorWindow.GetWindow(typeof(FirestoreToJsonEditor));
        foreach (var collection in collections)
        {
            Debug.Log(collection);
            if (collection == "createCharacter")
            {
                foreach (var document in tribes)
                {
                    ExportFirestoreDataToJson(collection, document);
                }
            }
            else
            {
                foreach (var document in jobs)
                {
                    ExportFirestoreDataToJson(collection, document);
                }
            }
        }
    }

    private static void ExportFirestoreDataToJson(string collection, string document)
    {
        FirebaseFirestore db = FirebaseFirestore.DefaultInstance;

        db.Collection(collection).Document(document).GetSnapshotAsync().ContinueWithOnMainThread(task =>
        {
            if (task.IsFaulted)
            {
                UnityEngine.Debug.LogError($"Error fetching document: {document}");
                return;
            }

            DocumentSnapshot snapshot = task.Result;
            if (!snapshot.Exists)
            {
                UnityEngine.Debug.Log($"Document does not exist: {document}");
                return;
            }

            Dictionary<string, object> documentData = snapshot.ToDictionary();
            string json = JsonConvert.SerializeObject(documentData, Formatting.Indented);

            // Answer ������ �ִ��� Ȯ���ϰ�, ������ �����մϴ�.
            string folderPath = Path.Combine(Application.dataPath, "createCharacter");
            if (!Directory.Exists(folderPath))
            {
                Directory.CreateDirectory(folderPath);
            }

            // JSON ������ ���� ID �̸����� Answer ������ �����մϴ�.
            string filePath = Path.Combine(folderPath, $"{document}.json");
            File.WriteAllText(filePath, json);

            UnityEngine.Debug.Log($"Data exported to {filePath}");
        });

        // Unity �����Ͱ� �� ������ �ν��ϵ��� �����մϴ�. ��ġ ������ �ʿ��� ��� ������ �̵�
        AssetDatabase.Refresh();
    }
}
#endif
