// ---------------------------------------------------------
// DebugSystemScript.cs
//
// �쐬��:10/19
// �X�V��:12/15
// �쐬��:�F�J�q
// --------------------------------------------------------- 
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// �z����̃f�[�^��\������
/// </summary>
public class DebugSystemScript : MonoBehaviour
{
	[SerializeField,Header("�o�͂���e�L�X�g")]
	private Text _text = default;
	[SerializeField,Header("�ǂ̃v���C���[�̔z���\�����邩")]
	private int _selectPlayerNum = default;
	[SerializeField,Header("�Q�[���}�l�[�W���[")]
	private GameManagerScript _gameManager = default;

	private string _data = default;

	private void Update()
	{
		//������
		_data = "";
		//�z����̏������ׂ�string�Ɋi�[����
		for (int i = _gameManager.playField[_selectPlayerNum].FieldObjectManagerScript.FieldDataScript.FieldDataArrayColLength - 1; i >= 0; i--)
		{
			for (int k = 0; k < _gameManager.playField[_selectPlayerNum].FieldObjectManagerScript.FieldDataScript.FieldDataArrayRowLength; k++)
			{
				_data += ((int)_gameManager.playField[_selectPlayerNum].FieldObjectManagerScript.FieldDataScript.GetFieldData(k, i) + ",");
			}
			_data += "\n";
		}
		//�i�[�����f�[�^���e�L�X�g�ɓ����
		_text.text = _data;
	}
}
