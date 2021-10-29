using UnityEngine;
using UnityEngine.InputSystem;
using Random = UnityEngine.Random;

public class MusicPlayer : MonoBehaviour
{
    [SerializeField] private AudioSource m_Audio;
    [SerializeField] private AudioClip Song1;
    [SerializeField] private AudioClip Song2;
    [SerializeField] private AudioClip Song3;
    [SerializeField] private AudioClip Song4;
    [SerializeField] private AudioClip Song5;
    [SerializeField] private AudioClip Song6;
    [SerializeField] private AudioClip Song7;
    [SerializeField] private float songToPlay;

    private void Update()
    {
        if (!m_Audio.isPlaying)
        {
            songToPlay = Random.Range(1, 8);
        }
        
        if (songToPlay > 6)
        {
            m_Audio.PlayOneShot(Song7);
            songToPlay = 0;
        }
        
        if (songToPlay > 5)
        {
            m_Audio.PlayOneShot(Song6);
            songToPlay = 0;
        }
        
        if (songToPlay > 4)
        {
            m_Audio.PlayOneShot(Song5);
            songToPlay = 0;
        }
        
        if (songToPlay > 3)
        {
            m_Audio.PlayOneShot(Song4);
            songToPlay = 0;
        }
        
        if (songToPlay > 2)
        {
            m_Audio.PlayOneShot(Song3);
            songToPlay = 0;
        }
        
        if (songToPlay > 1)
        {
            m_Audio.PlayOneShot(Song2);
            songToPlay = 0;
        }
        
        if (songToPlay > 0)
        {
            m_Audio.PlayOneShot(Song1);
            songToPlay = 0;
        }

        if (Keyboard.current.pKey.wasPressedThisFrame)
        {
            m_Audio.Stop();
        }

        if (Keyboard.current.mKey.wasPressedThisFrame)
        {
            m_Audio.Stop();
            songToPlay = 7;
        }
    }
}
