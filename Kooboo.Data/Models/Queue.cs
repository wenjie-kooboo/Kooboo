//Copyright (c) 2018 Yardi Technology Limited. Http://www.kooboo.com
//All rights reserved.
using System;

namespace Kooboo.Data.Models
{
    public class Queue : IGolbalObject
    {
        /// <summary>
        /// Full name of task model type..
        /// </summary>
        public string TaskModelType { get; set; }

        public string JsonModel { get; set; }

        public Guid WebSiteId { get; set; }

        private Guid _id;

        public Guid Id
        {
            get
            {
                if (_id == default(Guid))
                {
                    _id = System.Guid.NewGuid();
                }
                return _id;
            }

            set => _id = value;
        }
    }
}