﻿using Moq;
using TotvsChallengePoC.Data.Contracts;
using TotvsChallengePoC.Entities;

namespace TotvsChallengePoC.Tests.MockRepository
{
    public class ClientRepositoryMock
    {
        public Mock<IClientRepository> clientRepository { get; set; }
        private const string clientId = "3FA85F64-5717-4562-B3FC-2C963F66AFA1";

        public ClientRepositoryMock()
        {
            clientRepository = new Mock<IClientRepository>();
        }
    }
}
