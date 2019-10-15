//Copyright (c) 2018 Yardi Technology Limited. Http://www.kooboo.com
//All rights reserved.
using System;

namespace Kooboo.Data.Models
{
    public class UserBalance
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public decimal Balance { get; set; } = 0;
        public Currency Currency { get; set; }
    }
}