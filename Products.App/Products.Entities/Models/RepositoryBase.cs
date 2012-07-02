namespace Products.Entities.Models
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Mindscape.LightSpeed;

    public abstract class RepositoryBase : IRepository
    {
        protected UnitOfWorkScopeBase<DbUnitOfWork> _unitOfWorkScope;

        protected RepositoryBase(UnitOfWorkScopeBase<DbUnitOfWork> unitOfWorkScope)
        {
            _unitOfWorkScope = unitOfWorkScope;
        }

        public void SaveChanges()
        {
            _unitOfWorkScope.Current.SaveChanges();
        }
    }
}
