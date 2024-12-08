using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using Modules.WheelOfFortuneSystem.Managers;
using Modules.WheelOfFortuneSystem.Architecture.Data;
using NaughtyAttributes;
using System.Linq;
using WheelOfFortuneSystem.Architecture.Enum;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using Utilities;
using WheelOfFortune.Modules.EventSystem;
using WheelOfFortune.Modules.RewardSystem.Data;
using TMPro;
using Modules.RewardSystem.Enum;

namespace Modules.WheelOfFortuneSystem.Controllers
{
    public class WheelOfFortuneController : MonoBehaviour
    {
        [Header("UI Elements")]
        [SerializeField] private RectTransform _wofPanel;
        [SerializeField] private Image _wofBG;

        [Space]
        [SerializeField] private TextMeshProUGUI _wheelTypeText;
        [SerializeField] private TextMeshProUGUI _wheelUpToText;

        [Space]
        [SerializeField] private Button _giveUpButton;

        [SerializeField] private RectTransform _wofItemsHolder;
        [SerializeField] private Button _spinButton;
        [SerializeField] private GameObject _wofConffeti;

        [Space]
        [SerializeField] private RectTransform _wofArrow;
        [SerializeField] private Image _wofArrowImage;

        [Header("Resources")]
        [SerializeField] private Sprite _wofGoldenBG;
        [SerializeField] private Sprite _wofSilverBG;
        [SerializeField] private Sprite _wofBronzeBG;
        [Space]
        [SerializeField] private Sprite _wofGoldenArrow;
        [SerializeField] private Sprite _wofSilverArrow;
        [SerializeField] private Sprite _wofBronzeArrow;

        private List<WheelOfFortuneItemController> _wofItems;

        private List<Reward> _currentRewards;
        private int _currentSpinCount;
        private int _currentRewardIndex;

        private EWOFState _wofState;
        private EWOFType _wofType;

        [Button("Test")]
        private async void Test()
        {
            Init();
            await SetWheel();
        }

        private async void Awake()
        {
            Init();
            await SetWheel();
        }

        private void OnDestroy()
        {
            EventManager.OnRevivePlayer -= async () => await SetWheel();
            EventManager.OnResetPlayer -= async () => await ResetWheel();
        }

        private void Init()
        {
            _spinButton.onClick.RemoveAllListeners();
            _spinButton.onClick.AddListener(SpinWheel);
            _wofItems = _wofItemsHolder.GetComponentsInChildren<WheelOfFortuneItemController>(true).ToList();
            _wofConffeti.SetActive(false);
            EventManager.OnRevivePlayer += async () => await SetWheel();
            EventManager.OnResetPlayer += async () => await ResetWheel();
        }

        private async UniTask ResetWheel()
        {
            _currentSpinCount = 0;
            _currentRewardIndex = 0;
            await SetWheel();
        }

        private async UniTask SetWheel()
        {
            _wofState = EWOFState.Initializing;
            AnimateSpinButton(EAnimationType.Initial);
            SetModel();
            SetView();
            await SetWOFItems();
            SetGiveUpButton(true);
            AnimateSpinButton(EAnimationType.Result);
            _wofState = EWOFState.Initialized;
        }

        private void SetModel()
        {
            _wofType = WheelOfFortuneManager.GetWOFType(_currentSpinCount);
            _currentRewardIndex = Random.Range(0, _wofItems.Count);
            _currentRewards = WheelOfFortuneManager.CalculateRewards(_currentSpinCount, _wofItems.Count, _wofType);
        }

        private async void SetView()
        {
            var differentRewardsCount = _currentRewards.Where(r => r.Type != ERewardType.Bomb)
                .GroupBy(r => r.Type)
                .Count();

            switch (_wofType)
            {
                case EWOFType.Bronze:
                    _wofBG.sprite = _wofBronzeBG;
                    _wofArrowImage.sprite = _wofBronzeArrow;
                    _wheelTypeText.text = "Spin!";
                    break;
                case EWOFType.Silver:
                    _wofBG.sprite = _wofSilverBG;
                    _wofArrowImage.sprite = _wofSilverArrow;
                    _wheelTypeText.text = "Silver Spin!";
                    break;
                case EWOFType.Gold:
                    _wofBG.sprite = _wofGoldenBG;
                    _wofArrowImage.sprite = _wofGoldenArrow;
                    _wheelTypeText.text = "Gold Spin!";
                    break;
            }

            await _wofPanel.DOPunchScale(new Vector3(0.1f, 0.1f, 0.1f), 0.2f, 10, 1).SetEase(Ease.OutQuart).AsyncWaitForCompletion();
            await _wheelTypeText.transform.DOPunchScale(new Vector3(0.1f, 0.1f, 0.1f), 0.5f, 10, 1).SetEase(Ease.OutQuart).AsyncWaitForCompletion();
            _wheelUpToText.text = $"Up to {differentRewardsCount} different rewards!";
            await _wheelUpToText.transform.DOPunchScale(new Vector3(0.1f, 0.1f, 0.1f), 0.2f, 10, 1).SetEase(Ease.OutQuart).AsyncWaitForCompletion();
        }

