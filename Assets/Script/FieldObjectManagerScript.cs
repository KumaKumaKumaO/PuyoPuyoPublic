// ---------------------------------------------------------
// FieldObjectManagerScript.cs
//
// 作成日:10/19
// 更新日:12/15
// 作成者:熊谷航
// --------------------------------------------------------- 

using System.Collections.Generic;
using UnityEngine;
using System;
using Interface;

/// <summary>
/// フィールドに配置されているオブジェクトの操作
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
	/// 初期化されたぷよ取得する
	/// </summary>
	private void GetInitPuyo()
	{
		FieldDataType fieldDataType = default;
		//順番待ちのぷよを操作ぷよに移動して順番待ちのぷよを用意し初期化する
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
	/// 配列で扱うデータをランダムで選ぶ
	/// </summary>
	/// <returns>選ばれたデータ</returns>
	private FieldDataType SelectDataType()
	{
		return (FieldDataType)UnityEngine.Random.Range(2, Enum.GetValues(typeof(FieldDataType)).Length);
	}

	/// <summary>
	/// 順番待ちにいるぷよをポップする
	/// </summary>
	public void NextPuyoPop()
	{
		//初期化したぷよを取得
		GetInitPuyo();
		//位置を初期化する
		MoveNextPos(_nextPos);
		//まとめて動かすクラスを初期化する
		_myCompositePuyo.Initialization();
		//スタートの場所に移動して二つめを右にずらす
		_myCompositePuyo.MoveFieldCompositePuyo(_popPos, Vector2.right);
	}

	/// <summary>
	/// 渡された場所と同じ場所にあるオブジェクトを消す
	/// </summary>
	/// <param name="col">行</param>
	/// <param name="row">列</param>
	public void RemoveFieldObject(int row, int col)
	{
		//フィールドに存在するオブジェクトの中から一致するものを消す
		for (int i = 0; i < _fieldObjects.Count; i++)
		{
			//行と列が取得したオブジェクトと同じか
			if (_fieldObjects[i].Col == col && _fieldObjects[i].Row == row)
			{
				IPuyoDataGetable puyoDataGetableTemp = _fieldObjects[i];
				//フィールドに存在するオブジェクトのリストから消す
				_fieldObjects.Remove(puyoDataGetableTemp);
				//配列の中身をなにもない(None)にする
				((IFieldArrayDataSetable)_fieldDataScript).SetFieldArrayData(row, col, FieldDataType.None);
				//オブジェクトを破棄する
				_objectPoolScript.DeletePuyo((PuyoScript)puyoDataGetableTemp);
			}
		}
	}

	/// <summary>
	/// フィールドに存在するオブジェクトを更新する
	/// </summary>
	public void UpdateFieldObject()
	{
		//フィールドに存在するPuyoを下にぶつかるまで移動させる
		foreach (IPuyoDataGetable puyoDataGetable in _fieldObjects)
		{
			//いまいる位置の配列データをなにもない(None)にする
			((IFieldArrayDataSetable)_fieldDataScript).SetFieldArrayData(puyoDataGetable.Row, puyoDataGetable.Col, FieldDataType.None);
			//下になにかあるところまで下に移動させる
			((IPuyoOperatable)puyoDataGetable).FallPuyo(_fieldDataScript);
			//下に移動したあとに自分のデータを配列に入力する
			((IFieldArrayDataSetable)_fieldDataScript).SetFieldArrayData(puyoDataGetable.Row, puyoDataGetable.Col, puyoDataGetable.MyFieldData);
		}
	}

	/// <summary>
	/// フィールドに配置されたPuyoとして格納する
	/// </summary>
	/// <param name="basePuyoScript">格納したいPuyoScript</param>
	public void AddFieldPuyoObject(IPuyoDataGetable basePuyoScript)
	{
		_fieldObjects.Add(basePuyoScript);
	}

	/// <summary>
	/// 位置を初期化
	/// </summary>
	/// <param name="initializePos">初期の位置</param>
	public void MoveNextPos(Vector2 initializePos)
	{
		_nextPuyos[0].MyTransform.position = initializePos;
		_nextPuyos[1].MyTransform.position = initializePos + Vector2.down;
	}
}
