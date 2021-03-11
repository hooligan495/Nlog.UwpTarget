// Copyright (c) 2004-2020 Jaroslaw Kowalski <jaak@jkowalski.net>, Kim Christensen, Julian Verdurmen
//  
//  All rights reserved.
//  
//  Redistribution and use in source and binary forms, with or without 
//  modification, are permitted provided that the following conditions 
//  are met:
//  
//  * Redistributions of source code must retain the above copyright notice, 
//    this list of conditions and the following disclaimer. 
//  
//  * Redistributions in binary form must reproduce the above copyright notice,
//    this list of conditions and the following disclaimer in the documentation
//    and/or other materials provided with the distribution. 
//  
//  * Neither the name of Jaroslaw Kowalski nor the names of its 
//    contributors may be used to endorse or promote products derived from this
//    software without specific prior written permission. 
//  
//  THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS"
//  AND ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE 
//  IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE 
//  ARE DISCLAIMED. IN NO EVENT SHALL THE COPYRIGHT OWNER OR CONTRIBUTORS BE 
//  LIABLE FOR ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR 
//  CONSEQUENTIAL DAMAGES (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF
//  SUBSTITUTE GOODS OR SERVICES


using System;
using System.IO;
using System.Text;
using NLog.Config;
using NLog.Layouts;
using NLog.Targets;
using NLog.UnitTests;
using NLog.Uwp.Target;
using Xunit;

namespace NLog.Uwp.Tests.Target
{
    //using NLog.UnitTests;

    public class UwpTargetTests : NLogTestBase
    {
        private readonly ILogger logger = LogManager.GetLogger("NLog.UnitTests.Targets.UwpFileTargetTests");

        /// <summary>
        /// because UWP .. this won't ever pass unless you go to Settings->Privacy->File System and allow Apps to access your filesystem THEN turn on access to this test app :/
        /// </summary>
        [Fact]
        public void BroadFileSystemAccessTest()
        {
            var logFile = "C:\\Temp\\randomfile.txt";
            try
            {
                var fileTarget = new UwpFileTarget
                {
                    FileName = SimpleLayout.Escape(logFile),
                    Layout = "${level} ${message}",
                };

                SimpleConfigurator.ConfigureForTargetLogging(fileTarget, LogLevel.Debug);

                logger.Debug("aaa");
                logger.Info("bbb");
                logger.Warn("ccc");

                LogManager.Configuration = null; // Flush

                AssertUwpFileContents(logFile, $"Debug aaa{Environment.NewLine}Info bbb{Environment.NewLine}Warn ccc{Environment.NewLine}", Encoding.UTF8, false);
            }
            finally
            {
                if (File.Exists(logFile))
                    File.Delete(logFile);
            }

        }
        
        [Fact]
        public void SimpleFileTest()
        {
            var logFile = Path.GetTempFileName();
            try
            {
                var fileTarget = new UwpFileTarget
                {
                    FileName = SimpleLayout.Escape(logFile),
                    Layout = "${level} ${message}",
                };

                SimpleConfigurator.ConfigureForTargetLogging(fileTarget, LogLevel.Debug);

                logger.Debug("aaa");
                logger.Info("bbb");
                logger.Warn("ccc");

                LogManager.Configuration = null; // Flush

               AssertFileContents(logFile, $"Debug aaa{Environment.NewLine}Info bbb{Environment.NewLine}Warn ccc{Environment.NewLine}", Encoding.UTF8);
            }
            finally
            {
                if (File.Exists(logFile))
                    File.Delete(logFile);
            }

        }

        [Fact]
        public void SetupNLogWithUwpTarget()
        {

            try
            {

                const string LoggerConfig = @"
                <nlog>
                    <extensions>
                      <add assembly='UwpTarget'/>
                    </extensions>
                    <targets><target name='UwpFileTest' type='UwpFile' fileName='c:/Temp/sample.txt' layout='${message}'/></targets>
                    <rules>
                        <logger name='*' minlevel='Trace' writeTo='UwpFileTest'/>
                    </rules>
                </nlog>";

                LogManager.Configuration = XmlLoggingConfiguration.CreateFromXmlString(LoggerConfig);
                ILogger loggerA = LogManager.GetLogger("UwpFileLoggingTest");
                loggerA.Trace("Test");
                // The starting state for logging is enable.
                Assert.True(LogManager.IsLoggingEnabled());
                LogManager.Shutdown();

                AssertUwpFileContents(@"c:\Temp\sample.txt", $"Test{Environment.NewLine}", Encoding.UTF8, false);
            }
            finally
            {
                if (File.Exists(@"c:\Temp\sample.txt"))
                    File.Delete(@"c:\Temp\sample.txt");

            }


        }
    }
}