using Event;
using Player.Events;
using UnityEngine;

namespace InputSystem
{
    public class InputManager : MonoBehaviour
    {
        private void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                EventSystem.Raise(new BlockPlaceRequestedEvent());
            }
        }
    }
}