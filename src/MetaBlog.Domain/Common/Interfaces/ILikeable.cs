using MetaBlog.Domain.Likes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MetaBlog.Domain.Common.Interfaces
{
   
        public interface ILikable
        {
            Guid Id { get; }            
            int likesCount { get; }      

            void IncrementLike();      
            void DecrementLike();       
        }
    


}
