namespace Interface
{
    /// <summary>
    /// �z��f�[�^���擾�ł���
    /// </summary>
    public interface IFieldArrayDataGetable
    {
        /// <summary>
        /// �ǂ̌������܂߂���̒���
        /// </summary>
        public int FieldDataArrayColLength { get; }
        /// <summary>
        /// �ǂ̌������܂߂��s�̒���
        /// </summary>
        public int FieldDataArrayRowLength { get; }
        /// <summary>
        /// �ǂ̌������܂߂Ȃ��s�̒���
        /// </summary>
        public int FieldDataArrayRowLengthNoneWall { get; }
        /// <summary>
        /// �ǂ̌������܂߂Ȃ���̒���
        /// </summary>
        public int FieldDataArrayColLengthNoneWall { get; }
        /// <summary>
        /// �z��f�[�^�̎Q��
        /// </summary>
        /// <param name="row">��</param>
        /// <param name="col">�s</param>
        /// <returns>�Q�Ɛ�̃f�[�^</returns>
        FieldDataType GetFieldData(int row, int col);
    }
    /// <summary>
    /// �z��f�[�^�ɏ������߂�
    /// </summary>
    public interface IFieldArrayDataSetable
    {
        /// <summary>
        /// �z��f�[�^�̑���
        /// </summary>
        /// <param name="row">��</param>
        /// <param name="col">�s</param>
        /// <param name="data">�ύX��̃f�[�^</param>
        void SetFieldArrayData(int row, int col, FieldDataType data);
    }

    /// <summary>
    /// �z��f�[�^��ǂݏ����ł���
    /// </summary>
    public interface IFieldArrayDataControllable : IFieldArrayDataSetable, IFieldArrayDataGetable
	{

	}
}