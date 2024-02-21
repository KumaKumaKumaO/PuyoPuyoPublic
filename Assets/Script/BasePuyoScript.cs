// ---------------------------------------------------------
// BasePuyoScript.cs
//
// 作成日:10/19
// 更新日:12/15
// 作成者:熊谷航
// --------------------------------------------------------- 

using UnityEngine;
using Interface;

/// <summary>
/// ぷよのベース
/// </summary>
public abstract class BasePuyoScript : MonoBehaviour, IPuyoDataGetable, IPuyoOperatable
{
	[SerializeField,Header("配列内における列")]
	private int _fieldDataCol = default;
	[SerializeField,Header("配列内における行")]
	private int _fieldDataRow = default;
	[SerializeField,Header("配列内におけるデータ")]
	private FieldDataType _myFieldDataType = default;

	private SpriteRenderer _mySpriteRenderer = default;
	private Transform _myTransform = default;

	public int Col { get { return _fieldDataCol; } }
	public int Row { get { return _fieldDataRow; } }

	public FieldDataType MyFieldData { get { return _myFieldDataType; } }

	public Transform MyTransform
	{
		get
		{
			if (_myTransform == null)
			{
				_myTransform = transform;
			}
			return _myTransform;
		}
	}
	public GameObject MyGameObject { get { return gameObject; } }

	public SpriteRenderer MySpriteRenderer
	{
		get
		{
			if (_mySpriteRenderer == null)
			{
				_mySpriteRenderer = GetComponent<SpriteRenderer>();
			}
			return _mySpriteRenderer;
		}
	}

	/// <summary>
	/// 初期化
	/// </summary>
	/// <param name="fieldDataCol">配列内の自分の列</param>
	/// <param name="fieldDataRow">配列内の自分の行</param>
	/// <param name="myFieldDataType">配列内の自分のデータ</param>
	public virtual void Initialization(int fieldDataCol, int fieldDataRow, FieldDataType myFieldDataType)
	{
		this._fieldDataCol = fieldDataCol;
		this._fieldDataRow = fieldDataRow;
		this._myFieldDataType = myFieldDataType;
	}

	/// <summary>
	/// ぷよを落ちれるとこまで落とす
	/// </summary>
	/// <param name="fieldDataGetable">配列データ</param>
	public virtual void FallPuyo(IFieldArrayDataGetable fieldDataGetable)
	{
		//下に落ちれるか
		for (; CanMovePuyo(Vector2.down, fieldDataGetable);)
		{
			//下に移動する
			MovePuyo(Vector2.down);
		}
	}

	/// <summary>
	/// ぷよが動けるか
	/// </summary>
	/// <param name="vector">確認したいベクトル</param>
	/// <param name="fieldDataGetable">配列データ</param>
	/// <returns>その方向に動かせるか</returns>
	public virtual bool CanMovePuyo(Vector2 vector, IFieldArrayDataGetable fieldDataGetable)
	{
		return fieldDataGetable.GetFieldData((int)(_fieldDataRow + vector.x), (int)(_fieldDataCol + vector.y))
			== FieldDataType.None;
	}

	/// <summary>
	/// ぷよを動かす
	/// </summary>
	/// <param name="vector">動くベクトル</param>
	public void MovePuyo(Vector2 vector)
	{
		//動きたいベクトルに動く
		_myTransform.position += (Vector3)vector;
		//自身の配列内座標を更新する
		UpdateMyData(vector);
	}

	/// <summary>
	/// 自分の位置情報の更新
	/// </summary>
	/// <param name="vector">更新したいベクトル</param>
	private void UpdateMyData(Vector2 vector)
	{
		//自身の配列内座標を移動する
		_fieldDataCol += (int)vector.y;
		_fieldDataRow += (int)vector.x;
	}
}
