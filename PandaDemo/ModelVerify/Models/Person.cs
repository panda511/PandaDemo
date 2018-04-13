using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ModelVerify.Models
{
    //[DataType(DataType.EmailAddress)] //DataType属性用于格式化目的，而不是用于验证。

    public class Person
    {
        [Required]
        [StringLength(3, MinimumLength = 2)]
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


        [DataType(DataType.Password)]
        public String Password { get; set; }

        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "密码要一致")]
        public String RptPassword { get; set; }

    }
}