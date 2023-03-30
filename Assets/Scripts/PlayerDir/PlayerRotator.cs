using System;
using UnityEngine;

namespace PlayerDir
{
    [Serializable]
    public class PlayerRotator
    {
        [SerializeField] private Transform camTransform;
        [SerializeField] private float sensitivity;
        [SerializeField] private Vector2 yBounds;

        private Transform playerTransform;
        private Vector2 lastTouchPosition;
        private float rotationX;
        private float rotationY;
        private bool isTouching;
        private int touchFingerId;

        public Touch CurrentTouch
        {
            get
            {
                foreach (var touch in Input.touches)
                {
                    if (touch.fingerId == touchFingerId)
                    {
                        return touch;
                    }
                }

                Debug.LogError("Unexpected");
                return default;
            }
        }

        public void Init(Player player)
        {
            playerTransform = player.Transform;
            var gameUI = SceneC.Instance.UIHolder.GameUI;
            gameUI.OnDragTriggerDown += OnDragPointerDown;
            gameUI.OnDragTriggerUp += OnDragPointerUp;
        }

        private void OnDragPointerDown()
        {
            if(isTouching) return;
            isTouching = true;

            if (Input.touchSupported)
            {
                foreach (var touch in Input.touches)
                {
                    if (touch.phase == TouchPhase.Began)
                    {
                        touchFingerId = touch.fingerId;
                        lastTouchPosition = touch.position;
                        break;
                    }
                }
            }
            else
            {
                lastTouchPosition = Input.mousePosition;
            }
        }

        private void OnDragPointerUp()
        {
            if (Input.touchSupported)
            {
                if (CurrentTouch.phase is TouchPhase.Ended or TouchPhase.Canceled)
                {
                    isTouching = false;
                }
            }
            else
            {
                isTouching = false;
            }
        }

        public void UpdateRotation()
        {
            if(!isTouching) return;

            Vector2 newTouchPosition = Input.touchSupported
                ? CurrentTouch.position
                : Input.mousePosition;
            var deltaTouchPosition = newTouchPosition - lastTouchPosition;

            rotationX += deltaTouchPosition.x * sensitivity;
            rotationY -= deltaTouchPosition.y * sensitivity;
            rotationY = Mathf.Clamp(rotationY, yBounds.x, yBounds.y);

            camTransform.localEulerAngles =
                new Vector3(rotationY, camTransform.localEulerAngles.y, 0);
            playerTransform.localEulerAngles
                = new Vector3(playerTransform.localEulerAngles.x, rotationX, 0);

            lastTouchPosition = newTouchPosition;
        }
    }
}
