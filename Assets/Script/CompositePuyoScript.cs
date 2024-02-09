// ---------------------------------------------------------
// CompositePuyoScript.cs
//
// �쐬��:10/19
// �X�V��:12/15
// �쐬��:�F�J�q
// --------------------------------------------------------- 

using UnityEngine;
using Interface;

/// <summary>
/// �Ղ����܂Ƃ߂ē������N���X
/// </summary>
public class CompositePuyoScript : ICompositePuyoOperatable, ICompositePuyoStateChackable
{
	/// <summary>
	/// ���݂̉�]�̃X�e�[�g
	/// </summary>
	private enum RotationState
	{
		Top,
		Right,
		Bottom,
		Left,
	}

	private IFieldArrayDataControllable _fieldArrayDataControllable = default;
	private IFieldPuyoObjectAddble _fieldObjectAddble = default;
	private RotationState _myRotationState = RotationState.Right;
	private CompositePuyoState _myState = default;

	private PuyoScript[] _puyos = new PuyoScript[2];

	public CompositePuyoState CompositePuyoState { get { return _myState; } }

	public CompositePuyoScript(FieldDataScript fieldDataScript
		, IFieldPuyoObjectAddble fieldObjectAddble, PuyoScript[] puyos)
	{
		this._fieldArrayDataControllable = fieldDataScript;
		this._fieldObjectAddble = fieldObjectAddble;
		this._puyos = puyos;
		Initialization();
	}

	/// <summary>
	/// ������
	/// </summary>
	public void Initialization()
	{
		//���g�̃X�e�[�g���u������v�ɕύX����
		this._myState = CompositePuyoState.CanMoving;
		//��]�󋵂��E�ɐݒ肷��
		this._myRotationState = RotationState.Right;
	}

	/// <summary>
	/// ���삷���̂Ղ���w�肵���ʒu�Ɉړ�����
	/// </summary>
	/// <param name="movePos">�ړ���</param>
	public void MoveFieldCompositePuyo(Vector2 movePos, Vector2 offsetVector)
	{
		//��̂Ղ�̈ʒu���ړ�����
		foreach (IPuyoDataGetable puyoDataGetable in _puyos)
		{
			puyoDataGetable.MyTransform.position = movePos;
		}
		//��ڂ̂Ղ�̈ʒu���E�ɂ��炷
		_puyos[1].MovePuyo(offsetVector);
	}

	/// <summary>
	/// �t�B�[���h��ňړ�����
	/// </summary>
	/// <param name="moveVector">�����x�N�g��</param>
	public void MoveCompositePuyo(Vector2 moveVector)
	{

		if (_myState == CompositePuyoState.CanMoving)
		{
			//�z����̂Ղ悪�����x�N�g���ɓ����邩�m�F����
			foreach (IPuyoOperatable puyoOperatable in _puyos)
			{
				if (!puyoOperatable.CanMovePuyo(moveVector, _fieldArrayDataControllable))
				{
					//�ǂ��炩�����Ɉړ��ł��Ȃ��ꍇ
					if (moveVector == Vector2.down)
					{
						//�n�[�h�h���b�v�ɂ���
						HardDropCompositePuyo();
						//���g�̃X�e�[�g���u�I���v�ɂ���
						_myState = CompositePuyoState.End;
					}
					return;
				}
			}
			//�����̂Ղ�𓮂��������x�N�g���ɓ�����
			foreach (IPuyoOperatable puyoOperatable in _puyos)
			{
				puyoOperatable.MovePuyo(moveVector);
			}
		}
	}

	/// <summary>
	/// �n�[�h�h���b�v
	/// </summary>
	public void HardDropCompositePuyo()
	{
		if (_myState == CompositePuyoState.CanMoving)
		{
			//���g�̉�]�̃X�e�[�g���u��v�̏ꍇ�͉����̂Ղ悩�珈������
			if (_myRotationState == RotationState.Top)
			{
				for (int i = 0; i < _puyos.Length; i++)
				{
					HardDropProcess(i);
				}
			}
			//���̂ق��̏ꍇ�͏㑤�̂Ղ悩�珈������
			else
			{
				for (int i = _puyos.Length - 1; i >= 0; i--)
				{
					HardDropProcess(i);
				}
			}
			_myState = CompositePuyoState.End;
		}
	}

