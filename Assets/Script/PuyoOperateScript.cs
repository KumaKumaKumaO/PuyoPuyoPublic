// ---------------------------------------------------------
// PuyoOperateScript.cs
//
// �쐬��:10/19
// �X�V��:12/15
// �쐬��:�F�J�q
// --------------------------------------------------------- 

using UnityEngine;
using Interface;

/// <summary>
/// ���͂�`����N���X
/// </summary>
public class PuyoOperateScript
{
	//�^�C�}�[
	private OperationTimerScript _timerScript = default;

	public PuyoOperateScript()
	{
		_timerScript = new OperationTimerScript();
		_timerScript.TimerInitialization();
	}

	/// <summary>
	/// �Ղ�𑀍삷��
	/// </summary>
	/// <param name="compositePuyoOperatable">���삵�����Ղ�̂܂Ƃ܂�</param>
	/// <param name="input">����</param>
	public void PuyoControl(ICompositePuyoOperatable compositePuyoOperatable, IInput input)
	{
		//�^�C�}�[���I��������
		if (_timerScript.TimerStateUpdate() == TimerState.Termination)
		{
			//�P������
			compositePuyoOperatable.MoveCompositePuyo(Vector2.down);
			//�^�C�}�[������������
			_timerScript.TimerInitialization();
		}

		//�E�Ɉړ���������Ă�����
		if (input.IsRightMove())
		{
			//�E�Ɉړ�����
			compositePuyoOperatable.MoveCompositePuyo(Vector2.right);
		}

		//���Ɉړ���������Ă�����
		if (input.IsLeftMove())
		{
			//���Ɉړ�����
			compositePuyoOperatable.MoveCompositePuyo(Vector2.left);
		}

		//���Ɉړ���������Ă�����
		if (input.IsDownMove())
		{
			//���Ɉړ�����
			compositePuyoOperatable.MoveCompositePuyo(Vector2.down);
			//�^�C�}�[������������
			_timerScript.TimerInitialization();
		}
		//�n�[�h�h���b�v��������Ă�����
		else if (input.IsHardDrop())
		{
			//�n�[�h�h���b�v������
			compositePuyoOperatable.HardDropCompositePuyo();
		}

		//�E�ɉ�]��������Ă�����
		if (input.IsRightTurn())
		{
			//�E�ɉ�]����
			compositePuyoOperatable.RotationCompositePuyo(RotateDirection.Right);
		}

		//���ɉ�]��������Ă�����
		if (input.IsLeftTurn())
		{
			//���ɉ�]����
			compositePuyoOperatable.RotationCompositePuyo(RotateDirection.Left);
		}

	}
}
