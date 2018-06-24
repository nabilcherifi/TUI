namespace Tui.Flights.Persistence.Api.Infrastructure
{
    using System.Net;
    using System.Security.Principal;
    using Microsoft.Reporting.WebForms;

    /// <summary>
    /// CustomReportCredentials
    /// </summary>
    public class CustomReportCredentials : IReportServerCredentials
    {
        private readonly string _userName;
        private readonly string _passWord;
        private readonly string _domainName;

        /// <summary>
        /// Gets impersonationUser
        /// </summary>
        public WindowsIdentity ImpersonationUser => null;

        /// <summary>
        /// Gets networkCredentials
        /// </summary>
        public ICredentials NetworkCredentials => new NetworkCredential(this._userName, this._passWord, this._domainName);

        /// <summary>
        /// Initializes a new instance of the <see cref="CustomReportCredentials"/> class.
        /// CustomReportCredentials
        /// </summary>
        /// <param name="userName">userName</param>
        /// <param name="passWord">passWord</param>
        /// <param name="domainName">domainName</param>
        public CustomReportCredentials(string userName, string passWord, string domainName)
        {
            this._userName = userName;
            this._passWord = passWord;
            this._domainName = domainName;
        }

        /// <summary>
        /// GetFormsCredentials
        /// </summary>
        /// <param name="authCookie">authCookie</param>
        /// <param name="userName">userName</param>
        /// <param name="password">password</param>
        /// <param name="authority">authority</param>
        /// <returns>bool</returns>
        public bool GetFormsCredentials(out Cookie authCookie, out string userName, out string password, out string authority)
        {
            authCookie = null;
            userName = null;
            password = null;
            authority = null;

            return false;
        }
    }
}