	/// <summary>
	/// �n�[�h�h���b�v�̓���
	/// </summary>
	/// <param name="index">�ǂ���̂Ղ悩�̎w�W</param>
	private void HardDropProcess(int index)
	{
		//�Ղ�𗎂Ƃ���Ƃ��܂ŗ��Ƃ�
		_puyos[index].FallPuyo(_fieldArrayDataControllable);
		//�f�[�^��z��Ɋi�[����
		_fieldArrayDataControllable.SetFieldArrayData((_puyos[index]).Row
			, (_puyos[index]).Col, (_puyos[index]).MyFieldData);
		//�t�B�[���h�ɐݒu���ꂽ�I�u�W�F�N�g�Ƃ��Ċi�[����
		_fieldObjectAddble.AddFieldPuyoObject(_puyos[index]);
	}

	/// <summary>
	/// ��]����
	/// </summary>
	/// <param name="rotateDirection">��]�̕���</param>
	public void RotationCompositePuyo(RotateDirection rotateDirection)
	{
		if (_myState == CompositePuyoState.CanMoving)
		{
			if (CanNormalRotation(rotateDirection))
			{
				NormalRotation(rotateDirection);
			}
			else if (CanWallRotation(rotateDirection))
			{
				WallRotation(rotateDirection);
			}
		}
	}

	/// <summary>
	/// �ʏ�̉�]�̔���
	/// </summary>
	/// <param name="rotateDirection">��]�̕���</param>
	/// <returns>�ʏ�̉�]���ł��邩</returns>
	private bool CanNormalRotation(RotateDirection rotateDirection)
	{
		switch (_myRotationState)
		{
			//������Ȃ��ق��̂Ղ悪��ɂ���Ƃ�
			case RotationState.Top:
				{
					//�E��]���������ꍇ
					if (rotateDirection == RotateDirection.Right)
					{
						//���̈�E�ɂȂɂ��Ȃ���
						return _fieldArrayDataControllable.GetFieldData(_puyos[0].Row + 1, _puyos[0].Col) == FieldDataType.None;
					}
					//����]���������ꍇ
					else
					{
						//���̈���ɂȂɂ��Ȃ���
						return _fieldArrayDataControllable.GetFieldData(_puyos[0].Row - 1, _puyos[0].Col) == FieldDataType.None;
					}
				}
			//������Ȃ��ق��̂Ղ悪�E�ɂ���Ƃ�
			case RotationState.Right:
				{
					//�E��]���������ꍇ
					if (rotateDirection == RotateDirection.Right)
					{
						//���̈���ɂȂɂ��Ȃ���
						return _fieldArrayDataControllable.GetFieldData(_puyos[0].Row, _puyos[0].Col - 1) == FieldDataType.None;
					}
					//���̏�ɂ͕K�������Ȃ��̂�true��Ԃ�
					return true;
				}
			//������Ȃ��ق��̂Ղ悪���ɂ���Ƃ�
			case RotationState.Bottom:
				{
					//�E��]���������ꍇ
					if (rotateDirection == RotateDirection.Right)
					{
						//���̈���ɂȂɂ��Ȃ���
						return _fieldArrayDataControllable.GetFieldData(_puyos[0].Row - 1, _puyos[0].Col) == FieldDataType.None;
					}
					//����]���������ꍇ
					else
					{
						//���̈�E�ɂȂɂ��Ȃ���
						return _fieldArrayDataControllable.GetFieldData(_puyos[0].Row + 1, _puyos[0].Col) == FieldDataType.None;
					}
				}
			//������Ȃ��ق��̂Ղ悪���ɂ���Ƃ�
			case RotationState.Left:
				{
					//�E��]���������ꍇ
					if (rotateDirection == RotateDirection.Right)
					{
						//���̏�ɂ͕K�������Ȃ��̂�true��Ԃ�
						return true;
					}
					//����]���������ꍇ
					else
					{
						//���̈���ɂȂɂ��Ȃ���
						return _fieldArrayDataControllable.GetFieldData(_puyos[0].Row, _puyos[0].Col - 1) == FieldDataType.None;
					}
				}
		}
		//��O����
		return false;
	}

