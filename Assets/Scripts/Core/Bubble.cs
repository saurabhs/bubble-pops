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
        [SerializeField] private EType _value;

        public EType Type => _value;

        public TextMeshProUGUI label = null;

        private void Start() => label.text = $"{(int)_value}";
        //{
        //    var ss = gameObject.name.Split('_');
        //    if(ss.Length < 2)
        //        return;
        //    var row = int.Parse(ss[1]);
        //    var col = int.Parse(ss[2]);
        //    var index = (col * 10) + row;

        //    label.fontSize = 0.18f;

        //    gameObject.name += $"_{index}";
        //}
    }
}