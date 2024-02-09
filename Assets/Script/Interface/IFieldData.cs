namespace Interface
{
    /// <summary>
    /// 配列データを取得できる
    /// </summary>
    public interface IFieldArrayDataGetable
    {
        /// <summary>
        /// 壁の厚さを含めた列の長さ
        /// </summary>
        public int FieldDataArrayColLength { get; }
        /// <summary>
        /// 壁の厚さを含めた行の長さ
        /// </summary>
        public int FieldDataArrayRowLength { get; }
        /// <summary>
        /// 壁の厚さを含めない行の長さ
        /// </summary>
        public int FieldDataArrayRowLengthNoneWall { get; }
        /// <summary>
        /// 壁の厚さを含めない列の長さ
        /// </summary>
        public int FieldDataArrayColLengthNoneWall { get; }
        /// <summary>
        /// 配列データの参照
        /// </summary>
        /// <param name="row">列</param>
        /// <param name="col">行</param>
        /// <returns>参照先のデータ</returns>
        FieldDataType GetFieldData(int row, int col);
    }
    /// <summary>
    /// 配列データに書き込める
    /// </summary>
    public interface IFieldArrayDataSetable
    {
        /// <summary>
        /// 配列データの操作
        /// </summary>
        /// <param name="row">列</param>
        /// <param name="col">行</param>
        /// <param name="data">変更後のデータ</param>
        void SetFieldArrayData(int row, int col, FieldDataType data);
    }

    /// <summary>
    /// 配列データを読み書きできる
    /// </summary>
    public interface IFieldArrayDataControllable : IFieldArrayDataSetable, IFieldArrayDataGetable
	{

	}
}