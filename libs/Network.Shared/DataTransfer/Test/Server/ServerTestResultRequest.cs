﻿using System;
using Network.Shared.DataTransfer.Interface;

namespace Network.Shared.DataTransfer.Test.Server {

    [Serializable]
    public class ServerTestResultRequest : IRequest {
        public string Filepath { get; set; }
    }

}