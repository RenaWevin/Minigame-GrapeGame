
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
    #region 外部方法-播放音樂

    public void PlayBGM() {
        //AudioSource_BGM.volume = PlayerPrefHelper.GetSetting_Volume_BGM(); ★
        AudioSource_BGM.Play();
    }

    #endregion
    #region 外部方法-播放音效

    public void PlaySound(AudioClip audioClip) {
        //★
        soundPrevIndex++;
        if (soundPrevIndex < 0) { soundPrevIndex = 0; }
        if (soundPrevIndex >= AudioSource_SoundFX.Length) { soundPrevIndex = 0; }
        AudioSource_SoundFX[soundPrevIndex].clip = audioClip;
        //AudioSource_SoundFX[soundPrevIndex].volume = PlayerPrefHelper.GetSetting_Volume_SoundFX(); ★
        AudioSource_SoundFX[soundPrevIndex].Play();
    }

    #endregion

}
