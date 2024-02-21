// ---------------------------------------------------------
// TitileManager.cs
//
// 作成日:10/19
// 更新日:12/15
// 作成者:熊谷航
// --------------------------------------------------------- 

using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// タイトルマネージャー
/// </summary>
public class TitileManager : MonoBehaviour
{
    private PlayerInputScript inputScript = new PlayerInputScript();

    private void Update()
    {
        //決定が押されたら
        if (inputScript.IsSubmit())
        {
            //シーンを変える
            SceneManager.LoadScene("GameScene");
        }
    }
}
