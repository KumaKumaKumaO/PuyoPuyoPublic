// ---------------------------------------------------------
// PlayFieldManagerScript.cs
//
// 作成日:10/19
// 更新日:12/15
// 作成者:熊谷航
// --------------------------------------------------------- 

using UnityEngine;
using Interface;

/// <summary>
/// ゲーム時のフィールドを管理する
/// </summary>
public class PlayFieldManagerScript : IGameManagerStateChangable
{
	//全体のオブジェクトを管理するクラス
	private FieldObjectManagerScript _fieldObjectManagerScript = default;
	private GameObject _pauseCanvasObject = default;
	private GameObject _gameOverCanvasObject = default;
	//現在の状態
	private BaseGameStateScript _nowGameState = default;
	//前回のステート
	private BaseGameStateScript _beforeState = default;
	private IInput _input = default;

	private int _canDeletePuyoValue = default;

	public FieldObjectManagerScript FieldObjectManagerScript { get { return _fieldObjectManagerScript; } }

	/// <summary>
	/// 初期化
	/// </summary>
	public PlayFieldManagerScript(IInput input, GameData gameData, ObjectPoolScript objectPoolScript, GameObject pauseCanvas, GameObject gameOverCanvas)
	{
		this._input = input;
		_canDeletePuyoValue = gameData.CanDeletePuyoValue;
		_fieldObjectManagerScript = new FieldObjectManagerScript(gameData, objectPoolScript);
		_pauseCanvasObject = pauseCanvas;
		_gameOverCanvasObject = gameOverCanvas;
		//ゲームオーバーキャンバスを非表示にする
		_gameOverCanvasObject.SetActive(false);
		//ポーズキャンバスを非表示にする
		_pauseCanvasObject.SetActive(false);
		//最初のステートをプレイに設定
		_nowGameState = new PlayStateScript(this, this._input, _fieldObjectManagerScript);
		_nowGameState.Enter();
	}

	/// <summary>
	/// ゲームオーバー状態へ遷移
	/// </summary>
	public void ChangeToGameOverState()
	{
		//終了する
		_nowGameState.Exit();
		//前回のステートを上書きする
		_beforeState = _nowGameState;
		//新しくインスタンスを生成する
		_nowGameState = new GameOverStateScript(this, _input, _gameOverCanvasObject);
		//開始する
		_nowGameState.Enter();
	}

	/// <summary>
	/// ぷよ削除状態へ遷移
	/// </summary>
	public void ChangeToPuyoStayState()
	{
		//終了する
		_nowGameState.Exit();
		//前回のステートを上書きする
		_beforeState = _nowGameState;
		//新しくインスタンスを生成する
		_nowGameState = new StayStateScript(this, _input, _fieldObjectManagerScript, _canDeletePuyoValue);
		//開始する
		_nowGameState.Enter();
	}

	/// <summary>
	/// プレイヤー操作状態へ遷移
	/// </summary>
	public void ChangeToPlayState()
	{
		//終了する
		_nowGameState.Exit();
		//前回のステートを上書きする
		_beforeState = _nowGameState;
		//新しくインスタンスを生成する
		_nowGameState = new PlayStateScript(this, _input, _fieldObjectManagerScript);
		//開始する
		_nowGameState.Enter();
	}

	/// <summary>
	/// ポーズ状態へ遷移
	/// </summary>
	public void ChangeToPauseState()
	{
		//終了する
		_nowGameState.Exit();
		//前回のステートを上書きする
		_beforeState = _nowGameState;
		//新しくインスタンスを生成する
		_nowGameState = new PauseStateScript(this, _input, _pauseCanvasObject);
		//開始する
		_nowGameState.Enter();
	}

	/// <summary>
	/// 一つ前のステートに戻す
	/// </summary>
	public void ChangeToBeforeState()
	{
		//例外処理
		if (_beforeState == null)
		{
			Debug.LogError("前のステートがないためステートを変更しません。");
			return;
		}
		//終了する
		_nowGameState.Exit();
		//現在のステートをバッファする
		BaseGameStateScript nowGameStateTemp = _nowGameState;
		//前回のステートを現在のステートに入れる
		_nowGameState = _beforeState;
		//前回のステートを上書きする
		_beforeState = nowGameStateTemp;
		//開始する
		_nowGameState.Enter();
		//参照を切る
		nowGameStateTemp = null;
	}

	/// <summary>
	/// 現在のステートの実行
	/// </summary>
	public void PlayUpdate()
	{
		_nowGameState.Execute();
	}
}
