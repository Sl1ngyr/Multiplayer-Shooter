using UnityEngine;

namespace UI
{
    public class LoadingView : MonoBehaviour
    {
        [SerializeField] private GameObject _loadingComponents;
        
        public void LoadingStatusManagement(bool status)
        {
            _loadingComponents.SetActive(status);
        }
    }
}