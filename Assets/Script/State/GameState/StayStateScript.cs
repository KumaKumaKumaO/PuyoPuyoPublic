// ---------------------------------------------------------
// StayStateScript.cs
//
// 作成日:10/19
// 更新日:12/15
// 作成者:熊谷航
// --------------------------------------------------------- 

using Interface;
using UnityEngine;

/// <summary>
/// プレイヤーが操作できないステート
/// </summary>
public class StayStateScript : BaseGameStateScript
{
	private float _deleteLagTime = 0.6f;
	private float _updateLagTime = 0.4f;
	private float _startTime = default;

	private GameRuleScript gameRuleScript = default;
	private IFieldObjectUpdatable _fieldObjectUpdatable = default;
	public StayStateScript(IGameManagerStateChangable gameManagerStateChangable, IInput input
		, FieldObjectManagerScript fieldObjectManagerScript, int canDeletePuyoValue) : base(gameManagerStateChangable, input)
	{
		//エラー処理いれる
		FieldDataScript fieldDataScript = fieldObjectManagerScript.FieldDataScript;
		gameRuleScript = new GameRuleScript(fieldDataScript, fieldObjectManagerScript, canDeletePuyoValue);
		_fieldObjectUpdatable = fieldObjectManagerScript;
	}

	public override void Enter()
	{
		//消せるぷよが見つからなかった場合
		if (!gameRuleScript.ContainDeletePuyo())
		{
			//ゲームオーバー条件を満たした場合
			if (gameRuleScript.IsGameOver())
			{
				//ゲームオーバーステートに変更
				_gameManagerStateChangable.ChangeToGameOverState();
				return;
			}
			//操作ステートに変更
			_gameManagerStateChangable.ChangeToPlayState();
			return;
		}
		
		//操作ができないステートに変更された時間を記録
		_startTime = Time.time;
	}

	public override void Execute()
	{
		base.Execute();
		//削除後に更新の時間になった場合
		if (_startTime + _updateLagTime + _deleteLagTime <= Time.time)
		{
			//フィールドにあるぷよをの位置を更新する
			_fieldObjectUpdatable.UpdateFieldObject();
			//タイマーを初期化する
			_startTime = Time.time;
			//削除可能ぷよが他に存在しない場合
			if (!gameRuleScript.ContainDeletePuyo())
			{
				
				//前のステートに変更する
				_gameManagerStateChangable.ChangeToBeforeState();
				return;
			}
			return;
		}
		//削除までの時間になった場合
		else if (_startTime + _deleteLagTime <= Time.time)
		{
			//ぷよを消す
			gameRuleScript.DeletePuyo();
			//消す時の音再生
			AudioScript.InstanceAudioScript.PlayDeleteSound();
			return;
		}
	}
}
