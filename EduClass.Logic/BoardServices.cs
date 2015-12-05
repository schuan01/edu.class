using EduClass.Entities;
using EduClass.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EduClass.Logic
{
    public class BoardServices : EntityService<Group>, IBoardServices
    {

        IUnitOfWork _unitOfWork;
        IBoardRepository _boardRepository;

        public BoardServices(IUnitOfWork unitOfWork, IBoardRepository boardRepository)
            : base(unitOfWork, boardRepository)
        {
            _unitOfWork = unitOfWork;
            _boardRepository = boardRepository;
        }

        public IList<Post> GetPosts(int id)
        {
            return _boardRepository.GetPosts(id);
        }
    }
}
