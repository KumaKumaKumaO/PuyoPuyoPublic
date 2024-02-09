using UnityEngine;
namespace Interface
{
	/// <summary>
	/// ぷよのデータを取得できる
	/// </summary>
	public interface IPuyoDataGetable
	{
		Transform MyTransform { get; }

		int Col { get; }

		int Row { get; }

		GameObject MyGameObject { get; }

		FieldDataType MyFieldData { get; }
	}

	/// <summary>
	/// ぷよを操作できる
	/// </summary>
	public interface IPuyoOperatable
	{
		/// <summary>
		/// ぷよを移動する
		/// </summary>
		/// <param name="moveDirection">移動したい方向</param>
		void MovePuyo(Vector2 moveDirection);

		/// <summary>
		/// ぷよが移動できるかの判定
		/// </summary>
		/// <param name="direction">確認する方向</param>
		/// <param name="fieldDataGetable">配列データ</param>
		/// <returns>ぷよが移動できるか</returns>
		bool CanMovePuyo(Vector2 direction, IFieldArrayDataGetable fieldDataGetable);

		/// <summary>
		/// ぷよをできるだけ下に下げる
		/// </summary>
		/// <param name="fieldDataGetable">配列データ</param>
		void FallPuyo(IFieldArrayDataGetable fieldDataGetable);

	}

	/// <summary>
	/// 操作、取得のどちらもできる
	/// </summary>

	public interface IPuyoControllable : IPuyoOperatable, IPuyoDataGetable
	{

	}
}