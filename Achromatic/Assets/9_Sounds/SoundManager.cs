using UnityEngine;


public class SoundManager : MonoBehaviour
{
    private AudioSource source;
    public void PlaySound(string soundName)
    {
        source = GameObject.FindGameObjectWithTag(soundName).GetComponent<AudioSource>();
        source.Play();
    }

    // ---------------------

    private static AudioSource static_source;

    public static void Static_PlaySound(string soundName, bool isLoop)
    {
        static_source = GameObject.FindGameObjectWithTag(soundName).GetComponent<AudioSource>();
        static_source.loop = isLoop;
        static_source.Play();
    }

    public static void Static_StopSound(string soundName)
    {
        static_source = GameObject.FindGameObjectWithTag(soundName).GetComponent<AudioSource>();
        static_source.loop = false;
        static_source.Stop();
    }

    // ---------------------

    private AudioSource personal_source;

    public AudioClip[] audioClips;

    public void Personal_PlaySound(string soundName, bool isSingle)
    {
        switch (isSingle)
        {
            case true:
                personal_source = GetComponent<AudioSource>();
                personal_source.Play();
                break;

            case false:
                personal_source = GetComponent<AudioSource>();

                switch(soundName)
                {
                    case "Failed":
                        personal_source.clip = audioClips[0];
                        break;
                    case "AlmostFailed":
                        personal_source.clip = audioClips[1];
                        break;
                    case "Succeed":
                        personal_source.clip = audioClips[2];
                        break;
                    case "WonderfulSucceed":
                        personal_source.clip = audioClips[3];
                        break;
                }

                personal_source.Play();
                break;
        }
    }
}
