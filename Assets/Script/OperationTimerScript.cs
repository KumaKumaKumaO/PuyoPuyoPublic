// ---------------------------------------------------------
// OperationTimerScript.cs
//
// 作成日:10/19
// 更新日:12/15
// 作成者:熊谷航
// --------------------------------------------------------- 

using UnityEngine;

/// <summary>
/// タイマーを管理するスクリプト
/// </summary>
public class OperationTimerScript
{
	private float _fallTime = 1.5f;
	private float _startTime = default;
	private TimerState _myState = default;

	/// <summary>
	/// タイマーの状態を制御する
	/// </summary>
	/// <returns>現在の状態</returns>
	public TimerState TimerStateUpdate()
	{
		switch (_myState)
		{
			//動作中だったら
			case TimerState.Processing:
				{
					//特定の時間をすぎたか
					if (Time.time >= _startTime + _fallTime)
					{
						//タイマーを終了に変更する
						_myState = TimerState.Termination;
					}
					break;
				}
			//終了したら
			case TimerState.Termination:
				{
					//タイマーを初期化する
					TimerInitialization();
					break;
				}
		}
		return _myState;
	}

	/// <summary>
	/// タイマーの初期化
	/// </summary>
	public void TimerInitialization()
	{
		//現在の時間を保存
		_startTime = Time.time;
		//タイマーのステートを動作中に変更
		_myState = TimerState.Processing;
	}
}
