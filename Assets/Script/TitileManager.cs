// ---------------------------------------------------------
// TitileManager.cs
//
// �쐬��:10/19
// �X�V��:12/15
// �쐬��:�F�J�q
// --------------------------------------------------------- 

using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// �^�C�g���}�l�[�W���[
/// </summary>
public class TitileManager : MonoBehaviour
{
    private PlayerInputScript inputScript = new PlayerInputScript();

    private void Update()
    {
        //���肪�����ꂽ��
        if (inputScript.IsSubmit())
        {
            //�V�[����ς���
            SceneManager.LoadScene("GameScene");
        }
    }
}
