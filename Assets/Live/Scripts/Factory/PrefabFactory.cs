using Live.Scripts.Effect;
using UnityEngine;
using UnityEngine.UI;

namespace Live.Scripts.Factory
{
    public class PrefabFactory : UnityEngine.MonoBehaviour
    {
        public GameObject notePrefab;
        public GameObject noteSpritePrefab;
        public HitEffect hitEffectPrefab;
        public LineMeshTest lineEffectPrefab;
        public GameObject noteConnecter;

        public GameObject GetNote()
        {
            // TODO: オブジェクトプール
            var note = (GameObject) Instantiate(notePrefab);
            if (note)
            {
                note.GetComponent<Image>().raycastTarget = false;
            }

            var debugText = note.GetComponentInChildren<Text>();
            if (debugText)
            {
                debugText.raycastTarget = false;
            }

            return note;
        }

        public GameObject GetNoteSprite()
        {
            // TODO: オブジェクトプール
            var note = (GameObject) Instantiate(noteSpritePrefab);
            return note;
        }

        public HitEffect GetHitEffect()
        {
            // TODO: オブジェクトプール
            var hitEffect = (HitEffect) Instantiate(hitEffectPrefab);
            return hitEffect;
        }

        public LineMeshTest GetLineEffect()
        {
            // TODO: オブジェクトプール
            var effect = (LineMeshTest) Instantiate(lineEffectPrefab);
            return effect;
        }

        public GameObject GetNoteConnecter()
        {
            // TODO: オブジェクトプール
            var connecter = (GameObject) Instantiate(this.noteConnecter);
            return connecter;
        }
    }
}