using UnityEngine;
using UnityEngine.Events;

public class MusicPlayer : MonoBehaviour
{   

    public void PlayLifeStagesMusic()
    {
        AkSoundEngine.PostEvent("Play_LifeStagesMusic", gameObject);
        Debug.Log("Playing life stages music.");
    }

    // Public function to stop the life stages music
    public void StopLifeStagesMusic()
    {
        AkSoundEngine.PostEvent("Stop_LifeStagesMusic", gameObject);
        Debug.Log("Stopping life stages music.");
    }

    // Public function to turn on the "brightness" RTPC (set it to 100)
    public void SetBrightnessOn()
    {
        AkSoundEngine.SetRTPCValue("Brightness", 100, gameObject);
        Debug.Log("Brightness set to 100.");
    }

    // Public function to turn off the "brightness" RTPC (set it to 0)
    public void SetBrightnessOff()
    {
        AkSoundEngine.SetRTPCValue("Brightness", 0, gameObject);
        Debug.Log("Brightness set to 0.");
    }
}
