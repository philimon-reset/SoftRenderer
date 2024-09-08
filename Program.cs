//-----------------------------------------------------------------------
// <copyright file="Program.cs" company="SoftEngine">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace SoftRenderer
{
    using System;
    using System.Windows.Forms;
    using SoftRendererForm;

    /// <summary>
    /// main entry function.
    /// </summary>
    internal static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        private static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new SoftRendererForm());
        }
    }
}
