using UnityEngine;

namespace NongTrai.UI
{
    public static class UiAudio
    {
        static AudioSource sfxSource;
        static AudioSource voiceSource;

        static void EnsureCreated()
        {
            if (sfxSource != null && voiceSource != null)
                return;

            var go = new GameObject("UiAudio");
            Object.DontDestroyOnLoad(go);
            sfxSource = go.AddComponent<AudioSource>();
            voiceSource = go.AddComponent<AudioSource>();
            sfxSource.playOnAwake = false;
            voiceSource.playOnAwake = false;
        }

        public static void Play(AudioClip clip)
        {
            if (clip == null) return;
            EnsureCreated();
            sfxSource.PlayOneShot(clip);
        }

        public static void PlayVoice(AudioClip clip)
        {
            if (clip == null) return;
            EnsureCreated();
            voiceSource.Stop();
            voiceSource.clip = clip;
            voiceSource.Play();
        }

        public static void StopVoice()
        {
            if (voiceSource != null)
                voiceSource.Stop();
        }
    }
}
