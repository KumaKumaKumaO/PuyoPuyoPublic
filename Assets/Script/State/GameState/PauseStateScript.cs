// ---------------------------------------------------------
// PauseStateScript.cs
//
// 作成日:10/19
// 更新日:12/15
// 作成者:熊谷航
// --------------------------------------------------------- 

using Interface;
using UnityEngine;

/// <summary>
/// ポーズステート
/// </summary>
public class PauseStateScript : BaseGameStateScript
{
	private GameObject _pauseCanvas = default;

	public PauseStateScript(IGameManagerStateChangable gameManagerStateChangable
		, IInput input, GameObject pauseCanvasObject)
		: base(gameManagerStateChangable, input)
	{
		_pauseCanvas = pauseCanvasObject;
	}

	public override void Enter()
	{
		//ポーズキャンバスを表示する
		_pauseCanvas.SetActive(true);
	}

	public override void Execute()
	{
		//ポーズボタンが押された場合
		if (_input.IsPause())
		{
			//前のステートに戻す
			_gameManagerStateChangable.ChangeToBeforeState();
		}

	}

	public override void Exit()
	{
		//ポーズキャンバスを消す
		_pauseCanvas.SetActive(false);
		//ポーズキャンバスの参照を切る
		_pauseCanvas = null;
	}
}
