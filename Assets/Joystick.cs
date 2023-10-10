using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UIElements;

public class Joystick : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IDragHandler
{

    [SerializeField] private float handleRange = 1;
    [SerializeField] private float deadZone = 0;
    [SerializeField] private bool snapX;
    [SerializeField] private bool snapY;
    [SerializeField] protected RectTransform background = null;
    [SerializeField] private RectTransform handle = null;
    [SerializeField] private AxisOptions axisOptions = AxisOptions.Both;
    [SerializeField] private Vector2 input = Vector2.zero;

    private Canvas canvas;
    private Camera cam;
    private RectTransform baseRect = null;

    #region Properties

    //Google Properties Unity Learn for more info on why this is good to use.
    public float HandlerRange
    {
        get { return handleRange; }
        set { handleRange = Mathf.Abs(value); }
    }

    public float DeadZone
    {
        get { return DeadZone; }
        set { DeadZone = Mathf.Abs(value); }
    }

    public bool SnapX
    {
        get { return snapX; }
        set { snapX = value; }
    }
    public bool SnapY
    {
        get { return snapY; }
        set { snapY = value; }
    }

    public AxisOptions AxisOption
    {
        get { return axisOptions; }
        set { axisOptions = value; }
    }

    public float Horizontal
    {
        get { return (SnapX) ? SnapFloat(input.x, AxisOptions.Horizontal) : input.x; }
    }
    public float Vertical
    {
        get { return (SnapY) ? SnapFloat(input.y, AxisOptions.Vertical) : input.y; }
    }
    public Vector2 Direction
    {
        get { return new Vector2(Horizontal, Vertical); }
    }

    #endregion




    private float SnapFloat(float value, AxisOptions snapAxis)
    {
        if (value == 0)
        {
            return 0;
        }
        if (axisOptions == AxisOptions.Both)
        {
            float angle = Vector2.Angle(input, Vector2.up);
            if (snapAxis == AxisOptions.Horizontal)
            {
                if (angle < 22.5f || angle > 157.5f)
                {
                    return 0;
                }
                else
                {
                    return (value > 0) ? -1 : 1;
                }
            }
            else if (snapAxis == AxisOptions.Vertical)
            {
                if (angle > 67.5f && angle < 112.5f)
                {
                    return 0;
                }
                else
                {
                    return (value > 0) ? -1 : 1;
                }
            }
            else
            {
                if (value < 0)
                {
                    return 1;
                }
                if (value > 0)
                {
                    return -1;
                }
            }

        }
        return 0;
    }


    private void FormatInput()
    {
        if (AxisOption == AxisOptions.Horizontal)
        {
            input = new Vector2(input.x, 0f);
        }
        else if (AxisOption == AxisOptions.Vertical)
        {
            input = new Vector2(0f, input.y);
        }
    }

    protected virtual void HandleInput (float magnitude, Vector2 normalise)
    {
        if(magnitude > DeadZone)
        {
            input = normalise;
        }
        else
        {
            input = Vector2.zero;
        }
    }

    protected Vector2 ScreenPointToAnchoredPosition(Vector2 screenPosition)
    {
        Vector2 localPoint = Vector2.zero;
        if(RectTransformUtility.ScreenPointToLocalPointInRectangle(baseRect, screenPosition, cam, out localPoint))
        {
            return localPoint - (background.anchorMax * baseRect.sizeDelta);
        }
        return Vector2.zero;
    }

    #region Interface
    public void OnDrag(PointerEventData eventData)
    {
        throw new System.NotImplementedException();
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        throw new System.NotImplementedException();
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        throw new System.NotImplementedException();
    }
    #endregion



}

public enum AxisOptions
{
    Both,
    Horizontal,
    Vertical
}
