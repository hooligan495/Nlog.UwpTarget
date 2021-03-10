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


using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace NLog.Uwp.Target
{
    using System;
    using System.IO;
    using System.Text;
    using NLog.Layouts;
    using NLog.Targets;
    using NLog.Uwp.Target.Streams;

    /// <summary>
    /// UwpFileTarget for using the Windows.Storage APIs
    /// </summary>
    [Target("UwpFile")]
    public class UwpFileTarget : TargetWithLayoutHeaderAndFooter
    {

        private IFileStreamCache _fileStreamCache;

        /// <summary>
        /// File Target for running on UWP platform
        /// </summary>
        public UwpFileTarget() : this(FileStreamCache.Empty)
        {
        }

        /// <summary>
        /// File Target for running on UWP platform taking a file stream cache
        /// </summary>
        /// <param name="cache"></param>
        public UwpFileTarget(FileStreamCache cache)
        {
            _fileStreamCache = cache;
        }

        /// <summary>
        /// Core filename for the target.
        /// </summary>
        public Layout FileName { get; set; }

        /// <summary>
        /// Default encoding of the file being written to
        /// </summary>
        public Encoding Encoding { get; set; } = System.Text.Encoding.UTF8;

        private string _currentFileName;
        private Stream _currentFileStream;


        /// <summary>
        /// Setup the target
        /// </summary>
        protected override void InitializeTarget()
        {
            base.InitializeTarget();
            _fileStreamCache = new FileStreamCache(1, new FileStreamFactory());
        }

        /// <summary>
        /// Write event to target
        /// </summary>
        /// <param name="logEvent"></param>
        protected override void Write(LogEventInfo logEvent)
        {
            string fileContent = RenderLogEvent(this.Layout, logEvent);
            string fileName = RenderLogEvent(this.FileName, logEvent);

            var fileStream = GetFileStream(fileName);

            var nl = Encoding.GetBytes(Environment.NewLine);
            var bytes = Encoding.GetBytes(fileContent);
            var bytes1 = bytes.Concat(nl).ToArray();
            fileStream.Write(bytes1, 0, bytes1.Length);
            fileStream.Flush();
        }

        /// <summary>
        /// Close the current file target
        /// </summary>
        protected override void CloseTarget()
        {
            _currentFileName = null;
            _currentFileStream?.Dispose();
            _currentFileStream = null;
        }

        Stream GetFileStream(string fileName)
        {
            var fileStream = _currentFileStream;
            if (_currentFileName != fileName)
            {
                _currentFileStream?.Dispose();
                _currentFileStream = null;
                fileStream = _currentFileStream = _fileStreamCache.Factory.Open(fileName);
                _currentFileName = fileName;
            }
            return fileStream;
        }
    }
}
