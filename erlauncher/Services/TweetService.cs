using System;
using System.Threading.Tasks;
using erlauncher.Models;

namespace erlauncher.Services
{
    /// <summary>
    /// ツイート送信サービスの実装
    /// </summary>
    public class TweetService : ITweetService
    {
        /// <summary>
        /// ツイートを送信します。
        /// </summary>
        /// <param name="tweetInfo">送信する TweetInfo</param>
        public Task SendTweet(TweetInfo tweetInfo)
        {
            throw new NotImplementedException();
        }
    }
}