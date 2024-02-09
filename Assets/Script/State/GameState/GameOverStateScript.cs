//---------------------------------------------------------
// GameOverStateScript.cs
//
// �쐬��:10/19
// �X�V��:12/15
// �쐬��:�F�J�q
// --------------------------------------------------------- 

using Interface;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// �Q�[���I�[�o�[���̃X�e�[�g
/// </summary>
public class GameOverStateScript : BaseGameStateScript
{
	private GameObject _gameOverCanvasObject = default;

	private string _titleSceneName = "TitleScene";

	public GameOverStateScript(IGameManagerStateChangable gameManagerStateChangable, IInput input, GameObject gameOverCanvasObject) : base(gameManagerStateChangable, input)
	{
		_gameOverCanvasObject = gameOverCanvasObject;
	}

	public override void Enter()
	{
		//�S�ẴQ�[���I�u�W�F�N�g�̒�����J�����A�Q�[���}�l�[�W���[�A�I�[�f�B�I�ȊO�̃I�u�W�F�N�g������
		foreach (GameObject obj in Object.FindObjectsOfType(typeof(GameObject)))
		{
			if (obj.GetComponent<Camera>() != null || obj.GetComponent<GameManagerScript>() != null || obj.GetComponent<AudioScript>() != null)
			{
				continue;
			}
			obj.SetActive(false);
		}
		//�Q�[���I�[�o�[�p�L�����o�X���o��
		_gameOverCanvasObject.SetActive(true);
		//BGM�̃��[�v���~�߂āA�Q�[���I�[�o�[BGM�𗬂�
		AudioScript.InstanceAudioScript.BGMLoopMute();
		AudioScript.InstanceAudioScript.PlayGameOverSound();
	}
	public override void Execute()
	{
		//���肪�����ꂽ��
		if (_input.IsSubmit())
		{
			//�^�C�g���V�[�������[�h����
			SceneManager.LoadScene(_titleSceneName);
		}
	}
	public override void Exit()
	{
		//�N���X�̎Q�Ƃ�؂�
		_gameOverCanvasObject = null;
		//BGM���܂��Đ��ł���悤�ɂ���
		AudioScript.InstanceAudioScript.BGMLoopUnMute();
	}
}
