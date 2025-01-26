using UnityEngine;

public class LoopMusic : Singleton<LoopMusic>
{
    [SerializeField]
    public AudioSource LoopedSource;

    [SerializeField]
    public GameObject NonLoopGO;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        DontDestroyOnLoad(this);
        DontDestroyOnLoad(NonLoopGO);

        AudioSource NonLoopAudioSource = NonLoopGO.GetComponent<AudioSource>();
        if (!NonLoopAudioSource.isPlaying)
        {
            NonLoopAudioSource.Play();
        }

        LoopedSource.loop = true;
        LoopedSource.PlayDelayed(126.4615208333333f);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
