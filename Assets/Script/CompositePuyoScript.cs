// ---------------------------------------------------------
// CompositePuyoScript.cs
//
// 作成日:10/19
// 更新日:12/15
// 作成者:熊谷航
// --------------------------------------------------------- 

using UnityEngine;
using Interface;

/// <summary>
/// ぷよ二つをまとめて動かすクラス
/// </summary>
public class CompositePuyoScript : ICompositePuyoOperatable, ICompositePuyoStateChackable
{
	/// <summary>
	/// 現在の回転のステート
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
	/// 初期化
	/// </summary>
	public void Initialization()
	{
		//自身のステートを「動ける」に変更する
		this._myState = CompositePuyoState.CanMoving;
		//回転状況を右に設定する
		this._myRotationState = RotationState.Right;
	}

	/// <summary>
	/// 操作する二つのぷよを指定した位置に移動する
	/// </summary>
	/// <param name="movePos">移動先</param>
	public void MoveFieldCompositePuyo(Vector2 movePos, Vector2 offsetVector)
	{
		//二つのぷよの位置を移動する
		foreach (IPuyoDataGetable puyoDataGetable in _puyos)
		{
			puyoDataGetable.MyTransform.position = movePos;
		}
		//二つ目のぷよの位置を右にずらす
		_puyos[1].MovePuyo(offsetVector);
	}

	/// <summary>
	/// フィールド上で移動する
	/// </summary>
	/// <param name="moveVector">動くベクトル</param>
	public void MoveCompositePuyo(Vector2 moveVector)
	{

		if (_myState == CompositePuyoState.CanMoving)
		{
			//配列内のぷよが動くベクトルに動けるか確認する
			foreach (IPuyoOperatable puyoOperatable in _puyos)
			{
				if (!puyoOperatable.CanMovePuyo(moveVector, _fieldArrayDataControllable))
				{
					//どちらかが下に移動できない場合
					if (moveVector == Vector2.down)
					{
						//ハードドロップにする
						HardDropCompositePuyo();
						//自身のステートを「終了」にする
						_myState = CompositePuyoState.End;
					}
					return;
				}
			}
			//両方のぷよを動かしたいベクトルに動かす
			foreach (IPuyoOperatable puyoOperatable in _puyos)
			{
				puyoOperatable.MovePuyo(moveVector);
			}
		}
	}

