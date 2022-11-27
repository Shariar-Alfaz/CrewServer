using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.AccessControl;
using System.Text;
using System.Threading.Tasks;

namespace Entity.DTO
{
    public class SendData<T>
    {
        public bool HasError { get; set; } = false;
        public string? Message { get; set; }
        public List<T?> Data { get; set; } = new List<T?>();
        public bool Success { get; set; }
        public T? SingleData { get; set; }

        public static implicit operator SendData<T>(SendData<Teacher> v)
        {
            throw new NotImplementedException();
        }
    }
}
