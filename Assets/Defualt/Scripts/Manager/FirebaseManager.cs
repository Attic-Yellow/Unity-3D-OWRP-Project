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
    public FirebaseAuth auth { get; private set; }

    FirebaseFirestore db;
    public bool IsFirebaseInitialized = false; // �ʱ�ȭ ���� �÷���

    private void Awake()
    {
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

    // �Խ�Ʈ �α���
    public void SignInAnonymously(Action<bool> onCompletion)
    {
        auth.SignInAnonymouslyAsync().ContinueWithOnMainThread(task =>
        {
            if (task.IsFaulted || task.IsCanceled)
            {
                print("�Խ�Ʈ �α��� ����: " + task.Exception);
                onCompletion(false);
            }
            else
            {
                FirebaseUser newUser = task.Result.User; // �Խ�Ʈ �α��� ����
                InitializeUserData(newUser.UserId, success =>
                {
                    if (success)
                    {
                        GameManager.Instance.firebaseManager.UpdateGuestStatus(newUser.UserId, true, guestUpdated => { });
                        GameManager.Instance.firebaseManager.UpdateChangedToEmailAccount(newUser.UserId, false, guestUpdated => { });
                    }
                    else
                    {
                        print("�Խ�Ʈ ����� ������ �ʱ�ȭ ����");
                    }
                    onCompletion(true);
                });
            }
        });
    }

    /*** ���ε� ***/

    // ĳ���� ���� �� Firestore�� ���ε��ϴ� �޼���
    public async Task<bool> CreateCharacter(string userId, string job, string tribe, string serverName, string characterName)
    {
        bool isCharacterCreated = false;

        print($"{job}, {tribe}, {serverName}, {characterName}");
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
            print(loadAssetBundleRequest);
            AssetBundle bundle = await loadAssetBundleRequest.ToTask();
            print(bundle);
            if (bundle == null)
            {
                print("���� ���� �ε� ����");
                return false;
            }

            TextAsset loadedJobAsset = await bundle.LoadAssetAsync<TextAsset>(job).ToTask<TextAsset>();
            print(loadedJobAsset);
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

                print(totalSTR);
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
                print(totalSTR);
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
            DocumentReference docRef = db.Collection("users").Document(userId).Collection(serverName).Document(uniqueCharacterID);

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
    public async void LoadUserData(string userId, Action<Dictionary<string, object>> onCompletion)
    {
        var docRef = db.Collection("users").Document(userId);
        try
        {
            var snapshot = await docRef.GetSnapshotAsync();
            if (!snapshot.Exists)
            {
                print("���� ������ �ε� ����");
                onCompletion(null);
                return;
            }

            var userData = snapshot.ToDictionary();
            onCompletion(userData);
        }
        catch (Exception ex)
        {
            print($"���� ������ �ε� �� ���� �߻�: {ex.Message}");
            onCompletion(null);
        }
    }

    // ĳ���� �ε�
    public async Task LoadCharacter(string userId, Action<List<Dictionary<string, object>>> onCompletion)
    {
        try
        {
            var userDocRef = db.Collection("users").Document(userId); // "users" �÷��ǿ��� �α��ε� ������� ������ Ž��
            var userSnapshot = await userDocRef.GetSnapshotAsync();

            if (!userSnapshot.Exists)
            {
                print("����� ������ ã�� �� �����ϴ�.");
                onCompletion(null);
                return;
            }
            
            var serverCharactersRef = userDocRef.Collection("server1");  // ������ ���� �̸����� ���� �÷��� ���� ��� ĳ���� ������ ��ȸ
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
    public void InitializeUserData(string userId, Action<bool> onCompletion)
    {
        var docRef = db.Collection("users").Document(userId);
        var user = new Dictionary<string, object>
    {
        { "Guest" , GameManager.Instance.GetIsUserGuest() },
        { "emailauthentication", GameManager.Instance.GetIsEmailAuthentication() },
        { "ChangedToEmailAccount", GameManager.Instance.GetIsChangedToEmailAccount() }
    };
        docRef.SetAsync(user).ContinueWithOnMainThread(task =>
        {
            if (task.IsFaulted)
            {
                print("����� ������ �ʱ�ȭ ����");
                onCompletion?.Invoke(false);
            }
            else
            {
                onCompletion?.Invoke(true);
            }
        });
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

    // �Խ�Ʈ ���� ���� ������Ʈ
    public void UpdateGuestStatus(string userId, bool isGuest, Action<bool> onCompletion)
    {
        var docRef = db.Collection("users").Document(userId);
        Dictionary<string, object> updates = new Dictionary<string, object>
    {
        { "Guest", isGuest }
    };
        docRef.UpdateAsync(updates).ContinueWithOnMainThread(task =>
        {
            if (task.IsFaulted)
            {
                print("Guest ���� ������Ʈ ����: " + task.Exception);
                onCompletion(false);
            }
            else
            {
                print("Guest ���� ������Ʈ ����");
                onCompletion(true);
            }
        });
    }

    // �Խ�Ʈ ���� ȸ�� ��ȯ ���� ������Ʈ
    public void UpdateChangedToEmailAccount(string userId, bool isChangedToEmailAccount, Action<bool> onCompletion)
    {
        var docRef = db.Collection("users").Document(userId);
        Dictionary<string, object> updates = new Dictionary<string, object>
    {
        { "ChangedToEmailAccount", isChangedToEmailAccount }
    };
        docRef.UpdateAsync(updates).ContinueWithOnMainThread(task =>
        {
            if (task.IsFaulted)
            {
                // print("ȸ�� ��ȯ ���� ������Ʈ ����: " + task.Exception);
                onCompletion(false);
            }
            else
            {
                // print("ȸ�� ��ȯ ���� ������Ʈ ����");
                onCompletion(true);
            }
        });
    }

    // �̸��� ���� ���� ������Ʈ
    public void UpdateEmailAuthentication(string userId, bool isEmailAuthentication, Action<bool> onCompletion)
    {
        var docRef = db.Collection("users").Document(userId);
        Dictionary<string, object> updates = new Dictionary<string, object>
    {
        { "emailauthentication", isEmailAuthentication }
    };
        docRef.UpdateAsync(updates).ContinueWithOnMainThread(task =>
        {
            if (task.IsFaulted)
            {
                print("�̸��� ���� ���� ������Ʈ ����: " + task.Exception);
                onCompletion(false);
            }
            else
            {
                print("�̸��� ���� ���� ������Ʈ ����");
                onCompletion(true);
            }
        });
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
                print(totalSTR);
                print(JobStatus.GetValue<int>("str"));
                totalSTR += JobStatus.GetValue<int>("str");
                totalINT += JobStatus.GetValue<int>("int");
                totalDEX += JobStatus.GetValue<int>("dex");
                totalSPI += JobStatus.GetValue<int>("spi");
                totalVIT += JobStatus.GetValue<int>("vit");
                totalCRT += JobStatus.GetValue<int>("crt");
                totalDH += JobStatus.GetValue<int>("dh");
                totalDET += JobStatus.GetValue<int>("det");
                totalSKS += JobStatus.GetValue<int>("sks");
                totalSPS += JobStatus.GetValue<int>("sps");
                totalTEN += JobStatus.GetValue<int>("ten");
                totalPIE += JobStatus.GetValue<int>("pie");
                totalDEF += JobStatus.GetValue<int>("def");
                totalMDF += JobStatus.GetValue<int>("mef");
                totalLUK += JobStatus.GetValue<int>("luk");
            }

            var TribeStatus = await db.Collection("createCharacter").Document(tribe).GetSnapshotAsync();
            if (TribeStatus.Exists)
            {
                totalSTR += TribeStatus.GetValue<int>("str");
                totalINT += TribeStatus.GetValue<int>("int");
                totalDEX += TribeStatus.GetValue<int>("dex");
                totalSPI += TribeStatus.GetValue<int>("spi");
                totalVIT += TribeStatus.GetValue<int>("vit");
                totalCRT += TribeStatus.GetValue<int>("crt");
                totalDH += TribeStatus.GetValue<int>("dh");
                totalDET += TribeStatus.GetValue<int>("det");
                totalSKS += TribeStatus.GetValue<int>("sks");
                totalSPS += TribeStatus.GetValue<int>("sps");
                totalTEN += TribeStatus.GetValue<int>("ten");
                totalPIE += TribeStatus.GetValue<int>("pie");
                totalDEF += TribeStatus.GetValue<int>("def");
                totalMDF += TribeStatus.GetValue<int>("mef");
                totalLUK += TribeStatus.GetValue<int>("luk");
            }
*/