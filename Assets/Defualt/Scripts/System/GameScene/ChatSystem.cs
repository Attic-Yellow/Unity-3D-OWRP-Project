using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class ChatSystem : MonoBehaviourPunCallbacks
{
    [SerializeField] private GameObject chatPanel;
    [SerializeField] private TMP_InputField chatInputField;
    [SerializeField] private TextMeshProUGUI chatTextPrefab;
    [SerializeField] private PlayerInput playerInput;

    void Start()
    {
        chatInputField.onSelect.AddListener(_ => SwitchToChatInput());
        chatInputField.onDeselect.AddListener(_ => SwitchToGameplayInput());
        chatInputField.onEndEdit.AddListener(TrySendChatMessage);
    }

    private void TrySendChatMessage(string input)
    {
        if (!string.IsNullOrEmpty(input))
        {
            string filteredMessage = FilterBadWords(input.Trim());
            SendChatMessage(filteredMessage);
            chatInputField.text = ""; // 입력 필드 초기화
            chatInputField.DeactivateInputField();
        }
    }

    public void SendChatMessage(string message)
    {
        string fullMessage = $"{PhotonNetwork.NickName}: {message}"; // 닉네임과 메시지를 포함하여 전체 메시지를 포맷팅
        photonView.RPC("ReceiveMessage", RpcTarget.All, fullMessage);
    }

    [PunRPC]
    public void ReceiveMessage(string fullMessage, PhotonMessageInfo info)
    {
        AddMessage(fullMessage); // 화면에 메시지 추가
    }

    public void AddMessage(string message)
    {
        TMP_Text newMessage = Instantiate(chatTextPrefab, chatPanel.transform);
        newMessage.text = message;

        LayoutRebuilder.ForceRebuildLayoutImmediate((RectTransform)chatPanel.transform); // 콘텐츠 크기 갱신
        StartCoroutine(ScrollToBottom()); // 스크롤을 맨 아래로 이동
    }

    private IEnumerator ScrollToBottom()
    {
        yield return new WaitForEndOfFrame();
        chatPanel.GetComponentInParent<ScrollRect>().normalizedPosition = new Vector2(0, 0);
    }

    private string FilterBadWords(string message)
    {
        string badWordsPattern = @"시발|씨발|씨빨|ㅆㅃ|ㅅㅂ|ㅆㅂ|ㅅㅃ";
        return Regex.Replace(message, badWordsPattern, match => new string('*', match.Value.Length));
    }

    private void SwitchToChatInput()
    {
        // "UI" 액션 맵으로 전환합니다.
        playerInput.SwitchCurrentActionMap("Chat");
    }

    private void SwitchToGameplayInput()
    {
        // "Gameplay" 액션 맵으로 전환합니다.
        playerInput.SwitchCurrentActionMap("Player");
    }
}
