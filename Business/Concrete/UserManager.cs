using Business.Abstract;
using Core.Aspects.Autofac.Validation;
using Core.Entities.Concrete;
using DataAccess.Abstract;
using System.Collections.Generic;
using System.Linq;

namespace Business.Concrete
{
    public class UserManager : IUserService
    {
        IUserDal _userDal;

        public UserManager(IUserDal userDal)
        {
            _userDal = userDal;
        }

        public List<OperationClaim> GetClaims(EUser user)
        {
            return _userDal.GetClaims(user);
        }
        [ValidationAspect(typeof(EUser))]
        public void Add(EUser user)
        {
            _userDal.Add(user);
        }

        public EUser GetByMail(string email)
        {
            return _userDal.GetAll().SingleOrDefault(x => x.Email == email);
        }
    }
}
