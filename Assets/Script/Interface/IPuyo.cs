using UnityEngine;
namespace Interface
{
	/// <summary>
	/// �Ղ�̃f�[�^���擾�ł���
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
	/// �Ղ�𑀍�ł���
	/// </summary>
	public interface IPuyoOperatable
	{
		/// <summary>
		/// �Ղ���ړ�����
		/// </summary>
		/// <param name="moveDirection">�ړ�����������</param>
		void MovePuyo(Vector2 moveDirection);

		/// <summary>
		/// �Ղ悪�ړ��ł��邩�̔���
		/// </summary>
		/// <param name="direction">�m�F�������</param>
		/// <param name="fieldDataGetable">�z��f�[�^</param>
		/// <returns>�Ղ悪�ړ��ł��邩</returns>
		bool CanMovePuyo(Vector2 direction, IFieldArrayDataGetable fieldDataGetable);

		/// <summary>
		/// �Ղ���ł��邾�����ɉ�����
		/// </summary>
		/// <param name="fieldDataGetable">�z��f�[�^</param>
		void FallPuyo(IFieldArrayDataGetable fieldDataGetable);

	}

	/// <summary>
	/// ����A�擾�̂ǂ�����ł���
	/// </summary>

	public interface IPuyoControllable : IPuyoOperatable, IPuyoDataGetable
	{

	}
}