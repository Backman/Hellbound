using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// Input manager, a class to get the state of generic inputs.
/// </summary>
/// By: Aleksi Lindeman
public class InputManager : MonoBehaviour {
	public enum Button
	{
		Forward,
		Backward,
		Left,
		Right,
		Run,
		Crouch,
		Use,
		
		// For xbox controller, Examine and rotate should be the same key,
		// but on pc, rotate should be right mouse button
		Examine,
		Rotate,
		
		Pause
	};
	
	enum ButtonState
	{
		None,
		Pressed,
		Pressing,
		Released,
		Releasing
	};
	
	static Dictionary<Button, ButtonState> m_ButtonState = new Dictionary<Button, ButtonState>();
	static Dictionary<Button, ButtonState> m_StickyButtonState = new Dictionary<Button, ButtonState>();
	static Dictionary<Button, float> m_StickyKeyTiming = new Dictionary<Button, float>();
	public static float m_AxisDeadlock = 0.5f;
	public static float m_StickyKeyUpdateDuration = 0.5f; // 0.1 is 100 ms
	
	void Start(){
		m_ButtonState[Button.Forward] = ButtonState.None;
		m_ButtonState[Button.Backward] = ButtonState.None;
		m_ButtonState[Button.Left] = ButtonState.None;
		m_ButtonState[Button.Right] = ButtonState.None;
		m_ButtonState[Button.Run] = ButtonState.None;
		m_ButtonState[Button.Crouch] = ButtonState.None;
		
		m_ButtonState[Button.Use] = ButtonState.None;
		
		m_ButtonState[Button.Examine] = ButtonState.None;
		m_ButtonState[Button.Rotate] = ButtonState.None;
		
		m_ButtonState[Button.Pause] = ButtonState.None;
		
		
		foreach(Button key in m_ButtonState.Keys){
			m_StickyButtonState[key] = ButtonState.None;
			m_StickyKeyTiming[key] = 0.0f;
		}
	}
	
