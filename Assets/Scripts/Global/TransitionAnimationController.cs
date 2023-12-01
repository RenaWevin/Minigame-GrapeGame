
using System.Threading.Tasks;
using UnityEngine;

public class TransitionAnimationController : MonoBehaviour {

    public static TransitionAnimationController Instance;

    [SerializeField]
    private Animator animator;

    [SerializeField]
    private CanvasGroup canvasGroup;

    private void Awake() {
        Instance = this;
    }

    public async Task Play_Show() {
        canvasGroup.SetEnable(true);
        animator.SetTrigger("Show");
        await Task.Delay(1000);
    }

    public async Task Play_Hide() {
        canvasGroup.SetEnable(true);
        animator.SetTrigger("Hide");
        await Task.Delay(1000);
    }

}
