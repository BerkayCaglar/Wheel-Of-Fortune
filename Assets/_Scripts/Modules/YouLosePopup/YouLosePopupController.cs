using Cysharp.Threading.Tasks;
using DG.Tweening;
using Modules.RewardSystem.Enum;
using UnityEngine;
using UnityEngine.UI;
using WheelOfFortune.InventorySystem;
using WheelOfFortune.Modules.EventSystem;

namespace Modules.YouLosePopup
{
    public class YouLosePopupController : MonoBehaviour
    {
        [SerializeField] private RectTransform _popup;
        [SerializeField] private Button _reviveButton;
        [SerializeField] private Button _giveUpButton;

        private void Awake()
        {
            _popup.localScale = Vector3.zero;
            EventManager.OnShowYouLosePopup += Init;
        }

        private void Start()
        {
            _reviveButton.onClick.AddListener(OnReviveButtonClicked);
            _giveUpButton.onClick.AddListener(OnGiveUpButtonClicked);
        }

        private void OnDestroy()
        {
            EventManager.OnShowYouLosePopup -= Init;
        }

        private async void Init()
        {
            if (!InventoryManager.HasAmount(ERewardType.Cash, 200)) { _reviveButton.interactable = false; }
            await Show();
        }

        public async void OnReviveButtonClicked()
        {
            await Disable();
            EventManager.InvokeReviveButtonClicked();
        }

        public async void OnGiveUpButtonClicked()
        {
            await Disable();
            EventManager.InvokeGiveUpButtonClicked();
        }

        private async UniTask Show()
        {
            await _popup.DOScale(Vector3.one, 0.5f).SetEase(Ease.OutBack).AsyncWaitForCompletion();
        }

        private async UniTask Disable()
        {
            await _popup.DOScale(Vector3.zero, 0.5f).SetEase(Ease.InBack).AsyncWaitForCompletion();
        }
    }
}
