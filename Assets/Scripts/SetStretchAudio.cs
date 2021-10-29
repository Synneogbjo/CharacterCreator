using UnityEngine;

public class SetStretchAudio : MonoBehaviour
{
    private AudioSource _Audio;
    [SerializeField] private AudioClip stretchSound;
    [SerializeField] private AudioClip sansSound;

    private void Start()
    {
        _Audio = GetComponent<AudioSource>();
    }

    private void Update()
    {
        if (MyInput.sans)
        {
            _Audio.clip = sansSound;
        }
        else _Audio.clip = stretchSound;
    }
}
