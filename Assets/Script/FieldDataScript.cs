// ---------------------------------------------------------
// FieldDataScript.cs
//
// �쐬��:10/19
// �X�V��:12/15
// �쐬��:�F�J�q
// --------------------------------------------------------- 

using Interface;

/// <summary>
/// �t�B�[���h�̔z��f�[�^�𑀍�A�Q��
/// </summary>
public class FieldDataScript : IFieldArrayDataGetable, IFieldArrayDataSetable, IFieldArrayDataControllable
{
	private FieldDataType[,] _fieldDataArray = default;
	private int _wallRow = default;
	private int _wallCol = default;

	/// <summary>
	/// �ǂ̌������܂߂���̒���
	/// </summary>
	public int FieldDataArrayColLength { get { return _fieldDataArray.GetLength(1); } }

	/// <summary>
	/// �ǂ̌������܂߂��s�̒���
	/// </summary>
	public int FieldDataArrayRowLength { get { return _fieldDataArray.GetLength(0); } }

	/// <summary>
	/// �ǂ̌������܂߂Ȃ��s�̒���
	/// </summary>
	public int FieldDataArrayRowLengthNoneWall { get { return _fieldDataArray.GetLength(0) - _wallRow; } }

	/// <summary>
	/// �ǂ̌������܂߂Ȃ���̒���
	/// </summary>
	public int FieldDataArrayColLengthNoneWall { get { return _fieldDataArray.GetLength(1) - _wallCol; } }

	/// <summary>
	/// 
	/// </summary>
	/// <param name="stageRow">�ǂ��܂߂��s</param>
	/// <param name="stageCol">�ǂ��܂߂���</param>
	/// <param name="wallRow">�s�̕ǂ̌���</param>
	/// <param name="wallCol">��̕ǂ̌���</param>
	public FieldDataScript(int stageRow, int stageCol, int wallRow, int wallCol)
	{
		_fieldDataArray = new FieldDataType[stageRow, stageCol];
		this._wallCol = wallCol;
		this._wallRow = wallRow;
	}

	/// <summary>
	/// �z��f�[�^�̎Q��
	/// </summary>
	/// <param name="row">��</param>
	/// <param name="col">�s</param>
	/// <returns>�Q�Ɛ�̃f�[�^</returns>
	public FieldDataType GetFieldData(int row, int col)
	{
		if (row >= _fieldDataArray.GetLength(0) || col >= _fieldDataArray.GetLength(1) || row < 0 || col < 0)
		{
			return FieldDataType.Wall;
		}
		return _fieldDataArray[row, col];
	}

	/// <summary>
	/// �z��f�[�^�̑���
	/// </summary>
	/// <param name="row">��</param>
	/// <param name="col">�s</param>
	/// <param name="data">�ύX��̃f�[�^</param>
	public void SetFieldArrayData(int row, int col, FieldDataType data)
	{
		_fieldDataArray[row, col] = data;
	}
}
