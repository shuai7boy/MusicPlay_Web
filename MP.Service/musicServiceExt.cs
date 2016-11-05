using MP.Entity.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MP.Service
{
    public partial class musicService
    {
        /// <summary>
        /// 获取第一条音乐
        /// </summary>
        /// <returns></returns>
        public music GetFirstMusic()
        {
            music model = this.FirstOrDefault(c => true);            
            return model;
        }
    }
}