        private async UniTask SetWOFItems()
        {
            for (int i = 0; i < _wofItems.Count; i++)
            {
                var item = _wofItems[i];
                if (item.isActiveAndEnabled)
                    await item.PlayAnimation(EAnimationType.Out);
                item.Init(_currentRewards[i], i);
            }

            var tempItems = _wofItems.ToList();
            tempItems.Shuffle();

            foreach (var item in tempItems)
            {
                await item.PlayAnimation(EAnimationType.Initial);
                ShakeSpin();
            }
        }

        private void SpinWheel()
        {
            if (_wofState != EWOFState.Initialized) return;
            if (_wofState == EWOFState.Spinning || _wofState == EWOFState.Result) return;

            _wofState = EWOFState.Spinning;
            _wofConffeti.SetActive(false);

            AnimateSpinButton(EAnimationType.Spin);

            _currentSpinCount++;

            Spin();
        }

        private void Spin()
        {
            var resultIndex = _currentRewardIndex;
            var resultItem = _wofItems[resultIndex];

            float anglePerItem = 360f / _wofItems.Count;
            float finalAngle = (4 * 360f) + (resultIndex * anglePerItem);
            var rotationDuration = 5f;

            SetGiveUpButton(false);

            _wofPanel.DORotate(new Vector3(0, 0, finalAngle), rotationDuration, RotateMode.FastBeyond360).SetEase(Ease.OutQuart).OnComplete(async () =>
            {
                _wofState = EWOFState.Result;
                await resultItem.PlayAnimation(EAnimationType.Result);
                ShakeSpin();
                EventManager.InvokeSpinWheelResult(resultItem.Reward);

                if (resultItem.Reward.Type == ERewardType.Bomb) return;

                _wofConffeti.SetActive(true);
                await UniTask.Delay(1000);
                await SetWheel();
            });
        }

        private void AnimateSpinButton(EAnimationType animationType)
        {
            switch (animationType)
            {
                case EAnimationType.Spin:
                    _spinButton.transform.DOPunchScale(new Vector3(0.1f, 0.1f, 0.1f), 0.5f, 10, 1).SetEase(Ease.OutQuart).OnComplete(() =>
                    {
                        _spinButton.transform.DOScale(Vector3.zero, 0.2f);
                    });
                    break;
                case EAnimationType.Result:
                    _spinButton.transform.DOScale(Vector3.one, 0.2f).OnComplete(() =>
                    {
                        _spinButton.transform.DOPunchScale(new Vector3(0.1f, 0.1f, 0.1f), 0.5f, 10, 1).SetEase(Ease.OutQuart);
                    });
                    break;
                case EAnimationType.Initial:
                    _spinButton.transform.localScale = Vector3.zero;
                    break;
            }
        }

        private void SetGiveUpButton(bool active)
        {
            if (_currentSpinCount == 0 || _wofType == EWOFType.Bronze)
            {
                _giveUpButton.transform.localScale = Vector3.zero;
                return;
            }

            if (!active)
            {
                _giveUpButton.onClick.RemoveAllListeners();
                _giveUpButton.transform.DOScale(Vector3.zero, 0.2f).SetEase(Ease.OutQuart).OnComplete(() =>
                {
                    _giveUpButton.gameObject.SetActive(false);
                });
                return;
            }

            _giveUpButton.onClick.RemoveAllListeners();
            _giveUpButton.gameObject.SetActive(true);
            _giveUpButton.transform.DOScale(Vector3.one, 0.2f).SetEase(Ease.OutQuart).OnComplete(() =>
            {
                _giveUpButton.onClick.AddListener(() =>
                {
                    _giveUpButton.onClick.RemoveAllListeners();
                    EventManager.InvokeGiveUpButtonClicked();
                    _giveUpButton.transform.DOScale(Vector3.zero, 0.2f).SetEase(Ease.OutQuart).OnComplete(() =>
                    {
                        _giveUpButton.gameObject.SetActive(false);
                    });
                });
            });
        }

        private void ShakeSpin()
        {
            if (DOTween.IsTweening(_wofPanel)) return;
            _wofPanel.DOShakeAnchorPos(0.15f, 10, 100, 90, false, true).OnComplete(() =>
            {
                _wofPanel.anchoredPosition = Vector2.zero;
            });
        }
    }
}