using UnityEngine;

public class SetMusicOnLoad : MonoBehaviour
{
    void Start()
    {
        if (PlayerPrefs.HasKey("musicVolume"))
        {
            GetComponent<AudioSource>().volume = PlayerPrefs.GetFloat("musicVolume");
        }
    }
}
