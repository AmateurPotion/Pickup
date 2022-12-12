using System;
using Pickup.Contents.Items;
using Pickup.Contents.Items.Equipments;
using Pickup.Utils;
using Pickup.Utils.Override;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Pickup.Players
{
    public sealed class Controller : MonoBehaviour
    {
        [Header("Components")]
        public PlayerManager manager;
        [SerializeField] private Camera mainCamera;
        
        // base events listener
        private void Update()
        {
            if (inMoveOperation)
            {
                manager.player.Move(moveDirection);
            }
        }

        private void FixedUpdate()
        {
            if (mainCamera)
            {
                worldMousePoint =
                    mainCamera.ScreenToWorldPoint(new Vector3(screenMousePoint.x, screenMousePoint.y, transform.position.z - mainCamera.transform.position.z));
                viewportMousePoint = mainCamera.ScreenToViewportPoint(screenMousePoint);
            }
        }


        // Input Action Events listener
        [SerializeField] private bool inMoveOperation = false;
        [SerializeField] private Vector2 moveDirection = Vector2.zero;
        private void OnMove (InputValue value)
        {
            if(!manager.player) return;
            
            moveDirection = value.Get<Vector2>();
            inMoveOperation = moveDirection != Vector2.zero;
            
            // rotation
            if (moveDirection != Vector2.zero)
            {
                var rotate = Vector2.zero.GetAngle(moveDirection) - 90;
                manager.player.transform.localEulerAngles = new Vector3(0, 0, rotate);
            }
        }

        public Vector2 screenMousePoint;
        public Vector2 viewportMousePoint;
        public Vector2 worldMousePoint;
        private void OnPoint(InputValue value)
        {
            screenMousePoint = value.Get<Vector2>();
        }

        [SerializeField] private float cameraEnlargeSpeed = 2;
        private void OnScrollWheel(InputValue value)
        {
            if (Input.GetKey(KeyCode.LeftControl))
            {
                var direction = value.Get<Vector2>().y;

                switch (direction)
                {
                    case > 0 : 
                        PlayerManager.Instance.virtualCamera.m_Lens.FieldOfView += cameraEnlargeSpeed;
                        break;
                    case < 0 :
                        PlayerManager.Instance.virtualCamera.m_Lens.FieldOfView -= cameraEnlargeSpeed;
                        break;
                }

                int b = 1, a =2;
                switch ((a, b))
                {
                    case (> 0, > 1):
                    {
                        break;
                    }
                }
            }
        }
    }
}