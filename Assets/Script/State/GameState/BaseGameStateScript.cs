// ---------------------------------------------------------
// BaseGameStateScript.cs
//
// 作成日:10/19
// 更新日:12/15
// 作成者:熊谷航
// --------------------------------------------------------- 

using Interface;

/// <summary>
/// ゲーム全体のステート
/// </summary>
public abstract class BaseGameStateScript
{

	protected IInput _input = default;
	protected IGameManagerStateChangable _gameManagerStateChangable = default;

	public BaseGameStateScript(IGameManagerStateChangable gameManagerStateChangable, IInput input)
	{
		this._input = input;
		this._gameManagerStateChangable = gameManagerStateChangable;
	}

	/// <summary>
	/// このステートになったときに１度だけ実行する
	/// </summary>
	public virtual void Enter()
	{

	}

	/// <summary>
	/// このステートのとき毎フレーム実行する
	/// </summary>
	public virtual void Execute()
	{
		//ポーズボタンが押されていたら
		if (_input.IsPause())
		{
			//ステートをポーズステートに変更する
			_gameManagerStateChangable.ChangeToPauseState();
		}
	}

	/// <summary>
	/// このステートから他のステートになるときに１度だけ実行する
	/// </summary>
	public virtual void Exit()
	{

	}
}
