﻿using StackoverflowGuide.BLL.Models.Tag;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StackoverflowGuide.BLL.RepositoryInterfaces
{
    public interface ITagBQRepository: IBaseBQRepository<BqTag>
    {
        public List<BqTag> GetAll();
    }
}
