using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyTiskTask2.Model;

public class UserModel
{
    public int Id { get; set; }
    
    public string Password { get; set; }

    
    // Внешний ключ ( ссылка на Library)
    public int UserId { get; set; }
    
    // Навигационное свойство на User
    [ForeignKey("UserId")]
    public virtual User User { get; set; }
}
