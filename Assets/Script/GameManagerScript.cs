// ---------------------------------------------------------
// GameManagerScript.cs
//
// �쐬��:10/19
// �X�V��:12/15
// �쐬��:�F�J�q
// --------------------------------------------------------- 

using UnityEngine;

/// <summary>
/// �Q�[���}�l�[�W���[
/// </summary>
public class GameManagerScript : MonoBehaviour
{
	[SerializeField]
	private GameObject _wallPrefab = default;
	[SerializeField]
	private GameObject _gameOverZonePrefab = default;

	[SerializeField, Tooltip("���E�̕�")]
	private int _stageRowLength = 6;
	[SerializeField, Tooltip("����")]
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
		//Canvas�����ꂼ��擾����
		GameObject pauseCanvasObject = GameObject.FindWithTag("PauseCanvas");
		if (pauseCanvasObject == null)
		{
			Debug.LogError("�G���[�FCanvas�I�u�W�F�N�g���擾�ł��܂���ł����B");
		}
		GameObject gameOverCanvasObject = GameObject.FindWithTag("GameOverCanvas");
		if (gameOverCanvasObject == null)
		{
			Debug.LogError("�G���[�FCanvas�I�u�W�F�N�g���擾�ł��܂���ł����B");
		}
		//ObjectPool�p�̃N���X���擾����
		ObjectPoolScript objectPoolScript = GetComponent<ObjectPoolScript>();
		//ObjectPool������������
		objectPoolScript.Initialization(_fieldCount, _stageColLength * _stageRowLength);

		GameData gameData = default;
		//�t�B�[���h�̌��������z����쐬����
		_playFieldManagers = new PlayFieldManagerScript[_fieldCount];
		Vector2 nextPos = default;
		Vector2 popPos = default;
		FieldDataScript fieldDataScript = default;
		//�t�B�[���h�̌����̃t�B�[���h�𐶐�����
		for (int i = 0; i < _fieldCount; i++)
		{
			//����J�n�|�W�V�������쐬
			popPos = _initPos + Vector2.up * (_stageColLength) + Vector2.right * (_stageRowLength / 2 + _stageColLength % 2);
			//�����|�W�V�������쐬
			nextPos = _initPos + Vector2.up * _stageColLength
				+ Vector2.right * (_stageRowLength + STAGE_WALL_LEFTSIDE_SIZE + STAGE_WALL_RIGHTSIDE_SIZE);
			//�z��f�[�^���쐬
			fieldDataScript = new FieldDataScript(_stageRowLength + STAGE_WALL_LEFTSIDE_SIZE + STAGE_WALL_RIGHTSIDE_SIZE
				, _stageColLength + STAGE_WALL_BOTTOMSIDE_SIZE + 2, STAGE_WALL_LEFTSIDE_SIZE + STAGE_WALL_RIGHTSIDE_SIZE, STAGE_WALL_BOTTOMSIDE_SIZE);
			//�X�e�[�W�𐶐�
			InstanceStage(_initPos, fieldDataScript, popPos);
			//�������p�̃f�[�^���쐬
			gameData = new GameData(canDeletePuyoValue, nextPos, popPos, fieldDataScript);
			//�v���C���[�̔ԍ��Ɠ��͂����ѕt����
			foreach (int item in _playerPlayFieldNumber)
			{
				if (item == i)
				{
					//�v���C���[�p�̃t�B�[���h�𐶐�
					_playFieldManagers[i] = new PlayFieldManagerScript(new PlayerInputScript(), gameData, objectPoolScript
						, pauseCanvasObject, gameOverCanvasObject);
					continue;
				}
				//AI�╡���l�v���C�ɂ͑Ή����Ă��Ȃ����ߕ����t�B�[���h������Ă����삪�����ɂȂ�
				_playFieldManagers[i] = new PlayFieldManagerScript(new PlayerInputScript(), gameData, objectPoolScript
					, pauseCanvasObject, gameOverCanvasObject);
			}
			//�������p�|�W�V���������炷
			_initPos += Vector2.right * (STAGE_WALL_LEFTSIDE_SIZE + STAGE_WALL_RIGHTSIDE_SIZE + _stageRowLength + STAGE_NEXT_ROW_SIZE);
		}
		//�g��Ȃ��Q�Ƃ�؂�
		objectPoolScript = null;
		pauseCanvasObject = null;
		gameOverCanvasObject = null;
		fieldDataScript = null;
	}

	private void Update()
	{
		//�S�Ẵt�B�[���h�𓮂���
		foreach (PlayFieldManagerScript item in _playFieldManagers)
		{
			item.PlayUpdate();
		}
	}

	/// <summary>
	/// �X�e�[�W�̐���
	/// </summary>
	/// <param name="fieldBottomLeftCorner">�X�e�[�W�𐶐�����Ƃ��̊</param>
	/// <param name="fieldDataScript">�z��f�[�^</param>
	/// <param name="popPos">�|�b�v����ꏊ</param>
	private void InstanceStage(Vector2 fieldBottomLeftCorner, FieldDataScript fieldDataScript, Vector2 popPos)
	{
		int rowLength = STAGE_WALL_LEFTSIDE_SIZE + STAGE_WALL_RIGHTSIDE_SIZE + _stageRowLength;
		//��ԉ��̒i�̐���
		for (int i = 0; i < STAGE_WALL_BOTTOMSIDE_SIZE; i++)
		{
			for (int k = 0; k < rowLength; k++)
			{
				Instantiate(_wallPrefab, fieldBottomLeftCorner + k * Vector2.right + i * Vector2.up, Quaternion.identity).name = "BottomWall";
				fieldDataScript.SetFieldArrayData(k, i, FieldDataType.Wall);
			}
		}
		//�����̕ǂ̐���
		for (int i = 0; i < STAGE_WALL_LEFTSIDE_SIZE; i++)
		{
			for (int k = 1; k <= _stageColLength; k++)
			{
				Instantiate(_wallPrefab, fieldBottomLeftCorner + Vector2.left * i + Vector2.up * k, Quaternion.identity).name = "LeftWall";
				fieldDataScript.SetFieldArrayData(i, k, FieldDataType.Wall);
			}
		}
		//�E���̕ǂ̐���
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
