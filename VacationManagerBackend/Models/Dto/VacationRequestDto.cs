using VacationManagerBackend.Enums;

namespace VacationManagerBackend.Models.Dto
{
    public class VacationRequestDto
    {
        public VacationRequestState? NewState { get; set; }
        public int RequestId { get; set; }
    }
}
