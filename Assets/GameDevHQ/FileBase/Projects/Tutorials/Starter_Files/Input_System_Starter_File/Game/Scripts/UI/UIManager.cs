using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Game.Scripts.UI
{
    public class UIManager : MonoBehaviour
    {
        #region Singleton
        private static UIManager _instance;
        public static UIManager Instance
        {
            get
            {
                if (_instance == null)
                    Debug.LogError("UI Manager is NULL.");

                return _instance;
            }
        }
        #endregion

        [SerializeField]
        private Text _interactableZone;
        [SerializeField]
        private Image _inventoryDisplay;
        [SerializeField]
        private RawImage _droneCamView;

        [SerializeField] TextMeshProUGUI _droneSwitch_UI;

        private void Awake()
        {
            _instance = this;//input
        }

        public void DisplayInteractableZoneMessage(bool showMessage, string message = null)
        {
            _interactableZone.text = message;
            _interactableZone.gameObject.SetActive(showMessage);
        }

        public void UpdateInventoryDisplay(Sprite icon)
        {            
            _inventoryDisplay.sprite = icon;
        }

        public void DroneView(bool Active)
        {
            _droneCamView.enabled = Active;
        }

        // ADDED
        public void EnableDroneSwitch_UI()
        {
            _droneSwitch_UI.gameObject.SetActive(true);
            StartCoroutine(DisableDroneSwitch_UI());
        }

        // ADDED
        IEnumerator DisableDroneSwitch_UI()
        {
            yield return new WaitForSeconds(9f);
            _droneSwitch_UI.gameObject.SetActive(false);
        }

    }
}

