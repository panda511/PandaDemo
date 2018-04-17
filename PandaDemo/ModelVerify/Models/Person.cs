using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ModelVerifyDemo.Models
{
    //[DataType(DataType.EmailAddress)] //DataType属性用于格式化目的，而不是用于验证。

    public class Person
    {
        [Required, StringLength(3, MinimumLength = 3)]
        [RegularExpression(@"\w{2,15}", ErrorMessage = "名称应为2-15长度的字母组合")]
        public string Name { get; set; }


        [Range(0, 2, ErrorMessage = "Age错误")]
        public int Age { get; set; } = -99;

        [Required]
        public int? Sex { get; set; }



        [EmailAddress]
        [Required(ErrorMessage = "Email不能为空")]
        public string Email { get; set; }


        [Range(typeof(DateTime), "1900-1-1", "2900-1-1", ErrorMessage = "Time错误")]
        public DateTime BirthDay { get; set; }


        public bool HasBrotherOrSister { get; set; }

        [Display(Name="密码")] //优化属性名称的输出
        public string Password { get; set; }

        //[DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "密码要一致")]
        public string RptPassword { get; set; }

        [MyValidation]
        public string TestName { get; set; }


    }

    /// <summary>
    /// 自定义验证方式
    /// </summary>
    public class MyValidationAttribute : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            string msg = value as string;
            if (msg == "张三")
            {
                return true;
            }
            ErrorMessage = "内容错误";
            return false;
        }
    }
}