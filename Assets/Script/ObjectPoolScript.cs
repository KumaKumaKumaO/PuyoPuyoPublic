// ---------------------------------------------------------
// ObjectPoolScript.cs
//
// 作成日:10/31
// 更新日:12/15
// 作成者:熊谷航
// --------------------------------------------------------- 

using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// オブジェクトプール
/// </summary>
public class ObjectPoolScript : MonoBehaviour
{
	[SerializeField]
	private GameObject _puyoPrefab = default;
	[SerializeField]
	private Sprite[] _sprites = default;
	private Queue<PuyoScript> _puyoQueue = new Queue<PuyoScript>();

	/// <summary>
	/// 初期化
	/// </summary>
	/// <param name="fieldCount">フィールドの個数</param>
	/// <param name="fieldSize">１つのフィールドの大きさ</param>
	public void Initialization(int fieldCount, int fieldSize)
	{
		PuyoScript puyoScriptTemp = default;
		for (int i = 0; i < fieldSize * fieldCount; i++)
		{
			puyoScriptTemp = Instantiate(_puyoPrefab).GetComponent<PuyoScript>();
			_puyoQueue.Enqueue(puyoScriptTemp);
			puyoScriptTemp.MyGameObject.SetActive(false);
		}
	}

	/// <summary>
	/// 初期化されていないぷよの取り出し
	/// </summary>
	/// <param name="fieldDataType">配列でのデータ</param>
	/// <returns>取り出したぷよ</returns>
	public PuyoScript GetNonInitPuyo(FieldDataType fieldDataType)
	{
		PuyoScript puyoScriptTemp;
		if (_puyoQueue.Count == 0)
		{
			puyoScriptTemp = Instantiate(_puyoPrefab).GetComponent<PuyoScript>();
		}
		else
		{
			puyoScriptTemp = _puyoQueue.Dequeue();
		}
		puyoScriptTemp.MyGameObject.SetActive(true);
		puyoScriptTemp.MySpriteRenderer.sprite = _sprites[(int)fieldDataType - 2];

		return puyoScriptTemp;
	}

	/// <summary>
	/// ぷよを削除する
	/// </summary>
	/// <param name="puyoScript">ぷよのデータ</param>
	public void DeletePuyo(PuyoScript puyoScript)
	{
		puyoScript.MyGameObject.SetActive(false);

		_puyoQueue.Enqueue(puyoScript);
	}
}
