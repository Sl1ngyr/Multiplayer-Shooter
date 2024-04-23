using CameraComponents;
using Fusion;
using UnityEngine;

namespace Player
{
    public class PlayerCameraController : NetworkBehaviour
    {
        private Camera _camera;

        public override void Spawned()
        {
            if (Object.HasInputAuthority)
            {
                _camera = Camera.main;
                
                _camera.GetComponent<FollowCamera>().CameraAnchorPoint = transform;
            }
        }
        
    }
}