	/// <summary>
	/// �ʏ�̉�]
	/// </summary>
	/// <param name="rotateDirection">��]�̕���</param>
	private void NormalRotation(RotateDirection rotateDirection)
	{
		switch (_myRotationState)
		{
			//������Ȃ��ق��̂Ղ悪��ɂ���Ƃ�
			case RotationState.Top:
				{
					//��]�������������E�̂Ƃ�
					if (rotateDirection == RotateDirection.Right)
					{
						//��]�̃X�e�[�g���E�ɕύX
						_myRotationState = RotationState.Right;
						//������Ȃ��ق��̂Ղ�����̉E�Ɉړ�
						_puyos[1].MovePuyo(Vector2.down + Vector2.right);
					}
					//��]���������������̂Ƃ�
					else
					{
						//��]�̃X�e�[�g�����ɕύX
						_myRotationState = RotationState.Left;
						//������Ȃ��ق��̂Ղ�����̍��Ɉړ�
						_puyos[1].MovePuyo(Vector2.down + Vector2.left);
					}
					break;
				}
			//������Ȃ��ق��̂Ղ悪�E�ɂ���Ƃ�
			case RotationState.Right:
				{
					//��]�������������E�̂Ƃ�
					if (rotateDirection == RotateDirection.Right)
					{
						//��]�̃X�e�[�g�����ɕύX
						_myRotationState = RotationState.Bottom;
						//������Ȃ��ق��̂Ղ�����̉��Ɉړ�
						_puyos[1].MovePuyo(Vector2.down + Vector2.left);
					}
					//��]���������������̂Ƃ�
					else
					{
						//��]�̃X�e�[�g����ɕύX
						_myRotationState = RotationState.Top;
						//������Ȃ��ق��̂Ղ�����̏�Ɉړ�
						_puyos[1].MovePuyo(Vector2.up + Vector2.left);
					}
					break;
				}
			//������Ȃ��ق��̂Ղ悪���ɂ���Ƃ�
			case RotationState.Bottom:
				{
					//��]�������������E�̂Ƃ�
					if (rotateDirection == RotateDirection.Right)
					{
						//��]�̃X�e�[�g�����ɕύX
						_myRotationState = RotationState.Left;
						//������Ȃ��ق��̂Ղ�����̍��Ɉړ�
						_puyos[1].MovePuyo(Vector2.up + Vector2.left);
					}
					//��]���������������̂Ƃ�
					else
					{
						//��]�̃X�e�[�g���E�ɕύX
						_myRotationState = RotationState.Right;
						//������Ȃ��ق��̂Ղ�����̉E�Ɉړ�
						_puyos[1].MovePuyo(Vector2.up + Vector2.right);
					}
					break;
				}
			//������Ȃ��ق��̂Ղ悪���ɂ���Ƃ�
			case RotationState.Left:
				{
					//��]�������������E�̂Ƃ�
					if (rotateDirection == RotateDirection.Right)
					{
						//��]�̃X�e�[�g����ɕύX
						_myRotationState = RotationState.Top;
						//������Ȃ��ق��̂Ղ�����̏�Ɉړ�
						_puyos[1].MovePuyo(Vector2.up + Vector2.right);
					}
					//��]���������������̂Ƃ�
					else
					{
						//��]�̃X�e�[�g�����ɕύX
						_myRotationState = RotationState.Bottom;
						//������Ȃ��ق��̂Ղ�����̉��Ɉړ�
						_puyos[1].MovePuyo(Vector2.down + Vector2.right);
					}
					break;
				}
		}
	}

