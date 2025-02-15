using AutoMapper;
using UniversityPilot.BLL.Areas.Schedule.Interfaces;
using UniversityPilot.BLL.Areas.Schedule.Models;
using UniversityPilot.DAL.Areas.AcademicCalendar.Interfaces;

namespace UniversityPilot.BLL.Areas.Schedule.Services
{
    public class GroupsScheduleService : IGroupsScheduleService
    {
        private readonly IMapper _mapper;
        private readonly ISemesterRepository _semesterRepository;

        public GroupsScheduleService(IMapper mapper,
            ISemesterRepository semesterRepository)
        {
            _mapper = mapper;
            _semesterRepository = semesterRepository;
        }

        public async Task<IEnumerable<SemesterDTO>> GetUpcomingSemestersAsync(int count = 3)
        {
            return _mapper.Map<List<SemesterDTO>>(await _semesterRepository.GetUpcomingSemestersAsync(count));
        }
    }
}