﻿using CSRedis;
using JetBrains.Annotations;
using System.Collections.Generic;

namespace SharpAbp.Abp.CSRedisCore
{
    public interface ICSRedisClientFactory
    {
        /// <summary>
        /// Get redis client by name
        /// </summary>
        /// <param name="name">name</param>
        /// <returns></returns>
        [NotNull]
        CSRedisClient Get([NotNull] string name = DefaultClient.Name);


        /// <summary>
        /// Get all redis client
        /// </summary>
        /// <returns></returns>
        List<CSRedisClient> GetAll();
    }
}
