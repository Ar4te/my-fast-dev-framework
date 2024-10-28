using Application.IService.RuleEngine;
using Domain.Entity.RuleEngine;
using Domain.IRepository;

namespace Application.Service.RuleEngine;

public class RuleFlowNodeService : BaseService<RuleFlowNode>, IRuleFlowNodeService
{
    private readonly IBaseRepository<RuleFlowNode> _repo;

    public RuleFlowNodeService(IBaseRepository<RuleFlowNode> repo)
    {
        _repo = repo;
        BaseRepo = repo;
    }
}
