// ---------------------------------------------------------
// DebugSystemScript.cs
//
// 作成日:10/19
// 更新日:12/15
// 作成者:熊谷航
// --------------------------------------------------------- 
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 配列内のデータを表示する
/// </summary>
public class DebugSystemScript : MonoBehaviour
{
	[SerializeField,Header("出力するテキスト")]
	private Text _text = default;
	[SerializeField,Header("どのプレイヤーの配列を表示するか")]
	private int _selectPlayerNum = default;
	[SerializeField,Header("ゲームマネージャー")]
	private GameManagerScript _gameManager = default;

	private string _data = default;

	private void Update()
	{
		//初期化
		_data = "";
		//配列内の情報をすべてstringに格納する
		for (int i = _gameManager.playField[_selectPlayerNum].FieldObjectManagerScript.FieldDataScript.FieldDataArrayColLength - 1; i >= 0; i--)
		{
			for (int k = 0; k < _gameManager.playField[_selectPlayerNum].FieldObjectManagerScript.FieldDataScript.FieldDataArrayRowLength; k++)
			{
				_data += ((int)_gameManager.playField[_selectPlayerNum].FieldObjectManagerScript.FieldDataScript.GetFieldData(k, i) + ",");
			}
			_data += "\n";
		}
		//格納したデータをテキストに入れる
		_text.text = _data;
	}
}
