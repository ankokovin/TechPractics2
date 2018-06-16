using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TechPractics2.Models.UtilityModels;
using System.Security.Cryptography;

namespace TechPractics2.Models.Repos
{
    public class SiteUtilitiesRepos
    {
        #region Tokes
        static private int saltLength = int.Parse(GlobalResources.SiteResources.SaltLength);

        private SiteUtilitiesContainer Container;

        private SHA256 sHA256;

        public SiteUtilitiesRepos()
        {
            sHA256 = SHA256.Create();
            Container = new SiteUtilitiesContainer();
        }

        private HashedToken HashTheToken(Token token,string PrevToken=null)
        {
            HashedToken hashedToken = new HashedToken();
            byte[] tokenVal = Convert.FromBase64String(token.Value);
            byte[] tokenDate = BitConverter.GetBytes(token.TokenInfo.dateTime.ToBinary());
            byte[] tokenSalt = Convert.FromBase64String(token.TokenInfo.Salt);
            byte[] serverSalt = Convert.FromBase64String(GlobalResources.SiteResources.MySalt);
            var inputString = tokenVal.Concat(tokenDate).Concat(tokenSalt).Concat(serverSalt);
            if (PrevToken != null) inputString = inputString.Concat(Convert.FromBase64String(PrevToken));
            hashedToken.Value = Convert.ToBase64String(sHA256.ComputeHash(inputString.ToArray()));
            return hashedToken;
        }

        private HashedToken GetHashedToken(string input) => Container.HashedTokenSet.Where(x => x.Value == input).FirstOrDefault();

        public string SuccessfulManualLogin(string Login, string Password, string Source)
        {
            var tokeninfo = Container.TokenInfoSet.Where(x => x.Login == Login).FirstOrDefault();
            if (tokeninfo == null)
            {
                Token token = new Token
                {
                    Value = Convert.ToBase64String(Guid.NewGuid().ToByteArray()),
                    TokenInfo = new TokenInfo
                    {
                        dateTime = DateTime.UtcNow.AddDays(int.Parse(GlobalResources.SiteResources.CookiesExpirationDays)),
                        Salt = GenerateSalt(),
                        Login = Login,
                        Source = Source
                    }
                };
                token.TokenInfo.HashedToken = HashTheToken(token);
                Container.Entry(token).State = token.Id == 0 ? System.Data.Entity.EntityState.Added : System.Data.Entity.EntityState.Modified;
                Container.Entry(token.TokenInfo).State = token.TokenInfo.Id == 0 ? System.Data.Entity.EntityState.Added : System.Data.Entity.EntityState.Modified;
                Container.Entry(token.TokenInfo.HashedToken).State = token.TokenInfo.HashedToken.Id == 0 ? System.Data.Entity.EntityState.Added : System.Data.Entity.EntityState.Modified;
                Container.SaveChanges();
                return token.TokenInfo.HashedToken.Value;
            }
            else
            {
                tokeninfo.Salt = GenerateSalt();
                tokeninfo.Source = Source;
                tokeninfo.dateTime = DateTime.UtcNow.AddDays(int.Parse(GlobalResources.SiteResources.CookiesExpirationDays));
                var oldhashedtoken = tokeninfo.HashedToken;
                var oldhash = oldhashedtoken.Value;
                Container.HashedTokenSet.Remove(oldhashedtoken);
                tokeninfo.HashedToken = HashTheToken(tokeninfo.Token, oldhash);
                Container.Entry(tokeninfo).State = tokeninfo.Id == 0 ? System.Data.Entity.EntityState.Added : System.Data.Entity.EntityState.Modified;
                Container.Entry(tokeninfo.HashedToken).State = tokeninfo.HashedToken.Id == 0 ? System.Data.Entity.EntityState.Added : System.Data.Entity.EntityState.Modified;
                Container.Entry(tokeninfo.Token).State = tokeninfo.Token.Id == 0 ? System.Data.Entity.EntityState.Added : System.Data.Entity.EntityState.Modified;
                Container.SaveChanges();
                return tokeninfo.HashedToken.Value;
            }
        }

        private string GenerateSalt()
        {
            RNGCryptoServiceProvider rNGCryptoServiceProvider = new RNGCryptoServiceProvider();
            byte[] salt = new byte[saltLength];
            rNGCryptoServiceProvider.GetBytes(salt, 0, saltLength);
            return Convert.ToBase64String(salt);
        }

        public bool TrySignIn(string Login, string inToken, string Source, out string token)
        {
            token = string.Empty;
            var hashedToken = GetHashedToken(inToken);
            if (hashedToken == null) return false;
            var nTokenInfo = hashedToken.TokenInfo;
            if (Login == nTokenInfo.Login && Source == nTokenInfo.Source)
            {
                nTokenInfo.dateTime = DateTime.Now;
                var nHashedToken = HashTheToken(nTokenInfo.Token);
                nTokenInfo.Salt = GenerateSalt();
                Container.HashedTokenSet.Remove(hashedToken);
                nTokenInfo.HashedToken = nHashedToken;
                Container.Entry(nTokenInfo).State = nTokenInfo.Id == 0 ? System.Data.Entity.EntityState.Added : System.Data.Entity.EntityState.Modified;
                Container.Entry(nTokenInfo.Token).State = nTokenInfo.Token.Id == 0 ? System.Data.Entity.EntityState.Added : System.Data.Entity.EntityState.Modified;
                Container.Entry(nTokenInfo.HashedToken).State = nTokenInfo.HashedToken.Id == 0 ? System.Data.Entity.EntityState.Added : System.Data.Entity.EntityState.Modified;
                Container.SaveChanges();
                token =  nTokenInfo.HashedToken.Value;
                return true;
            }else
            {
                if (Login == nTokenInfo.Login)
                {
                    Container.TokenSet.Remove(nTokenInfo.Token);
                    Container.HashedTokenSet.Remove(nTokenInfo.HashedToken);
                    Container.TokenInfoSet.Remove(nTokenInfo);
                    Container.SaveChanges();

                }
                return false;
            }
        }
        #endregion

        #region Excel
        public string Export()
        {
            return null;
        }
        #endregion
    }

}