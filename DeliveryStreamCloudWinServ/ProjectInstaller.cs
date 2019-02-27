﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration.Install;
using System.Linq;

namespace DeliveryStreamCloudWinServ
{
    /// <summary>
    /// Partial ProjectInstaller class
    /// </summary>
    [RunInstaller(true)]
    public partial class ProjectInstaller : System.Configuration.Install.Installer
    {
        /// <summary>
        /// Default constructor
        /// </summary>
        public ProjectInstaller()
        {
            InitializeComponent();
        }

        public override void Install(System.Collections.IDictionary stateSaver)
        {
            RetrieveServiceName();
            base.Install(stateSaver);
        }

        public override void Uninstall(System.Collections.IDictionary savedState)
        {
            RetrieveServiceName();
            base.Uninstall(savedState);
        }

        private void RetrieveServiceName()
        {
            var serviceName = Context.Parameters["servicename"];
            if (!string.IsNullOrEmpty(serviceName))
            {
                this.DeliveryStreamCloudService1.ServiceName = serviceName;
                this.DeliveryStreamCloudService1.DisplayName = serviceName;
            }
        }
    }
}
