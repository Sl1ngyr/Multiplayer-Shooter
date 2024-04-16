using TMPro;
using UnityEngine;

namespace UI
{
    public class PlayerResultData : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI PlayerName;
        [SerializeField] private TextMeshProUGUI PlayerKills;
        [SerializeField] private TextMeshProUGUI PlayerDamage;

        public void Init(string name, string kills, string damage)
        {
            PlayerName.text += name;
            PlayerKills.text = kills;
            PlayerDamage.text = damage;
        }
    }
}