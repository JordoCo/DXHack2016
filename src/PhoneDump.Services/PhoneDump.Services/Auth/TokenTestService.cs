using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PhoneDump.Contract.Servers;
using PhoneDump.Entity.Auth;
using XamlingCore.Portable.Contract.Repos.Base;

namespace PhoneDump.Services.Auth
{
    public class TokenTestService : ITokenTestService
    {
        private readonly IXWebRepo<TokenResult> _tokenResult;

        public TokenTestService(IXWebRepo<TokenResult> tokenResult)
        {
            _tokenResult = tokenResult;
            _tokenResult.SetEndPoint("https://jwtparse.azurewebsites.net/api/JWTWithRsaValidator?code=r3gqblabmyfju0c6zbhknwyiimhdrdn1yld7");
        }

        public async Task<TokenResult> TestToken(string token)
        {
            var n = new TokenRequest
            {
                Token = token,
                RsaKey = "PFJTQUtleVZhbHVlPjxNb2R1bHVzPjNhSm1GcHhJR2EvcDhlRWVLNXg5SzZSMGVWclh2YjEyczRIRWplQ29XUStKYnFrb1VpM29jdnYxRjFLT2tOWUdVTHBUWitYajFxS3FOSXBlTWxLUTBwRFllcUpwYTRWYU14TGJtejRIbEFYNnhVZWRLdzNrM2EzL1ZkUGZSbXc3aUIrdjlXNlVCL2NJMm1Db05VZFhnM3hPVndyZnBDTFJzaU1pWkE4NENUSDVPZmNFZUVWanZzMm4rSjcvZ0JEelRVc1JWMFFqaXRVMWxKYkFBNzNGdVQ2aXhTZktTRUw3ZWhWRFdXZEFSWU1nNjFkc0R1THY5UWQrdWMvYmdjUEdpa3dWVXRwa3o0dUV1aXBwcTQ4bEpjQTFmU2VHekxER3pVbG1ESkY1MWt6V201ZndUUXovNU9MY1QrbVhLSmhzQXlMTUNNL21MS0E3UFordTFFWFVrdz09PC9Nb2R1bHVzPjxFeHBvbmVudD5BUUFCPC9FeHBvbmVudD48L1JTQUtleVZhbHVlPg==",
                Issuer = "CentralAuthHost",
                Audience = "SomeDemoServer"
            };


            var result = await _tokenResult.Post(n);

            return result.Object;
        }
    }
}
