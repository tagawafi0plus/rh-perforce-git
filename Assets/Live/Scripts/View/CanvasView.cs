using System.Collections.Generic;
using Scripts.Util.Transform;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Button = UnityEngine.UI.Button;
using Toggle = UnityEngine.UI.Toggle;

namespace Live.Scripts.View
{
    public class CanvasView : MonoBehaviour
    {
        public Text timeText;

        public Button btn0;
        public Button btn1;
        public Button btn2;
        public Button btn3;
        public Button btn4;

        public GameObject panel;
        public Toggle toggle;

        public List<Button> btnItems;

        public Button restartBtn;

        // --------------------------------------------------
        // API
        // --------------------------------------------------
        public void StartGame()
        {
            btnItems = new List<Button>
            {
                btn0,
                btn1,
                btn2,
                btn3,
                btn4,
            };
            if (restartBtn)
            {
                restartBtn.onClick.AddListener(() =>
                {
                    SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
                });
            }
        }

        public void UpdateGame(float elapsedFromStartTime)
        {
            timeText.text = "" + (int) elapsedFromStartTime;
        }

        public void EndGame()
        {
        }

        public void OnTapBtn(int index, bool flag)
        {
            var btn = btnItems[index];
            if (!btn)
            {
                return;
            }

            var scale = flag ? 1.5f : 1.0f;
            btn.transform.SetLocalScale(scale, scale, scale);
        }
    }
}