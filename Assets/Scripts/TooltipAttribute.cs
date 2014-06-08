using UnityEngine;

/// <summary>
/// Tooltip attribute.
/// 
/// Used for enabeling drawing of tooltips
/// 
/// Created by Simon Jonasson
/// </summary>
public class TooltipAttribute : PropertyAttribute
{
	public readonly string text;
	
	public TooltipAttribute(string text)
	{
		this.text = text;
	}
}