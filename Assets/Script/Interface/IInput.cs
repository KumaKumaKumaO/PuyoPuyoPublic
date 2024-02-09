namespace Interface
{
	/// <summary>
	/// 入力
	/// </summary>
	public interface IInput
	{
		/// <summary>
		/// ポーズ入力
		/// </summary>
		/// <returns>入力がされているか</returns>
		bool IsPause();

		/// <summary>
		/// 決定入力
		/// </summary>
		/// <returns>入力がされているか</returns>
		bool IsSubmit();

		/// <summary>
		/// 右に移動入力
		/// </summary>
		/// <returns>入力がされているか</returns>
		bool IsRightMove();

		/// <summary>
		/// 左に移動入力
		/// </summary>
		/// <returns>入力がされているか</returns>
		bool IsLeftMove();

		/// <summary>
		/// 下に移動入力
		/// </summary>
		/// <returns>入力がされているか</returns>
		bool IsDownMove();

		/// <summary>
		/// ハードドロップ入力
		/// </summary>
		/// <returns>入力がされているか</returns>
		bool IsHardDrop();

		/// <summary>
		/// 右に回転入力
		/// </summary>
		/// <returns>入力がされているか</returns>
		bool IsRightTurn();

		/// <summary>
		/// 左に回転入力
		/// </summary>
		/// <returns>入力がされているか</returns>
		bool IsLeftTurn();
	}
}