	/// <summary>
	/// ハードドロップ
	/// </summary>
	public void HardDropCompositePuyo()
	{
		if (_myState == CompositePuyoState.CanMoving)
		{
			//自身の回転のステートが「上」の場合は下側のぷよから処理する
			if (_myRotationState == RotationState.Top)
			{
				for (int i = 0; i < _puyos.Length; i++)
				{
					HardDropProcess(i);
				}
			}
			//そのほかの場合は上側のぷよから処理する
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
	/// ハードドロップの動作
	/// </summary>
	/// <param name="index">どちらのぷよかの指標</param>
	private void HardDropProcess(int index)
	{
		//ぷよを落とせるとこまで落とす
		_puyos[index].FallPuyo(_fieldArrayDataControllable);
		//データを配列に格納する
		_fieldArrayDataControllable.SetFieldArrayData((_puyos[index]).Row
			, (_puyos[index]).Col, (_puyos[index]).MyFieldData);
		//フィールドに設置されたオブジェクトとして格納する
		_fieldObjectAddble.AddFieldPuyoObject(_puyos[index]);
	}

	/// <summary>
	/// 回転する
	/// </summary>
	/// <param name="rotateDirection">回転の方向</param>
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
	/// 通常の回転の判定
	/// </summary>
	/// <param name="rotateDirection">回転の方向</param>
	/// <returns>通常の回転ができるか</returns>
	private bool CanNormalRotation(RotateDirection rotateDirection)
	{
		switch (_myRotationState)
		{
			//軸じゃないほうのぷよが上にあるとき
			case RotationState.Top:
				{
					//右回転をしたい場合
					if (rotateDirection == RotateDirection.Right)
					{
						//軸の一つ右になにもないか
						return _fieldArrayDataControllable.GetFieldData(_puyos[0].Row + 1, _puyos[0].Col) == FieldDataType.None;
					}
					//左回転をしたい場合
					else
					{
						//軸の一つ左になにもないか
						return _fieldArrayDataControllable.GetFieldData(_puyos[0].Row - 1, _puyos[0].Col) == FieldDataType.None;
					}
				}
			//軸じゃないほうのぷよが右にあるとき
			case RotationState.Right:
				{
					//右回転をしたい場合
					if (rotateDirection == RotateDirection.Right)
					{
						//軸の一つ下になにもないか
						return _fieldArrayDataControllable.GetFieldData(_puyos[0].Row, _puyos[0].Col - 1) == FieldDataType.None;
					}
					//軸の上には必ず何もないのでtrueを返す
					return true;
				}
			//軸じゃないほうのぷよが下にあるとき
			case RotationState.Bottom:
				{
					//右回転をしたい場合
					if (rotateDirection == RotateDirection.Right)
					{
						//軸の一つ左になにもないか
						return _fieldArrayDataControllable.GetFieldData(_puyos[0].Row - 1, _puyos[0].Col) == FieldDataType.None;
					}
					//左回転をしたい場合
					else
					{
						//軸の一つ右になにもないか
						return _fieldArrayDataControllable.GetFieldData(_puyos[0].Row + 1, _puyos[0].Col) == FieldDataType.None;
					}
				}
			//軸じゃないほうのぷよが左にあるとき
			case RotationState.Left:
				{
					//右回転をしたい場合
					if (rotateDirection == RotateDirection.Right)
					{
						//軸の上には必ず何もないのでtrueを返す
						return true;
					}
					//左回転をしたい場合
					else
					{
						//軸の一つ下になにもないか
						return _fieldArrayDataControllable.GetFieldData(_puyos[0].Row, _puyos[0].Col - 1) == FieldDataType.None;
					}
				}
		}
		//例外処理
		return false;
	}

	/// <summary>
	/// 通常の回転
	/// </summary>
	/// <param name="rotateDirection">回転の方向</param>
	private void NormalRotation(RotateDirection rotateDirection)
	{
		switch (_myRotationState)
		{
			//軸じゃないほうのぷよが上にあるとき
			case RotationState.Top:
				{
					//回転したい方向が右のとき
					if (rotateDirection == RotateDirection.Right)
					{
						//回転のステートを右に変更
						_myRotationState = RotationState.Right;
						//軸じゃないほうのぷよを軸の右に移動
						_puyos[1].MovePuyo(Vector2.down + Vector2.right);
					}
					//回転したい方向が左のとき
					else
					{
						//回転のステートを左に変更
						_myRotationState = RotationState.Left;
						//軸じゃないほうのぷよを軸の左に移動
						_puyos[1].MovePuyo(Vector2.down + Vector2.left);
					}
					break;
				}
			//軸じゃないほうのぷよが右にあるとき
			case RotationState.Right:
				{
					//回転したい方向が右のとき
					if (rotateDirection == RotateDirection.Right)
					{
						//回転のステートを下に変更
						_myRotationState = RotationState.Bottom;
						//軸じゃないほうのぷよを軸の下に移動
						_puyos[1].MovePuyo(Vector2.down + Vector2.left);
					}
					//回転したい方向が左のとき
					else
					{
						//回転のステートを上に変更
						_myRotationState = RotationState.Top;
						//軸じゃないほうのぷよを軸の上に移動
						_puyos[1].MovePuyo(Vector2.up + Vector2.left);
					}
					break;
				}
			//軸じゃないほうのぷよが下にあるとき
			case RotationState.Bottom:
				{
					//回転したい方向が右のとき
					if (rotateDirection == RotateDirection.Right)
					{
						//回転のステートを左に変更
						_myRotationState = RotationState.Left;
						//軸じゃないほうのぷよを軸の左に移動
						_puyos[1].MovePuyo(Vector2.up + Vector2.left);
					}
					//回転したい方向が左のとき
					else
					{
						//回転のステートを右に変更
						_myRotationState = RotationState.Right;
						//軸じゃないほうのぷよを軸の右に移動
						_puyos[1].MovePuyo(Vector2.up + Vector2.right);
					}
					break;
				}
			//軸じゃないほうのぷよが左にあるとき
			case RotationState.Left:
				{
					//回転したい方向が右のとき
					if (rotateDirection == RotateDirection.Right)
					{
						//回転のステートを上に変更
						_myRotationState = RotationState.Top;
						//軸じゃないほうのぷよを軸の上に移動
						_puyos[1].MovePuyo(Vector2.up + Vector2.right);
					}
					//回転したい方向が左のとき
					else
					{
						//回転のステートを下に変更
						_myRotationState = RotationState.Bottom;
						//軸じゃないほうのぷよを軸の下に移動
						_puyos[1].MovePuyo(Vector2.down + Vector2.right);
					}
					break;
				}
		}
	}

	/// <summary>
	/// 壁にぶつかった際の回転の判定
	/// </summary>
	/// <param name="rotateDirection">回転の方向</param>
	/// <returns>壁にぶつかった際の回転ができるか</returns>
	private bool CanWallRotation(RotateDirection rotateDirection)
	{
		switch (_myRotationState)
		{
			//軸じゃないほうのぷよが上にあるとき
			case RotationState.Top:
				{
					//回転したい方向が右のとき
					if (rotateDirection == RotateDirection.Right)
					{
						//軸の一つ左がなにもないとき
						return _fieldArrayDataControllable.GetFieldData(_puyos[0].Row - 1, _puyos[0].Col) == FieldDataType.None;
					}
					//回転したい方向が左のとき
					else
					{
						//軸の一つ右がなにもないとき
						return _fieldArrayDataControllable.GetFieldData(_puyos[0].Row + 1, _puyos[0].Col) == FieldDataType.None;
					}
				}
			//軸じゃないほうのぷよが右にあるとき
			case RotationState.Right:
				{
					return true;
				}
			//軸じゃないほうのぷよが下にあるとき
			case RotationState.Bottom:
				{
					//回転したい方向が右のとき
					if (rotateDirection == RotateDirection.Right)
					{
						//軸の一つ右がなにもないとき
						return _fieldArrayDataControllable.GetFieldData(_puyos[0].Row + 1, _puyos[0].Col) == FieldDataType.None;
					}
					//回転したい方向が左のとき
					else
					{
						//軸の一つ左がなにもないとき
						return _fieldArrayDataControllable.GetFieldData(_puyos[0].Row - 1, _puyos[0].Col) == FieldDataType.None;
					}
				}
			//軸じゃないほうのぷよが左にあるとき
			case RotationState.Left:
				{
					//軸の一つ上がなにもないとき
					return _fieldArrayDataControllable.GetFieldData(_puyos[0].Row, _puyos[0].Col + 1) == FieldDataType.None;
				}
		}
		//例外処理
		return false;
	}

	/// <summary>
	/// 壁にぶつかった際の回転
	/// </summary>
	/// <param name="rotateDirection">回転の方向</param>
	private void WallRotation(RotateDirection rotateDirection)
	{
		switch (_myRotationState)
		{
			//軸じゃないほうのぷよが上にあるとき
			case RotationState.Top:
				{
					//回転したい方向が右のとき
					if (rotateDirection == RotateDirection.Right)
					{
						//回転ステートを右に設定
						_myRotationState = RotationState.Right;
						//軸じゃないほうのぷよを下に移動
						_puyos[1].MovePuyo(Vector2.down);
						//軸のぷよを左に移動
						_puyos[0].MovePuyo(Vector2.left);
					}
					//回転したい方向が左のとき
					else
					{
						//回転ステートを左に設定
						_myRotationState = RotationState.Left;
						//軸じゃないほうのぷよを下に移動
						_puyos[1].MovePuyo(Vector2.down);
						//軸のぷよを右に移動
						_puyos[0].MovePuyo(Vector2.right);
					}
					break;
				}
			//軸じゃないほうのぷよが右にあるとき
			case RotationState.Right:
				{
					//回転ステートを下に設定
					_myRotationState = RotationState.Bottom;
					//軸じゃないほうのぷよを左に移動
					_puyos[1].MovePuyo(Vector2.left);
					//軸のぷよを上に移動
					_puyos[0].MovePuyo(Vector2.up);
					break;
				}
			//軸じゃないほうのぷよが下にあるとき
			case RotationState.Bottom:
				{
					//回転したい方向が右のとき
					if (rotateDirection == RotateDirection.Right)
					{
						//回転ステートを左に設定
						_myRotationState = RotationState.Left;
						//軸じゃないほうのぷよを下に移動
						_puyos[1].MovePuyo(Vector2.up);
						//軸のぷよを右に移動
						_puyos[0].MovePuyo(Vector2.right);
					}
					//回転したい方向が左のとき
					else
					{
						//回転ステートを右に設定
						_myRotationState = RotationState.Right;
						//軸じゃないほうのぷよを上に移動
						_puyos[1].MovePuyo(Vector2.up);
						//軸のぷよを左に移動
						_puyos[0].MovePuyo(Vector2.left);
					}
					break;
				}
			//軸じゃないほうのぷよが左にあるとき
			case RotationState.Left:
				{
					//回転ステートを上に設定
					_myRotationState = RotationState.Top;
					//軸のぷよを左に移動
					_puyos[0].MovePuyo(Vector2.up);
					//軸じゃないほうのぷよを右上に移動
					_puyos[1].MovePuyo(Vector2.right);
					break;
				}
		}
	}

}
