namespace Interface
{
    /// <summary>
    /// ゲームマネージャーの状態を変更できる
    /// </summary>
    public interface IGameManagerStateChangable
    {
        /// <summary>
        /// ゲームオーバー状態に変更
        /// </summary>
        void ChangeToGameOverState();
        /// <summary>
        /// 操作中状態に変更
        /// </summary>
        void ChangeToPlayState();
        /// <summary>
        /// 削除待ち状態に変更
        /// </summary>
        void ChangeToPuyoStayState();
        /// <summary>
        /// ポーズ状態に変更
        /// </summary>
        void ChangeToPauseState();
        /// <summary>
        /// 前回のステートに変更
        /// </summary>
        void ChangeToBeforeState();
    }
}
