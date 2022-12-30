using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattlerNotificationImpl : IBattlerNotification
{
    private Color defaultColor = Color.yellow; // Todo, move this color to a constants file
    private string message;
    private Color color;

    public BattlerNotificationImpl(string message, Color? color = null)
    {
        this.message = message;
        this.color = (Color)(color == null ? defaultColor : color);
    }

    public string GetMessage()
    {
        return message;
    }

    public Color GetColor()
    {
        return color;
    }
}
