﻿using System;
namespace Util.Common
{
    [Serializable]
    public class ResponseApi<T>
    {
        public bool Status { get; set; }
        public string Message { get; set; }
        public T Data { get; set; }
    }
}
