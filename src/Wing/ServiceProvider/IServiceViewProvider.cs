using Wing.Model;
using Wing.Result;
using Wing.ServiceProvider.Dto;

namespace Wing.ServiceProvider
{
    public interface IServiceViewProvider
    {
        /// <summary>
        /// 服务明细
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        Task<PageResult<List<ServiceDetailDto>>> Detail(PageModel<ServiceSearchDto> dto);

        /// <summary>
        /// 服务汇总
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        Task<PageResult<List<ServiceDto>>> List(PageModel<string> dto);

        /// <summary>
        /// 删除服务
        /// </summary>
        /// <param name="serviceId"></param>
        /// <returns></returns>
        Task<bool> Delete(string serviceId);

        /// <summary>
        /// 服务死亡率排行
        /// </summary>
        /// <returns></returns>
        Task<List<ServiceCriticalDto>> CritiCalLvRanking();
    }
}
