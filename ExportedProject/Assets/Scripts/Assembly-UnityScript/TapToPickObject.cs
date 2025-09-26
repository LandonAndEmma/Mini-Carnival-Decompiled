using System;
using UnityEngine;

[Serializable]
public class TapToPickObject : MonoBehaviour
{
    public TextMesh textField;

    public virtual void OnEnable()
    {
        FingerGestures.OnFingerDown += FingerGestures_OnFingerDown;
    }

    public virtual void OnDisable()
    {
        FingerGestures.OnFingerDown -= FingerGestures_OnFingerDown;
    }

    public virtual void FingerGestures_OnFingerDown(int fingerIndex, Vector2 fingerPos)
    {
        GameObject picked = PickObject(fingerPos);
        if (picked != null)
        {
            DisplayText("You pressed " + picked.name);
        }
        else
        {
            DisplayText("You didn't pressed any object");
        }
    }

    public virtual void DisplayText(object text)
    {
        if (textField != null)
        {
            textField.text = Convert.ToString(text);
        }
        else
        {
            Debug.Log(text);
        }
    }

    public virtual GameObject PickObject(Vector2 screenPos)
    {
        Ray ray = Camera.main.ScreenPointToRay(screenPos);
        RaycastHit hitInfo;
        return Physics.Raycast(ray, out hitInfo) ? hitInfo.collider.gameObject : null;
    }

    public virtual void Main()
    {
    }
}
