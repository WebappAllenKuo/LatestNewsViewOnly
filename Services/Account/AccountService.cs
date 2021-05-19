using System;
using Infra.Models;
using Infra.Models.Account;
using Infra.Parameters;
using Repositories.Account;

namespace Services.Account
{
    public class AccountService
    {
        private readonly AccountRepository _accountRepository;
        private readonly HashService       _hashService;

        public AccountService(AccountRepository accountRepository,
                              HashService       hashService)
        {
            _accountRepository = accountRepository;
            _hashService       = hashService;
        }

        public UserInfoDto GetUserInfo(Guid? userGuid)
        {
            if (userGuid.HasValue == false)
            {
                return null;
            }

            var result = _accountRepository.GetUserByGuid(userGuid.Value);

            return result;
        }
    }
}
