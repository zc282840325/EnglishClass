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
    using System.ComponentModel.DataAnnotations;

    public partial class tb_Library
    {
        public int Lid { get; set; }
        [Display(Name = "视频标题")]
        public Nullable<int> VID { get; set; }
        [Display(Name = "难易度")]
        public Nullable<int> State { get; set; }
        [Display(Name = "题目")]
        public string Question { get; set; }
        [Display(Name = "选项A")]
        public string Answer1 { get; set; }
        [Display(Name = "选项B")]
        public string Answer2 { get; set; }
        [Display(Name = "选项C")]
        public string Answer3 { get; set; }
        [Display(Name = "正确选项")]
        public string TAnswer { get; set; }
    
        public virtual tb_Video tb_Video { get; set; }
    }
}