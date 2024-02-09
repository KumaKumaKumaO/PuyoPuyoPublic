// ---------------------------------------------------------
// FieldObjectManagerScript.cs
//
// �쐬��:10/19
// �X�V��:12/15
// �쐬��:�F�J�q
// --------------------------------------------------------- 

using System.Collections.Generic;
using UnityEngine;
using System;
using Interface;

/// <summary>
/// �t�B�[���h�ɔz�u����Ă���I�u�W�F�N�g�̑���
/// </summary>
public class FieldObjectManagerScript : INextPuyoPopable, IFieldPuyoObjectRemovable, IFieldPuyoObjectAddble, IFieldDataScriptGetable, IFieldObjectUpdatable
{
	private Vector2 _nextPos = default;
	private Vector2 _popPos = default;

	private FieldDataScript _fieldDataScript = default;
	private PuyoScript[] _nextPuyos = new PuyoScript[2];
	private PuyoScript[] _nowPuyos = new PuyoScript[2];
	private ObjectPoolScript _objectPoolScript;
	private List<IPuyoDataGetable> _fieldObjects = new List<IPuyoDataGetable>();
	private CompositePuyoScript _myCompositePuyo;

	public FieldDataScript FieldDataScript { get { return _fieldDataScript; } }

	public CompositePuyoScript MyCompositePuyoScript { get { return _myCompositePuyo; } }

	public FieldObjectManagerScript(GameData gameData, ObjectPoolScript objectPoolScript)
	{
		_objectPoolScript = objectPoolScript;
		_fieldDataScript = gameData.FieldDataScript;
		_popPos = gameData.PopPos;
		_nextPos = gameData.NextPos;
		GetInitPuyo();
		MoveNextPos(_nextPos);
		_myCompositePuyo = new CompositePuyoScript(_fieldDataScript, this, _nowPuyos);
	}

	/// <summary>
	/// ���������ꂽ�Ղ�擾����
	/// </summary>
	private void GetInitPuyo()
	{
		FieldDataType fieldDataType = default;
		//���ԑ҂��̂Ղ�𑀍�Ղ�Ɉړ����ď��ԑ҂��̂Ղ��p�ӂ�����������
		for (int i = 0; i < _nextPuyos.Length; i++)
		{
			_nowPuyos[i] = _nextPuyos[i];
			fieldDataType = SelectDataType();
			_nextPuyos[i] = _objectPoolScript.GetNonInitPuyo(fieldDataType);
			_nextPuyos[i].Initialization(_fieldDataScript.FieldDataArrayColLengthNoneWall - 2
				, _fieldDataScript.FieldDataArrayRowLengthNoneWall / 2 + _fieldDataScript.FieldDataArrayRowLengthNoneWall % 2, fieldDataType);
		}
	}

	/// <summary>
	/// �z��ň����f�[�^�������_���őI��
	/// </summary>
	/// <returns>�I�΂ꂽ�f�[�^</returns>
	private FieldDataType SelectDataType()
	{
		return (FieldDataType)UnityEngine.Random.Range(2, Enum.GetValues(typeof(FieldDataType)).Length);
	}

	/// <summary>
	/// ���ԑ҂��ɂ���Ղ���|�b�v����
	/// </summary>
	public void NextPuyoPop()
	{
		//�����������Ղ���擾
		GetInitPuyo();
		//�ʒu������������
		MoveNextPos(_nextPos);
		//�܂Ƃ߂ē������N���X������������
		_myCompositePuyo.Initialization();
		//�X�^�[�g�̏ꏊ�Ɉړ����ē�߂��E�ɂ��炷
		_myCompositePuyo.MoveFieldCompositePuyo(_popPos, Vector2.right);
	}

	/// <summary>
	/// �n���ꂽ�ꏊ�Ɠ����ꏊ�ɂ���I�u�W�F�N�g������
	/// </summary>
	/// <param name="col">�s</param>
	/// <param name="row">��</param>
	public void RemoveFieldObject(int row, int col)
	{
		//�t�B�[���h�ɑ��݂���I�u�W�F�N�g�̒������v������̂�����
		for (int i = 0; i < _fieldObjects.Count; i++)
		{
			//�s�Ɨ񂪎擾�����I�u�W�F�N�g�Ɠ�����
			if (_fieldObjects[i].Col == col && _fieldObjects[i].Row == row)
			{
				IPuyoDataGetable puyoDataGetableTemp = _fieldObjects[i];
				//�t�B�[���h�ɑ��݂���I�u�W�F�N�g�̃��X�g�������
				_fieldObjects.Remove(puyoDataGetableTemp);
				//�z��̒��g���Ȃɂ��Ȃ�(None)�ɂ���
				((IFieldArrayDataSetable)_fieldDataScript).SetFieldArrayData(row, col, FieldDataType.None);
				//�I�u�W�F�N�g��j������
				_objectPoolScript.DeletePuyo((PuyoScript)puyoDataGetableTemp);
			}
		}
	}

	/// <summary>
	/// �t�B�[���h�ɑ��݂���I�u�W�F�N�g���X�V����
	/// </summary>
	public void UpdateFieldObject()
	{
		//�t�B�[���h�ɑ��݂���Puyo�����ɂԂ���܂ňړ�������
		foreach (IPuyoDataGetable puyoDataGetable in _fieldObjects)
		{
			//���܂���ʒu�̔z��f�[�^���Ȃɂ��Ȃ�(None)�ɂ���
			((IFieldArrayDataSetable)_fieldDataScript).SetFieldArrayData(puyoDataGetable.Row, puyoDataGetable.Col, FieldDataType.None);
			//���ɂȂɂ�����Ƃ���܂ŉ��Ɉړ�������
			((IPuyoOperatable)puyoDataGetable).FallPuyo(_fieldDataScript);
			//���Ɉړ��������ƂɎ����̃f�[�^��z��ɓ��͂���
			((IFieldArrayDataSetable)_fieldDataScript).SetFieldArrayData(puyoDataGetable.Row, puyoDataGetable.Col, puyoDataGetable.MyFieldData);
		}
	}

	/// <summary>
	/// �t�B�[���h�ɔz�u���ꂽPuyo�Ƃ��Ċi�[����
	/// </summary>
	/// <param name="basePuyoScript">�i�[������PuyoScript</param>
	public void AddFieldPuyoObject(IPuyoDataGetable basePuyoScript)
	{
		_fieldObjects.Add(basePuyoScript);
	}

	/// <summary>
	/// �ʒu��������
	/// </summary>
	/// <param name="initializePos">�����̈ʒu</param>
	public void MoveNextPos(Vector2 initializePos)
	{
		_nextPuyos[0].MyTransform.position = initializePos;
		_nextPuyos[1].MyTransform.position = initializePos + Vector2.down;
	}
}
