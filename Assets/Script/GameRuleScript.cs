// ---------------------------------------------------------
// GameRuleScript.cs
//
// 作成日:10/19
// 更新日:12/15
// 作成者:熊谷航
// --------------------------------------------------------- 

using System.Collections.Generic;
using Interface;

/// <summary>
/// ぷよが消える判定とゲームオーバー判定
/// </summary>
public class GameRuleScript
{
	/// <summary>
	/// 配列内での場所
	/// </summary>
	private struct ArrayPosData
	{
		int _row;
		int _col;
		/// <summary>
		/// 列
		/// </summary>
		public int Row { get { return _row; } }
		/// <summary>
		/// 行
		/// </summary>
		public int Col { get { return _col; } }
		public ArrayPosData(int row, int col)
		{
			this._row = row;
			this._col = col;
		}
	}

	//消えるために必要なぷよの数
	private int canPuyoDeleteValue = 4;
	//フィールドからオブジェクトを消すためのインターフェース
	private IFieldPuyoObjectRemovable _fieldObjectRemovable = default;
	//フィールド配列からデータを取得するためのインターフェース
	private IFieldArrayDataGetable _fieldDataGetable = default;
	//探索したぷよの場所データを格納するリスト
	private List<ArrayPosData> _searchedFieldDataPosList = new List<ArrayPosData>();
	//消すべきぷよの場所データを格納するリスト
	private List<ArrayPosData> _deletePuyoDataPosList = new List<ArrayPosData>();

	//探索したか
	private bool isSearched = false;

	/// <summary>
	/// 初期化
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
	/// ゲームオーバーになるかどうか
	/// </summary>
	/// <returns>ゲームオーバーゾーンに何かあるか</returns>
	public bool IsGameOver()
	{
		return _fieldDataGetable.GetFieldData(_fieldDataGetable.FieldDataArrayRowLength / 2, _fieldDataGetable.FieldDataArrayColLength - 1 - 2)
			!= FieldDataType.None;
	}

	/// <summary>
	/// 消せるぷよが一つでも存在するか
	/// </summary>
	public bool ContainDeletePuyo()
	{
		//配列データの一時保存用変数
		FieldDataType fieldDataTypeTemp;
		//配列を端から読み込む
		for (int i = 0; i < _fieldDataGetable.FieldDataArrayColLength; i++)
		{
			for (int k = 0; k < _fieldDataGetable.FieldDataArrayRowLength; k++)
			{
				//配列データを一時的に保存
				fieldDataTypeTemp = _fieldDataGetable.GetFieldData(k, i);
				//配列データが無いまたは、壁の場合
				if (fieldDataTypeTemp == FieldDataType.None
					|| fieldDataTypeTemp == FieldDataType.Wall)
				{
					//次のデータを読むために指標を進める
					continue;
				}
				//隣接している同じ種類のぷよを探索用リストに格納する
				DeleteDitectPuyo(k, i, fieldDataTypeTemp, _searchedFieldDataPosList);
				//↑で更新された探索用リストの要素数を消すために必要なぷよの数が超えていたら
				if (_searchedFieldDataPosList.Count >= canPuyoDeleteValue)
				{
					//リストの中身を全て削除予定リストに追加する
					foreach (ArrayPosData arrayPosData in _searchedFieldDataPosList)
					{
						_deletePuyoDataPosList.Add(arrayPosData);
					}
				}
				//探索用リストの初期化
				_searchedFieldDataPosList.Clear();
			}
		}
		//探索が行われたフラグをオンにする
		isSearched = true;
		//消す予定リストの要素数が０より大きい場合に真を返す
		return (_deletePuyoDataPosList.Count > 0);
	}

	/// <summary>
	/// 消せるぷよを消す
	/// </summary>
	public void DeletePuyo()
	{
		//探索が行われたか
		if (!isSearched)
		{
			//消せるぷよが存在するか
			ContainDeletePuyo();
		}
		//配列の中身を取り出す
		foreach (ArrayPosData arrayPosData in _deletePuyoDataPosList)
		{
			//指定した位置にあるオブジェクトの破棄
			_fieldObjectRemovable.RemoveFieldObject(arrayPosData.Row, arrayPosData.Col);
		}
		//削除リストを初期化する
		_deletePuyoDataPosList.Clear();
		//探索が行われたフラグをオフにする
		isSearched = false;
	}

	/// <summary>
	/// 隣接している同じ種類のぷよの配列内位置データをリストに保存する
	/// 再帰的に呼び出す
	/// </summary>
	/// <param name="row">探したい列</param>
	/// <param name="col">探したい行</param>
	/// <param name="fieldDataType">探したいぷよの種類</param>
	/// <param name="posDataList">データを保存するリスト</param>
	private void DeleteDitectPuyo(int row, int col, FieldDataType fieldDataType, List<ArrayPosData> posDataList)
	{
		//探索したぷよリスト内に、探索しようとしているデータが無いかを確認する
		foreach (ArrayPosData arrayPosData in posDataList)
		{
			if (arrayPosData.Row == row && arrayPosData.Col == col)
			{
				//あった場合は探索を終了する
				return;
			}
		}
		//探索したリストにデータを格納する
		posDataList.Add(new ArrayPosData(row, col));
		//探索しているぷよの列 + 1が同じぷよの場合
		if (_fieldDataGetable.GetFieldData(row + 1, col) == fieldDataType)
		{
			DeleteDitectPuyo(row + 1, col, fieldDataType, posDataList);
		}
		//探索しているぷよの列 - 1が同じぷよの場合
		if (_fieldDataGetable.GetFieldData(row - 1, col) == fieldDataType)
		{
			DeleteDitectPuyo(row - 1, col, fieldDataType, posDataList);
		}
		//探索しているぷよの行 + 1が同じぷよの場合
		if (_fieldDataGetable.GetFieldData(row, col + 1) == fieldDataType)
		{
			DeleteDitectPuyo(row, col + 1, fieldDataType, posDataList);
		}
		//探索しているぷよの行 - 1が同じぷよの場合
		if (_fieldDataGetable.GetFieldData(row, col - 1) == fieldDataType)
		{
			DeleteDitectPuyo(row, col - 1, fieldDataType, posDataList);
		}
	}
}
