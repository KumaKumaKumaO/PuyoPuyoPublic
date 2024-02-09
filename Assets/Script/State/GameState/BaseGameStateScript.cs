// ---------------------------------------------------------
// BaseGameStateScript.cs
//
// �쐬��:10/19
// �X�V��:12/15
// �쐬��:�F�J�q
// --------------------------------------------------------- 

using Interface;

/// <summary>
/// �Q�[���S�̂̃X�e�[�g
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
	/// ���̃X�e�[�g�ɂȂ����Ƃ��ɂP�x�������s����
	/// </summary>
	public virtual void Enter()
	{

	}

	/// <summary>
	/// ���̃X�e�[�g�̂Ƃ����t���[�����s����
	/// </summary>
	public virtual void Execute()
	{
		//�|�[�Y�{�^����������Ă�����
		if (_input.IsPause())
		{
			//�X�e�[�g���|�[�Y�X�e�[�g�ɕύX����
			_gameManagerStateChangable.ChangeToPauseState();
		}
	}

	/// <summary>
	/// ���̃X�e�[�g���瑼�̃X�e�[�g�ɂȂ�Ƃ��ɂP�x�������s����
	/// </summary>
	public virtual void Exit()
	{

	}
}
