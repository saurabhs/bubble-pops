using BubblePops.Core;
using UnityEngine;

namespace BubblePops.UI
{
    public class ParticleSystemManager : MonoBehaviour
    {
        /// <summary>
        /// 
        /// </summary>
        [SerializeField] private Texture2D _levelUpParticle = null;

        public void Start()
        {
            var scoreManager = FindObjectOfType<ScoreManager>();
            if(scoreManager == null)
                scoreManager = new GameObject("ScoreMaanager").AddComponent<ScoreManager>();
        }
    }
}