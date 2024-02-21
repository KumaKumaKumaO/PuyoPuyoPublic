// ---------------------------------------------------------
// AudioScript.cs
//
// 作成日:10/31
// 更新日:12/15
// 作成者:熊谷航
// --------------------------------------------------------- 

using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// 音声をコントロールする
/// </summary>
public class AudioScript : MonoBehaviour
{
	[SerializeField]
	//ゲームシーンの名前
	private string _gameSceneName = "GameScene";
	[SerializeField]
	//タイトルシーンの名前
	private string _titleSceneName = "TitleScene";
	//現在のシーンの名前
	private string _nowSceneName = default;
	private bool isMute = false;

	[SerializeField]
	private AudioSource _audioSource = default;
	[SerializeField]
	//インゲームのBGM
	private AudioClip _gameBGM = default;
	[SerializeField]
	//タイトルのBGM
	private AudioClip _titleBGM = default;
	[SerializeField]
	//ぷよを消した時の音
	private AudioClip _deleteSound = default;
	[SerializeField]
	//ゲームオーバーのBGM
	private AudioClip _gameOverSound = default;
	//自身のインスタンス
	private static AudioScript _instance = default;

	public static AudioScript InstanceAudioScript { get => _instance; }
	private void Start()
	{
		//シングルトン化
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
		//ミュートしているか
		if (isMute)
		{
			return;
		}
		//現在のシーンが現在と同じか
		if (SceneManager.GetActiveScene().name == _nowSceneName)
		{
			//再生中か
			if (!_audioSource.isPlaying)
			{
				//現在のシーンがタイトルシーンか
				if (_nowSceneName == _titleSceneName)
				{
					PlayTitleBGM();
				}
				//現在のシーンがゲームシーンか
				else if (_nowSceneName == _gameSceneName)
				{
					PlayInGameBGM();
				}
			}
		}
		//現在のシーンとは違う場合
		else
		{
			//現在のシーン名取得
			_nowSceneName = SceneManager.GetActiveScene().name;
			//再生を停止
			_audioSource.Stop();
		}
	}

	/// <summary>
	/// BGMをミュートにする
	/// </summary>
	public void BGMLoopMute()
	{
		isMute = true;
		_audioSource.Stop();
	}

	/// <summary>
	/// BGMのミュートを解除する
	/// </summary>
	public void BGMLoopUnMute()
	{
		isMute = false;
	}

	/// <summary>
	/// インゲームのBGMを再生する
	/// </summary>
	private void PlayInGameBGM()
	{
		_audioSource.clip = _gameBGM;
		_audioSource.Play();
	}

	/// <summary>
	/// タイトルのBGMを再生する
	/// </summary>
	private void PlayTitleBGM()
	{
		_audioSource.clip = _titleBGM;
		_audioSource.Play();
	}

	/// <summary>
	/// ぷよ削除のSEを再生する
	/// </summary>
	public void PlayDeleteSound()
	{
		_audioSource.PlayOneShot(_deleteSound);
	}

	/// <summary>
	/// ゲームオーバーのSEを再生する
	/// </summary>
	public void PlayGameOverSound()
	{
		_audioSource.PlayOneShot(_gameOverSound);
	}
}
