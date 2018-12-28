﻿using PKISharp.WACS.Clients;
using PKISharp.WACS.DomainObjects;
using PKISharp.WACS.Services;
using System.Linq;

namespace PKISharp.WACS.Plugins.ValidationPlugins.Http
{
    internal class Sftp : HttpValidation<SftpOptions, Sftp>
    {
        private SshFtpClient _sshFtpClient;

        public Sftp(ScheduledRenewal renewal, TargetPart target, SftpOptions options, ILogService log, IInputService input, ProxyService proxy, string identifier) : 
            base(log, input, options, proxy, renewal, target, identifier)
        {
            _sshFtpClient = new SshFtpClient(_options.Credential.GetCredential(), log);
        }

        protected override char PathSeparator => '/';

        protected override void DeleteFile(string path)
        {
            _sshFtpClient.Delete(path, SshFtpClient.FileType.File);
        }

        protected override void DeleteFolder(string path)
        {
            _sshFtpClient.Delete(path, SshFtpClient.FileType.Directory);
        }

        protected override bool IsEmpty(string path)
        {
            return !_sshFtpClient.GetFiles(path).Any();
        }

        protected override void WriteFile(string path, string content)
        {
            _sshFtpClient.Upload(path, content);
        }

        public override void CleanUp()
        {
            base.CleanUp();
            // Switched setting this to null, since this class will be needed for deleting files and folder structure
            _sshFtpClient = null;
        }
    }
}
