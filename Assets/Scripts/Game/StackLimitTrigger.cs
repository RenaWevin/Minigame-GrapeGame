
using UnityEngine;
using UnityEngine.Events;

public class StackLimitTrigger : MonoBehaviour {

    public readonly UnityEvent onFruitTouch = new UnityEvent();

    private void OnTriggerStay2D(Collider2D collision) {
        FruitObject fruit = collision.gameObject.GetComponent<FruitObject>();
        if (fruit != null) {
            if (fruit.isTouchedAnotherFruit) {
                onFruitTouch.Invoke();
            }
        }
    }

}
