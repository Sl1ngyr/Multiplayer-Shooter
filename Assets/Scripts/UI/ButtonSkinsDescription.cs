using UnityEngine;

namespace UI
{
    public class ButtonSkinsDescription : MonoBehaviour
    {
        [SerializeField] private string _skinName;
        
        public string SkinName => _skinName;
    }
}