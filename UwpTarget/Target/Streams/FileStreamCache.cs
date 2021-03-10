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

namespace NLog.Uwp.Target.Streams
{
    /// <summary>
    /// File stream management if we need to have more than one stream open
    /// or if we want to have metadata associated with a stream we could manage
    /// it through here
    /// </summary>
    public class FileStreamCache : IFileStreamCache
    {
        /// <summary>
        /// An "empty" instance of the <see cref="FileStreamCache"/> class with zero size and empty list of appenders.
        /// </summary>
        public static readonly FileStreamCache Empty = new FileStreamCache();

        /// <summary>
        /// Return the factor for stream creation
        /// </summary>
        public IFileStreamFactory Factory { get; }

        /// <summary>
        /// Number of file streams we can have open
        /// </summary>
        public int Size { get; }


        /// <summary>
        /// Default empty cache
        /// </summary>
        public FileStreamCache() : this(0, null)
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="size"></param>
        /// <param name="factory"></param>
        public FileStreamCache(int size, IFileStreamFactory factory)
        {
            Size = size;
            Factory = factory;
        }
    }
}
