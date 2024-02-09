//---------------------------------------------------------
// GameOverStateScript.cs
//
// 作成日:10/19
// 更新日:12/15
// 作成者:熊谷航
// --------------------------------------------------------- 

using Interface;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// ゲームオーバー時のステート
/// </summary>
public class GameOverStateScript : BaseGameStateScript
{
	private GameObject _gameOverCanvasObject = default;

	private string _titleSceneName = "TitleScene";

	public GameOverStateScript(IGameManagerStateChangable gameManagerStateChangable, IInput input, GameObject gameOverCanvasObject) : base(gameManagerStateChangable, input)
	{
		_gameOverCanvasObject = gameOverCanvasObject;
	}

	public override void Enter()
	{
		//全てのゲームオブジェクトの中からカメラ、ゲームマネージャー、オーディオ以外のオブジェクトを消す
		foreach (GameObject obj in Object.FindObjectsOfType(typeof(GameObject)))
		{
			if (obj.GetComponent<Camera>() != null || obj.GetComponent<GameManagerScript>() != null || obj.GetComponent<AudioScript>() != null)
			{
				continue;
			}
			obj.SetActive(false);
		}
		//ゲームオーバー用キャンバスを出す
		_gameOverCanvasObject.SetActive(true);
		//BGMのループを止めて、ゲームオーバーBGMを流す
		AudioScript.InstanceAudioScript.BGMLoopMute();
		AudioScript.InstanceAudioScript.PlayGameOverSound();
	}
	public override void Execute()
	{
		//決定が押されたら
		if (_input.IsSubmit())
		{
			//タイトルシーンをロードする
			SceneManager.LoadScene(_titleSceneName);
		}
	}
	public override void Exit()
	{
		//クラスの参照を切る
		_gameOverCanvasObject = null;
		//BGMをまた再生できるようにする
		AudioScript.InstanceAudioScript.BGMLoopUnMute();
	}
}
