using UnityEngine;
using UnityEngine.Events;

namespace BubblePops.Core
{
    public class OnScoreUpdate : UnityEvent<int> {  }
    public class OnFillBarUpdate : UnityEvent<float> { }
    public class OnLevelUp : UnityEvent<int> {  }

    public class ScoreManager : MonoBehaviour
    {
        /// <summary>
        /// 
        /// </summary>
        private int _score = 0;

        /// <summary>
        /// 
        /// </summary>
        private int _level = 1;

        /// <summary>
        /// 
        /// </summary>
        private int _nextLevelUp = 0;

        /// <summary>
        /// 
        /// </summary>
        public OnScoreUpdate onScoreUpdate = null;

        /// <summary>
        /// 
        /// </summary>
        public OnLevelUp onLevelUp = null;

        /// <summary>
        /// 
        /// </summary>
        public OnFillBarUpdate onFillBarUpdate = null;

        public int Score => _score;
        public int Level => _level;

        private void Awake()
        {
            onScoreUpdate = new OnScoreUpdate();
            onLevelUp = new OnLevelUp();
            onFillBarUpdate = new OnFillBarUpdate();

            _score = GetNextLevelUp(_level - 1);
        }

        public void UpdateScore(int change)
        {
            _score += change;
            onScoreUpdate.Invoke(_score);

            CheckForLevelUp();
        }

        public void UpgradeLevel() 
        {
            ++_level;
            onLevelUp.Invoke(_level);
        }

        private void CheckForLevelUp()
        {
            var lastLevelUpScore = GetNextLevelUp(_level - 1);
            var nextLevelUpScore = GetNextLevelUp(_level);

            var fillAmount = (float)(_score - lastLevelUpScore) / (float)(nextLevelUpScore - lastLevelUpScore);
            onFillBarUpdate.Invoke(fillAmount);

            if(_score >= nextLevelUpScore)
                UpgradeLevel();
        }

        private int GetNextLevelUp(int currentLevel)
        {
            return ((currentLevel + 1) * 1000);
        }

        [NaughtyAttributes.Button]
        public void UpdateScore()
        {
            UpdateScore(100);
        }
    }
}