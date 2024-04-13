using System.Collections.Generic;
using Fusion;

namespace Services
{
    public class StatisticsPlayersData : NetworkBehaviour
    {
        private List<int> _playersKey = new List<int>();
        private Dictionary<int, int> _playersDamage = new Dictionary<int, int>();
        private Dictionary<int, int> _playersKills = new Dictionary<int, int>();

        public void InitPlayers(int id)
        {
            _playersKey.Add(id);
            _playersDamage.Add(id, 0);
            _playersKills.Add(id, 0);
        }

        public void AddPlayerDamageToData(int id, int damage)
        {
            _playersDamage[id] += damage;
        }
        
        public void AddPlayerKillsToData(int id)
        {
            _playersKills[id] += 1;
        }
        
        
    }
}