// ---------------------------------------------------------
// PauseStateScript.cs
//
// �쐬��:10/19
// �X�V��:12/15
// �쐬��:�F�J�q
// --------------------------------------------------------- 

using Interface;
using UnityEngine;

/// <summary>
/// �|�[�Y�X�e�[�g
/// </summary>
public class PauseStateScript : BaseGameStateScript
{
	private GameObject _pauseCanvas = default;

	public PauseStateScript(IGameManagerStateChangable gameManagerStateChangable
		, IInput input, GameObject pauseCanvasObject)
		: base(gameManagerStateChangable, input)
	{
		_pauseCanvas = pauseCanvasObject;
	}

	public override void Enter()
	{
		//�|�[�Y�L�����o�X��\������
		_pauseCanvas.SetActive(true);
	}

	public override void Execute()
	{
		//�|�[�Y�{�^���������ꂽ�ꍇ
		if (_input.IsPause())
		{
			//�O�̃X�e�[�g�ɖ߂�
			_gameManagerStateChangable.ChangeToBeforeState();
		}

	}

	public override void Exit()
	{
		//�|�[�Y�L�����o�X������
		_pauseCanvas.SetActive(false);
		//�|�[�Y�L�����o�X�̎Q�Ƃ�؂�
		_pauseCanvas = null;
	}
}
