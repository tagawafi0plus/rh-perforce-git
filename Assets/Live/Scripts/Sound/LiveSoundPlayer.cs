using UnityEngine;

namespace Live.Scripts.Sound
{
    public class LiveSoundPlayer : MonoBehaviour
    {
        private AudioSource bgmSource;
        private AudioSource perfectSource;
        private AudioSource unperfectSource;
        private AudioSource normalSource;

        // --------------------------------------------------
        // Game Cycle API
        // --------------------------------------------------
        public void StartGame()
        {
            var items = GetComponents<AudioSource>();
            foreach (var item in items)
            {
                // Debug.Log(item.clip.name);
                switch (item.clip.name)
                {
                    case "se_perfect":
                        perfectSource = item;
                        break;
                    case "se_unperfect":
                        unperfectSource = item;
                        break;
                    case "se_normal":
                        normalSource = item;
                        break;
                }
            }

            var items2 = GetComponentsInChildren<AudioSource>();
            foreach (var item in items2)
            {
                Debug.Log(item.clip.name);
                switch (item.clip.name)
                {
                    case "bgm_daydream_cafe":
                    case "bgm_kirakira alamode_final":
                        bgmSource = item;
                        break;
                }
            }
            
            if (!bgmSource) return;
            bgmSource.Play();
        }

        public void UpdateGame()
        {
        }

        public void EndGame()
        {
            if (!bgmSource) return;
            bgmSource.Stop();
        }

        public int GetTimeSamples()
        {
            if (!bgmSource) return 0;
            return bgmSource.timeSamples;
        }

        public float GetTimeSec()
        {
            if (!bgmSource) return 0;
            return bgmSource.time;
        }

        // --------------------------------------------------
        // Sound API
        // --------------------------------------------------
        public void PlayPerfect()
        {
            if (!perfectSource) return;
            perfectSource.Play();
        }
        
        public void PlayUnperfect()
        {
            if (!unperfectSource) return;
            unperfectSource.Play();
        }
        public void PlayNormal()
        {
            if (!normalSource) return;
            normalSource.Play();
        }
    }
}