using UnityEngine;

/// <summary>
/// Generador de efectos de audio procedurales en memoria (retro / chiptune).
/// Asegura que el proyecto tenga sonidos funcionando inmediatamente sin depender de assets externos.
/// </summary>
public static class ProceduralAudioHelper
{
    private const int SampleRate = 44100;

    public static AudioClip CreateJumpSound()
    {
        float duration = 0.22f;
        int sampleCount = (int)(SampleRate * duration);
        float[] samples = new float[sampleCount];

        for (int i = 0; i < sampleCount; i++)
        {
            float t = (float)i / SampleRate;
            float freq = Mathf.Lerp(220f, 660f, t / duration);
            float envelope = 1f - (t / duration);
            samples[i] = Mathf.Sin(2f * Mathf.PI * freq * t) * envelope * 0.4f;
        }

        AudioClip clip = AudioClip.Create("Procedural_Jump", sampleCount, 1, SampleRate, false);
        clip.SetData(samples, 0);
        return clip;
    }

    public static AudioClip CreateLandSound()
    {
        float duration = 0.12f;
        int sampleCount = (int)(SampleRate * duration);
        float[] samples = new float[sampleCount];

        for (int i = 0; i < sampleCount; i++)
        {
            float t = (float)i / SampleRate;
            float freq = Mathf.Lerp(160f, 60f, t / duration);
            float envelope = Mathf.Exp(-t * 25f);
            samples[i] = (Random.value * 2f - 1f) * 0.3f * envelope + Mathf.Sin(2f * Mathf.PI * freq * t) * envelope * 0.4f;
        }

        AudioClip clip = AudioClip.Create("Procedural_Land", sampleCount, 1, SampleRate, false);
        clip.SetData(samples, 0);
        return clip;
    }

    public static AudioClip CreateCollectSound()
    {
        float duration = 0.28f;
        int sampleCount = (int)(SampleRate * duration);
        float[] samples = new float[sampleCount];

        for (int i = 0; i < sampleCount; i++)
        {
            float t = (float)i / SampleRate;
            float freq = t < duration * 0.5f ? 587f : 880f; // Arpegio D5 -> A5
            float envelope = 1f - (t / duration);
            samples[i] = Mathf.Sin(2f * Mathf.PI * freq * t) * envelope * 0.35f;
        }

        AudioClip clip = AudioClip.Create("Procedural_Collect", sampleCount, 1, SampleRate, false);
        clip.SetData(samples, 0);
        return clip;
    }

    public static AudioClip CreateGameOverSound()
    {
        float duration = 0.6f;
        int sampleCount = (int)(SampleRate * duration);
        float[] samples = new float[sampleCount];

        for (int i = 0; i < sampleCount; i++)
        {
            float t = (float)i / SampleRate;
            float freq = Mathf.Lerp(440f, 110f, t / duration);
            float envelope = 1f - (t / duration);
            samples[i] = Mathf.Sin(2f * Mathf.PI * freq * t) * envelope * 0.45f;
        }

        AudioClip clip = AudioClip.Create("Procedural_GameOver", sampleCount, 1, SampleRate, false);
        clip.SetData(samples, 0);
        return clip;
    }

    public static AudioClip CreateButtonClickSound()
    {
        float duration = 0.06f;
        int sampleCount = (int)(SampleRate * duration);
        float[] samples = new float[sampleCount];

        for (int i = 0; i < sampleCount; i++)
        {
            float t = (float)i / SampleRate;
            float envelope = 1f - (t / duration);
            samples[i] = Mathf.Sin(2f * Mathf.PI * 1000f * t) * envelope * 0.25f;
        }

        AudioClip clip = AudioClip.Create("Procedural_UIClick", sampleCount, 1, SampleRate, false);
        clip.SetData(samples, 0);
        return clip;
    }
}
