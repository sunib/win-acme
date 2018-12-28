﻿using PKISharp.WACS.Clients;
using PKISharp.WACS.DomainObjects;
using PKISharp.WACS.Plugins.Interfaces;
using PKISharp.WACS.Services;

namespace PKISharp.WACS.Plugins.InstallationPlugins
{
    internal class Script : ScriptClient, IInstallationPlugin
    {
        private ScheduledRenewal _renewal;
        private ScriptOptions _options;

        public Script(ScheduledRenewal renewal, ScriptOptions options, ILogService logService) : base(logService)
        {
            _renewal = renewal;
            _options = options;
        }

        void IInstallationPlugin.Install(CertificateInfo newCertificate, CertificateInfo oldCertificate)
        {
            RunScript(
                  _options.Script,
                  _options.ScriptParameters,
                  _renewal.FriendlyName,
                  Properties.Settings.Default.PFXPassword,
                  newCertificate.PfxFile.FullName,
                  newCertificate.Store?.Name ?? newCertificate.PfxFile.Directory.FullName,
                  newCertificate.Certificate.FriendlyName,
                  newCertificate.Certificate.Thumbprint,
                  newCertificate.PfxFile.Directory.FullName);
        }
    }
}