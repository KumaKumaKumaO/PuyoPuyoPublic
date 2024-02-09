// ---------------------------------------------------------
// PlayFieldManagerScript.cs
//
// �쐬��:10/19
// �X�V��:12/15
// �쐬��:�F�J�q
// --------------------------------------------------------- 

using UnityEngine;
using Interface;

/// <summary>
/// �Q�[�����̃t�B�[���h���Ǘ�����
/// </summary>
public class PlayFieldManagerScript : IGameManagerStateChangable
{
	//�S�̂̃I�u�W�F�N�g���Ǘ�����N���X
	private FieldObjectManagerScript _fieldObjectManagerScript = default;
	private GameObject _pauseCanvasObject = default;
	private GameObject _gameOverCanvasObject = default;
	//���݂̏��
	private BaseGameStateScript _nowGameState = default;
	//�O��̃X�e�[�g
	private BaseGameStateScript _beforeState = default;
	private IInput _input = default;

	private int _canDeletePuyoValue = default;

	public FieldObjectManagerScript FieldObjectManagerScript { get { return _fieldObjectManagerScript; } }

	/// <summary>
	/// ������
	/// </summary>
	public PlayFieldManagerScript(IInput input, GameData gameData, ObjectPoolScript objectPoolScript, GameObject pauseCanvas, GameObject gameOverCanvas)
	{
		this._input = input;
		_canDeletePuyoValue = gameData.CanDeletePuyoValue;
		_fieldObjectManagerScript = new FieldObjectManagerScript(gameData, objectPoolScript);
		_pauseCanvasObject = pauseCanvas;
		_gameOverCanvasObject = gameOverCanvas;
		//�Q�[���I�[�o�[�L�����o�X���\���ɂ���
		_gameOverCanvasObject.SetActive(false);
		//�|�[�Y�L�����o�X���\���ɂ���
		_pauseCanvasObject.SetActive(false);
		//�ŏ��̃X�e�[�g���v���C�ɐݒ�
		_nowGameState = new PlayStateScript(this, this._input, _fieldObjectManagerScript);
		_nowGameState.Enter();
	}

	/// <summary>
	/// �Q�[���I�[�o�[��Ԃ֑J��
	/// </summary>
	public void ChangeToGameOverState()
	{
		//�I������
		_nowGameState.Exit();
		//�O��̃X�e�[�g���㏑������
		_beforeState = _nowGameState;
		//�V�����C���X�^���X�𐶐�����
		_nowGameState = new GameOverStateScript(this, _input, _gameOverCanvasObject);
		//�J�n����
		_nowGameState.Enter();
	}

	/// <summary>
	/// �Ղ�폜��Ԃ֑J��
	/// </summary>
	public void ChangeToPuyoStayState()
	{
		//�I������
		_nowGameState.Exit();
		//�O��̃X�e�[�g���㏑������
		_beforeState = _nowGameState;
		//�V�����C���X�^���X�𐶐�����
		_nowGameState = new StayStateScript(this, _input, _fieldObjectManagerScript, _canDeletePuyoValue);
		//�J�n����
		_nowGameState.Enter();
	}

	/// <summary>
	/// �v���C���[�����Ԃ֑J��
	/// </summary>
	public void ChangeToPlayState()
	{
		//�I������
		_nowGameState.Exit();
		//�O��̃X�e�[�g���㏑������
		_beforeState = _nowGameState;
		//�V�����C���X�^���X�𐶐�����
		_nowGameState = new PlayStateScript(this, _input, _fieldObjectManagerScript);
		//�J�n����
		_nowGameState.Enter();
	}

	/// <summary>
	/// �|�[�Y��Ԃ֑J��
	/// </summary>
	public void ChangeToPauseState()
	{
		//�I������
		_nowGameState.Exit();
		//�O��̃X�e�[�g���㏑������
		_beforeState = _nowGameState;
		//�V�����C���X�^���X�𐶐�����
		_nowGameState = new PauseStateScript(this, _input, _pauseCanvasObject);
		//�J�n����
		_nowGameState.Enter();
	}

	/// <summary>
	/// ��O�̃X�e�[�g�ɖ߂�
	/// </summary>
	public void ChangeToBeforeState()
	{
		//��O����
		if (_beforeState == null)
		{
			Debug.LogError("�O�̃X�e�[�g���Ȃ����߃X�e�[�g��ύX���܂���B");
			return;
		}
		//�I������
		_nowGameState.Exit();
		//���݂̃X�e�[�g���o�b�t�@����
		BaseGameStateScript nowGameStateTemp = _nowGameState;
		//�O��̃X�e�[�g�����݂̃X�e�[�g�ɓ����
		_nowGameState = _beforeState;
		//�O��̃X�e�[�g���㏑������
		_beforeState = nowGameStateTemp;
		//�J�n����
		_nowGameState.Enter();
		//�Q�Ƃ�؂�
		nowGameStateTemp = null;
	}

	/// <summary>
	/// ���݂̃X�e�[�g�̎��s
	/// </summary>
	public void PlayUpdate()
	{
		_nowGameState.Execute();
	}
}
