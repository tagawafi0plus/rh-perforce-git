using Scripts.Util.Transform;
using UnityEngine;
using UnityEngine.UI;

namespace Live.Scripts.Effect
{
    public class EffectTest : MonoBehaviour
    {
        public Canvas canvas;
        public HitEffect hitEffectPrefab;

        public Button hitEffectButton;
        public Button lineEffectButton;

        public GameObject CubePrefab;

        public GameObject button0;
        public GameObject button1;
        public GameObject button2;
        public GameObject button3;
        public GameObject button4;

        public Camera MainCamera, UICamera;

        public GameObject testCube;


        public void Start()
        {
            if (hitEffectButton)
            {
                hitEffectButton.onClick.AddListener(() => { EmitHitEffect(); });
            }

            if (lineEffectButton)
            {
                lineEffectButton.onClick.AddListener(() => { EmitLineEffect(); });
            }
        }

        public void EmitHitEffect()
        {
            var hitEffect = (HitEffect) Instantiate(hitEffectPrefab);
            float x = -200 + 200 * Random.Range(0, 5);
            float y = 0;
            hitEffect.StartTweens(canvas.transform, x, y);
        }

        public void EmitLineEffect()
        {
            /*
            Debug.Log("---------------");
            Debug.LogWarning(testCube.transform.position);
            Debug.LogWarning(testCube.transform.localPosition);

            Debug.LogWarning(button0.transform.position);
            Debug.LogWarning(button0.transform.localPosition);
            Debug.Log("---------------");

            // ボタンのワールド座標
            var buttonPos = button0.transform.position;
            buttonPos = button0.transform.position;

            Debug.LogWarning(UICamera.ScreenToWorldPoint(buttonPos));
            Debug.LogWarning(UICamera.ScreenToViewportPoint(buttonPos));

            Debug.LogWarning(UICamera.ViewportToScreenPoint(buttonPos));
            Debug.LogWarning(UICamera.ViewportToWorldPoint(buttonPos));

            Debug.LogWarning(UICamera.WorldToScreenPoint(buttonPos));
            Debug.LogWarning(UICamera.WorldToViewportPoint(buttonPos));
            */

            var cubePos = button0.transform.localPosition;
            cubePos.z = -10;
            cubePos.x += Random.value * 10;
            cubePos.y += Random.value * 10;
            cubePos.z += Random.value * 10;

            var cube = Instantiate(CubePrefab);
            cube.transform.SetParent(canvas.transform);
            cube.gameObject.layer = LayerMask.NameToLayer("UI");            
            cube.transform.localPosition = cubePos;
            cube.transform.SetLocalScale(100, 100, 100);
        }
    }
}