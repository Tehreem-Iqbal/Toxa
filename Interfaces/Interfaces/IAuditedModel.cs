namespace ProjectManagementApplication.Interfaces
{
    public interface IAuditedModel
    {
        public string? CreatedByUserId { get; set; }
        public DateTime CreatedDate { get; set; }
        public string? LastModifiedUserId { get; set; }
        public DateTime? LastModifiedDate { get; set; }
    }
}
