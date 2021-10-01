﻿using PKISharp.WACS.DomainObjects;
using PKISharp.WACS.Plugins.Base.Options;
using PKISharp.WACS.Plugins.Interfaces;
using PKISharp.WACS.Services;
using System;
using System.Threading.Tasks;

namespace PKISharp.WACS.Plugins.Base.Factories.Null
{
    /// <summary>
    /// Null implementation
    /// </summary>
    internal class NullStoreOptionsFactory : IStorePluginOptionsFactory, INull
    {
        Type IPluginOptionsFactory.InstanceType => typeof(NullStore);
        Type IPluginOptionsFactory.OptionsType => typeof(NullStoreOptions);
        Task<StorePluginOptions?> Generate() => Task.FromResult<StorePluginOptions?>(new NullStoreOptions());
        Task<StorePluginOptions?> IPluginOptionsFactory<StorePluginOptions>.Aquire(IInputService inputService, RunLevel runLevel) => Generate();
        Task<StorePluginOptions?> IPluginOptionsFactory<StorePluginOptions>.Default() => Generate();
        (bool, string?) IPluginOptionsFactory.Disabled => (false, null);
        string IPluginOptionsFactory.Name => NullStoreOptions.PluginName;
        string IPluginOptionsFactory.Description => new NullStoreOptions().Description;
        bool IPluginOptionsFactory.Match(string name) => string.Equals(name, new NullInstallationOptions().Name, StringComparison.InvariantCultureIgnoreCase);
        int IPluginOptionsFactory.Order => int.MaxValue;
    }

    /// <summary>
    /// Do not make INull, we actually need to store these options to override
    /// the default behaviour of CertificateStore
    /// </summary>
    [Plugin("cfdd7caa-ba34-4e9e-b9de-2a3d64c4f4ec")]
    internal class NullStoreOptions : StorePluginOptions<NullStore>
    {
        internal const string PluginName = "None";
        public override string Name => PluginName;
        public override string Description => "No (additional) store steps";
    }

    internal class NullStore : IStorePlugin
    {
        (bool, string?) IPlugin.Disabled => (false, null);
        public Task Delete(CertificateInfo certificateInfo) => Task.CompletedTask;
        public Task Save(CertificateInfo certificateInfo) {
            certificateInfo.StoreInfo.Add(GetType(),
                    new StoreInfo()
                    {
                        Name = NullStoreOptions.PluginName,
                        Path = ""
                    });
            return Task.CompletedTask;
        }
    }

}
