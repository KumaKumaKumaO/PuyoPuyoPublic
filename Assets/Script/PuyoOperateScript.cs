// ---------------------------------------------------------
// PuyoOperateScript.cs
//
// 作成日:10/19
// 更新日:12/15
// 作成者:熊谷航
// --------------------------------------------------------- 

using UnityEngine;
using Interface;

/// <summary>
/// 入力を伝えるクラス
/// </summary>
public class PuyoOperateScript
{
	//タイマー
	private OperationTimerScript _timerScript = default;

	public PuyoOperateScript()
	{
		_timerScript = new OperationTimerScript();
		_timerScript.TimerInitialization();
	}

	/// <summary>
	/// ぷよを操作する
	/// </summary>
	/// <param name="compositePuyoOperatable">操作したいぷよのまとまり</param>
	/// <param name="input">入力</param>
	public void PuyoControl(ICompositePuyoOperatable compositePuyoOperatable, IInput input)
	{
		//タイマーが終了したら
		if (_timerScript.TimerStateUpdate() == TimerState.Termination)
		{
			//１つ下げる
			compositePuyoOperatable.MoveCompositePuyo(Vector2.down);
			//タイマーを初期化する
			_timerScript.TimerInitialization();
		}

		//右に移動が押されていたら
		if (input.IsRightMove())
		{
			//右に移動する
			compositePuyoOperatable.MoveCompositePuyo(Vector2.right);
		}

		//左に移動が押されていたら
		if (input.IsLeftMove())
		{
			//左に移動する
			compositePuyoOperatable.MoveCompositePuyo(Vector2.left);
		}

		//下に移動が押されていたら
		if (input.IsDownMove())
		{
			//下に移動する
			compositePuyoOperatable.MoveCompositePuyo(Vector2.down);
			//タイマーを初期化する
			_timerScript.TimerInitialization();
		}
		//ハードドロップが押されていたら
		else if (input.IsHardDrop())
		{
			//ハードドロップをする
			compositePuyoOperatable.HardDropCompositePuyo();
		}

		//右に回転が押されていたら
		if (input.IsRightTurn())
		{
			//右に回転する
			compositePuyoOperatable.RotationCompositePuyo(RotateDirection.Right);
		}

		//左に回転が押されていたら
		if (input.IsLeftTurn())
		{
			//左に回転する
			compositePuyoOperatable.RotationCompositePuyo(RotateDirection.Left);
		}

	}
}
