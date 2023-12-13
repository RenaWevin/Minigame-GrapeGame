
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PointerUpDownButton : Selectable {

    protected PointerUpDownButton() { }

    public readonly UnityEvent onPointerDown = new UnityEvent();
    public readonly UnityEvent onPointerUp = new UnityEvent();

    public override void OnPointerDown(PointerEventData eventData) {
        if (eventData.button == PointerEventData.InputButton.Left) {
            onPointerDown.Invoke();
        }
        base.OnPointerDown(eventData);
    }

    public override void OnPointerUp(PointerEventData eventData) {
        if (eventData.button == PointerEventData.InputButton.Left) {
            onPointerUp.Invoke();
        }
        base.OnPointerUp(eventData);
    }

}
