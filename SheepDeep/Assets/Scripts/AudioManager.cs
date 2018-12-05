using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour {

    private static AudioManager instance;

    private AudioSource source;

    public List<AudioClip> birthShepherdClips;
    public List<AudioClip> wolfAttackClips;
    public List<AudioClip> sheepShearClips;
    public List<AudioClip> birthSheepClips;
    public List<AudioClip> sheepDyingClips;
    public List<AudioClip> shaverClips;
    public List<AudioClip> weirdClips;
    public List<AudioClip> randomSheepNoises;
    public List<AudioClip> shepherdsWin;
    public List<AudioClip> wolfCaughtClips;


    // Use this for initialization
    void Start () {
        instance = this;
        source = GetComponent<AudioSource>();

        StartCoroutine(randomSheepNoisesCoroutine());
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private IEnumerator randomSheepNoisesCoroutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(Random.Range(5, 15));

            PlayRandomFromList(randomSheepNoises, Random.Range(0.5f, 1f));
        }
    }


    private void PlayRandomFromList(List<AudioClip> list, float volume)
    {
        AudioClip selected = list[Random.Range(0, list.Count)];
        source.PlayOneShot(selected, volume);
    }

    public static void PlayShepherdWins()
    {
        instance.PlayRandomFromList(instance.shepherdsWin, 1);
    }

    public static void PlayWolfCaught()
    {
        instance.PlayRandomFromList(instance.wolfCaughtClips, 1);
    }

    public static void PlayBirthShepherd()
    {
        instance.PlayRandomFromList(instance.birthShepherdClips, 1);
    }

    public static void PlayWolfAttack()
    {
        instance.PlayRandomFromList(instance.wolfAttackClips, .5f);
    }

    public static void PlaySheepShear()
    {
        instance.PlayRandomFromList(instance.sheepShearClips, 1);
    }

    public static void PlayBirthSheep()
    {
        instance.PlayRandomFromList(instance.birthSheepClips, 1);
    }

    public static void PlaySheepDying()
    {
        instance.PlayRandomFromList(instance.sheepDyingClips, 1);
    }

    public static void PlayShaver()
    {
        instance.PlayRandomFromList(instance.shaverClips, 1);
    }
}
