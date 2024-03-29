using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class test : MonoBehaviour
{
    public void PrintCurrentRoomPlayers()
    {
        if (PhotonNetwork.InRoom)
        {
            foreach (var player in PhotonNetwork.PlayerList)
            {
                Debug.Log($"Player ID: {player.UserId}, Name: {player.NickName}");
            }
        }
        else
        {
            Debug.Log("���� �濡 �������� �ʽ��ϴ�.");
        }
    }
}
