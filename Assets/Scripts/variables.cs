using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class variables : MonoBehaviour {

    public static float birthSpeed      = 1000;
    public static float gravityConstant = 100000f;
    public static float distanceScale   = 0.1f;
    public static float size            = 2;
    public static float density         = 0.3f;
    public static float planetMaxSize   = 500;
    public static float planetMinSize   = 15;

    public static bool takeControl = true;
    public static bool collisions  = true;
    public static bool paused      = false;
    public static bool flat        = false;


    public static AudioSource speaker;
    public static List<AudioClip> explosionSounds = new List<AudioClip>();
    public static float screenX;
    public static float screenY;

    void Start()
    {
        screenX = Screen.currentResolution.width;
        screenY = Screen.currentResolution.height;
        speaker = GameObject.Find("Main Camera").GetComponent<AudioSource>();
        load_clips();
    }

    void load_clips()
    {
        foreach (AudioClip clip in Resources.LoadAll("Audio/RExplosions", typeof(AudioClip)))
        {
            explosionSounds.Add(clip);
        }
    }

    public static Color trailColor(float speed)
    {
        Gradient gradient = new Gradient();
        gradient.SetKeys(
            new GradientColorKey[] { new GradientColorKey(Color.green, 0.0f), new GradientColorKey(Color.yellow, 0.5f), new GradientColorKey(Color.red, 1.0f) },
            new GradientAlphaKey[] { new GradientAlphaKey(1f, 0.0f), new GradientAlphaKey(1f, 1.0f) }
        );
        return gradient.Evaluate(speed);
    }

}
