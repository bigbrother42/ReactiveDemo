using Newtonsoft.Json;
using System;

namespace BaseDemo.Data
{
    public class AppErrorInfo
    {
        /// <summary>
        /// エラー発生日時
        /// </summary>
        [JsonProperty("timestamp")]
        public DateTime Timestamp { get; set; }

        /// <summary>
        /// エラーコード
        /// </summary>
        [JsonProperty("code")]
        public string ErrorCode { get; set; }

        /// <summary>
        /// エラーメッセージ
        /// </summary>
        [JsonProperty("message")]
        public string ErrorMessage { get; set; }

        /// <summary>
        /// 例外のクラス名
        /// </summary>
        [JsonProperty("exceptionClass")]
        public string ExceptionClass { get; set; }

        /// <summary>
        /// 例外の原因情報
        /// </summary>
        [JsonProperty("exceptionCause")]
        public string ExceptionCause { get; set; }

        /// <summary>
        /// 例外のスタックトレース情報
        /// </summary>
        [JsonProperty("exceptionStackTrace")]
        public string ExceptionStackTrace { get; set; }

        /// <summary>
        /// HTTPリクエストがタイムアウトのフラグ
        /// </summary>
        [JsonProperty("isRequestTimeout")]
        public bool IsRequestTimeout { get; set; }

        /// <summary>
        /// HTTPリクエストがリトライアウトのフラグ
        /// </summary>
        [JsonProperty("isRetryOut")]
        public bool IsRetryOut { get; set; }
    }
}
