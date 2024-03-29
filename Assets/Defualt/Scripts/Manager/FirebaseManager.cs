using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase;
using Firebase.Firestore;
using Firebase.Extensions;
using System;
using Firebase.Auth;
using UnityEngine.Networking;
using Newtonsoft.Json;
using System.Threading.Tasks;
using System.IO;

public class FirebaseManager : MonoBehaviour
{
    private static FirebaseManager Instance;

    public FirebaseAuth auth { get; private set; }

    FirebaseFirestore db;
    public bool IsFirebaseInitialized = false; // �ʱ�ȭ ���� �÷���

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        GameManager.Instance.firebaseManager = this;

        FirebaseApp.CheckAndFixDependenciesAsync().ContinueWith(task =>
        {
            if (task.IsFaulted || task.IsCanceled)
            {
                print("Could not resolve all Firebase dependencies: " + task.Exception);
                return;
            }

            var dependencyStatus = task.Result;

            if (dependencyStatus == DependencyStatus.Available)
            {
                db = FirebaseFirestore.DefaultInstance;
                auth = FirebaseAuth.DefaultInstance;
                IsFirebaseInitialized = true; // �ʱ�ȭ �Ϸ�
                print("Firebase�� ����� �غ� ��");
            }
            else
            {
                print($"��� Firebase ���Ӽ��� �ذ��� �� ����: {dependencyStatus}");
            }
        });
    }

    /*** ���ε� ***/

    // ĳ���� ���� �� Firestore�� ���ε��ϴ� �޼���
    public async Task<bool> CreateCharacter(string userId, string email, string job, string tribe, string serverName, string characterName)
    {
        bool isCharacterCreated = false;

        try
        {
            /*** �⺻ ���� ***/
            int totalSTR = 0; // �� (striking power)
            int totalINT = 0; // ���� (intelligence)
            int totalDEX = 0; // ��ø (dexterity)
            int totalSPI = 0; // ���ŷ� (spirit)
            int totalVIT = 0; // Ȱ�� (vitality)

            /*** ���� ���� ***/
            int totalCRT = 0; // �ش�,ġ��Ÿ (critical hit)
            int totalDH = 0; // ����,���� (direct hit rate)
            int totalDET = 0; // ����,���� (determination)
            int totalSKS = 0; // ��� ���� �ӵ�,���� ���� �ӵ� (skill speed)
            int totalSPS = 0; // ���� ���� �ӵ�,�ֹ� �ӵ� (spell speed)
            int totalTEN = 0; // �ұ�,�γ� (tenacity)
            int totalPIE = 0; // �ž�,���� (piety)

            /*** ��� ����***/
            int totalDEF = 0; // ���� ���� (defense)
            int totalMDF = 0; // ���� ���� (magic defense)

            /*** ��Ÿ ����***/
            int totalLUK = 0; // �� (luck)

            AssetBundleCreateRequest loadAssetBundleRequest = AssetBundle.LoadFromFileAsync(Path.Combine(Application.streamingAssetsPath, "AssetBundles", "createcharacter"));
            AssetBundle bundle = await loadAssetBundleRequest.ToTask();
            if (bundle == null)
            {
                print("���� ���� �ε� ����");
                return false;
            }

            TextAsset loadedJobAsset = await bundle.LoadAssetAsync<TextAsset>(job).ToTask<TextAsset>();
            if (loadedJobAsset != null)
            {
                string dataAsJson = loadedJobAsset.text;
                Dictionary<string, int> jobData = JsonConvert.DeserializeObject<Dictionary<string, int>>(dataAsJson);

                totalSTR += jobData["str"];
                totalINT += jobData["int"];
                totalDEX += jobData["dex"];
                totalSPI += jobData["spi"];
                totalVIT += jobData["vit"];
                totalCRT += jobData["crt"];
                totalDH  += jobData["dh"];
                totalDET += jobData["det"];
                totalSKS += jobData["sks"];
                totalSPS += jobData["sps"];
                totalTEN += jobData["ten"];
                totalPIE += jobData["pie"];
                totalDEF += jobData["def"];
                totalMDF += jobData["mef"];
                totalLUK += jobData["luk"];
            }
            else
            {
                print($"���� ������ �ε� ����: {job}");
                return false;
            }

            TextAsset loadedTribeAsset = await bundle.LoadAssetAsync<TextAsset>(tribe).ToTask<TextAsset>();
            if (loadedTribeAsset != null)
            {
                string dataAsJson = loadedTribeAsset.text;
                Dictionary<string, int> tribeData = JsonConvert.DeserializeObject<Dictionary<string, int>>(dataAsJson);

                totalSTR += tribeData["str"];
                totalINT += tribeData["int"];
                totalDEX += tribeData["dex"];
                totalSPI += tribeData["spi"];
                totalVIT += tribeData["vit"];
                totalCRT += tribeData["crt"];
                totalDH  += tribeData["dh"];
                totalDET += tribeData["det"];
                totalSKS += tribeData["sks"];
                totalSPS += tribeData["sps"];
                totalTEN += tribeData["ten"];
                totalPIE += tribeData["pie"];
                totalDEF += tribeData["def"];
                totalMDF += tribeData["mef"];
                totalLUK += tribeData["luk"];
            }
            else
            {
                print($"���� ������ �ε� ����: {tribe}");
                return false;
            }

            Dictionary<string, object> newCharacter = new Dictionary<string, object>
            {
                { "name", characterName },
                { "job", job },
                { "tribe", tribe },
                { "level", 1 },
                { "server", serverName }, 
                { "str", totalSTR},
                { "int", totalINT},
                { "dex", totalDEX},
                { "spi", totalSPI},
                { "vit", totalVIT},
                { "crt", totalCRT},
                { "dh", totalDH},
                { "det", totalDET},
                { "sks", totalSKS},
                { "sps", totalSPS},
                { "ten", totalTEN},
                { "pie", totalPIE},
                { "def", totalDEF},
                { "mdf", totalMDF},
                { "luk", totalLUK}
            };

            string uniqueCharacterID = System.Guid.NewGuid().ToString();

            // ������ ���ε� ��� ����: users/{userId}/{serverName}/{characterId}
            DocumentReference docRef = db.Collection("users").Document("email").Collection(email).Document(userId).Collection(serverName).Document(uniqueCharacterID);

            // Firestore�� ĳ���� ������ ���ε�
            await docRef.SetAsync(newCharacter);
            isCharacterCreated = true;
            print($"ĳ���� {characterName} ���� �� ���ε� �Ϸ�.");
        }
        catch (Exception e)
        {
            isCharacterCreated = false;
            print($"ĳ���� ���� �� ���� �߻�: {e.Message}");
        }

        return isCharacterCreated;
    }

    /*** �ε� ***/

    // ����� ������ �ε�
    public async Task LoadUserData(string userId, string email, Action<Dictionary<string, object>> onCompletion)
    {
        var docRef = db.Collection("users").Document("email").Collection(email);
        var userSnapshot = await docRef.GetSnapshotAsync();
        try
        {
            if (userSnapshot == null)
            {
                print("���� ������ �ε� ����");
                onCompletion(null);
                return;
            }

            var userData = docRef.Document(userId);
            var userDocSnapshot = await userData.GetSnapshotAsync();
            var user = userDocSnapshot.ToDictionary();
            onCompletion(user);
        }
        catch (Exception ex)
        {
            print($"���� ������ �ε� �� ���� �߻�: {ex.Message}");
            onCompletion(null);
        }
    }

    // ĳ���� �ε�
    public async Task LoadCharacter(string userId, string server, Action<List<Dictionary<string, object>>> onCompletion)
    {
        var user = auth.CurrentUser;
        try
        {
            var userDocRef = db.Collection("users").Document("email").Collection(user.Email); // "users" �÷��ǿ��� �α��ε� ������� ������ Ž��
            var userSnapshot = await userDocRef.GetSnapshotAsync();

            if (userSnapshot == null)
            {
                print("����� ������ ã�� �� �����ϴ�.");
                onCompletion(null);
                return;
            }
            
            var serverCharactersRef = userDocRef.Document(userId).Collection(server); // ������ ������ �������� ĳ���� ������ Ž��
            var querySnapshot = await serverCharactersRef.GetSnapshotAsync();

            var characters = new List<Dictionary<string, object>>();
            foreach (var document in querySnapshot.Documents)
            {
                var character = document.ToDictionary();
                characters.Add(character);
            }

            // ĳ���Ͱ� ���� ��� null�� ��ȯ
            if (characters.Count == 0)
            {
                onCompletion(null);
                return;
            }

            onCompletion(characters);
        }
        catch (Exception e)
        {
            print($"ĳ���� �ε� �� ���� �߻�: {e.Message}");
            onCompletion(null);
        }
    }

    // ����� ������ �ʱ�ȭ
    public async Task InitializeUserData(string userId, string email, Action<bool> onCompletion)
    {
        var docRef = db.Collection("users").Document("email").Collection(email);
        var userSnapshot = await docRef.GetSnapshotAsync();

        if (userSnapshot != null)
        {
            print("����� ������ �ʱ�ȭ ����");
            onCompletion(false);
            return;
        }

        var user = new List<Dictionary<string, object>>
        {
            new Dictionary<string, object>
            {
                { "guest", GameManager.Instance.GetIsUserGuest() },
                { "emailauthentication", GameManager.Instance.GetIsEmailAuthentication() },
                { "manager", false }
            }
        };

        await docRef.Document(userId).SetAsync(user[0]);
        onCompletion(true);
    }

    //  �α׾ƿ�
    public void SignOut()
    {
        if (auth.CurrentUser != null)
        {
            auth.SignOut();
            Debug.Log("�α׾ƿ� ����");
        }
    }

    // ���� �ʵ� �� ���� ���� Ȯ��
    public void CheckFieldValueExists(string document, string userValue, System.Action<bool> onResult)
    {
        if (!IsFirebaseInitialized)
        {
            print("Firebase�� ���� �ʱ�ȭ���� �ʾ���");
            onResult(false);
            return;
        }

        DocumentReference docRef = db.Collection("answer").Document(document);
        docRef.GetSnapshotAsync().ContinueWithOnMainThread(task =>
        {
            if (task.IsFaulted)
            {
                print("���� ��ȸ �� ���� �߻�");
                onResult(false);
                return;
            }

            var documentSnapshot = task.Result;
            if (!documentSnapshot.Exists)
            {
                print("korean ������ �������� �ʽ��ϴ�.");
                onResult(false);
                return;
            }


            if (documentSnapshot.ContainsField(userValue)) // ����� �Է� ���� �ʵ�� �����ϴ��� Ȯ��
            {
                onResult(true); // print($"�ʵ� '{userValue}'�� ������");
            }
            else
            {
                onResult(false); // print($"�ʵ� '{userValue}'�� �������� ����);
            }
        });
    }
}
public static class AsyncOperationExtensions
{
    public static Task<AssetBundle> ToTask(this AssetBundleCreateRequest request)
    {
        var completionSource = new TaskCompletionSource<AssetBundle>();
        request.completed += _ => completionSource.TrySetResult(request.assetBundle);
        return completionSource.Task;
    }

    public static Task<T> ToTask<T>(this AssetBundleRequest request) where T : UnityEngine.Object
    {
        var completionSource = new TaskCompletionSource<T>();
        request.completed += _ =>
        {
            if (request.asset != null)
            {
                completionSource.TrySetResult(request.asset as T);
            }
            else
            {
                completionSource.TrySetException(new NullReferenceException("AssetBundleRequest returned null asset."));
            }
        };
        return completionSource.Task;
    }
}

/*
            var JobStatus = await db.Collection("createCharacterJob").Document(job).GetSnapshotAsync();
            if (JobStatus.Exists)
            {
                print(JobStatus.GetValue<int>("str"));
            }

            var TribeStatus = await db.Collection("createCharacter").Document(tribe).GetSnapshotAsync();
            if (TribeStatus.Exists)
            {
                totalSTR += TribeStatus.GetValue<int>("str");
            }
*/