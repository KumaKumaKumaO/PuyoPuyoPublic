// ---------------------------------------------------------
// FieldDataScript.cs
//
// 作成日:10/19
// 更新日:12/15
// 作成者:熊谷航
// --------------------------------------------------------- 

using Interface;

/// <summary>
/// フィールドの配列データを操作、参照
/// </summary>
public class FieldDataScript : IFieldArrayDataGetable, IFieldArrayDataSetable, IFieldArrayDataControllable
{
	private FieldDataType[,] _fieldDataArray = default;
	private int _wallRow = default;
	private int _wallCol = default;

	/// <summary>
	/// 壁の厚さを含めた列の長さ
	/// </summary>
	public int FieldDataArrayColLength { get { return _fieldDataArray.GetLength(1); } }

	/// <summary>
	/// 壁の厚さを含めた行の長さ
	/// </summary>
	public int FieldDataArrayRowLength { get { return _fieldDataArray.GetLength(0); } }

	/// <summary>
	/// 壁の厚さを含めない行の長さ
	/// </summary>
	public int FieldDataArrayRowLengthNoneWall { get { return _fieldDataArray.GetLength(0) - _wallRow; } }

	/// <summary>
	/// 壁の厚さを含めない列の長さ
	/// </summary>
	public int FieldDataArrayColLengthNoneWall { get { return _fieldDataArray.GetLength(1) - _wallCol; } }

	/// <summary>
	/// 
	/// </summary>
	/// <param name="stageRow">壁を含めた行</param>
	/// <param name="stageCol">壁を含めた列</param>
	/// <param name="wallRow">行の壁の厚さ</param>
	/// <param name="wallCol">列の壁の厚さ</param>
	public FieldDataScript(int stageRow, int stageCol, int wallRow, int wallCol)
	{
		_fieldDataArray = new FieldDataType[stageRow, stageCol];
		this._wallCol = wallCol;
		this._wallRow = wallRow;
	}

	/// <summary>
	/// 配列データの参照
	/// </summary>
	/// <param name="row">列</param>
	/// <param name="col">行</param>
	/// <returns>参照先のデータ</returns>
	public FieldDataType GetFieldData(int row, int col)
	{
		if (row >= _fieldDataArray.GetLength(0) || col >= _fieldDataArray.GetLength(1) || row < 0 || col < 0)
		{
			return FieldDataType.Wall;
		}
		return _fieldDataArray[row, col];
	}

	/// <summary>
	/// 配列データの操作
	/// </summary>
	/// <param name="row">列</param>
	/// <param name="col">行</param>
	/// <param name="data">変更後のデータ</param>
	public void SetFieldArrayData(int row, int col, FieldDataType data)
	{
		_fieldDataArray[row, col] = data;
	}
}
