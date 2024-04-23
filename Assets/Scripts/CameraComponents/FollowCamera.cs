using UnityEngine;

namespace CameraComponents
{
    public class FollowCamera : MonoBehaviour
    {
        private Transform _cameraAnchorPoint;

        public Transform CameraAnchorPoint
        {
            get => _cameraAnchorPoint;
            set => _cameraAnchorPoint = value;
        }
        
        private void LateUpdate()
        {
            if(_cameraAnchorPoint == null) return;
            
            transform.position = new Vector3(_cameraAnchorPoint.position.x, _cameraAnchorPoint.position.y,
                transform.position.z);
        }
    }
}