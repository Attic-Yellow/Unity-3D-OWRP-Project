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
            chatInputField.text = ""; // �Է� �ʵ� �ʱ�ȭ
            chatInputField.DeactivateInputField();
        }
    }

    public void SendChatMessage(string message)
    {
        string fullMessage = $"{PhotonNetwork.NickName}: {message}"; // �г��Ӱ� �޽����� �����Ͽ� ��ü �޽����� ������
        photonView.RPC("ReceiveMessage", RpcTarget.All, fullMessage);
    }

    [PunRPC]
    public void ReceiveMessage(string fullMessage, PhotonMessageInfo info)
    {
        AddMessage(fullMessage); // ȭ�鿡 �޽��� �߰�
    }

    public void AddMessage(string message)
    {
        TMP_Text newMessage = Instantiate(chatTextPrefab, chatPanel.transform);
        newMessage.text = message;

        LayoutRebuilder.ForceRebuildLayoutImmediate((RectTransform)chatPanel.transform); // ������ ũ�� ����
        StartCoroutine(ScrollToBottom()); // ��ũ���� �� �Ʒ��� �̵�
    }

    private IEnumerator ScrollToBottom()
    {
        yield return new WaitForEndOfFrame();
        chatPanel.GetComponentInParent<ScrollRect>().normalizedPosition = new Vector2(0, 0);
    }

    private string FilterBadWords(string message)
    {
        string badWordsPattern = @"�ù�|����|����|����|����|����|����";
        return Regex.Replace(message, badWordsPattern, match => new string('*', match.Value.Length));
    }

    private void SwitchToChatInput()
    {
        // "UI" �׼� ������ ��ȯ�մϴ�.
        playerInput.SwitchCurrentActionMap("Chat");
    }

    private void SwitchToGameplayInput()
    {
        // "Gameplay" �׼� ������ ��ȯ�մϴ�.
        playerInput.SwitchCurrentActionMap("Player");
    }
}
