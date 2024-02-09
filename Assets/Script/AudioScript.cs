// ---------------------------------------------------------
// AudioScript.cs
//
// �쐬��:10/31
// �X�V��:12/15
// �쐬��:�F�J�q
// --------------------------------------------------------- 

using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// �������R���g���[������
/// </summary>
public class AudioScript : MonoBehaviour
{
	[SerializeField]
	//�Q�[���V�[���̖��O
	private string _gameSceneName = "GameScene";
	[SerializeField]
	//�^�C�g���V�[���̖��O
	private string _titleSceneName = "TitleScene";
	//���݂̃V�[���̖��O
	private string _nowSceneName = default;
	private bool isMute = false;

	[SerializeField]
	private AudioSource _audioSource = default;
	[SerializeField]
	//�C���Q�[����BGM
	private AudioClip _gameBGM = default;
	[SerializeField]
	//�^�C�g����BGM
	private AudioClip _titleBGM = default;
	[SerializeField]
	//�Ղ�����������̉�
	private AudioClip _deleteSound = default;
	[SerializeField]
	//�Q�[���I�[�o�[��BGM
	private AudioClip _gameOverSound = default;
	//���g�̃C���X�^���X
	private static AudioScript _instance = default;

	public static AudioScript InstanceAudioScript { get => _instance; }
	private void Start()
	{
		//�V���O���g����
		if (_instance == null)
		{
			DontDestroyOnLoad(this);
			_instance = this;
			_nowSceneName = SceneManager.GetActiveScene().name;
		}
		else
		{
			Destroy(this);
		}
	}

	private void Update()
	{
		//�~���[�g���Ă��邩
		if (isMute)
		{
			return;
		}
		//���݂̃V�[�������݂Ɠ�����
		if (SceneManager.GetActiveScene().name == _nowSceneName)
		{
			//�Đ�����
			if (!_audioSource.isPlaying)
			{
				//���݂̃V�[�����^�C�g���V�[����
				if (_nowSceneName == _titleSceneName)
				{
					PlayTitleBGM();
				}
				//���݂̃V�[�����Q�[���V�[����
				else if (_nowSceneName == _gameSceneName)
				{
					PlayInGameBGM();
				}
			}
		}
		//���݂̃V�[���Ƃ͈Ⴄ�ꍇ
		else
		{
			//���݂̃V�[�����擾
			_nowSceneName = SceneManager.GetActiveScene().name;
			//�Đ����~
			_audioSource.Stop();
		}
	}

	/// <summary>
	/// BGM���~���[�g�ɂ���
	/// </summary>
	public void BGMLoopMute()
	{
		isMute = true;
		_audioSource.Stop();
	}

	/// <summary>
	/// BGM�̃~���[�g����������
	/// </summary>
	public void BGMLoopUnMute()
	{
		isMute = false;
	}

	/// <summary>
	/// �C���Q�[����BGM���Đ�����
	/// </summary>
	private void PlayInGameBGM()
	{
		_audioSource.clip = _gameBGM;
		_audioSource.Play();
	}

	/// <summary>
	/// �^�C�g����BGM���Đ�����
	/// </summary>
	private void PlayTitleBGM()
	{
		_audioSource.clip = _titleBGM;
		_audioSource.Play();
	}

	/// <summary>
	/// �Ղ�폜��SE���Đ�����
	/// </summary>
	public void PlayDeleteSound()
	{
		_audioSource.PlayOneShot(_deleteSound);
	}

	/// <summary>
	/// �Q�[���I�[�o�[��SE���Đ�����
	/// </summary>
	public void PlayGameOverSound()
	{
		_audioSource.PlayOneShot(_gameOverSound);
	}
}
