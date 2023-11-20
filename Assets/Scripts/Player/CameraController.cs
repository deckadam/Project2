using Cinemachine;
using Zenject;

namespace Player
{
    public class CameraController
    {
        private CinemachineVirtualCamera _vCam;
        private PlayerController _bindedPlayerController;

        [Inject]
        public void Inject(CinemachineVirtualCamera vCam)
        {
            _vCam = vCam;
        }

        public void Initialize(PlayerController playerController)
        {
            _bindedPlayerController = playerController;
            _vCam.m_Follow = _bindedPlayerController.transform;
        }
    }
}