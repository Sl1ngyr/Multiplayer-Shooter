using System;
using Fusion;
using TMPro;
using UnityEngine;

namespace Wave
{
    public class TimerWaveController : NetworkBehaviour
    {
        [SerializeField] private TextMeshProUGUI _timerText;
        [SerializeField] private WaveController _waveController;
        
        private int time = 60;
        private float _breakTime;
        private float _waveTime;
        private bool _isStartWave = false;
        
        public Action EndWave;

        public bool IsStartWave => _isStartWave;
        
        public void Init(float breakTime, float waveTime)
        {
            _breakTime = breakTime;
            _waveTime = waveTime;
        }

        [Rpc]
        public void RPC_TimerStatusManagement(bool status)
        {
            gameObject.SetActive(status);
        }
        
        public override void FixedUpdateNetwork()
        {
            if(!_waveController.IsWaveLaunch) return;

            WaveTimeProcessing();
        }

        private void WaveTimeProcessing()
        {
            if (_breakTime == 0)
            {
                _isStartWave = true;
            }
            else
            {
                RPC_CalculateTime(_breakTime);
            }
            
            if(!_isStartWave) return;
            
            if (_waveTime == 0)
            {
                EndWave?.Invoke();
                _isStartWave = false;
            }
            else
            {
                RPC_CalculateTime(_waveTime);
            }
        }
        
        [Rpc]
        private void RPC_CalculateTime(float remainingTime)
        {
            float calculator = remainingTime;

            calculator -= Runner.DeltaTime;

            int minutes = Mathf.FloorToInt(calculator / time);
            int seconds = Mathf.FloorToInt(calculator % time);

            _timerText.text = string.Format("{0:00}:{1:00}", minutes.ToString(), seconds.ToString());

            if (remainingTime == _breakTime)
            {
                _breakTime = calculator;
            }
            else
            {
                _waveTime = calculator;
            }
            
        }
        
    }
}