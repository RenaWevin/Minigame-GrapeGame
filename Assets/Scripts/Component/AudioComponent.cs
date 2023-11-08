
using UnityEngine;

public class AudioComponent : MonoBehaviour {

    #region 參數參考區

    //音效上次使用的通道編號
    private int soundPrevIndex = -1;

    #endregion
    #region 音樂音效AudioSource

    [SerializeField]
    private AudioSource AudioSource_BGM;

    [SerializeField]
    private AudioSource[] AudioSource_SoundFX;

    #endregion
    #region 外部方法-更改音樂音量

    public void SetBGMVolume(int volume) {
        float volumeFloat = volume * 0.01f;
        AudioSource_BGM.volume = volumeFloat;
    }

    #endregion
    #region 外部方法-音樂切換靜音

    public void SetBGMMute(bool mute) {
        AudioSource_BGM.mute = mute;
    }

    #endregion
    #region 外部方法-播放音樂

    public void PlayBGM() {
        AudioSource_BGM.mute = !PlayerPrefHelper.GetSetting_Enable_BGM();
        AudioSource_BGM.volume = PlayerPrefHelper.GetSetting_Volume_BGM();
        AudioSource_BGM.Play();
    }

    #endregion
    #region 外部方法-更改音效音量

    public void SetSoundFXVolume(int volume) {
        float volumeFloat = volume * 0.01f;
        for (int i = 0; i < AudioSource_SoundFX.Length; i++) {
            AudioSource_SoundFX[i].volume = volumeFloat;
        }
    }

    #endregion
    #region 外部方法-音效切換靜音

    public void SetSoundMute(bool mute) {
        for (int i = 0; i < AudioSource_SoundFX.Length; i++) {
            AudioSource_SoundFX[i].mute = mute;
        }
    }

    #endregion
    #region 外部方法-播放音效

    public void PlaySound(AudioClip audioClip) {
        //★
        if (!PlayerPrefHelper.GetSetting_Enable_SoundFX()) {
            //沒有啟用就不播放
            return;
        }
        soundPrevIndex++;
        if (soundPrevIndex < 0) { soundPrevIndex = 0; }
        if (soundPrevIndex >= AudioSource_SoundFX.Length) { soundPrevIndex = 0; }
        AudioSource_SoundFX[soundPrevIndex].clip = audioClip;
        AudioSource_SoundFX[soundPrevIndex].mute = !PlayerPrefHelper.GetSetting_Enable_SoundFX();
        AudioSource_SoundFX[soundPrevIndex].volume = PlayerPrefHelper.GetSetting_Volume_SoundFX();
        AudioSource_SoundFX[soundPrevIndex].Play();
    }

    #endregion

}
