﻿using UnityEngine;

namespace Tsinghua.HCI.IoThingsLab
{
    public class AudioItem : MonoBehaviour
    {
        GenericItem _audio;

        private AudioSource _audioSource;
        
        void Start()
        {
            _audioSource = GetComponent<AudioSource>();
        }


        
        public void PauseAudioSource()
        {
            if (IsAudioSourcePlaying()) _audioSource.Pause();
        }

        public void PlayAudioSource()
        {
            if (!IsAudioSourcePlaying()) _audioSource.Play();
        }

        public bool IsAudioSourcePlaying()
        {
            return _audioSource.isPlaying;
        }

        public void ToggleAudioSource()
        {
            Debug.Log("Trigger change status:audio");
            if (IsAudioSourcePlaying()) PauseAudioSource();
            else PlayAudioSource();
        }

        public void MuteAudioSource()
        {
            _audioSource.mute = true;
        }

        public void UnMuteAudioSource()
        {
            _audioSource.mute = false;
        }

        public void DecreaseVolumeAudioSource(float value = 0.1f)
        {
            _audioSource.volume -= value;
        }

        public void IncreaseVolumeAudioSource(float value = 0.1f)
        {
            _audioSource.volume += value;
        }

        public void ChangeAudioSource(AudioClip newAudioClip)
        {
            _audioSource.clip = newAudioClip;
        }

        /// <summary>
        /// Saving the state of the audio Source (the name)
        /// </summary>
        /// <param name="name"></param>
        void SerializeValue(string name = "")
        {
            _audio.name = name + "_audio";
            _audio.state = _audioSource.ToString();
            _audio.type = "Audio";
        }
        
        private void Update() {
            IncreaseVolumeAudioSource() ;
        }
    }
}