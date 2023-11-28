using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaseDemo.Util
{
    public class PasswordEncoderUtil
    {
        public static bool VerifyUserPassword(string rawPassword, string encodedPassword)
        {
            return GetUserPasswordEncoder().Matches(rawPassword, encodedPassword);
        }

        private static IPasswordEncoder GetUserPasswordEncoder()
        {
            return new BCryptPasswordEncoder(8);
        }
    }

    public interface IPasswordEncoder
    {
        string Encode(string rawPassword);

        bool Matches(string rawPassword, string encodedPassword);
    }

    public class BCryptPasswordEncoder : IPasswordEncoder
    {
        private readonly int rounds = 4;

        public BCryptPasswordEncoder(int rounds)
        {
            this.rounds = rounds;
        }

        public string Encode(string rawPassword)
        {
            return BCrypt.Net.BCrypt.HashPassword(rawPassword, rounds);
        }

        public bool Matches(string rawPassword, string encodedPassword)
        {
            return BCrypt.Net.BCrypt.Verify(rawPassword, encodedPassword);
        }
    }
}
