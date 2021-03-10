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


using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace NLog.Targets
{
    /// <summary>
    /// IFilesystem is used to abstract out specific filesystem access methods.  Due to the nature of
    /// platforms like UWP, if you want to write to file locations outside a UWP app's sandbox you
    /// need to turn on the BroadFileSystem access capability AND use teh Windows.Storage APIs vs the
    /// System.IO APIs.  This Interface provides for abstracting these APIs
    /// </summary>
    interface IFileSystem
    {
        /// <summary>
        /// List of log files in currently configured Nlog Directory
        /// </summary>
        List<string> LookupFileNames();
        
        /// <summary>
        /// Delete specified file
        /// </summary>
        /// <param name="name">File to be deleted</param>
        void DeleteFile(string name);

        /// <summary>
        /// Open a file and return a Stream.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        Stream OpenFileStream(string name);


        /// <summary>
        /// Test for the existence of a file (full path name)
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        bool FileExists(string name);


    }
}
