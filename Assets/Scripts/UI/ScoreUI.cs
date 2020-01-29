using System.Collections;
using BubblePops.Core;
using NaughtyAttributes;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace BubblePops.UI
{
    public class ScoreUI : MonoBehaviour
    {
        /// <summary>
        /// ref to TMP Text component of score
        /// </summary>
        [SerializeField] private TextMeshProUGUI _score = null;

        /// <summary>
        /// ref to TMP Text component of multiplier
        /// </summary>
        [SerializeField] private TextMeshProUGUI _multiplier = null;
        
        /// <summary>
        /// ref to pause button in main scene UI
        /// </summary>
        [SerializeField] private Button _pause;

        /// <summary>
        /// 
        /// </summary>
        [SerializeField] private TextMeshProUGUI _currentLevel = null;

        /// <summary>
        /// 
        /// </summary>
        [SerializeField] private TextMeshProUGUI _nextLevel = null;

        /// <summary>
        /// 
        /// </summary>
        [SerializeField] private Image _levelFiller = null;

        /// <summary>
        /// 
        /// </summary>
        [SerializeField] private float fillBarFillSpeed = 7.5f;

        /// <summary>
        /// 
        /// </summary>
        [SerializeField] private ParticleSystem _particleSystem = null;

        private void Start()
        {
            var scoreManager = FindObjectOfType<ScoreManager>();
            if(scoreManager == null)
                scoreManager = new GameObject("ScoreMaanager").AddComponent<ScoreManager>();

            scoreManager.onScoreUpdate.AddListener(UpdateScore);
            scoreManager.onLevelUp.AddListener(UpdateLevel);
            scoreManager.onFillBarUpdate.AddListener(UpdateFillbar);

            _score.text = scoreManager.Score.ToString();
            _currentLevel.text = scoreManager.Level.ToString();
            _nextLevel.text = (scoreManager.Level + 1).ToString();
        }

        private void OnEnable() => _pause.onClick.AddListener(() => OnPause() );

        private void OnDisable() => _pause.onClick.RemoveAllListeners();

        private void OnPause() => print("Pause Clicked");

        public void UpdateScore(int score) => _score.text = score.ToString();

        public void UpdateLevel(int level)
        {
            _currentLevel.text = level.ToString();
            _nextLevel.text = (level + 1).ToString();

            _particleSystem.Play();
        }

        public void UpdateFillbar(float fillAmount) 
        {
            // _levelFiller.fillAmount = fillAmount;
            StartCoroutine(AnimateFillbar(fillAmount, 0.5f));
        }

        private IEnumerator AnimateFillbar(float fillAmount, float delay)
        {
            if(fillAmount < 1f)
                StartCoroutine(AnimateFillBarIncrement(fillAmount, fillBarFillSpeed / 10));

            yield return new WaitForSeconds(delay);

            if(fillAmount >= 1f)
                StartCoroutine(ResetFillAmount(fillBarFillSpeed));
        }

        private IEnumerator ResetFillAmount(float decrement)
        {
            _levelFiller.fillAmount -= decrement * Time.deltaTime;
            
            yield return new WaitForEndOfFrame();

            if(_levelFiller.fillAmount > 0)
                StartCoroutine(ResetFillAmount(decrement));
        }

        private IEnumerator AnimateFillBarIncrement(float fillAmount, float increment)
        {
            _levelFiller.fillAmount += increment * Time.deltaTime;

            yield return new WaitForEndOfFrame();

            if(_levelFiller.fillAmount < fillAmount)
                StartCoroutine(AnimateFillBarIncrement(fillAmount, increment));
            else if(_levelFiller.fillAmount > fillAmount)
                _levelFiller.fillAmount = fillAmount;
        }
    }
}