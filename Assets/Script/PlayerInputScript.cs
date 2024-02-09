// ---------------------------------------------------------
// PlayerInputScript.cs
//
// 作成日:10/19
// 更新日:12/15
// 作成者:熊谷航
// --------------------------------------------------------- 

using UnityEngine;
using Interface;

/// <summary>
/// プレイヤーの入力
/// </summary>
public class PlayerInputScript : IInput
{
	private bool isPause = default;
	private bool isEnter = default;
	private bool isRightMove = default;
	private bool isLeftMove = default;
	private bool isHardDrop = default;
	private bool isDownMove = default;

	public bool IsPause()
	{
		if (!isPause && Input.GetAxis("Cancel") != 0)
		{
			isPause = true;
			return true;
		}
		else if (isPause && Input.GetAxis("Cancel") == 0)
		{
			isPause = false;
		}
		return false;
	}

	public bool IsSubmit()
	{
		if (!isEnter && Input.GetAxis("Submit") != 0)
		{
			isEnter = true;
			return true;
		}
		else if (isEnter && Input.GetAxis("Submit") == 0)
		{
			isEnter = false;
		}
		return false;
	}

	public bool IsRightMove()
	{
		if (!isRightMove && Input.GetAxisRaw("Horizontal") > 0)
		{
			isRightMove = true;
			return true;
		}
		else if (isRightMove && !(Input.GetAxisRaw("Horizontal") > 0))
		{
			isRightMove = false;
		}
		return false;
	}
	public bool IsLeftMove()
	{
		if (!isLeftMove && Input.GetAxisRaw("Horizontal") < 0)
		{
			isLeftMove = true;
			return true;
		}
		else if (isLeftMove && !(Input.GetAxisRaw("Horizontal") < 0))
		{
			isLeftMove = false;
		}
		return false;
	}

	public bool IsHardDrop()
	{
		if (!isHardDrop && Input.GetAxisRaw("Vertical") > 0)
		{
			isHardDrop = true;
			return true;
		}
		else if (isHardDrop && !(Input.GetAxisRaw("Vertical") > 0))
		{
			isHardDrop = false;
		}
		return false;
	}
	public bool IsDownMove()
	{
		if (!isDownMove && Input.GetAxisRaw("Vertical") < 0)
		{
			isDownMove = true;
			return true;
		}
		else if (isDownMove && !(Input.GetAxisRaw("Vertical") < 0))
		{
			isDownMove = false;
		}
		return false;
	}

	public bool IsRightTurn()
	{
		return (Input.GetKeyDown(KeyCode.E) || Input.GetKeyDown("joystick button 5"));
	}

	public bool IsLeftTurn()
	{
		return (Input.GetKeyDown(KeyCode.Q) || Input.GetKeyDown("joystick button 4"));
	}
}