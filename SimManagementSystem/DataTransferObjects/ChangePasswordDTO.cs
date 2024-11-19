namespace SimManagementSystem.DataTransferObjects
{
    public class ChangePasswordDTO
    {
        public string OldPassword { get; set; } = null!;
        public string NewPassword { get; set; } = null!;
    }
}
