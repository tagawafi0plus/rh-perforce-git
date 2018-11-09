using DG.Tweening;
using Scripts.Util.Transform;
using UnityEngine;
using UnityEngine.UI;

namespace Live.Scripts.Effect
{
    public class HitEffect : MonoBehaviour
    {
        public Image starEffectPrefab;
        public Image circleEffectPrefab;

        public float sec0 = 0.6f;
        public float sec1 = 0.2f;
        public float sec2 = 0.7f;

        public float scale0 = 0.5f;
        public float scale1 = 1.1f;

        public float scale2 = 0.2f;
        public float scale3 = 2.0f;

        private Image star;
        private Image circle;

        public void OnStart()
        {
        }

        public void StartTweens(Transform transform, float x, float y)
        {
            star = Instantiate(starEffectPrefab);
            circle = Instantiate(circleEffectPrefab);

            star.transform.SetParent(transform);
            circle.transform.SetParent(transform);

            star.raycastTarget = false;
            circle.raycastTarget = false;

            star.transform.SetLocalPosition(x, y, 0);
            circle.transform.SetLocalPosition(x, y, 0);

//        DOTween.KillAll();
            PlayScaleUpTween(star, scale0, scale1);
            PlayScaleUpTween(circle, scale2, scale3);
        }

        private void PlayScaleUpTween(Image image, float iniScale, float toScale)
        {
            var scale = image.rectTransform.localScale;
            scale.x = iniScale;
            scale.y = iniScale;
            image.rectTransform.localScale = scale;

            var mySequence = DOTween.Sequence();
            mySequence
                // init
                .Append(image.DOFade(1, 0))

                // scaleUp
                .Append(image.rectTransform.DOScale(new Vector3(toScale, toScale, 1), sec0).SetEase(Ease.OutSine))

                // fade serial
//            .AppendInterval(sec1)
//            .Append(starEffect.DOFade(0, sec2));

                // fade overlap
                .Insert(sec1, image.DOFade(0, sec2))
                .AppendCallback(() =>
                {
                    if (star != null)
                    {
                        star.transform.SetParent(null);
                    }

                    if (circle != null)
                    {
                        circle.transform.SetParent(null);
                    }

                    GameObject.Destroy(star.gameObject);
                    GameObject.Destroy(circle.gameObject);
                    GameObject.Destroy(gameObject);
                });
        }
    }
}