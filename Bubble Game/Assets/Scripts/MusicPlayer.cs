using UnityEngine;
using UnityEngine.Events;

public class MusicPlayer : MonoBehaviour
{   
    [SerializeField] private string lifeStagesSwitchGroup = "LifeStages"; // Switch Group name
    [SerializeField] private string[] lifeStages = { "Kid", "YA", "Adult", "Elder" }; // Ordered life stages

    private int currentStageIndex = -1; // Track the current stage

    void Start()
    {
        // Set the initial switch state to "Kid"
        SetLifeStage("Kid");
    }

    // Public function to change the switch to the next life stage
    public void NextLifeStage()
    {
        // Increment the index and wrap back to 0 if it exceeds the array length
        currentStageIndex = (currentStageIndex + 1) % lifeStages.Length;

        // Set the next switch state
        SetLifeStage(lifeStages[currentStageIndex]);
    }

    // Private function to set the switch state
    private void SetLifeStage(string stage)
    {
        AkSoundEngine.SetSwitch(lifeStagesSwitchGroup, stage, gameObject);
        Debug.Log($"Life stage switched to: {stage}");
    }

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

    public void PlayRain()
    {
        AkSoundEngine.PostEvent("Play_Rain", gameObject);
    }

    // Public function to stop the life stages music
    public void StopRain()
    {
        AkSoundEngine.PostEvent("Stop_Rain", gameObject);
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
