// ---------------------------------------------------------
// StayStateScript.cs
//
// �쐬��:10/19
// �X�V��:12/15
// �쐬��:�F�J�q
// --------------------------------------------------------- 

using Interface;
using UnityEngine;

/// <summary>
/// �v���C���[������ł��Ȃ��X�e�[�g
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
		//�G���[���������
		FieldDataScript fieldDataScript = fieldObjectManagerScript.FieldDataScript;
		gameRuleScript = new GameRuleScript(fieldDataScript, fieldObjectManagerScript, canDeletePuyoValue);
		_fieldObjectUpdatable = fieldObjectManagerScript;
	}

	public override void Enter()
	{
		//������Ղ悪������Ȃ������ꍇ
		if (!gameRuleScript.ContainDeletePuyo())
		{
			//�Q�[���I�[�o�[�����𖞂������ꍇ
			if (gameRuleScript.IsGameOver())
			{
				//�Q�[���I�[�o�[�X�e�[�g�ɕύX
				_gameManagerStateChangable.ChangeToGameOverState();
				return;
			}
			//����X�e�[�g�ɕύX
			_gameManagerStateChangable.ChangeToPlayState();
			return;
		}
		
		//���삪�ł��Ȃ��X�e�[�g�ɕύX���ꂽ���Ԃ��L�^
		_startTime = Time.time;
	}

	public override void Execute()
	{
		base.Execute();
		//�폜��ɍX�V�̎��ԂɂȂ����ꍇ
		if (_startTime + _updateLagTime + _deleteLagTime <= Time.time)
		{
			//�t�B�[���h�ɂ���Ղ���̈ʒu���X�V����
			_fieldObjectUpdatable.UpdateFieldObject();
			//�^�C�}�[������������
			_startTime = Time.time;
			//�폜�\�Ղ悪���ɑ��݂��Ȃ��ꍇ
			if (!gameRuleScript.ContainDeletePuyo())
			{
				
				//�O�̃X�e�[�g�ɕύX����
				_gameManagerStateChangable.ChangeToBeforeState();
				return;
			}
			return;
		}
		//�폜�܂ł̎��ԂɂȂ����ꍇ
		else if (_startTime + _deleteLagTime <= Time.time)
		{
			//�Ղ������
			gameRuleScript.DeletePuyo();
			//�������̉��Đ�
			AudioScript.InstanceAudioScript.PlayDeleteSound();
			return;
		}
	}
}
