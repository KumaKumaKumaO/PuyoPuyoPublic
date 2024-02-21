using UnityEngine;
namespace Interface
{
	/// <summary>
	/// ぷよのまとまりを操作できる
	/// </summary>
	public interface ICompositePuyoOperatable
	{
		/// <summary>
		/// ハードドロップをする
		/// </summary>
		void HardDropCompositePuyo();
		/// <summary>
		/// 移動する
		/// </summary>
		/// <param name="moveDirection">移動する向き</param>
		void MoveCompositePuyo(Vector2 moveDirection);
		/// <summary>
		/// 回転する
		/// </summary>
		/// <param name="rotateDirection">回転の向き</param>
		void RotationCompositePuyo(RotateDirection rotateDirection);
	}
	/// <summary>
	/// ぷよのまとまりの状態を確認できる
	/// </summary>
	public interface ICompositePuyoStateChackable
	{
		/// <summary>
		/// ぷよのまとまりの状態
		/// </summary>
		CompositePuyoState CompositePuyoState { get; }
	}
}
