using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using Game.Scripts.UI;

namespace Game.Scripts.LiveObjects
{
    public class Drone : MonoBehaviour
    {
        private enum Tilt
        {
            NoTilt, Forward, Back, Left, Right
        }

        [SerializeField]
        private Rigidbody _rigidbody;
        [SerializeField]
        private float _speed = 5f;
        [SerializeField] private float _turningSpeed = 25f;
        [SerializeField] private float _rotationSpeed = 25f;
        private bool _inFlightMode = false;
        [SerializeField]
        private Animator _propAnim;
        [SerializeField]
        private CinemachineVirtualCamera _droneCam;
        [SerializeField]
        private InteractableZone _interactableZone;

        private bool _isDroneAccessible = false;
        private bool _beenInDroneState = false;

        [SerializeField] private DroneInputManager droneInputManager;// THIS LINE ADDED
        

        public static event Action OnEnterFlightMode;
        public static event Action onExitFlightmode;

        private void OnEnable()
        {
            InteractableZone.onZoneInteractionComplete += EnterFlightMode1stTime;
        }

        // LEGACY INPUT MANAGER
        // private void EnterFlightMode(InteractableZone zone)
        // {
        //     if (_inFlightMode != true && zone.GetZoneID() == 4) // drone Scene
        //     {
        //         _propAnim.SetTrigger("StartProps");
        //         _droneCam.Priority = 11;
        //         _inFlightMode = true;
        //         OnEnterFlightMode?.Invoke();
        //         UIManager.Instance.DroneView(true);// escape
        //         _interactableZone.CompleteTask(4);
        //         _isDroneAccessible = true;
        //     }
        // }

        // CHANGED AFTER NEW INPUT MANAGER
        private void EnterFlightMode1stTime(InteractableZone zone)
        {
            if (_inFlightMode != true && zone.GetZoneID() == 4) // drone Scene
            {
                _propAnim.SetTrigger("StartProps");
                _droneCam.Priority = 11;
                _inFlightMode = true;
                OnEnterFlightMode?.Invoke();
                UIManager.Instance.DroneView(true);
                _interactableZone.CompleteTask(4);
                _isDroneAccessible = true;
            }
        }

        // NEW INPUT MANAGER
        public void EnterFlightMode()
        {
            if (_isDroneAccessible)
            {
                _propAnim.SetTrigger("StartProps");
                _droneCam.Priority = 11;
                _inFlightMode = true;
                OnEnterFlightMode?.Invoke();
                droneInputManager.EnableInputManager();
                UIManager.Instance.DroneView(true);
            }
        }

        // NEW INPUT MANAGER
        public void ExitFlightMode()
        {            
            if (_beenInDroneState == false)
            {
                UIManager.Instance.EnableDroneSwitch_UI();
            }
            _droneCam.Priority = 9;
            _inFlightMode = false;
            _beenInDroneState = true;
            UIManager.Instance.DroneView(false);            
        }

        // CHANGED AFTER NEW INPUT MANAGER
        public void EscapeFlightMode()
        {
            if (_inFlightMode)
            {
                //CalculateTilt();
                //CalculateMovementUpdate(); // THESE LINES ARE REMOVED

                _inFlightMode = false;
                onExitFlightmode?.Invoke();
                droneInputManager.DisableInputManager();
                ExitFlightMode();
            }
        }

        private void FixedUpdate()
        {
            _rigidbody.AddForce(transform.up * (9.81f), ForceMode.Acceleration);
            // if (_inFlightMode)
            //     CalculateMovementFixedUpdate();// THESE LINES ARE REMOVED
        }
        
        // LEGACY INPUT MANAGER
        // private void CalculateMovementUpdate()
        // {
        //     if (Input.GetKey(KeyCode.LeftArrow))
        //     {
        //         var tempRot = transform.localRotation.eulerAngles;
        //         tempRot.y -= _speed / 3;
        //         transform.localRotation = Quaternion.Euler(tempRot);
        //     }
        //     if (Input.GetKey(KeyCode.RightArrow))
        //     {
        //         var tempRot = transform.localRotation.eulerAngles;
        //         tempRot.y += _speed / 3;
        //         transform.localRotation = Quaternion.Euler(tempRot);
        //     }
        // }

        // NEW INPUT MANAGER
        public void CalculateMovementUpdate(float direction)
        {
            var tempRot = transform.localRotation.eulerAngles;
                tempRot.y += direction * _turningSpeed * Time.deltaTime;
                transform.localRotation = Quaternion.Euler(tempRot);
        }

        // LEGACY INPUT MANAGER
        // private void CalculateMovementFixedUpdate()
        // {
            
        //     if (Input.GetKey(KeyCode.Space))
        //     {
        //         _rigidbody.AddForce(transform.up * _speed, ForceMode.Acceleration);
        //     }
        //     if (Input.GetKey(KeyCode.V))
        //     {
        //         _rigidbody.AddForce(-transform.up * _speed, ForceMode.Acceleration);
        //     }
        // }

        // NEW INPUT MANAGER
        public void Thrust(float verticalDirection)
        {
            
            if (_inFlightMode)
            {
                _rigidbody.AddForce(transform.up * _speed * verticalDirection, ForceMode.Acceleration);
            }
        }

        // LEGACY INPUT MANAGER
        // private void CalculateTilt()
        // {
        //     if (Input.GetKey(KeyCode.A)) 
        //         transform.rotation = Quaternion.Euler(00, transform.localRotation.eulerAngles.y, 30);
        //     else if (Input.GetKey(KeyCode.D))
        //         transform.rotation = Quaternion.Euler(0, transform.localRotation.eulerAngles.y, -30);
        //     else if (Input.GetKey(KeyCode.W))
        //         transform.rotation = Quaternion.Euler(30, transform.localRotation.eulerAngles.y, 0);
        //     else if (Input.GetKey(KeyCode.S))
        //         transform.rotation = Quaternion.Euler(-30, transform.localRotation.eulerAngles.y, 0);
        //     else 
        //         transform.rotation = Quaternion.Euler(0, transform.localRotation.eulerAngles.y, 0);
        // }

        // NEW INPUT MANAGER
        public void TiltDrone(Vector2 tilt)
        {
            transform.rotation = Quaternion.Euler(tilt.y * _rotationSpeed, transform.localRotation.eulerAngles.y, tilt.x * _rotationSpeed * -1);
        }

        private void OnDisable()
        {
            InteractableZone.onZoneInteractionComplete -= EnterFlightMode1stTime;
        }
    }
}
