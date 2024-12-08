using Cysharp.Threading.Tasks;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Utilities;
using WheelOfFortune.Modules.RewardSystem.Data;
using WheelOfFortuneSystem.Architecture.Enum;

namespace Modules.WheelOfFortuneSystem.Controllers
{
    public class WheelOfFortuneItemController : MonoBehaviour
    {
        [SerializeField] private RectTransform _rectTransform;
        [SerializeField] private CanvasGroup _canvasGroup;
        [SerializeField] private Image _rewardImage;
        [SerializeField] private GameObject _rewardTextHolder;
        [SerializeField] private TextMeshProUGUI _rewardText;

        private int _itemIndex;
        public int ItemIndex
        {
            get => _itemIndex;
            set => _itemIndex = value;
        }

        [SerializeField] private Reward _reward;
        public Reward Reward
        {
            get => _reward;
            set => _reward = value;
        }

        public void Init(Reward reward, int itemIndex)
        {
            _itemIndex = itemIndex;
            SetReward(reward);
        }

        public void SetActive(bool isActive)
        {
            gameObject.SetActive(isActive);
        }

        public async UniTask PlayAnimation(EAnimationType animationType)
        {
            switch (animationType)
            {
                case EAnimationType.Initial:
                    await PlayInitialAnimation();
                    break;
                case EAnimationType.Result:
                    await PlayResultAnimation();
                    break;
                case EAnimationType.Spin:
                    await PlaySpinAnimation();
                    break;
                case EAnimationType.Out:
                    await PlayOutAnimation();
                    break;
            }
        }

        private void SetReward(Reward reward)
        {
            _reward = reward;
            _rewardImage.sprite = reward.Sprite;
            _rewardText.text = $"x{reward.Amount.Abbreviate()}";
        }

        private async UniTask PlayInitialAnimation()
        {
            // Play initial animation
            _canvasGroup.alpha = 0;
            float angle = _rectTransform.eulerAngles.z * Mathf.Deg2Rad;
            float deltaX = 100 * Mathf.Cos(angle);
            float deltaY = 100 * Mathf.Sin(angle);
            _rectTransform.anchoredPosition = new Vector2(_rectTransform.anchoredPosition.x + deltaX, _rectTransform.anchoredPosition.y + deltaY);
            _rectTransform.localScale = new Vector3(2f, 1.5f, 2f);

            _rewardTextHolder.transform.localScale = Vector3.zero;
            _rewardTextHolder.SetActive(false);

            SetActive(true);

            var sequence = DOTween.Sequence();
            sequence.Append(_canvasGroup.DOFade(1, 0.2f));
            sequence.Join(_rectTransform.DOJumpAnchorPos(new Vector2(0, 0), 50, 1, 0.2f));
            sequence.Join(_rectTransform.DOScale(Vector3.one, 0.2f));
            await sequence.Play().AsyncWaitForCompletion();

            _rewardTextHolder.SetActive(true);
            _rewardTextHolder.transform.DOScale(Vector3.one, 0.4f).SetEase(Ease.OutBack);

            _rectTransform.DOPunchScale(Vector3.one * 0.1f, 0.3f);
        }

        private async UniTask PlayResultAnimation()
        {
            // Play result animation
            _rewardTextHolder.transform.DOScale(Vector3.one * 1.2f, 0.2f).SetEase(Ease.OutBack);
            _rectTransform.DOPunchScale(Vector3.one * 0.1f, 0.3f);
        }

        private async UniTask PlaySpinAnimation()
        {
            // Play spin animation
        }

        private async UniTask PlayOutAnimation()
        {
            // Play out animation
            var sequence = DOTween.Sequence();
            sequence.Append(_canvasGroup.DOFade(0, 0.1f));
            sequence.Join(_rectTransform.DOScale(Vector3.zero, 0.1f));
            await sequence.Play().AsyncWaitForCompletion();

            SetActive(false);
        }
    }
}
