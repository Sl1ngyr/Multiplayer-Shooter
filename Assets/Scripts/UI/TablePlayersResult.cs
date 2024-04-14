﻿using System.Collections.Generic;
using UnityEngine;

namespace UI
{
    public class TablePlayersResult : MonoBehaviour
    {
        [SerializeField] private List<PlayerResultData> _playerResultDatas;
        [SerializeField] private GameObject _playersDataUI;
        
        public void SetResultData(int playerCount, int[] playerKey, int[] playerkills, int[] playerDamage) 
        {
            _playersDataUI.gameObject.SetActive(true);
            for (int i = 0; i < playerCount; i++)
            {
                _playerResultDatas[i].PlayerName.text += playerKey[i];
                _playerResultDatas[i].PlayerKills.text = playerkills[i].ToString();
                _playerResultDatas[i].PlayerDamage.text = playerDamage[i].ToString();
                
            }
        }
    }
}