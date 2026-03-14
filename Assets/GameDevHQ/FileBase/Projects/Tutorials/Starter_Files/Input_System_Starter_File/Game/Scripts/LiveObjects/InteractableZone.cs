using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game.Scripts.UI;
using UnityEngine.InputSystem;


namespace Game.Scripts.LiveObjects
{
    public class InteractableZone : MonoBehaviour
    {
        private enum ZoneType
        {
            Collectable,
            Action,
            HoldAction
        }

        private enum KeyState
        {
            Press,
            PressHold
        }

        [SerializeField]
        private ZoneType _zoneType;
        [SerializeField]
        private int _zoneID;
        [SerializeField]
        private int _requiredID;
        [SerializeField]
        [Tooltip("Press the (---) Key to .....")]
        private string _displayMessage;
        [SerializeField]
        private GameObject[] _zoneItems;
        private bool _inZone = false;
        private bool _itemsCollected = false;
        private bool _actionPerformed = false;
        [SerializeField]
        private Sprite _inventoryIcon;
        [SerializeField]
        private KeyCode _zoneKeyInput;
        [SerializeField]
        private KeyState _keyState;
        [SerializeField]
        private GameObject _marker;
        private static int _currentZoneID = 0;
        public static int CurrentZoneID
        { 
            get 
            { 
               return _currentZoneID; 
            }
            set
            {
                _currentZoneID = value; 
                         
            }
        }

        [SerializeField] private Crate _crate;


        public static event Action<InteractableZone> onZoneInteractionComplete;
        public static event Action<int> onHoldStarted;

        private AllInputActions _input;

        // NEW INPUT SYSTEM
        void Start()
        {
            InitializeInputs();
        }

        private void InitializeInputs()
        {
            _input = new AllInputActions();
            _input.Player.Enable();
            if (_zoneID == 6)
            {
                _input.Player.BrakeCrate.performed += BrakeCrate_performed;
                _input.Player.BrakeCrate.canceled += BrakeCrate_canceled;
                return;
            }
            if (_zoneID == 2)
            {
                _input.Player.BlowUpTaxi.performed += InteractableEvent_performed;
                return;
            }
            _input.Player.InteractableEvent.performed += InteractableEvent_performed;
            _input.Player.HackCameras.performed += HackCameras_performed;
        }
        // NEW INPUT SYSTEM
        void InteractableEvent_performed(InputAction.CallbackContext context)
        {
            StartInteractableAction();
        }
        // NEW INPUT SYSTEM
        void HackCameras_performed(InputAction.CallbackContext context)
        {
            StartInteractableActionByHolding();
        }
        private void OnEnable()
        {
            InteractableZone.onZoneInteractionComplete += SetMarker;//
        }

        private void BrakeCrate_performed(InputAction.CallbackContext context)// ADDED
        {
            if (_inZone == false)
            {
                return;
            }
            _crate.Break(5);
        }

        private void BrakeCrate_canceled(InputAction.CallbackContext context)// ADDED
        {
            if (_inZone == false)
            {
                return;
            }
            _crate.Break((int)context.duration + 1);
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player") && _currentZoneID > _requiredID)
            {
                switch (_zoneType)
                {
                    case ZoneType.Collectable:
                        if (_itemsCollected == false)
                        {
                            _inZone = true;
                            if (_displayMessage != null)
                            {
                                string message = $"Press the {_zoneKeyInput.ToString()} key to {_displayMessage}.";
                                UIManager.Instance.DisplayInteractableZoneMessage(true, message);
                            }
                            else
                                UIManager.Instance.DisplayInteractableZoneMessage(true, $"Press the {_zoneKeyInput.ToString()} key to collect");
                        }
                        break;

                    case ZoneType.Action:
                        if (_actionPerformed == false)
                        {
                            _inZone = true;
                            if (_displayMessage != null)
                            {
                                string message = $"Press the {_zoneKeyInput.ToString()} key to {_displayMessage}.";
                                UIManager.Instance.DisplayInteractableZoneMessage(true, message);
                            }
                            else
                                UIManager.Instance.DisplayInteractableZoneMessage(true, $"Press the {_zoneKeyInput.ToString()} key to perform action");
                        }
                        break;

                    case ZoneType.HoldAction:
                        _inZone = true;
                        if (_displayMessage != null)
                        {
                            string message = $"Press the {_zoneKeyInput.ToString()} key to {_displayMessage}.";
                            UIManager.Instance.DisplayInteractableZoneMessage(true, message);
                        }
                        else
                            UIManager.Instance.DisplayInteractableZoneMessage(true, $"Hold the {_zoneKeyInput.ToString()} key to perform action");
                        break;
                }
            }
        }
        // LEGACY INPUT SYSTEM
        // private void Update()
        // {
        //     if (_inZone == true)
        //     {

