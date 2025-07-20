using System.Threading.Tasks;
using erlauncher.Models;

namespace erlauncher.Services
{
    /// <summary>
    /// ツイート送信サービスのインターフェース
    /// </summary>
    public interface ITweetService
    {
        /// <summary>
        /// ツイートを送信します。
        /// </summary>
        /// <param name="tweetInfo">送信する TweetInfo</param>
        Task SendTweet(TweetInfo tweetInfo);
    }
}