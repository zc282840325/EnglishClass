//------------------------------------------------------------------------------
// <auto-generated>
//     此代码已从模板生成。
//
//     手动更改此文件可能导致应用程序出现意外的行为。
//     如果重新生成代码，将覆盖对此文件的手动更改。
// </auto-generated>
//------------------------------------------------------------------------------

namespace EnglishClass.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class tb_RecordVideo
    {
        public int RID { get; set; }
        public Nullable<int> UID { get; set; }
        public Nullable<int> VID { get; set; }
        public Nullable<System.DateTime> CreateTime { get; set; }
    
        public virtual tb_User tb_User { get; set; }
        public virtual tb_Video tb_Video { get; set; }
    }
}
