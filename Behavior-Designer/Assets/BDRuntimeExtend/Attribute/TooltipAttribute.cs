using System;

[AttributeUsage(AttributeTargets.Field, AllowMultiple = false, Inherited = false)]
public class TooltipAttribute : Attribute
{
    public readonly string mTooltip;

    public TooltipAttribute(string tooltip) { mTooltip = tooltip; }

    public string Tooltip { get { return mTooltip; } }
}
