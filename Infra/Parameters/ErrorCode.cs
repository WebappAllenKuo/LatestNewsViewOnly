using System;
using System.Collections.Generic;

namespace Infra.Parameters
{
    public class ErrorCode
    {
        private static readonly Dictionary<int, string> _errorMessages
            = new Dictionary<int, string>
              {
                  [E400001] = "驗證碼輸入不正確",
                  [E400002] = "帳號或密碼輸入不正確",
                  [E400003] = "表單驗証失敗",
                  [E400004] = "找不到對應的使用者資料",
                  [E400005] = "查無帳號",
                  [E400006] = "Request Body 超出指定容量",
                  [E400007] = "上傳檔案超過指定大小",
                  [E400008] = "上傳檔案有誤，請重新上傳",
                  [E400009] = "Cache Key 為空",
                  [E400010] = "資料不存在"
              };

        /// <summary>
        ///     驗證碼輸入不正確
        /// </summary>
        public static int E400001 => 400001;

        /// <summary>
        ///     帳號或密碼輸入不正確
        /// </summary>
        public static int E400002 => 400002;

        /// <summary>
        ///     表單驗証失敗
        /// </summary>
        public static int E400003 => 400003;

        /// <summary>
        ///     找不到對應的使用者資料
        /// </summary>
        public static int E400004 => 400004;

        /// <summary>
        ///     查無帳號
        /// </summary>
        public static int E400005 => 400005;

        /// <summary>
        ///     Request Body 超出指定容量
        /// </summary>
        public static int E400006 => 400006;

        /// <summary>
        ///     上傳檔案超過指定大小
        /// </summary>
        public static int E400007 => 400007;

        /// <summary>
        ///     上傳檔案有誤，請重新上傳
        /// </summary>
        public static int E400008 => 400008;

        /// <summary>
        ///     Cache Key 為空
        /// </summary>
        public static int E400009 => 400009;

        /// <summary>
        ///     資料不存在
        /// </summary>
        public static int E400010 => 400010;

        public static string GetErrorMessage(int errorCode)
        {
            var errorMessage = _errorMessages.GetValueOrDefault(errorCode);

            if (string.IsNullOrWhiteSpace(errorMessage))
            {
                throw new NotSupportedException();
            }

            return errorMessage;
        }
    }
}
