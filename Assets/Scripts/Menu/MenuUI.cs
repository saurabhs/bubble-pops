using UnityEngine;
using UnityEngine.UI;

namespace BubblePops.Menu
{
    public class MenuUI : MonoBehaviour
    {
        [SerializeField] private Button _play = null;

        private void Awake() => _play.onClick.AddListener(() => UnityEngine.SceneManagement.SceneManager.LoadScene("Main"));
    }
}