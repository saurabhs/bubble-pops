using TMPro;
using UnityEngine;

namespace BubblePops.Core
{
    public enum EType
    {
        Two = 2,
        Four = 4,
        Eight = 8,
        Sixteen = 16,
        ThirtyTwo = 32,
        SixtyFour = 64,
        OneTwentyEight = 128,
        TwoFiftySix = 256,
        FiveTwelve = 512,
        TenTwentyFour = 1024,
        TwentyFortyEight = 2048
    }

    public class Bubble : MonoBehaviour
    {
        /// <summary>
        /// 
        /// </summary>
        [SerializeField] private BubblePops.Core.EType _value;

        [SerializeField] private TextMeshProUGUI _label;

        private void Awake()
        {
            _label.text = ((int)_value).ToString();
        }

        private void OnEnable()
        {
        }

        private void OnDisable()
        {
        }
    }
}