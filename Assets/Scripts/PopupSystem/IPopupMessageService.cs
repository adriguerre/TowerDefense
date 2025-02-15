namespace PopupSystem
{
    public interface IPopupMessageService
    {
        /// <summary>
        /// Push popup message to the queue
        /// </summary>
        /// <param name="data"></param>
        void PushPopup(APopupData data);

        /// <summary>
        /// Get messages queue size
        /// </summary>
        /// <returns>Number of messages in the queue</returns>
        int getQueueSize();
        
        /// <summary>
        /// Get opened popup object
        /// </summary>
        /// <returns>popup object</returns>
        APopup GetOpenedPopup();
    }
}