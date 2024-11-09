using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;
    public AudioSource source;
    public AudioClip launchSound;
    public AudioClip hitSound;
    public AudioClip reloadSound;
    public AudioClip buttonPressSound;
    public AudioClip scoreUpdateSound;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    public void Play(string clipName)
    {
        switch (clipName)
        {
            case "LaunchSound":
                source.PlayOneShot(launchSound);
                break;
            case "HitSound":
                source.PlayOneShot(hitSound);
                break;
            case "ReloadSound":
                source.PlayOneShot(reloadSound);
                break;
            case "ButtonPress":
                source.PlayOneShot(buttonPressSound);
                break;
            case "ScoreUpdateSound":
                source.PlayOneShot(scoreUpdateSound);
                break;
        }
    }
}
