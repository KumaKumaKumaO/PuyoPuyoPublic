// ---------------------------------------------------------
// OperationTimerScript.cs
//
// �쐬��:10/19
// �X�V��:12/15
// �쐬��:�F�J�q
// --------------------------------------------------------- 

using UnityEngine;

/// <summary>
/// �^�C�}�[���Ǘ�����X�N���v�g
/// </summary>
public class OperationTimerScript
{
	private float _fallTime = 1.5f;
	private float _startTime = default;
	private TimerState _myState = default;

	/// <summary>
	/// �^�C�}�[�̏�Ԃ𐧌䂷��
	/// </summary>
	/// <returns>���݂̏��</returns>
	public TimerState TimerStateUpdate()
	{
		switch (_myState)
		{
			//���쒆��������
			case TimerState.Processing:
				{
					//����̎��Ԃ���������
					if (Time.time >= _startTime + _fallTime)
					{
						//�^�C�}�[���I���ɕύX����
						_myState = TimerState.Termination;
					}
					break;
				}
			//�I��������
			case TimerState.Termination:
				{
					//�^�C�}�[������������
					TimerInitialization();
					break;
				}
		}
		return _myState;
	}

	/// <summary>
	/// �^�C�}�[�̏�����
	/// </summary>
	public void TimerInitialization()
	{
		//���݂̎��Ԃ�ۑ�
		_startTime = Time.time;
		//�^�C�}�[�̃X�e�[�g�𓮍쒆�ɕύX
		_myState = TimerState.Processing;
	}
}
