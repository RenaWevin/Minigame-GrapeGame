
using System.Collections.Generic;
using UnityEngine;

public class AudioComponent : MonoBehaviour {

    #region 參數參考區

    //音效上次使用的通道編號
    private int soundPrevIndex = -1;

    private readonly Dictionary<SoundId, AudioClip> audioclipDict = new Dictionary<SoundId, AudioClip>();

    private bool enableBGM = true;
    private int volumeBGM = 50; //0~100

    private bool enableSound = true;
    private int volumeSound = 50; //0~100

    #endregion
    #region 音樂音效AudioSource

    [SerializeField]
    private AudioSource AudioSource_BGM;

    [SerializeField]
    private AudioSource[] AudioSource_SoundFX;

    #endregion
    #region 音效素材(Inspector)

    [Space]
    [Header("音效素材")]

    [SerializeField]
    private AudioClip Clip_Click_Normal;

    [SerializeField]
    private AudioClip Clip_Click_Close;

    [SerializeField]
    private AudioClip Clip_Click_StartGame;

    [SerializeField]
    private AudioClip Clip_Result_Win;

    [SerializeField]
    private AudioClip Clip_Result_Lose;

    [SerializeField]
    private AudioClip Clip_Fruit_Put;

    [SerializeField]
    private AudioClip Clip_Fruit_Combine;

    [SerializeField]
    private AudioClip Clip_Fruit_Gray;

    #endregion
    #region Awake

    private void Awake() {
        audioclipDict.Clear();
        audioclipDict.Add(SoundId.Click_Normal, Clip_Click_Normal);
        audioclipDict.Add(SoundId.Click_Close, Clip_Click_Close);
        audioclipDict.Add(SoundId.Click_StartGame, Clip_Click_StartGame);
        audioclipDict.Add(SoundId.Result_Win, Clip_Result_Win);
        audioclipDict.Add(SoundId.Result_Lose, Clip_Result_Lose);
        audioclipDict.Add(SoundId.Fruit_Put, Clip_Fruit_Put);
        audioclipDict.Add(SoundId.Fruit_Combine, Clip_Fruit_Combine);
        audioclipDict.Add(SoundId.Fruit_Gray, Clip_Fruit_Gray);

        enableBGM = PlayerPrefHelper.GetSetting_Enable_BGM();
        volumeBGM = PlayerPrefHelper.GetSetting_Volume_BGM();
        enableSound = PlayerPrefHelper.GetSetting_Enable_SoundFX();
        volumeSound = PlayerPrefHelper.GetSetting_Volume_SoundFX();
    }

    #endregion
    #region 外部方法-更改音樂音量

    public void SetBGMVolume(int volume) {
        volumeBGM = volume;
        float volumeFloat = volume * 0.01f;
        AudioSource_BGM.volume = volumeFloat;
    }

    #endregion
    #region 外部方法-音樂切換靜音

    public void SetBGMMute(bool mute) {
        enableBGM = !mute;
        AudioSource_BGM.mute = mute;
    }

    #endregion
    #region 外部方法-播放音樂

    public void PlayBGM() {
        AudioSource_BGM.mute = !enableBGM;
        AudioSource_BGM.volume = volumeBGM * 0.01f;
        AudioSource_BGM.Play();
    }

    #endregion
    #region 外部方法-更改音效音量

    public void SetSoundFXVolume(int volume) {
        volumeSound = volume;
        float volumeFloat = volume * 0.01f;
        for (int i = 0; i < AudioSource_SoundFX.Length; i++) {
            AudioSource_SoundFX[i].volume = volumeFloat;
        }
    }

    #endregion
    #region 外部方法-音效切換靜音

    public void SetSoundMute(bool mute) {
        enableSound = !mute;
        for (int i = 0; i < AudioSource_SoundFX.Length; i++) {
            AudioSource_SoundFX[i].mute = mute;
        }
    }

    #endregion
    #region 外部方法-播放音效

    /// <summary>
    /// 指定特定音效播放
    /// </summary>
    /// <param name="soundId"></param>
    public void PlaySound(SoundId soundId) {
        bool enable_SoundFX = enableSound;
        if (!enable_SoundFX) {
            //沒有啟用就不播放
            return;
        }
        int volumeInt = volumeSound;
        float volumeFloat = volumeInt * 0.01f;
        soundPrevIndex++;
        if (soundPrevIndex < 0) { soundPrevIndex = 0; }
        if (soundPrevIndex >= AudioSource_SoundFX.Length) { soundPrevIndex = 0; }
        if (audioclipDict.TryGetValue(soundId, out AudioClip audioClip)) {
            AudioSource_SoundFX[soundPrevIndex].clip = audioClip;
            AudioSource_SoundFX[soundPrevIndex].mute = !enable_SoundFX;
            AudioSource_SoundFX[soundPrevIndex].volume = volumeFloat;
            AudioSource_SoundFX[soundPrevIndex].Play();
        } else {
            Log.Error($"SoundId: {soundId} 的音效不存在");
        }
    }

    #endregion

}
