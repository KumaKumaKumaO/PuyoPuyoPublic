// ---------------------------------------------------------
// BasePuyoScript.cs
//
// �쐬��:10/19
// �X�V��:12/15
// �쐬��:�F�J�q
// --------------------------------------------------------- 

using UnityEngine;
using Interface;

/// <summary>
/// �Ղ�̃x�[�X
/// </summary>
public abstract class BasePuyoScript : MonoBehaviour, IPuyoDataGetable, IPuyoOperatable
{
	[SerializeField,Header("�z����ɂ������")]
	private int _fieldDataCol = default;
	[SerializeField,Header("�z����ɂ�����s")]
	private int _fieldDataRow = default;
	[SerializeField,Header("�z����ɂ�����f�[�^")]
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
	/// ������
	/// </summary>
	/// <param name="fieldDataCol">�z����̎����̗�</param>
	/// <param name="fieldDataRow">�z����̎����̍s</param>
	/// <param name="myFieldDataType">�z����̎����̃f�[�^</param>
	public virtual void Initialization(int fieldDataCol, int fieldDataRow, FieldDataType myFieldDataType)
	{
		this._fieldDataCol = fieldDataCol;
		this._fieldDataRow = fieldDataRow;
		this._myFieldDataType = myFieldDataType;
	}

	/// <summary>
	/// �Ղ�𗎂����Ƃ��܂ŗ��Ƃ�
	/// </summary>
	/// <param name="fieldDataGetable">�z��f�[�^</param>
	public virtual void FallPuyo(IFieldArrayDataGetable fieldDataGetable)
	{
		//���ɗ�����邩
		for (; CanMovePuyo(Vector2.down, fieldDataGetable);)
		{
			//���Ɉړ�����
			MovePuyo(Vector2.down);
		}
	}

	/// <summary>
	/// �Ղ悪�����邩
	/// </summary>
	/// <param name="vector">�m�F�������x�N�g��</param>
	/// <param name="fieldDataGetable">�z��f�[�^</param>
	/// <returns>���̕����ɓ������邩</returns>
	public virtual bool CanMovePuyo(Vector2 vector, IFieldArrayDataGetable fieldDataGetable)
	{
		return fieldDataGetable.GetFieldData((int)(_fieldDataRow + vector.x), (int)(_fieldDataCol + vector.y))
			== FieldDataType.None;
	}

	/// <summary>
	/// �Ղ�𓮂���
	/// </summary>
	/// <param name="vector">�����x�N�g��</param>
	public void MovePuyo(Vector2 vector)
	{
		//���������x�N�g���ɓ���
		_myTransform.position += (Vector3)vector;
		//���g�̔z������W���X�V����
		UpdateMyData(vector);
	}

	/// <summary>
	/// �����̈ʒu���̍X�V
	/// </summary>
	/// <param name="vector">�X�V�������x�N�g��</param>
	private void UpdateMyData(Vector2 vector)
	{
		//���g�̔z������W���ړ�����
		_fieldDataCol += (int)vector.y;
		_fieldDataRow += (int)vector.x;
	}
}
