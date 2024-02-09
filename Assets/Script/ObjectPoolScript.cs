// ---------------------------------------------------------
// ObjectPoolScript.cs
//
// �쐬��:10/31
// �X�V��:12/15
// �쐬��:�F�J�q
// --------------------------------------------------------- 

using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// �I�u�W�F�N�g�v�[��
/// </summary>
public class ObjectPoolScript : MonoBehaviour
{
	[SerializeField]
	private GameObject _puyoPrefab = default;
	[SerializeField]
	private Sprite[] _sprites = default;
	private Queue<PuyoScript> _puyoQueue = new Queue<PuyoScript>();

	/// <summary>
	/// ������
	/// </summary>
	/// <param name="fieldCount">�t�B�[���h�̌�</param>
	/// <param name="fieldSize">�P�̃t�B�[���h�̑傫��</param>
	public void Initialization(int fieldCount, int fieldSize)
	{
		PuyoScript puyoScriptTemp = default;
		for (int i = 0; i < fieldSize * fieldCount; i++)
		{
			puyoScriptTemp = Instantiate(_puyoPrefab).GetComponent<PuyoScript>();
			_puyoQueue.Enqueue(puyoScriptTemp);
			puyoScriptTemp.MyGameObject.SetActive(false);
		}
	}

	/// <summary>
	/// ����������Ă��Ȃ��Ղ�̎��o��
	/// </summary>
	/// <param name="fieldDataType">�z��ł̃f�[�^</param>
	/// <returns>���o�����Ղ�</returns>
	public PuyoScript GetNonInitPuyo(FieldDataType fieldDataType)
	{
		PuyoScript puyoScriptTemp;
		if (_puyoQueue.Count == 0)
		{
			puyoScriptTemp = Instantiate(_puyoPrefab).GetComponent<PuyoScript>();
		}
		else
		{
			puyoScriptTemp = _puyoQueue.Dequeue();
		}
		puyoScriptTemp.MyGameObject.SetActive(true);
		puyoScriptTemp.MySpriteRenderer.sprite = _sprites[(int)fieldDataType - 2];

		return puyoScriptTemp;
	}

	/// <summary>
	/// �Ղ���폜����
	/// </summary>
	/// <param name="puyoScript">�Ղ�̃f�[�^</param>
	public void DeletePuyo(PuyoScript puyoScript)
	{
		puyoScript.MyGameObject.SetActive(false);

		_puyoQueue.Enqueue(puyoScript);
	}
}
