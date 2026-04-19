using UnityEngine;

class Music : MonoBehaviour
{
    private AudioSource source;
    public AudioClip[] clips;
    public AudioClip battle;
    private int currentClipIndex;
    void Start()
    {
        source = GetComponent<AudioSource>();
        SwitchMusic();
    }

    void PlayMusic(AudioClip clip)
    {
        source.clip = clip;
        source.Play();
        Invoke("SwitchMusic", clip.length + 5.0f);
    }

    void SwitchMusic()
    {
        int index = currentClipIndex;
        while (index == currentClipIndex)
        {
            index = Random.Range(0, clips.Length);
        }
        AudioClip clipToPlay = clips[index];
        PlayMusic(clipToPlay);
    }

    void BattleMusic()
    {
        CancelInvoke("SwitchMusic");
        source.Stop();
        PlayMusic(battle);
    }
}