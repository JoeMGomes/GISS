using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    // Check to see if we're about to be destroyed.
    private static bool m_ShuttingDown = false;
    private static object m_Lock = new object();
    private static SoundManager m_Instance;

    public static SoundManager Instance
    {
        get
        {
            if (m_ShuttingDown)
            {
                Debug.LogWarning("Instance '" + typeof(SoundManager) +
                    "' already destroyed. Returning null.");
                return null;
            }

            lock (m_Lock)
            {
                if (m_Instance == null)
                {
                    // Search for existing instance.
                    m_Instance = FindObjectOfType<SoundManager>();

                    // Create new instance if one doesn't already exist.
                    if (m_Instance == null)
                    {
                        // Need to create a new GameObject to attach the singleton to.
                        var singletonObject = new GameObject();
                        m_Instance = singletonObject.AddComponent<SoundManager>();
                        singletonObject.name = typeof(SoundManager).ToString() + " (Singleton)";
                        m_Instance.Initialize();
                        // Make instance persistent.
                        DontDestroyOnLoad(singletonObject);
                    }
                }

                return m_Instance;
            }
        }
    }


    private void OnApplicationQuit()
    {
        m_ShuttingDown = true;
    }


    private void OnDestroy()
    {
        m_ShuttingDown = true;
    }

    public enum Sound
    {
        BackgroundMusic,
        DestroyBlock,
        GameLost,
        GameWin,
        LoseLife,
        LockPlat
    }

    private Dictionary<Sound, float> soundTimes;
    private LevelController levelManager;
    private GameObject soundObject;
    private AudioSource audioSource;

    public void Initialize()
    {
        levelManager = FindObjectOfType<LevelController>();

    }

    public void PlaySound(Sound sound)
    {

        if (soundObject == null)
        {
            soundObject = new GameObject("Sound");
            audioSource = soundObject.AddComponent<AudioSource>();
        }
        audioSource.PlayOneShot(GetClip(sound));

    }

    private AudioClip GetClip(Sound sound)
    {
        foreach (LevelController.SoundAudioClip s in levelManager.audioClips)
        {
            if (s.sound == sound)
            {
                return s.audioClip;
            }
        }
        Debug.LogError("Sound " + sound + " not found");
        return null;

    }



}
