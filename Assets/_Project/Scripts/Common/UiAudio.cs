using UnityEngine;

namespace NongTrai.UI
{
    /// <summary>
    /// Phát âm thanh giao diện từ bất kỳ đâu: UiAudio.Play(clip), UiAudio.PlayVoice(clip).
    /// Tự tạo một GameObject chứa 2 AudioSource ở lần gọi đầu tiên và giữ nó qua các scene.
    /// </summary>
    public static class UiAudio
    {
        static AudioSource sfxSource;
        static AudioSource voiceSource;

        static void EnsureCreated()
        {
            // So sánh "!= null" của Unity cũng đúng với object đã bị hủy.
            if (sfxSource != null && voiceSource != null)
                return;

            var go = new GameObject("UiAudio");
            Object.DontDestroyOnLoad(go);
            sfxSource = go.AddComponent<AudioSource>();
            voiceSource = go.AddComponent<AudioSource>();
            sfxSource.playOnAwake = false;
            voiceSource.playOnAwake = false;
        }

        /// <summary>Hiệu ứng ngắn (tiếng "bụp" khi bấm). Có thể chồng lên nhau.</summary>
        public static void Play(AudioClip clip)
        {
            if (clip == null) return;
            EnsureCreated();
            sfxSource.PlayOneShot(clip);
        }

        /// <summary>Giọng đọc. Câu mới cắt câu cũ, để hai câu không nói đè lên nhau.</summary>
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
