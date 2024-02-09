// ---------------------------------------------------------
// GameRuleScript.cs
//
// �쐬��:10/19
// �X�V��:12/15
// �쐬��:�F�J�q
// --------------------------------------------------------- 

using System.Collections.Generic;
using Interface;

/// <summary>
/// �Ղ悪�����锻��ƃQ�[���I�[�o�[����
/// </summary>
public class GameRuleScript
{
	/// <summary>
	/// �z����ł̏ꏊ
	/// </summary>
	private struct ArrayPosData
	{
		int _row;
		int _col;
		/// <summary>
		/// ��
		/// </summary>
		public int Row { get { return _row; } }
		/// <summary>
		/// �s
		/// </summary>
		public int Col { get { return _col; } }
		public ArrayPosData(int row, int col)
		{
			this._row = row;
			this._col = col;
		}
	}

	//�����邽�߂ɕK�v�ȂՂ�̐�
	private int canPuyoDeleteValue = 4;
	//�t�B�[���h����I�u�W�F�N�g���������߂̃C���^�[�t�F�[�X
	private IFieldPuyoObjectRemovable _fieldObjectRemovable = default;
	//�t�B�[���h�z�񂩂�f�[�^���擾���邽�߂̃C���^�[�t�F�[�X
	private IFieldArrayDataGetable _fieldDataGetable = default;
	//�T�������Ղ�̏ꏊ�f�[�^���i�[���郊�X�g
	private List<ArrayPosData> _searchedFieldDataPosList = new List<ArrayPosData>();
	//�����ׂ��Ղ�̏ꏊ�f�[�^���i�[���郊�X�g
	private List<ArrayPosData> _deletePuyoDataPosList = new List<ArrayPosData>();

	//�T��������
	private bool isSearched = false;

	/// <summary>
	/// ������
	/// </summary>
	/// <param name="fieldDataScript"></param>
	/// <param name="fieldObjectManagerScript"></param>
	/// <param name="canDeletePuyoValue"></param>
	public GameRuleScript(FieldDataScript fieldDataScript, FieldObjectManagerScript fieldObjectManagerScript, int canDeletePuyoValue)
	{
		_fieldObjectRemovable = fieldObjectManagerScript;
		_fieldDataGetable = fieldDataScript;
		canPuyoDeleteValue = canDeletePuyoValue;
	}

	/// <summary>
	/// �Q�[���I�[�o�[�ɂȂ邩�ǂ���
	/// </summary>
	/// <returns>�Q�[���I�[�o�[�]�[���ɉ������邩</returns>
	public bool IsGameOver()
	{
		return _fieldDataGetable.GetFieldData(_fieldDataGetable.FieldDataArrayRowLength / 2, _fieldDataGetable.FieldDataArrayColLength - 1 - 2)
			!= FieldDataType.None;
	}

	/// <summary>
	/// ������Ղ悪��ł����݂��邩
	/// </summary>
	public bool ContainDeletePuyo()
	{
		//�z��f�[�^�̈ꎞ�ۑ��p�ϐ�
		FieldDataType fieldDataTypeTemp;
		//�z���[����ǂݍ���
		for (int i = 0; i < _fieldDataGetable.FieldDataArrayColLength; i++)
		{
			for (int k = 0; k < _fieldDataGetable.FieldDataArrayRowLength; k++)
			{
				//�z��f�[�^���ꎞ�I�ɕۑ�
				fieldDataTypeTemp = _fieldDataGetable.GetFieldData(k, i);
				//�z��f�[�^�������܂��́A�ǂ̏ꍇ
				if (fieldDataTypeTemp == FieldDataType.None
					|| fieldDataTypeTemp == FieldDataType.Wall)
				{
					//���̃f�[�^��ǂނ��߂Ɏw�W��i�߂�
					continue;
				}
				//�אڂ��Ă��铯����ނ̂Ղ��T���p���X�g�Ɋi�[����
				DeleteDitectPuyo(k, i, fieldDataTypeTemp, _searchedFieldDataPosList);
				//���ōX�V���ꂽ�T���p���X�g�̗v�f�����������߂ɕK�v�ȂՂ�̐��������Ă�����
				if (_searchedFieldDataPosList.Count >= canPuyoDeleteValue)
				{
					//���X�g�̒��g��S�č폜�\�胊�X�g�ɒǉ�����
					foreach (ArrayPosData arrayPosData in _searchedFieldDataPosList)
					{
						_deletePuyoDataPosList.Add(arrayPosData);
					}
				}
				//�T���p���X�g�̏�����
				_searchedFieldDataPosList.Clear();
			}
		}
		//�T�����s��ꂽ�t���O���I���ɂ���
		isSearched = true;
		//�����\�胊�X�g�̗v�f�����O���傫���ꍇ�ɐ^��Ԃ�
		return (_deletePuyoDataPosList.Count > 0);
	}

