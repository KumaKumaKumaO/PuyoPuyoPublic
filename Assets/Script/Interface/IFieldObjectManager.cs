namespace Interface
{
	/// <summary>
	/// フィールドに設置したぷよを追加できる
	/// </summary>
	public interface IFieldPuyoObjectAddble
	{
		/// <summary>
		/// フィールドに設置したぷよをリストに追加する
		/// </summary>
		/// <param name="basePuyoScript">追加したいぷよのデータ</param>
		void AddFieldPuyoObject(IPuyoDataGetable basePuyoScript);
	}
	/// <summary>
	/// フィールドに設置したぷよを消せる
	/// </summary>
	public interface IFieldPuyoObjectRemovable
	{
		/// <summary>
		/// 渡された場所と同じ場所にあるオブジェクトを消す
		/// </summary>
		/// <param name="col">行</param>
		/// <param name="row">列</param>
		void RemoveFieldObject(int col, int row);
	}
	/// <summary>
	/// フィールドに設置したぷよの更新ができる
	/// </summary>
	public interface IFieldObjectUpdatable
	{
		/// <summary>
		/// フィールドに存在するオブジェクトを更新する
		/// </summary>
		void UpdateFieldObject();
	}
	/// <summary>
	/// 次のぷよをポップできる
	/// </summary>
	public interface INextPuyoPopable
	{
		/// <summary>
		/// 次のぷよをポップする
		/// </summary>
		void NextPuyoPop();
		/// <summary>
		/// ぷよのまとまり
		/// </summary>
		CompositePuyoScript MyCompositePuyoScript { get; }
	}
	/// <summary>
	/// 配列データを取得できる
	/// </summary>
	public interface IFieldDataScriptGetable
	{
		/// <summary>
		/// 配列データを取得する
		/// </summary>
		FieldDataScript FieldDataScript { get; }
	}
}