	void Update(){
		/********************************** VERTICAL (Forward and Backward) ****************************************/
		float verticalAxis = Input.GetAxis("Vertical");
		if(verticalAxis > m_AxisDeadlock || Input.GetKeyDown(KeyCode.W)){
			// Handle forward button state change on press
			if(m_ButtonState[Button.Forward] == ButtonState.None || m_ButtonState[Button.Forward] == ButtonState.Released || m_ButtonState[Button.Forward] == ButtonState.Releasing){
				m_ButtonState[Button.Forward] = ButtonState.Pressed;
				m_StickyKeyTiming[Button.Forward] = Time.time;
			}
			else{
				m_ButtonState[Button.Forward] = ButtonState.Pressing;
			}
		}
		else if( (verticalAxis < m_AxisDeadlock && verticalAxis > -m_AxisDeadlock) || Input.GetKeyUp(KeyCode.W) ){
			// Handle forward button state change on release
			if(m_ButtonState[Button.Forward] == ButtonState.Pressed || m_ButtonState[Button.Forward] == ButtonState.Pressing){
				m_ButtonState[Button.Forward] = ButtonState.Released;
			}
			else if(m_ButtonState[Button.Forward] == ButtonState.Released){
				m_ButtonState[Button.Forward] = ButtonState.Releasing;
			}
		}
		
		if(verticalAxis < -m_AxisDeadlock || Input.GetKeyDown(KeyCode.S)){
			// Handle backward button state change on press
			if(m_ButtonState[Button.Backward] == ButtonState.None || m_ButtonState[Button.Backward] == ButtonState.Released || m_ButtonState[Button.Backward] == ButtonState.Releasing){
				m_ButtonState[Button.Backward] = ButtonState.Pressed;
				m_StickyKeyTiming[Button.Backward] = Time.time;
			}
			else{
				m_ButtonState[Button.Backward] = ButtonState.Pressing;
			}
		}
		else if( (verticalAxis < m_AxisDeadlock && verticalAxis > -m_AxisDeadlock) || Input.GetKeyUp(KeyCode.S) ){
			// Handle backward button state change on release
			if(m_ButtonState[Button.Backward] == ButtonState.Pressed || m_ButtonState[Button.Backward] == ButtonState.Pressing){
				m_ButtonState[Button.Backward] = ButtonState.Released;
			}
			else if(m_ButtonState[Button.Backward] == ButtonState.Released){
				m_ButtonState[Button.Backward] = ButtonState.Releasing;
			}
		}
		
		
		
		/********************************** HORIZONTAL (Left and Right) ****************************************/
		float horizontalAxis = Input.GetAxis("Horizontal");
		if(horizontalAxis > m_AxisDeadlock || Input.GetKeyDown(KeyCode.D)){
			// Handle forward button state change on press
			if(m_ButtonState[Button.Right] == ButtonState.None || m_ButtonState[Button.Right] == ButtonState.Released || m_ButtonState[Button.Right] == ButtonState.Releasing){
				m_ButtonState[Button.Right] = ButtonState.Pressed;
				m_StickyKeyTiming[Button.Right] = Time.time;
			}
			else{
				m_ButtonState[Button.Right] = ButtonState.Pressing;
			}
		}
		else if( (horizontalAxis < m_AxisDeadlock && horizontalAxis > -m_AxisDeadlock) || Input.GetKeyUp(KeyCode.D) ){
			// Handle forward button state change on release
			if(m_ButtonState[Button.Right] == ButtonState.Pressed || m_ButtonState[Button.Right] == ButtonState.Pressing){
				m_ButtonState[Button.Right] = ButtonState.Released;
			}
			else if(m_ButtonState[Button.Right] == ButtonState.Released){
				m_ButtonState[Button.Right] = ButtonState.Releasing;
			}
		}
		
		if(horizontalAxis < -m_AxisDeadlock || Input.GetKeyDown(KeyCode.A)){
			// Handle backward button state change on press
			if(m_ButtonState[Button.Left] == ButtonState.None || m_ButtonState[Button.Left] == ButtonState.Released || m_ButtonState[Button.Left] == ButtonState.Releasing){
				m_ButtonState[Button.Left] = ButtonState.Pressed;
				m_StickyKeyTiming[Button.Left] = Time.time;
			}
			else{
				m_ButtonState[Button.Left] = ButtonState.Pressing;
			}
		}
		else if( (horizontalAxis < m_AxisDeadlock && horizontalAxis > -m_AxisDeadlock) || Input.GetKeyUp(KeyCode.A) ){
			// Handle backward button state change on release
			if(m_ButtonState[Button.Left] == ButtonState.Pressed || m_ButtonState[Button.Left] == ButtonState.Pressing){
				m_ButtonState[Button.Left] = ButtonState.Released;
			}
			else if(m_ButtonState[Button.Left] == ButtonState.Released){
				m_ButtonState[Button.Left] = ButtonState.Releasing;
			}
		}
		
		
		
		/********************************** STATIC KEYS (Run, Crouch, Use, Examine, Rotate and Pause) ****************************************/
		if(Input.GetButtonDown("Run")){
			m_ButtonState[Button.Run] = ButtonState.Pressed;
			m_StickyKeyTiming[Button.Run] = Time.time;
		}
		else{
			if(m_ButtonState[Button.Run] == ButtonState.Pressed){
				m_ButtonState[Button.Run] = ButtonState.Pressing;
			}
		}
		
		if(Input.GetButtonUp("Run")){
			m_ButtonState[Button.Run] = ButtonState.Released;
		}
		else{
			if(m_ButtonState[Button.Run] == ButtonState.Released){
				m_ButtonState[Button.Run] = ButtonState.Releasing;
			}
		}
		
		
		if(Input.GetButtonDown("Crouch")){
			m_ButtonState[Button.Crouch] = ButtonState.Pressed;
			m_StickyKeyTiming[Button.Crouch] = Time.time;
		}
		else{
			if(m_ButtonState[Button.Crouch] == ButtonState.Pressed){
				m_ButtonState[Button.Crouch] = ButtonState.Pressing;
			}
		}
		
		if(Input.GetButtonUp("Crouch")){
			m_ButtonState[Button.Crouch] = ButtonState.Released;
		}
		else{
			if(m_ButtonState[Button.Crouch] == ButtonState.Released){
				m_ButtonState[Button.Crouch] = ButtonState.Releasing;
			}
		}
		
		
		if(Input.GetButtonDown("Use")){
			m_ButtonState[Button.Use] = ButtonState.Pressed;
			m_StickyKeyTiming[Button.Use] = Time.time;
		}
		else{
			if(m_ButtonState[Button.Use] == ButtonState.Pressed){
				m_ButtonState[Button.Use] = ButtonState.Pressing;
			}
		}
		
		if(Input.GetButtonUp("Use")){
			m_ButtonState[Button.Use] = ButtonState.Released;
		}
		else{
			if(m_ButtonState[Button.Use] == ButtonState.Released){
				m_ButtonState[Button.Use] = ButtonState.Releasing;
			}
		}
		
		
		// Mouse button 1 is right mouse button
		if(Input.GetButtonDown("Examine") || Input.GetMouseButtonDown(1)){
			if(m_ButtonState[Button.Examine] != ButtonState.Pressing){
				m_ButtonState[Button.Examine] = ButtonState.Pressed;
				m_ButtonState[Button.Rotate] = ButtonState.Pressed;
				m_StickyKeyTiming[Button.Examine] = Time.time;
				m_StickyKeyTiming[Button.Rotate] = Time.time;
			}
		}
		else{
			if(m_ButtonState[Button.Examine] == ButtonState.Pressed){
				m_ButtonState[Button.Examine] = ButtonState.Pressing;
				m_ButtonState[Button.Rotate] = ButtonState.Pressing;
			}
		}
		
		// Mouse button 1 is right mouse button
		if(Input.GetButtonUp("Examine") || Input.GetMouseButtonUp(1)){
			if(m_ButtonState[Button.Examine] != ButtonState.Releasing){
				m_ButtonState[Button.Examine] = ButtonState.Released;
				m_ButtonState[Button.Rotate] = ButtonState.Released;
			}
		}
		else{
			if(m_ButtonState[Button.Examine] == ButtonState.Released){
				m_ButtonState[Button.Examine] = ButtonState.Releasing;
				m_ButtonState[Button.Rotate] = ButtonState.Releasing;
			}
		}
		
		
		if(Input.GetButtonDown("Pause")){
			m_ButtonState[Button.Pause] = ButtonState.Pressed;
			m_StickyKeyTiming[Button.Pause] = Time.time;
		}
		else{
			if(m_ButtonState[Button.Pause] == ButtonState.Pressed){
				m_ButtonState[Button.Pause] = ButtonState.Pressing;
			}
		}
		
		if(Input.GetButtonUp("Pause")){
			m_ButtonState[Button.Pause] = ButtonState.Released;
		}
		else{
			if(m_ButtonState[Button.Pause] == ButtonState.Released){
				m_ButtonState[Button.Pause] = ButtonState.Releasing;
			}
		}
		
		
		
		/**************************** HANDLE STICKY KEY TIMINGS *********************************/
		foreach(KeyValuePair<Button, ButtonState> entry in m_ButtonState){
			if(entry.Value == ButtonState.Pressed){
				m_StickyButtonState[entry.Key] = entry.Value;
			}
			else if(entry.Value == ButtonState.Pressing){
				bool setToPressed = false;
				if(Time.time - m_StickyKeyTiming[entry.Key] > m_StickyKeyUpdateDuration){
					if(m_StickyButtonState[entry.Key] == ButtonState.Pressed){
						m_StickyButtonState[entry.Key] = ButtonState.Pressing;
					}
					else{
						m_StickyButtonState[entry.Key] = ButtonState.Pressed;
						setToPressed = true;
					}
					m_StickyKeyTiming[entry.Key] = Time.time;
				}
				
				if(!setToPressed && m_StickyButtonState[entry.Key] == ButtonState.Pressed){
					m_StickyButtonState[entry.Key] = ButtonState.Pressing;
				}
			}
			else{
				m_StickyButtonState[entry.Key] = entry.Value;
			}
		}
	}
	
	public static bool getButtonDown(Button button, bool repeat = false){
		if(repeat){
			return m_StickyButtonState[button] == ButtonState.Pressed;
		}
		return m_ButtonState[button] == ButtonState.Pressed;
	}
	
	public static bool getButtonUp(Button button){
		return m_ButtonState[button] == ButtonState.Released;
	}
	
	public static bool getButton(Button button){
		return m_ButtonState[button] == ButtonState.Pressed || m_ButtonState[button] == ButtonState.Pressing;
	}
}