	/// <summary>
	/// ������Ղ������
	/// </summary>
	public void DeletePuyo()
	{
		//�T�����s��ꂽ��
		if (!isSearched)
		{
			//������Ղ悪���݂��邩
			ContainDeletePuyo();
		}
		//�z��̒��g�����o��
		foreach (ArrayPosData arrayPosData in _deletePuyoDataPosList)
		{
			//�w�肵���ʒu�ɂ���I�u�W�F�N�g�̔j��
			_fieldObjectRemovable.RemoveFieldObject(arrayPosData.Row, arrayPosData.Col);
		}
		//�폜���X�g������������
		_deletePuyoDataPosList.Clear();
		//�T�����s��ꂽ�t���O���I�t�ɂ���
		isSearched = false;
	}

	/// <summary>
	/// �אڂ��Ă��铯����ނ̂Ղ�̔z����ʒu�f�[�^�����X�g�ɕۑ�����
	/// �ċA�I�ɌĂяo��
	/// </summary>
	/// <param name="row">�T��������</param>
	/// <param name="col">�T�������s</param>
	/// <param name="fieldDataType">�T�������Ղ�̎��</param>
	/// <param name="posDataList">�f�[�^��ۑ����郊�X�g</param>
	private void DeleteDitectPuyo(int row, int col, FieldDataType fieldDataType, List<ArrayPosData> posDataList)
	{
		//�T�������Ղ惊�X�g���ɁA�T�����悤�Ƃ��Ă���f�[�^�����������m�F����
		foreach (ArrayPosData arrayPosData in posDataList)
		{
			if (arrayPosData.Row == row && arrayPosData.Col == col)
			{
				//�������ꍇ�͒T�����I������
				return;
			}
		}
		//�T���������X�g�Ƀf�[�^���i�[����
		posDataList.Add(new ArrayPosData(row, col));
		//�T�����Ă���Ղ�̗� + 1�������Ղ�̏ꍇ
		if (_fieldDataGetable.GetFieldData(row + 1, col) == fieldDataType)
		{
			DeleteDitectPuyo(row + 1, col, fieldDataType, posDataList);
		}
		//�T�����Ă���Ղ�̗� - 1�������Ղ�̏ꍇ
		if (_fieldDataGetable.GetFieldData(row - 1, col) == fieldDataType)
		{
			DeleteDitectPuyo(row - 1, col, fieldDataType, posDataList);
		}
		//�T�����Ă���Ղ�̍s + 1�������Ղ�̏ꍇ
		if (_fieldDataGetable.GetFieldData(row, col + 1) == fieldDataType)
		{
			DeleteDitectPuyo(row, col + 1, fieldDataType, posDataList);
		}
		//�T�����Ă���Ղ�̍s - 1�������Ղ�̏ꍇ
		if (_fieldDataGetable.GetFieldData(row, col - 1) == fieldDataType)
		{
			DeleteDitectPuyo(row, col - 1, fieldDataType, posDataList);
		}
	}
}
