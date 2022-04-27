using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
/// <summary>
/// BGM Functions
/// PlayBackgroundAudio(), StopBackgroundAudio(), PauseBackgroundAudio(), ChangeBackgroundAudio(AudioClip audioClip)
/// TransitionBackgroundAudio(AudioClip audioClip, float duration) Transition between current BGM and newly supplied BGM. Fade in and out by duration amount.
/// StartFade(float duration, float targetVolume) Fade out or fade in current audio by duration amount. Target volume between 0(muted) and 1(loudest).
/// 
/// SFX Functions
/// PlaySFX(AudioClip audioClip) Plays supplied audioClip
/// </summary>
///
[Serializable]
public class LevelToBGM
{
	public string levelName;
	public AudioClip levelBGM;
}
public class AudioManager : MonoBehaviour
{
	public static AudioManager current;

	[Tooltip("Audio Mixer Object")]
	[SerializeField] private AudioMixer audioMixer;

	[Tooltip("Volume to change string in audio mixer")]
	[SerializeField] private string exposedParamString;

	[Tooltip("AudioSource component for background music")]
    [SerializeField] private AudioSource musicAudioSource;

	[Tooltip("AudioSource component for sound effects")]
	[SerializeField] private AudioSource SFXAudioSource;

	private List<GameObject> _longSFXList = new List<GameObject>();

	public List<LevelToBGM> levelToBgmsList;

	private void Awake()
	{
		current = this;
		musicAudioSource = GetComponent<AudioSource>();

		DontDestroyOnLoad(gameObject);
		SceneManager.sceneLoaded += OnSceneLoaded;
	}

	private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
	{
		bool soundFound = false;
		for (int ii = 0; ii < SceneManager.sceneCount; ii++)
		{
			foreach (LevelToBGM obj in levelToBgmsList)
			{
				if (obj.levelName == SceneManager.GetSceneAt(ii).name)
				{
					ChangeBackgroundAudio(obj.levelBGM);
					soundFound = true;
					break;
				}
			}
			if (soundFound) break;
		}
		if(!soundFound) StopBackgroundAudio();
	}

	#region Background Audio Functions
	public void PlayBackgroundAudio()
	{
		musicAudioSource.Play();
	}

	public void StopBackgroundAudio()
	{
		musicAudioSource.Stop();
	}

	public void PauseBackgroundAudio()
	{
		musicAudioSource.Pause();
	}

	public void ChangeBackgroundAudio(AudioClip audioClip)
	{
		StopBackgroundAudio();
		musicAudioSource.clip = audioClip;
		PlayBackgroundAudio();
	}

	public void TransitionBackgroundAudio(AudioClip audioClip, float duration)
	{
		StartCoroutine(TransitionBackground_Coroutine(audioClip, duration));
	}

	private IEnumerator TransitionBackground_Coroutine(AudioClip audioClip, float duration)
	{
		StartFade(duration, 0f);
		yield return new WaitForSeconds(duration);
		ChangeBackgroundAudio(audioClip);
		StartFade(duration, 1f);
	}
	#endregion

	#region Audio Fade Functions
	public void StartFade(float duration, float targetVolume)
	{
		StartCoroutine(StartFade_Coroutine(duration, targetVolume));
	}

	private IEnumerator StartFade_Coroutine(float duration, float targetVolume)
	{
		float currentTime = 0;
		float currentVol;

		audioMixer.GetFloat(exposedParamString, out currentVol);
		currentVol = Mathf.Pow(10, currentVol / 20);
		float targetValue = Mathf.Clamp(targetVolume, 0.0001f, 1f);
		while (currentTime < duration)
		{
			currentTime += Time.deltaTime;
			float newVol = Mathf.Lerp(currentVol, targetValue, currentTime / duration);
			audioMixer.SetFloat(exposedParamString, Mathf.Log10(newVol) * 20);
			yield return null;
		}
		yield break;
	}
	#endregion

	#region SFX Functions
	public void PlaySFX(AudioClip audioClip)
	{
		SFXAudioSource.PlayOneShot(audioClip);
	}

	public void PlayLongSFX(string name, AudioClip audioClip)
	{
		GameObject spawnedObject = new GameObject(name);
		spawnedObject.AddComponent<AudioSource>();
		spawnedObject.GetComponent<AudioSource>().clip = audioClip;
		spawnedObject.GetComponent<AudioSource>().Play();
		_longSFXList.Add(spawnedObject);
	}

	public void StopLongSFX(string name)
	{
		GameObject tempGO = null;
		foreach (GameObject sfx in _longSFXList)
		{
			if (sfx.name == name)
			{
				tempGO = sfx;
				break;
			}
		}
		Destroy(tempGO);
		_longSFXList.Remove(tempGO);
	}

	#endregion
}