using Cinemachine;
using DG.Tweening;
using Event;
using Player.Events;
using Zenject;

namespace Player
{
    public class CameraController
    {
        private CinemachineVirtualCamera _vCam;
        private PlayerController _bindedPlayerController;
        private PlayerData _playerData;
        private CinemachinePOV _cinemachinePov;
        private Tweener _rotationTweener;

        [Inject]
        public void Inject(CinemachineVirtualCamera vCam, PlayerData playerData)
        {
            _vCam = vCam;
            _playerData = playerData;
        }

        public void Initialize(PlayerController playerController)
        {
            _bindedPlayerController = playerController;
            _vCam.m_Follow = _bindedPlayerController.transform;
            _vCam.m_LookAt = _bindedPlayerController.transform;
            _cinemachinePov = _vCam.GetCinemachineComponent<CinemachinePOV>();
            _cinemachinePov.m_VerticalAxis.Value = _playerData.CameraVerticalAxisValue;

            EventSystem.Subscribe<FinishLineReachedEvent>(OnFinishLineReached);
            EventSystem.Subscribe<GameStartRequestedEvent>(OnGameStartRequested);
        }


        public void Deinitialize()
        {
            EventSystem.Unsubscribe<FinishLineReachedEvent>(OnFinishLineReached);
            EventSystem.Unsubscribe<GameStartRequestedEvent>(OnGameStartRequested);
        }

        private void OnFinishLineReached(object obj)
        {
            StartRotatingAroundTarget();
        }

        private void OnGameStartRequested(object obj)
        {
            FocusCameraForGameplay();
        }

        private void StartRotatingAroundTarget()
        {
            _rotationTweener = DOVirtual.Float(0, 360, _playerData.CameraRotationDurationPerTurn, val => _cinemachinePov.m_HorizontalAxis.Value = val).SetLoops(-1, LoopType.Incremental);
        }

        private void FocusCameraForGameplay()
        {
            _rotationTweener.Kill();
            DOVirtual.Float(_cinemachinePov.m_HorizontalAxis.Value, 0, _playerData.CameraFocusDuration, val => _cinemachinePov.m_HorizontalAxis.Value = val);
        }
    }
}