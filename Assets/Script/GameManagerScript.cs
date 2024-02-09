// ---------------------------------------------------------
// GameManagerScript.cs
//
// 作成日:10/19
// 更新日:12/15
// 作成者:熊谷航
// --------------------------------------------------------- 

using UnityEngine;

/// <summary>
/// ゲームマネージャー
/// </summary>
public class GameManagerScript : MonoBehaviour
{
	[SerializeField]
	private GameObject _wallPrefab = default;
	[SerializeField]
	private GameObject _gameOverZonePrefab = default;

	[SerializeField, Tooltip("左右の壁")]
	private int _stageRowLength = 6;
	[SerializeField, Tooltip("高さ")]
	private int _stageColLength = 12;
	[SerializeField]
	private int canDeletePuyoValue = 4;
	[SerializeField]
	private int _fieldCount = 1;
	[SerializeField]
	private int[] _playerPlayFieldNumber = default;
	[SerializeField]
	private Vector2 _initPos = new Vector2(-2, -7);

	private PlayFieldManagerScript[] _playFieldManagers = default;

	private const int STAGE_WALL_RIGHTSIDE_SIZE = 1;
	private const int STAGE_WALL_LEFTSIDE_SIZE = 1;
	private const int STAGE_NEXT_ROW_SIZE = 2;
	private const int STAGE_WALL_BOTTOMSIDE_SIZE = 1;

	public PlayFieldManagerScript[] playField { get { return _playFieldManagers; } }

	private void Start()
	{
		//Canvasをそれぞれ取得する
		GameObject pauseCanvasObject = GameObject.FindWithTag("PauseCanvas");
		if (pauseCanvasObject == null)
		{
			Debug.LogError("エラー：Canvasオブジェクトが取得できませんでした。");
		}
		GameObject gameOverCanvasObject = GameObject.FindWithTag("GameOverCanvas");
		if (gameOverCanvasObject == null)
		{
			Debug.LogError("エラー：Canvasオブジェクトが取得できませんでした。");
		}
		//ObjectPool用のクラスを取得する
		ObjectPoolScript objectPoolScript = GetComponent<ObjectPoolScript>();
		//ObjectPoolを初期化する
		objectPoolScript.Initialization(_fieldCount, _stageColLength * _stageRowLength);

		GameData gameData = default;
		//フィールドの個数分だけ配列を作成する
		_playFieldManagers = new PlayFieldManagerScript[_fieldCount];
		Vector2 nextPos = default;
		Vector2 popPos = default;
		FieldDataScript fieldDataScript = default;
		//フィールドの個数分のフィールドを生成する
		for (int i = 0; i < _fieldCount; i++)
		{
			//操作開始ポジションを作成
			popPos = _initPos + Vector2.up * (_stageColLength) + Vector2.right * (_stageRowLength / 2 + _stageColLength % 2);
			//準備ポジションを作成
			nextPos = _initPos + Vector2.up * _stageColLength
				+ Vector2.right * (_stageRowLength + STAGE_WALL_LEFTSIDE_SIZE + STAGE_WALL_RIGHTSIDE_SIZE);
			//配列データを作成
			fieldDataScript = new FieldDataScript(_stageRowLength + STAGE_WALL_LEFTSIDE_SIZE + STAGE_WALL_RIGHTSIDE_SIZE
				, _stageColLength + STAGE_WALL_BOTTOMSIDE_SIZE + 2, STAGE_WALL_LEFTSIDE_SIZE + STAGE_WALL_RIGHTSIDE_SIZE, STAGE_WALL_BOTTOMSIDE_SIZE);
			//ステージを生成
			InstanceStage(_initPos, fieldDataScript, popPos);
			//初期化用のデータを作成
			gameData = new GameData(canDeletePuyoValue, nextPos, popPos, fieldDataScript);
			//プレイヤーの番号と入力を結び付ける
			foreach (int item in _playerPlayFieldNumber)
			{
				if (item == i)
				{
					//プレイヤー用のフィールドを生成
					_playFieldManagers[i] = new PlayFieldManagerScript(new PlayerInputScript(), gameData, objectPoolScript
						, pauseCanvasObject, gameOverCanvasObject);
					continue;
				}
				//AIや複数人プレイには対応していないため複数フィールドを作っても操作が同じになる
				_playFieldManagers[i] = new PlayFieldManagerScript(new PlayerInputScript(), gameData, objectPoolScript
					, pauseCanvasObject, gameOverCanvasObject);
			}
			//初期化用ポジションをずらす
			_initPos += Vector2.right * (STAGE_WALL_LEFTSIDE_SIZE + STAGE_WALL_RIGHTSIDE_SIZE + _stageRowLength + STAGE_NEXT_ROW_SIZE);
		}
		//使わない参照を切る
		objectPoolScript = null;
		pauseCanvasObject = null;
		gameOverCanvasObject = null;
		fieldDataScript = null;
	}

	private void Update()
	{
		//全てのフィールドを動かす
		foreach (PlayFieldManagerScript item in _playFieldManagers)
		{
			item.PlayUpdate();
		}
	}

	/// <summary>
	/// ステージの生成
	/// </summary>
	/// <param name="fieldBottomLeftCorner">ステージを生成するときの基準</param>
	/// <param name="fieldDataScript">配列データ</param>
	/// <param name="popPos">ポップする場所</param>
	private void InstanceStage(Vector2 fieldBottomLeftCorner, FieldDataScript fieldDataScript, Vector2 popPos)
	{
		int rowLength = STAGE_WALL_LEFTSIDE_SIZE + STAGE_WALL_RIGHTSIDE_SIZE + _stageRowLength;
		//一番下の段の生成
		for (int i = 0; i < STAGE_WALL_BOTTOMSIDE_SIZE; i++)
		{
			for (int k = 0; k < rowLength; k++)
			{
				Instantiate(_wallPrefab, fieldBottomLeftCorner + k * Vector2.right + i * Vector2.up, Quaternion.identity).name = "BottomWall";
				fieldDataScript.SetFieldArrayData(k, i, FieldDataType.Wall);
			}
		}
		//左側の壁の生成
		for (int i = 0; i < STAGE_WALL_LEFTSIDE_SIZE; i++)
		{
			for (int k = 1; k <= _stageColLength; k++)
			{
				Instantiate(_wallPrefab, fieldBottomLeftCorner + Vector2.left * i + Vector2.up * k, Quaternion.identity).name = "LeftWall";
				fieldDataScript.SetFieldArrayData(i, k, FieldDataType.Wall);
			}
		}
		//右側の壁の生成
		for (int i = 1; i <= STAGE_WALL_RIGHTSIDE_SIZE; i++)
		{
			for (int k = 1; k <= _stageColLength; k++)
			{
				Instantiate(_wallPrefab, fieldBottomLeftCorner + Vector2.right * (i + _stageRowLength) + Vector2.up * k, Quaternion.identity).name = "RightWall";
				fieldDataScript.SetFieldArrayData(i + _stageRowLength, k, FieldDataType.Wall);
			}
		}
		Instantiate(_gameOverZonePrefab, popPos, Quaternion.identity);
	}

}
