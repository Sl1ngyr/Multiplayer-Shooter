using TMPro;
using UnityEngine;

namespace UI
{
    public class PlayerResultData : MonoBehaviour
    {
        private TextMeshProUGUI PlayerName;
        private TextMeshProUGUI PlayerKills;
        private TextMeshProUGUI PlayerDamage;

        public void Init(string name, string kills, string damage)
        {
            PlayerName.text = name;
            PlayerKills.text = kills;
            PlayerDamage.text = damage;
        }
    }
}