// ---------------------------------------------------------
// PlayStateScript.cs
//
// 作成日:10/19
// 更新日:12/15
// 作成者:熊谷航
// --------------------------------------------------------- 

using Interface;

/// <summary>
/// プレイヤーが操作できるステート
/// </summary>
public class PlayStateScript : BaseGameStateScript
{
	private PuyoOperateScript _puyoOperateScript = default;
	private ICompositePuyoStateChackable _compositePuyoStateChackable = default;

	public PlayStateScript(IGameManagerStateChangable gameManagerStateChangable, IInput input
		, FieldObjectManagerScript fieldObjectManagerScript) : base(gameManagerStateChangable, input)
	{
		_puyoOperateScript = new PuyoOperateScript();
		_compositePuyoStateChackable = fieldObjectManagerScript.MyCompositePuyoScript;
		INextPuyoPopable _nextPuyoPopable = fieldObjectManagerScript;
		_nextPuyoPopable.NextPuyoPop();
	}

	public override void Execute()
	{
		base.Execute();


		switch (_compositePuyoStateChackable.CompositePuyoState)
		{
			//動かせる場合
			case CompositePuyoState.CanMoving:
				{
					//ぷよを操作する
					_puyoOperateScript.PuyoControl((CompositePuyoScript)_compositePuyoStateChackable, _input);
					break;
				}
			//動かすのを終了する場合
			case CompositePuyoState.End:
				{
					//ステートを待機に変更
					_gameManagerStateChangable.ChangeToPuyoStayState();
					break;
				}
		}
	}
}