	/// <summary>
	/// �ǂɂԂ������ۂ̉�]�̔���
	/// </summary>
	/// <param name="rotateDirection">��]�̕���</param>
	/// <returns>�ǂɂԂ������ۂ̉�]���ł��邩</returns>
	private bool CanWallRotation(RotateDirection rotateDirection)
	{
		switch (_myRotationState)
		{
			//������Ȃ��ق��̂Ղ悪��ɂ���Ƃ�
			case RotationState.Top:
				{
					//��]�������������E�̂Ƃ�
					if (rotateDirection == RotateDirection.Right)
					{
						//���̈�����Ȃɂ��Ȃ��Ƃ�
						return _fieldArrayDataControllable.GetFieldData(_puyos[0].Row - 1, _puyos[0].Col) == FieldDataType.None;
					}
					//��]���������������̂Ƃ�
					else
					{
						//���̈�E���Ȃɂ��Ȃ��Ƃ�
						return _fieldArrayDataControllable.GetFieldData(_puyos[0].Row + 1, _puyos[0].Col) == FieldDataType.None;
					}
				}
			//������Ȃ��ق��̂Ղ悪�E�ɂ���Ƃ�
			case RotationState.Right:
				{
					return true;
				}
			//������Ȃ��ق��̂Ղ悪���ɂ���Ƃ�
			case RotationState.Bottom:
				{
					//��]�������������E�̂Ƃ�
					if (rotateDirection == RotateDirection.Right)
					{
						//���̈�E���Ȃɂ��Ȃ��Ƃ�
						return _fieldArrayDataControllable.GetFieldData(_puyos[0].Row + 1, _puyos[0].Col) == FieldDataType.None;
					}
					//��]���������������̂Ƃ�
					else
					{
						//���̈�����Ȃɂ��Ȃ��Ƃ�
						return _fieldArrayDataControllable.GetFieldData(_puyos[0].Row - 1, _puyos[0].Col) == FieldDataType.None;
					}
				}
			//������Ȃ��ق��̂Ղ悪���ɂ���Ƃ�
			case RotationState.Left:
				{
					//���̈�オ�Ȃɂ��Ȃ��Ƃ�
					return _fieldArrayDataControllable.GetFieldData(_puyos[0].Row, _puyos[0].Col + 1) == FieldDataType.None;
				}
		}
		//��O����
		return false;
	}

	/// <summary>
	/// �ǂɂԂ������ۂ̉�]
	/// </summary>
	/// <param name="rotateDirection">��]�̕���</param>
	private void WallRotation(RotateDirection rotateDirection)
	{
		switch (_myRotationState)
		{
			//������Ȃ��ق��̂Ղ悪��ɂ���Ƃ�
			case RotationState.Top:
				{
					//��]�������������E�̂Ƃ�
					if (rotateDirection == RotateDirection.Right)
					{
						//��]�X�e�[�g���E�ɐݒ�
						_myRotationState = RotationState.Right;
						//������Ȃ��ق��̂Ղ�����Ɉړ�
						_puyos[1].MovePuyo(Vector2.down);
						//���̂Ղ�����Ɉړ�
						_puyos[0].MovePuyo(Vector2.left);
					}
					//��]���������������̂Ƃ�
					else
					{
						//��]�X�e�[�g�����ɐݒ�
						_myRotationState = RotationState.Left;
						//������Ȃ��ق��̂Ղ�����Ɉړ�
						_puyos[1].MovePuyo(Vector2.down);
						//���̂Ղ���E�Ɉړ�
						_puyos[0].MovePuyo(Vector2.right);
					}
					break;
				}
			//������Ȃ��ق��̂Ղ悪�E�ɂ���Ƃ�
			case RotationState.Right:
				{
					//��]�X�e�[�g�����ɐݒ�
					_myRotationState = RotationState.Bottom;
					//������Ȃ��ق��̂Ղ�����Ɉړ�
					_puyos[1].MovePuyo(Vector2.left);
					//���̂Ղ����Ɉړ�
					_puyos[0].MovePuyo(Vector2.up);
					break;
				}
			//������Ȃ��ق��̂Ղ悪���ɂ���Ƃ�
			case RotationState.Bottom:
				{
					//��]�������������E�̂Ƃ�
					if (rotateDirection == RotateDirection.Right)
					{
						//��]�X�e�[�g�����ɐݒ�
						_myRotationState = RotationState.Left;
						//������Ȃ��ق��̂Ղ�����Ɉړ�
						_puyos[1].MovePuyo(Vector2.up);
						//���̂Ղ���E�Ɉړ�
						_puyos[0].MovePuyo(Vector2.right);
					}
					//��]���������������̂Ƃ�
					else
					{
						//��]�X�e�[�g���E�ɐݒ�
						_myRotationState = RotationState.Right;
						//������Ȃ��ق��̂Ղ����Ɉړ�
						_puyos[1].MovePuyo(Vector2.up);
						//���̂Ղ�����Ɉړ�
						_puyos[0].MovePuyo(Vector2.left);
					}
					break;
				}
			//������Ȃ��ق��̂Ղ悪���ɂ���Ƃ�
			case RotationState.Left:
				{
					//��]�X�e�[�g����ɐݒ�
					_myRotationState = RotationState.Top;
					//���̂Ղ�����Ɉړ�
					_puyos[0].MovePuyo(Vector2.up);
					//������Ȃ��ق��̂Ղ���E��Ɉړ�
					_puyos[1].MovePuyo(Vector2.right);
					break;
				}
		}
	}

}
