using System.ComponentModel.DataAnnotations;


namespace ViewApi.Data
{
    public class User
	{
        [Key]
        public Guid Id { get; set; }
        public string UserName { get; set; }
        public string UserPassword { get; set; }
    }
}
