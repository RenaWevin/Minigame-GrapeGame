
using UnityEngine;
using UnityEngine.UI;

public class CanvasGroupWindow : MonoBehaviour {

    [SerializeField]
    private Image Image_Blocker;

    [SerializeField]
    private CanvasGroup m_CanvasGroup;

    [SerializeField]
    private Animator m_Animator;

    /// <summary>
    /// 顯示/關閉視窗
    /// </summary>
    /// <param name="show"></param>
    public void SetShowWindow(bool show) {
        Image_Blocker.enabled = show;
        m_CanvasGroup.interactable = show;
        m_CanvasGroup.blocksRaycasts = show;
        m_Animator.SetTrigger("Play");
        m_Animator.SetBool("Show", show);
    }

}