        //         if (Input.GetKeyDown(_zoneKeyInput) && _keyState != KeyState.PressHold)
        //         {
        //             //press
        //             switch (_zoneType)
        //             {
        //                 case ZoneType.Collectable:
        //                     if (_itemsCollected == false)
        //                     {
        //                         CollectItems();
        //                         _itemsCollected = true;
        //                         UIManager.Instance.DisplayInteractableZoneMessage(false);
        //                     }
        //                     break;

        //                 case ZoneType.Action:
        //                     if (_actionPerformed == false)
        //                     {
        //                         PerformAction();
        //                         _actionPerformed = true;
        //                         UIManager.Instance.DisplayInteractableZoneMessage(false);
        //                     }
        //                     break;
        //             }
        //         }
        //         else if (Input.GetKey(_zoneKeyInput) && _keyState == KeyState.PressHold && _inHoldState == false)
        //         {
        //             _inHoldState = true;

                   

        //             switch (_zoneType)
        //             {                      
        //                 case ZoneType.HoldAction:
        //                     PerformHoldAction();
        //                     break;           
        //             }
        //         }

        //         if (Input.GetKeyUp(_zoneKeyInput) && _keyState == KeyState.PressHold)
        //         {
        //             _inHoldState = false;
        //             onHoldEnded?.Invoke(_zoneID);
        //         }

               
        //     }
        // }

        // FOR NEW INPUT SYSTEM
        public void StartInteractableAction() // CHANGED
        {
            if (_inZone == true)
            {
                switch (_zoneType)
                    {
                        case ZoneType.Collectable:
                            if (_itemsCollected == false)
                            {
                                CollectItems();
                                _itemsCollected = true;
                                UIManager.Instance.DisplayInteractableZoneMessage(false);
                            }
                            break;

                        case ZoneType.Action:
                            if (_actionPerformed == false)
                            {
                                PerformAction();
                                _actionPerformed = true;
                                UIManager.Instance.DisplayInteractableZoneMessage(false);
                            }
                            break;
                    }
            }
        }
        // FOR NEW INPUT SYSTEM
        public void StartInteractableActionByHolding() // CHANGED
        {
            if (_inZone == true)
            {
                switch (_zoneType)
                    {                      
                        case ZoneType.HoldAction:
                            PerformHoldAction();
                            break;           
                    }
            }
        }
       
        private void CollectItems()
        {
            foreach (var item in _zoneItems)
            {
                item.SetActive(false);
            }

            UIManager.Instance.UpdateInventoryDisplay(_inventoryIcon);

            CompleteTask(_zoneID);

            onZoneInteractionComplete?.Invoke(this);

        }

        private void PerformAction()
        {
            foreach (var item in _zoneItems)
            {
                item.SetActive(true);
            }

            if (_inventoryIcon != null)
                UIManager.Instance.UpdateInventoryDisplay(_inventoryIcon);

            onZoneInteractionComplete?.Invoke(this);
        }

        private void PerformHoldAction()
        {
            UIManager.Instance.DisplayInteractableZoneMessage(false);
            onHoldStarted?.Invoke(_zoneID);
        }

        public GameObject[] GetItems()
        {
            return _zoneItems;
        }

        public int GetZoneID()
        {
            return _zoneID;
        }

        public void CompleteTask(int zoneID)
        {
            if (zoneID == _zoneID)
            {
                _input.Player.Disable();// A CHANGE
                _currentZoneID++;
                onZoneInteractionComplete?.Invoke(this);
            }
        }

        public void ResetAction(int zoneID)
        {
            if (zoneID == _zoneID)
                _actionPerformed = false;
        }

        public void SetMarker(InteractableZone zone)
        {
            if (_zoneID == _currentZoneID)
                _marker.SetActive(true);
            else
                _marker.SetActive(false);
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                _inZone = false;
                UIManager.Instance.DisplayInteractableZoneMessage(false);
            }
        }

        private void OnDisable()
        {
            InteractableZone.onZoneInteractionComplete -= SetMarker;

            // CHANGES
            if (_input != null)
            {
                _input.Player.Disable();
            }
        }
        private void OnDestroy()// ADDED
        {
            if (_input != null)
            {
                _input.Player.Disable();//
            }
        }

    }
}


