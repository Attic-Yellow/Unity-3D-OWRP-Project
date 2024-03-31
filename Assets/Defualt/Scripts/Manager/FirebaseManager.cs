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
    public bool IsFirebaseInitialized = false; // 초기화 상태 플래그

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
                IsFirebaseInitialized = true; // 초기화 완료
                print("Firebase를 사용할 준비가 됨");
            }
            else
            {
                print($"모든 Firebase 종속성을 해결할 수 없음: {dependencyStatus}");
            }
        });
    }

    /*** 업로드 ***/

    // 캐릭터 생성 및 Firestore에 업로드하는 메서드
    public async Task<bool> CreateCharacter(string userId, string email, string job, string tribe, string serverName, string characterName)
    {
        bool isCharacterCreated = false;

        try
        {
            /*** 기본 스탯 ***/
            int totalSTR = 0; // 힘 (striking power)
            int totalINT = 0; // 지능 (intelligence)
            int totalDEX = 0; // 민첩 (dexterity)
            int totalSPI = 0; // 정신력 (spirit)
            int totalVIT = 0; // 활력 (vitality)

            /*** 전투 스탯 ***/
            int totalCRT = 0; // 극대,치명타 (critical hit)
            int totalDH = 0; // 직격,명중 (direct hit rate)
            int totalDET = 0; // 의지,결의 (determination)
            int totalSKS = 0; // 기술 시전 속도,물리 공격 속도 (skill speed)
            int totalSPS = 0; // 마법 시전 속도,주문 속도 (spell speed)
            int totalTEN = 0; // 불굴,인내 (tenacity)
            int totalPIE = 0; // 신앙,마나 (piety)

            /*** 방어 스탯***/
            int totalDEF = 0; // 물리 방어력 (defense)
            int totalMDF = 0; // 마법 방어력 (magic defense)

            /*** 기타 스탯***/
            int totalLUK = 0; // 운 (luck)

            AssetBundleCreateRequest loadAssetBundleRequest = AssetBundle.LoadFromFileAsync(Path.Combine(Application.streamingAssetsPath, "AssetBundles", "createcharacter"));
            AssetBundle bundle = await loadAssetBundleRequest.ToTask();
            if (bundle == null)
            {
                print("에셋 번들 로드 실패");
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
                print($"직업 데이터 로드 실패: {job}");
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
                print($"종족 데이터 로드 실패: {tribe}");
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

            // 데이터 업로드 경로 설정: users/{userId}/{serverName}/{characterId}
            DocumentReference docRef = db.Collection("users").Document("email").Collection(email).Document(userId).Collection(serverName).Document(uniqueCharacterID);

            // Firestore에 캐릭터 데이터 업로드
            await docRef.SetAsync(newCharacter);
            isCharacterCreated = true;
            print($"캐릭터 {characterName} 생성 및 업로드 완료.");
        }
        catch (Exception e)
        {
            isCharacterCreated = false;
            print($"캐릭터 생성 중 오류 발생: {e.Message}");
        }

        return isCharacterCreated;
    }

    /*** 로드 ***/

    // 사용자 데이터 로드
    public async Task LoadUserData(string userId, string email, Action<Dictionary<string, object>> onCompletion)
    {
        var docRef = db.Collection("users").Document("email").Collection(email);
        var userSnapshot = await docRef.GetSnapshotAsync();
        try
        {
            if (userSnapshot == null)
            {
                print("유저 데이터 로드 실패");
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
            print($"유저 데이터 로드 중 오류 발생: {ex.Message}");
            onCompletion(null);
        }
    }

    // 캐릭터 로드
    public async Task LoadCharacter(string userId, string server, Action<List<Dictionary<string, object>>> onCompletion)
    {
        var user = auth.CurrentUser;
        try
        {
            var userDocRef = db.Collection("users").Document("email").Collection(user.Email); // "users" 컬렉션에서 로그인된 사용자의 문서를 탐색
            var userSnapshot = await userDocRef.GetSnapshotAsync();

            if (userSnapshot == null)
            {
                print("사용자 정보를 찾을 수 없습니다.");
                onCompletion(null);
                return;
            }
            
            var serverCharactersRef = userDocRef.Document(userId).Collection(server); // 가져온 유저의 서버에서 캐릭터 문서를 탐색
            var querySnapshot = await serverCharactersRef.GetSnapshotAsync();

            var characters = new List<Dictionary<string, object>>();
            foreach (var document in querySnapshot.Documents)
            {
                var character = document.ToDictionary();
                characters.Add(character);
            }

            // 캐릭터가 없는 경우 null을 반환
            if (characters.Count == 0)
            {
                onCompletion(null);
                return;
            }

            onCompletion(characters);
        }
        catch (Exception e)
        {
            print($"캐릭터 로드 중 오류 발생: {e.Message}");
            onCompletion(null);
        }
    }

    // 사용자 데이터 초기화
    public async Task InitializeUserData(string userId, string email, Action<bool> onCompletion)
    {
        var docRef = db.Collection("users").Document("email").Collection(email);
        var userSnapshot = await docRef.GetSnapshotAsync();

        if (userSnapshot == null)
        {
            print("사용자 데이터 초기화 실패");
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

    //  로그아웃
    public void SignOut()
    {
        if (auth.CurrentUser != null)
        {
            auth.SignOut();
            Debug.Log("로그아웃 성공");
        }
    }

    // 문서 필드 값 존재 여부 확인
    public void CheckFieldValueExists(string document, string userValue, System.Action<bool> onResult)
    {
        if (!IsFirebaseInitialized)
        {
            print("Firebase가 아직 초기화되지 않았음");
            onResult(false);
            return;
        }

        DocumentReference docRef = db.Collection("answer").Document(document);
        docRef.GetSnapshotAsync().ContinueWithOnMainThread(task =>
        {
            if (task.IsFaulted)
            {
                print("문서 조회 중 오류 발생");
                onResult(false);
                return;
            }

            var documentSnapshot = task.Result;
            if (!documentSnapshot.Exists)
            {
                print("korean 문서가 존재하지 않습니다.");
                onResult(false);
                return;
            }


            if (documentSnapshot.ContainsField(userValue)) // 사용자 입력 값이 필드로 존재하는지 확인
            {
                onResult(true); // print($"필드 '{userValue}'가 존재함");
            }
            else
            {
                onResult(false); // print($"필드 '{userValue}'가 존재하지 않음);
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