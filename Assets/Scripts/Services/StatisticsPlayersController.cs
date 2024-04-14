using Fusion;
using UI;
using UnityEngine;

namespace Services
{
    public class StatisticsPlayersController : NetworkBehaviour
    {
        [SerializeField] private TablePlayersResult _tablePlayersResult;
        
        
        [Rpc]
        public void RPC_SetStatisticsPlayersDataToUI(int[] playerKey, int[] playerkills, int[] playerDamage)
        {
            _tablePlayersResult.GetComponent<TablePlayersResult>().SetResultData(Runner.SessionInfo.PlayerCount, playerKey, playerkills, playerDamage);
        }
    }
}