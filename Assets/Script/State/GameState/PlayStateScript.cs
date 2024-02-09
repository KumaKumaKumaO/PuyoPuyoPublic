// ---------------------------------------------------------
// PlayStateScript.cs
//
// �쐬��:10/19
// �X�V��:12/15
// �쐬��:�F�J�q
// --------------------------------------------------------- 

using Interface;

/// <summary>
/// �v���C���[������ł���X�e�[�g
/// </summary>
public class PlayStateScript : BaseGameStateScript
{
	private PuyoOperateScript _puyoOperateScript = default;
	private ICompositePuyoStateChackable _compositePuyoStateChackable = default;

	public PlayStateScript(IGameManagerStateChangable gameManagerStateChangable, IInput input
		, FieldObjectManagerScript fieldObjectManagerScript) : base(gameManagerStateChangable, input)
	{
		_puyoOperateScript = new PuyoOperateScript();
		_compositePuyoStateChackable = fieldObjectManagerScript.MyCompositePuyoScript;
		INextPuyoPopable _nextPuyoPopable = fieldObjectManagerScript;
		_nextPuyoPopable.NextPuyoPop();
	}

	public override void Execute()
	{
		base.Execute();


		switch (_compositePuyoStateChackable.CompositePuyoState)
		{
			//��������ꍇ
			case CompositePuyoState.CanMoving:
				{
					//�Ղ�𑀍삷��
					_puyoOperateScript.PuyoControl((CompositePuyoScript)_compositePuyoStateChackable, _input);
					break;
				}
			//�������̂��I������ꍇ
			case CompositePuyoState.End:
				{
					//�X�e�[�g��ҋ@�ɕύX
					_gameManagerStateChangable.ChangeToPuyoStayState();
					break;
				}
		}
	}
}
